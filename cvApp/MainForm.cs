using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using Spire.Doc;
using Document = Spire.Doc.Document;
using CheckBox = System.Windows.Forms.CheckBox;
// --- ADD THESE LINES ---
using Mscc.GenerativeAI;
using System.Threading.Tasks;
using Image = System.Drawing.Image;

namespace cvApp
{
    public partial class MainForm : Form
    {
        // Note: Replace "YOUR_GEMINI_API_KEY_HERE" with your actual key. 
        // For production, use Environment.GetEnvironmentVariable("GEMINI_API_KEY") for security.
        private const string GeminiApiKey = "AIzaSyBoCT25bQ8HntrNcciBRqZ1uhHlIuN0EdA";
        public MainForm()
        {
            InitializeComponent();
            picLoader.Image = Image.FromFile("loader-wait.gif");
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Word Documents (*.docx)|*.docx|All Files (*.*)|*.*";
                ofd.Title = "Select CV File";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtCVPath.Text = ofd.FileName;
                }
            }
        }

        private async void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(txtCompany.Text))
                {
                    MessageBox.Show("Please enter company name.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCompany.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPosition.Text))
                {
                    MessageBox.Show("Please enter position.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPosition.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtKeywords.Text))
                {
                    MessageBox.Show("Please enter keywords.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtKeywords.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCVPath.Text) || !File.Exists(txtCVPath.Text))
                {
                    MessageBox.Show("Please select a valid CV file.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string keywords = txtKeywords.Text.Trim(); // Get user input

                // --- GEMINI INTEGRATION POINT ---
                if (!string.IsNullOrWhiteSpace(keywords))
                {
                    // Show loader and status
                    picLoader.Visible = true;
                    lblStatus.Text = "Please wait. Work on progress";
                    lblStatus.ForeColor = System.Drawing.Color.Blue;
                    Application.DoEvents();

                    // Combine Company and Position for a good context for the AI
                    string jobDescriptionInput = $"Company: {txtCompany.Text}, Position: {txtPosition.Text}\n{txtKeywords.Text}";

                    keywords = await ExtractKeywordsAsync(jobDescriptionInput);

                    if (keywords.StartsWith("ERROR:"))
                    {
                        // Handle extraction failure or API key error
                        picLoader.Visible = false;
                        lblStatus.Text = keywords; // Display the error message from the method
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        MessageBox.Show(keywords, "Gemini Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(keywords))
                    {
                        // If the AI returned nothing
                        picLoader.Visible = false;
                        lblStatus.Text = "Error: Gemini could not extract keywords. Please enter them manually.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        MessageBox.Show("Keywords extraction failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Populate the textbox with the AI's result for user visibility
                    txtKeywords.Text = keywords;
                }
                else
                {
                    // If the AI returned nothing
                    picLoader.Visible = false;
                    lblStatus.Text = "Error: Gemini could not extract keywords. Please enter them manually.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    MessageBox.Show("Keywords extraction failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // --- END GEMINI INTEGRATION ---

                // Process the CV
                bool convertToPdf = chkConvertToPdf.Checked;
                ProcessCV(txtCVPath.Text, txtCompany.Text, txtPosition.Text, txtKeywords.Text, convertToPdf);

                // Hide loader and update status
                picLoader.Visible = false;
                lblStatus.Text = "✓ Finished! CV has been customized successfully.\nThe file has been opened.";
                lblStatus.ForeColor = System.Drawing.Color.Green;

                MessageBox.Show("CV customized successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                picLoader.Visible = false;
                lblStatus.Text = $"Error: {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<string> ExtractKeywordsAsync(string jobDescription)
        {
            if (string.IsNullOrWhiteSpace(GeminiApiKey) || GeminiApiKey.Equals("YOUR_GEMINI_API_KEY_HERE"))
            {
                return "ERROR: Gemini API Key not set. Please update MainForm.cs.";
            }

            try
            {
                // 1. Initialize the GoogleAI client and select a capable model
                var googleAI = new GoogleAI(apiKey: GeminiApiKey);
                var model = googleAI.GenerativeModel(model: Model.Gemini25Flash); // gemini-2.5-flash is fast and capable.

                // 2. Craft a precise prompt (System Instruction)
                var prompt = $@"
                    You are an expert keyword extraction tool. 
                    Analyze the following job description to extract the most important technical skills, 
                    soft skills, and industry terms.
            
                    Return the extracted keywords as a single string, with each keyword 
                    separated **ONLY** by a comma and a single space (e.g., 'C#, .NET, Agile, Scrum Master'). 
                    Do not include any other text, explanations, or formatting (like bullet points or list numbers).
            
                    Job Description to Analyze:
                    ---
                    {jobDescription}
                    ---
                ";

                // 3. Call the API asynchronously
                var response = await model.GenerateContent(prompt);

                // 4. Return the cleaned-up result
                return response.Text.Trim().Trim('"', '\'').Trim();
            }
            catch (Exception ex)
            {
                // Return the error message to be displayed in the status bar
                return $"API Exception: {ex.Message}";
            }
        }

        private void ProcessCV(string sourcePath, string company, string position, string keywords, bool convertToPdf)
        {
            string directory = Path.GetDirectoryName(sourcePath) ?? "";
            string baseFileName = Path.GetFileNameWithoutExtension(sourcePath);

            string sanitizedCompany = SanitizeFileName(company);
            string sanitizedPosition = SanitizeFileName(position);

            string newFileName = $"{baseFileName}_{sanitizedCompany}_{sanitizedPosition}.docx";
            string newFilePath = Path.Combine(directory, newFileName);

            File.Copy(sourcePath, newFilePath, true);

            AddInvisibleKeywordsToFooter(newFilePath, keywords);

            string outputPath = newFilePath;

            if (convertToPdf)
            {
                string pdfFileName = $"{baseFileName}_{sanitizedCompany}_{sanitizedPosition}.pdf";
                outputPath = Path.Combine(directory, pdfFileName);

                Document document = new Document();
                document.LoadFromFile(newFilePath);
                document.SaveToFile(outputPath, FileFormat.PDF);

                File.Delete(newFilePath); // Clean up the temporary DOCX file
            }

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = outputPath,
                UseShellExecute = true
            });
        }

        private string SanitizeFileName(string fileName)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            string sanitized = new string(fileName
                .Where(c => !invalidChars.Contains(c))
                .ToArray());

            return sanitized.Replace(" ", "").ToLower();
        }

        private void AddInvisibleKeywordsToFooter(string filePath, string keywords)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
            {
                MainDocumentPart mainPart = doc.MainDocumentPart
                    ?? throw new InvalidOperationException("Document has no main part");

                var document = mainPart.Document;
                var body = document.Body ?? throw new InvalidOperationException("Document body is null");

                var sectionPropsList = body.Descendants<SectionProperties>().ToList();

                if (sectionPropsList.Count == 0)
                {
                    var sectionProps = new SectionProperties();
                    body.Append(sectionProps);
                    sectionPropsList.Add(sectionProps);
                }

                foreach (var sectionProps in sectionPropsList)
                {
                    FooterReference footerRef = sectionProps.Descendants<FooterReference>()
                        .FirstOrDefault(r => r.Type?.Value == HeaderFooterValues.Default);

                    FooterPart footerPart;
                    if (footerRef != null)
                    {
                        footerPart = (FooterPart)mainPart.GetPartById(footerRef.Id!);
                    }
                    else
                    {
                        footerPart = mainPart.AddNewPart<FooterPart>();
                        footerRef = new FooterReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(footerPart) };
                        sectionProps.Append(footerRef);
                    }

                    Footer footer = footerPart.Footer ?? new Footer();

                    Paragraph para = new Paragraph();

                    ParagraphProperties paraProps = new ParagraphProperties();
                    Justification justification = new Justification() { Val = JustificationValues.Left };
                    paraProps.Append(justification);
                    para.Append(paraProps);

                    Run run = new Run();
                    RunProperties runProps = new RunProperties();

                    Color color = new Color() { Val = "FFFFFF" };
                    runProps.Append(color);

                    FontSize fontSize = new FontSize() { Val = "2" }; // 1pt font size
                    runProps.Append(fontSize);

                    run.Append(runProps);
                    run.Append(new Text(keywords) { Space = SpaceProcessingModeValues.Preserve });

                    para.Append(run);
                    footer.Append(para);

                    footerPart.Footer = footer;
                    footerPart.Footer.Save();
                }

                mainPart.Document.Save();
            }
        }
    }
}
