using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Mscc.GenerativeAI;
using Spire.Doc;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheckBox = System.Windows.Forms.CheckBox;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using Document = Spire.Doc.Document;
using Image = System.Drawing.Image;
using System.Text.RegularExpressions; // For new sanitization

namespace cvApp
{
    //public partial class MainForm : Form
    //{
    //    // Note: Replace "YOUR_GEMINI_API_KEY_HERE" with your actual key. 
    //    // For production, use Environment.GetEnvironmentVariable("GEMINI_API_KEY") for security.
    //    private const string GeminiApiKey = "AIzaSyBoCT25bQ8HntrNcciBRqZ1uhHlIuN0EdA";
    //    public MainForm()
    //    {
    //        InitializeComponent();
    //        picLoader.Image = Image.FromFile("loader-wait.gif");
    //        // Set default to 5, which is already in designer, but good to ensure
    //        txtRelevantQANum.Text = "5";
    //    }

    //    private void btnBrowse_Click(object sender, EventArgs e)
    //    {
    //        using (OpenFileDialog ofd = new OpenFileDialog())
    //        {
    //            ofd.Filter = "Word Documents (*.docx)|*.docx|All Files (*.*)|*.*";
    //            ofd.Title = "Select CV File";

    //            if (ofd.ShowDialog() == DialogResult.OK)
    //            {
    //                txtCVPath.Text = ofd.FileName;
    //            }
    //        }
    //    }

    //    private async void btnProcess_Click(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            // Validate inputs
    //            if (string.IsNullOrWhiteSpace(txtCompany.Text))
    //            {
    //                MessageBox.Show("Please enter company name.", "Validation Error",
    //                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
    //                txtCompany.Focus();
    //                return;
    //            }

    //            if (string.IsNullOrWhiteSpace(txtPosition.Text))
    //            {
    //                MessageBox.Show("Please enter position.", "Validation Error",
    //                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
    //                txtPosition.Focus();
    //                return;
    //            }

    //            if (string.IsNullOrWhiteSpace(txtKeywords.Text))
    //            {
    //                MessageBox.Show("Please enter keywords.", "Validation Error",
    //                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
    //                txtKeywords.Focus();
    //                return;
    //            }

    //            if (string.IsNullOrWhiteSpace(txtCVPath.Text) || !File.Exists(txtCVPath.Text))
    //            {
    //                MessageBox.Show("Please select a valid CV file.", "Validation Error",
    //                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
    //                return;
    //            }

    //            if (!int.TryParse(txtRelevantQANum.Text, out int qaCount) || qaCount < 1)
    //            {
    //                MessageBox.Show("Please enter a valid number (1 or more) for Q&A to generate.", "Validation Error",
    //                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
    //                txtRelevantQANum.Focus();
    //                return;
    //            }

    //            string keywords = txtKeywords.Text.Trim(); // Get user input

    //            // --- GEMINI INTEGRATION POINT ---
    //            if (!string.IsNullOrWhiteSpace(keywords))
    //            {
    //                // Show loader and status
    //                picLoader.Visible = true;
    //                lblStatus.Text = "Please wait. Work on progress";
    //                lblStatus.ForeColor = System.Drawing.Color.Blue;
    //                Application.DoEvents();

    //                // Combine Company and Position for a good context for the AI
    //                string jobDescriptionInput = $"Company: {txtCompany.Text}, Position: {txtPosition.Text}\n{txtKeywords.Text}";

    //                keywords = await ExtractKeywordsAsync(jobDescriptionInput);

    //                if (keywords.StartsWith("ERROR:"))
    //                {
    //                    // Handle extraction failure or API key error
    //                    picLoader.Visible = false;
    //                    lblStatus.Text = keywords; // Display the error message from the method
    //                    lblStatus.ForeColor = System.Drawing.Color.Red;
    //                    MessageBox.Show(keywords, "Gemini Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //                    return;
    //                }

    //                if (string.IsNullOrWhiteSpace(keywords))
    //                {
    //                    // If the AI returned nothing
    //                    picLoader.Visible = false;
    //                    lblStatus.Text = "Error: Gemini could not extract keywords. Please enter them manually.";
    //                    lblStatus.ForeColor = System.Drawing.Color.Red;
    //                    MessageBox.Show("Keywords extraction failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //                    return;
    //                }

    //                // Populate the textbox with the AI's result for user visibility
    //                txtKeywords.Text = keywords;
    //            }
    //            else
    //            {
    //                // If the AI returned nothing
    //                picLoader.Visible = false;
    //                lblStatus.Text = "Error: Gemini could not extract keywords. Please enter them manually.";
    //                lblStatus.ForeColor = System.Drawing.Color.Red;
    //                MessageBox.Show("Keywords extraction failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //                return;
    //            }
    //            // --- END GEMINI INTEGRATION ---

    //            // Process the CV
    //            bool convertToPdf = chkConvertToPdf.Checked;
    //            ProcessCV(txtCVPath.Text, txtCompany.Text, txtPosition.Text, txtKeywords.Text, convertToPdf);

    //            // Hide loader and update status
    //            picLoader.Visible = false;
    //            lblStatus.Text = "✓ Finished! CV has been customized successfully.\nThe file has been opened.";
    //            lblStatus.ForeColor = System.Drawing.Color.Green;

    //            MessageBox.Show("CV customized successfully!", "Success",
    //                MessageBoxButtons.OK, MessageBoxIcon.Information);
    //        }
    //        catch (Exception ex)
    //        {
    //            picLoader.Visible = false;
    //            lblStatus.Text = $"Error: {ex.Message}";
    //            lblStatus.ForeColor = System.Drawing.Color.Red;
    //            MessageBox.Show($"Error: {ex.Message}", "Error",
    //                MessageBoxButtons.OK, MessageBoxIcon.Error);
    //        }
    //    }

    //    private async Task<string> ExtractKeywordsAsync(string jobDescription)
    //    {
    //        if (string.IsNullOrWhiteSpace(GeminiApiKey) || GeminiApiKey.Equals("YOUR_GEMINI_API_KEY_HERE"))
    //        {
    //            return "ERROR: Gemini API Key not set. Please update MainForm.cs.";
    //        }

    //        try
    //        {
    //            // 1. Initialize the GoogleAI client and select a capable model
    //            var googleAI = new GoogleAI(apiKey: GeminiApiKey);
    //            var model = googleAI.GenerativeModel(model: Model.Gemini25Flash); // gemini-2.5-flash is fast and capable.

    //            // 2. Craft a precise prompt (System Instruction)
    //            var prompt = $@"
    //                You are an expert keyword extraction tool. 
    //                Analyze the following job description to extract the most important technical skills, 
    //                soft skills, and industry terms.

    //                Return the extracted keywords as a single string, with each keyword 
    //                separated **ONLY** by a comma and a single space (e.g., 'C#, .NET, Agile, Scrum Master'). 
    //                Do not include any other text, explanations, or formatting (like bullet points or list numbers).

    //                Job Description to Analyze:
    //                ---
    //                {jobDescription}
    //                ---
    //            ";

    //            // 3. Call the API asynchronously
    //            var response = await model.GenerateContent(prompt);

    //            // 4. Return the cleaned-up result
    //            return response.Text.Trim().Trim('"', '\'').Trim();
    //        }
    //        catch (Exception ex)
    //        {
    //            // Return the error message to be displayed in the status bar
    //            return $"API Exception: {ex.Message}";
    //        }
    //    }

    //    private void ProcessCV(string sourcePath, string company, string position, string keywords, bool convertToPdf)
    //    {
    //        string directory = Path.GetDirectoryName(sourcePath) ?? "";
    //        string baseFileName = Path.GetFileNameWithoutExtension(sourcePath);

    //        string sanitizedCompany = SanitizeFileName(company);
    //        string sanitizedPosition = SanitizeFileName(position);

    //        string newFileName = $"{baseFileName}_{sanitizedCompany}_{sanitizedPosition}.docx";
    //        string newFilePath = Path.Combine(directory, newFileName);

    //        File.Copy(sourcePath, newFilePath, true);

    //        AddInvisibleKeywordsToFooter(newFilePath, keywords);

    //        string outputPath = newFilePath;

    //        if (convertToPdf)
    //        {
    //            string pdfFileName = $"{baseFileName}_{sanitizedCompany}_{sanitizedPosition}.pdf";
    //            outputPath = Path.Combine(directory, pdfFileName);

    //            Document document = new Document();
    //            document.LoadFromFile(newFilePath);
    //            document.SaveToFile(outputPath, FileFormat.PDF);

    //            File.Delete(newFilePath); // Clean up the temporary DOCX file
    //        }

    //        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
    //        {
    //            FileName = outputPath,
    //            UseShellExecute = true
    //        });
    //    }

    //    private string SanitizeFileName(string fileName)
    //    {
    //        char[] invalidChars = Path.GetInvalidFileNameChars();
    //        string sanitized = new string(fileName
    //            .Where(c => !invalidChars.Contains(c))
    //            .ToArray());

    //        return sanitized.Replace(" ", "").ToLower();
    //    }

    //    private void AddInvisibleKeywordsToFooter(string filePath, string keywords)
    //    {
    //        using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
    //        {
    //            MainDocumentPart mainPart = doc.MainDocumentPart
    //                ?? throw new InvalidOperationException("Document has no main part");

    //            var document = mainPart.Document;
    //            var body = document.Body ?? throw new InvalidOperationException("Document body is null");

    //            var sectionPropsList = body.Descendants<SectionProperties>().ToList();

    //            if (sectionPropsList.Count == 0)
    //            {
    //                var sectionProps = new SectionProperties();
    //                body.Append(sectionProps);
    //                sectionPropsList.Add(sectionProps);
    //            }

    //            foreach (var sectionProps in sectionPropsList)
    //            {
    //                FooterReference footerRef = sectionProps.Descendants<FooterReference>()
    //                    .FirstOrDefault(r => r.Type?.Value == HeaderFooterValues.Default);

    //                FooterPart footerPart;
    //                if (footerRef != null)
    //                {
    //                    footerPart = (FooterPart)mainPart.GetPartById(footerRef.Id!);
    //                }
    //                else
    //                {
    //                    footerPart = mainPart.AddNewPart<FooterPart>();
    //                    footerRef = new FooterReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(footerPart) };
    //                    sectionProps.Append(footerRef);
    //                }

    //                Footer footer = footerPart.Footer ?? new Footer();

    //                Paragraph para = new Paragraph();

    //                ParagraphProperties paraProps = new ParagraphProperties();
    //                Justification justification = new Justification() { Val = JustificationValues.Left };
    //                paraProps.Append(justification);
    //                para.Append(paraProps);

    //                Run run = new Run();
    //                RunProperties runProps = new RunProperties();

    //                Color color = new Color() { Val = "FFFFFF" };
    //                runProps.Append(color);

    //                FontSize fontSize = new FontSize() { Val = "2" }; // 1pt font size
    //                runProps.Append(fontSize);

    //                run.Append(runProps);
    //                run.Append(new Text(keywords) { Space = SpaceProcessingModeValues.Preserve });

    //                para.Append(run);
    //                footer.Append(para);

    //                footerPart.Footer = footer;
    //                footerPart.Footer.Save();
    //            }

    //            mainPart.Document.Save();
    //        }
    //    }
    //}
    public partial class MainForm : Form
    {
        // Note: Replace "YOUR_GEMINI_API_KEY_HERE" with your actual key. 
        // For production, use Environment.GetEnvironmentVariable("GEMINI_API_KEY") for security.
        private const string GeminiApiKey = "AIzaSyBoCT25bQ8HntrNcciBRqZ1uhHlIuN0EdA";

        public MainForm()
        {
            InitializeComponent();
            picLoader.Image = Image.FromFile("loader-wait.gif");
            // Set default to 5, which is already in designer, but good to ensure
            txtRelevantQANum.Text = "5";
        }

        /// <summary>
        /// Handles the Click event of the Browse button, allowing the user to select a file.
        /// </summary>
        /// <remarks>Opens a file dialog to allow the user to select a file. The selected file's path is
        /// displayed in the associated text box. The dialog filters files to display Word documents (*.docx) by
        /// default, but the user can choose to view all files.</remarks>
        /// <param name="sender">The source of the event, typically the Browse button.</param>
        /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the click event of the "Process" button, performing a series of operations to customize a CV and
        /// generate supplementary documents.
        /// </summary>
        /// <remarks>This method validates user input, creates an output folder, generates a job
        /// description document, extracts keywords using an AI service, processes the CV, and generates a relevant Q&A
        /// document. The generated files are saved in the output folder, and the folder is opened for the user upon
        /// successful completion.</remarks>
        /// <param name="sender">The source of the event, typically the "Process" button.</param>
        /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
        private async void btnProcess_Click(object sender, EventArgs e)
        {
            // Initial validation and setup
            try
            {
                if (string.IsNullOrWhiteSpace(txtCompany.Text) || string.IsNullOrWhiteSpace(txtPosition.Text) ||
                    string.IsNullOrWhiteSpace(txtKeywords.Text) || string.IsNullOrWhiteSpace(txtCVPath.Text) ||
                    !File.Exists(txtCVPath.Text))
                {
                    MessageBox.Show("Please fill all fields and select a valid CV file.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtRelevantQANum.Text, out int qaCount) || qaCount < 1)
                {
                    MessageBox.Show("Please enter a valid number (1 or more) for Q&A to generate.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRelevantQANum.Focus();
                    return;
                }

                // Show loader and status
                picLoader.Visible = true;
                lblStatus.Text = "Please wait. Work in progress...";
                lblStatus.ForeColor = System.Drawing.Color.Blue;
                Application.DoEvents();

                // ------------------------------------------------------------------
                // 1. Create Folder
                // ------------------------------------------------------------------
                string sanitizedCompany = SanitizeFolderName(txtCompany.Text);
                string sanitizedPosition = SanitizeFolderName(txtPosition.Text);
                string folderName = $"{sanitizedCompany}_{sanitizedPosition}";
                string parentDirectory = Path.GetDirectoryName(txtCVPath.Text) ?? Directory.GetCurrentDirectory();
                string outputFolder = Path.Combine(parentDirectory, folderName);

                // Create the folder if it doesn't exist
                Directory.CreateDirectory(outputFolder);
                lblStatus.Text = "Folder created: " + outputFolder;
                Application.DoEvents();

                string jobDescriptionInput = txtKeywords.Text.Trim();
                string positionText = txtPosition.Text.Trim();
                string companyText = txtCompany.Text.Trim();

                // ------------------------------------------------------------------
                // 2. Create Job Description Document
                // ------------------------------------------------------------------
                // Enforce PDF for JD document
                string jdFileName = $"JobDescription_{sanitizedCompany}_{sanitizedPosition}.pdf"; // Changed extension to .pdf
                string jdFilePath = Path.Combine(outputFolder, jdFileName);

                // Pass 'true' for convertToPdf as per the request to make it PDF
                CreateJobDescriptionDocument(jdFilePath, positionText, companyText, jobDescriptionInput, true);
                lblStatus.Text = $"Job Description document created: {jdFileName}";
                Application.DoEvents();

                // ------------------------------------------------------------------
                // 3. Get Keywords from Gemini and Process CV (Existing Logic)
                // ------------------------------------------------------------------
                string extractedKeywords = await ExtractKeywordsAsync($"Company: {companyText}, Position: {positionText}\n{jobDescriptionInput}");

                if (extractedKeywords.StartsWith("ERROR:"))
                {
                    // Handle extraction failure
                    throw new Exception(extractedKeywords);
                }

                txtKeywords.Text = extractedKeywords; // Update the UI with AI keywords
                lblKeywords.Text = "Keywords:"; // CHANGE 1: Set label to "Keywords"
                lblStatus.Text = "Keywords extracted by Gemini. Processing CV...";
                Application.DoEvents();

                // Get keyword placement choice
                bool writeToFooter = chkWriteToFooter.Checked; // NEW: Get choice

                // The ProcessCV now saves to the new folder
                string finalCVPath = ProcessCV(txtCVPath.Text, companyText, positionText, extractedKeywords, chkConvertToPdf.Checked, outputFolder, writeToFooter); // NEW: Pass choice
                lblStatus.Text = $"CV processed and saved to: {finalCVPath}";
                Application.DoEvents();


                // ------------------------------------------------------------------
                // 4. Generate Relevant Q&A Document
                // ------------------------------------------------------------------
                lblStatus.Text = $"Generating {qaCount} Q&A pairs with Gemini...";
                Application.DoEvents();

                string qaDocumentContent = await GenerateTechnicalQnA(positionText, jobDescriptionInput, qaCount);

                if (qaDocumentContent.StartsWith("ERROR:"))
                {
                    throw new Exception(qaDocumentContent);
                }

                // Enforce PDF for Q&A document
                string qaFileName = $"InterviewQA_{sanitizedCompany}_{sanitizedPosition}.pdf"; // Changed extension to .pdf
                string qaFilePath = Path.Combine(outputFolder, qaFileName);
                // CreateDocumentFromText now handles the DOCX-to-PDF conversion internally
                CreateDocumentFromText(qaFilePath, qaDocumentContent);
                lblStatus.Text = $"Q&A document created: {qaFileName}";
                Application.DoEvents();
                // ------------------------------------------------------------------

                // Final status update
                picLoader.Visible = false;
                lblStatus.Text = "✓ Finished! All files have been created successfully in the folder.";
                lblStatus.ForeColor = System.Drawing.Color.Green;

                // Open the output folder for the user
                System.Diagnostics.Process.Start("explorer.exe", outputFolder);

                MessageBox.Show("CV customized and supplementary documents created successfully!", "Success",
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

        /// <summary>
        /// Sanitizes a folder name by removing invalid characters and replacing spaces and non-alphanumeric characters
        /// with underscores.
        /// </summary>
        /// <remarks>This method ensures that the resulting folder name is valid for use in file system
        /// paths by removing characters that are not allowed in file or folder names. Spaces are removed for
        /// abbreviation, and all characters are converted to lowercase for consistency.</remarks>
        /// <param name="name">The original folder name to be sanitized. Cannot be null.</param>
        /// <returns>A sanitized version of the folder name, with invalid characters replaced by underscores and converted to
        /// lowercase.</returns>
        private string SanitizeFolderName(string name)
        {
            // Remove invalid path characters and replace spaces/non-alphanumeric with underscore
            string invalidChars = Regex.Escape(new string(Path.GetInvalidPathChars()) + new string(Path.GetInvalidFileNameChars()));
            string regex = string.Format(@"[{0} ]", invalidChars);

            // Abbreviate and sanitize
            string abbreviated = name.Replace(" ", ""); // Removes spaces for abbreviation effect

            return Regex.Replace(abbreviated, regex, "_").ToLowerInvariant();
        }

        /// <summary>
        /// Creates a job description document in DOCX format and optionally converts it to a PDF.
        /// </summary>
        /// <remarks>The method generates a temporary DOCX file, formats the content with the specified
        /// position, company name, and job description, and saves it to the specified output path. If <paramref
        /// name="convertToPdf"/> is <see langword="true"/>, the DOCX file is converted to a PDF and then
        /// deleted.</remarks>
        /// <param name="outputPath">The file path where the final document will be saved. If <paramref name="convertToPdf"/> is <see
        /// langword="true"/>, this will be the PDF file path.</param>
        /// <param name="position">The job position title to include in the document. This will be displayed in bold.</param>
        /// <param name="company">The name of the company to include in the document.</param>
        /// <param name="jobDescription">The detailed job description to include in the document. Line breaks will be preserved.</param>
        /// <param name="convertToPdf">A value indicating whether the generated DOCX file should be converted to a PDF. If <see langword="true"/>,
        /// the DOCX file will be deleted after conversion.</param>
        private void CreateJobDescriptionDocument(string outputPath, string position, string company, string jobDescription, bool convertToPdf)
        {
            // The outputPath will be the PDF path. We determine the temporary DOCX path.
            string docxPath = Path.Combine(Path.GetDirectoryName(outputPath),
                Path.GetFileNameWithoutExtension(outputPath) + ".docx");

            using (WordprocessingDocument doc = WordprocessingDocument.Create(docxPath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                // **FIX 1: Explicitly use OpenXML's Body**
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(new DocumentFormat.OpenXml.Wordprocessing.Body());

                // 1. Position - Bold
                Paragraph pPosition = new Paragraph(new Run(
                    new RunProperties(new Bold()),
                    new Text(position) { Space = SpaceProcessingModeValues.Preserve }));
                mainPart.Document.Body.Append(pPosition);

                // 2. Company Name
                Paragraph pCompany = new Paragraph(new Run(new Text(company) { Space = SpaceProcessingModeValues.Preserve }));
                mainPart.Document.Body.Append(pCompany);

                // 3. Two New Lines (Empty Paragraphs)
                mainPart.Document.Body.Append(new Paragraph());
                mainPart.Document.Body.Append(new Paragraph());

                // 4. Job Description
                // Replace common line breaks with Word breaks
                string formattedJD = jobDescription.Replace("\r\n", "\v").Replace("\n", "\v").Replace("\r", "\v");

                Paragraph pJD = new Paragraph();
                foreach (string line in formattedJD.Split('\v'))
                {
                    pJD.Append(new Run(new Text(line) { Space = SpaceProcessingModeValues.Preserve }));

                    // **FIX 2: Explicitly use OpenXML's Break**
                    pJD.Append(new Run(new DocumentFormat.OpenXml.Wordprocessing.Break()));
                }
                mainPart.Document.Body.Append(pJD);

                mainPart.Document.Save();
            }

            // Always convert to PDF and clean up the DOCX for this document
            ConvertDocxToPdf(docxPath, outputPath);
            File.Delete(docxPath);
        }

        /// <summary>
        /// Creates a PDF document from the specified text content and saves it to the specified output path.
        /// </summary>
        /// <remarks>This method generates a temporary Word document (.docx) from the provided text
        /// content, converts it to a PDF, and then deletes the temporary Word document. Text lines ending with a
        /// question mark or starting with "Q:" are formatted in bold.</remarks>
        /// <param name="outputPath">The file path where the resulting PDF document will be saved. The path must include the file name and the
        /// ".pdf" extension.</param>
        /// <param name="content">The text content to be included in the document. Each line of text will be added as a separate paragraph.</param>
        private void CreateDocumentFromText(string outputPath, string content)
        {
            // The outputPath will be the PDF path. We determine the temporary DOCX path.
            string docxPath = Path.Combine(Path.GetDirectoryName(outputPath),
                Path.GetFileNameWithoutExtension(outputPath) + ".docx");

            using (WordprocessingDocument doc = WordprocessingDocument.Create(docxPath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                // **FIX 1: Explicitly use OpenXML's Body**
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(new DocumentFormat.OpenXml.Wordprocessing.Body());

                string[] lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    Paragraph p = new Paragraph();
                    Run r = new Run();
                    r.Append(new Text(line) { Space = SpaceProcessingModeValues.Preserve });
                    p.Append(r);

                    // **FIX 2: Explicitly use OpenXML's Break**
                    p.Append(new Run(new DocumentFormat.OpenXml.Wordprocessing.Break()));

                    // Simple heuristic for bolding (e.g., questions)
                    if (line.Trim().EndsWith("?") || line.Trim().StartsWith("Q:"))
                    {
                        r.RunProperties = new RunProperties(new Bold());
                    }

                    mainPart.Document.Body.Append(p);
                }

                mainPart.Document.Save();
            }

            // Always convert to PDF and clean up the DOCX for this document
            string pdfPath = outputPath.Replace(".docx", ".pdf");
            ConvertDocxToPdf(docxPath, pdfPath);
            File.Delete(docxPath);
        }

        /// <summary>
        /// Converts a Word document (.docx) to a PDF file.
        /// </summary>
        /// <remarks>This method loads the specified Word document and saves it as a PDF file. Ensure that
        /// the input file path is valid and accessible.</remarks>
        /// <param name="docxPath">The full file path of the source Word document. The file must exist and be in .docx format.</param>
        /// <param name="pdfPath">The full file path where the resulting PDF file will be saved. If the file already exists, it will be
        /// overwritten.</param>
        private void ConvertDocxToPdf(string docxPath, string pdfPath)
        {
            Document document = new Document();
            document.LoadFromFile(docxPath);
            document.SaveToFile(pdfPath, FileFormat.PDF);
        }

        /// <summary>
        /// Processes a CV file by appending metadata, optionally converting it to PDF, and saving it to a specified
        /// output folder.
        /// </summary>
        /// <remarks>This method modifies the file name of the CV to include the specified company and
        /// position, and optionally embeds keywords as invisible metadata. If <paramref name="convertToPdf"/> is <see
        /// langword="true"/>, the method converts the modified DOCX file to PDF format and deletes the intermediate
        /// DOCX file.</remarks>
        /// <param name="sourcePath">The full path to the source CV file to be processed. The file must be in DOCX format.</param>
        /// <param name="company">The name of the company to include in the processed file name.</param>
        /// <param name="position">The name of the position to include in the processed file name.</param>
        /// <param name="keywords">A string of keywords to embed in the CV file for metadata purposes.</param>
        /// <param name="convertToPdf">A value indicating whether the processed CV file should be converted to PDF format. If <see
        /// langword="true"/>, the DOCX file will be converted to PDF and the original DOCX file will be deleted.</param>
        /// <param name="outputFolder">The folder where the processed file will be saved. The folder must exist prior to calling this method.</param>
        /// <param name="writeToFooter">A value indicating whether the keywords should be embedded in the footer of the CV file. If <see
        /// langword="false"/>, the keywords will be embedded on the last page of the document.</param>
        /// <returns>The full path to the processed file. If <paramref name="convertToPdf"/> is <see langword="true"/>, the path
        /// will point to the generated PDF file; otherwise, it will point to the modified DOCX file.</returns>
        private string ProcessCV(string sourcePath, string company, string position, string keywords, bool convertToPdf, string outputFolder, bool writeToFooter)
        {
            string baseFileName = Path.GetFileNameWithoutExtension(sourcePath);
            string sanitizedCompany = SanitizeFileName(company);
            string sanitizedPosition = SanitizeFileName(position);

            string newBaseName = $"{baseFileName}_{sanitizedCompany}_{sanitizedPosition}";
            string newDocxFileName = $"{newBaseName}.docx";
            string newFilePath = Path.Combine(outputFolder, newDocxFileName); // Save to the new folder

            File.Copy(sourcePath, newFilePath, true);

            if (writeToFooter)
            {
                AddInvisibleKeywordsToFooter(newFilePath, keywords);
            }
            else
            {
                AddInvisibleKeywordsToLastPage(newFilePath, keywords); // NEW: Call new function
            }

            string outputPath = newFilePath;

            if (convertToPdf)
            {
                string pdfFileName = $"{newBaseName}.pdf";
                outputPath = Path.Combine(outputFolder, pdfFileName); // Save PDF to the new folder

                ConvertDocxToPdf(newFilePath, outputPath);

                File.Delete(newFilePath); // Clean up the temporary DOCX file
            }

            return outputPath;
        }

        /// <summary>
        /// Extracts keywords from the provided job description, including technical skills, soft skills, and industry
        /// terms.
        /// </summary>
        /// <remarks>This method uses the Gemini API to analyze the job description and extract relevant
        /// keywords. Ensure that the <c>GeminiApiKey</c> is properly configured before calling this method.</remarks>
        /// <param name="jobDescription">The job description to analyze. This parameter must not be null, empty, or consist solely of whitespace.</param>
        /// <returns>A comma-separated string of extracted keywords (e.g., "C#, .NET, Agile, Scrum Master"). If the Gemini API
        /// key is not set, or an exception occurs during processing, an error message is returned.</returns>
        private async Task<string> ExtractKeywordsAsync(string jobDescription)
        {
            if (string.IsNullOrWhiteSpace(GeminiApiKey) || GeminiApiKey.Equals("YOUR_GEMINI_API_KEY_HERE"))
            {
                return "ERROR: Gemini API Key not set. Please update MainForm.cs.";
            }

            try
            {
                var googleAI = new GoogleAI(apiKey: GeminiApiKey);
                var model = googleAI.GenerativeModel(model: Model.Gemini25Flash);

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

                var response = await model.GenerateContent(prompt);
                return response.Text.Trim().Trim('"', '\'').Trim();
            }
            catch (Exception ex)
            {
                return $"ERROR: API Exception during keyword extraction: {ex.Message}";
            }
        }

        /// <summary>
        /// Generates a list of technical interview questions and their concise answers based on the provided job
        /// position and job description.
        /// </summary>
        /// <remarks>This method uses the Gemini API to generate content based on the provided input.
        /// Ensure that the API key is correctly configured before calling this method. The output is formatted as a
        /// simple list of questions and answers, with each pair separated by two empty lines.</remarks>
        /// <param name="position">The title of the job position for which the technical questions are to be generated.</param>
        /// <param name="jobDescription">A detailed description of the job, including responsibilities, required skills, and qualifications.</param>
        /// <param name="count">The number of question-and-answer pairs to generate.</param>
        /// <returns>A string containing the generated technical interview questions and answers formatted as a Q&A list. If the
        /// Gemini API key is not set, or an exception occurs during the API call, an error message is returned.</returns>
        private async Task<string> GenerateTechnicalQnA(string position, string jobDescription, int count)
        {
            if (string.IsNullOrWhiteSpace(GeminiApiKey) || GeminiApiKey.Equals("YOUR_GEMINI_API_KEY_HERE"))
            {
                return "ERROR: Gemini API Key not set. Please update MainForm.cs.";
            }

            try
            {
                var googleAI = new GoogleAI(apiKey: GeminiApiKey);
                var model = googleAI.GenerativeModel(model: Model.Gemini25Pro); // Use a more capable model for Q&A

                var prompt = $@"
                    You are an expert technical interviewer assistant.
                    Based on the following job position and job description, generate {count} highly relevant technical interview questions and their concise answers. 
                    Format the output as a simple question-and-answer list, using 'Q:' for the question and 'A:' for the answer.
                    Separate each Q&A pair with two empty lines.

                    **Example Format:**
                    Q: What is a deadlock in multithreading and how do you prevent it?
                    A: A deadlock is a state where two or more threads are waiting for each other to release resources, leading to a standstill. Prevention methods include ordered resource acquisition, timeout locks, and avoiding nested locks.

                    Job Position: {position}
                    Job Description:
                    ---
                    {jobDescription}
                    ---
                ";

                var response = await model.GenerateContent(prompt);
                return response.Text.Trim();
            }
            catch (Exception ex)
            {
                return $"ERROR: API Exception during Q&A generation: {ex.Message}";
            }
        }


        // ------------------------------------------------------------------
        // Remaining Original Methods (SanitizeFileName, AddInvisibleKeywordsToFooter)
        // ------------------------------------------------------------------

        /// <summary>
        /// Removes invalid characters from the specified file name and converts it to lowercase.
        /// </summary>
        /// <param name="fileName">The file name to sanitize. Cannot be null.</param>
        /// <returns>A sanitized version of the file name with invalid characters removed, spaces removed, and all characters
        /// converted to lowercase.</returns>
        private string SanitizeFileName(string fileName)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            string sanitized = new string(fileName
                .Where(c => !invalidChars.Contains(c))
                .ToArray());

            return sanitized.Replace(" ", "").ToLower();
        }

        /// <summary>
        /// Adds invisible keywords to the footer of a Word document.
        /// </summary>
        /// <remarks>This method modifies the footer of the specified Word document by appending the
        /// provided keywords as invisible text. The text is styled with a white font color and a very small font size,
        /// making it effectively invisible in the document. If the document does not already contain a footer, a new
        /// footer is created. The method ensures that the keywords are not duplicated if the method is called multiple
        /// times on the same document.</remarks>
        /// <param name="filePath">The file path of the Word document to modify. The document must exist and be writable.</param>
        /// <param name="keywords">The keywords to add to the footer. These will be rendered as invisible text.</param>
        /// <exception cref="InvalidOperationException">Thrown if the document does not have a main part or body, or if an error occurs while accessing the footer.</exception>
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

                    // Clear existing content to prevent duplication on multiple runs
                    footer.RemoveAllChildren();

                    Paragraph para = new Paragraph();

                    ParagraphProperties paraProps = new ParagraphProperties();
                    Justification justification = new Justification() { Val = JustificationValues.Left };
                    paraProps.Append(justification);
                    para.Append(paraProps);

                    Run run = new Run();
                    RunProperties runProps = new RunProperties();

                    // White text on white background (effectively invisible)
                    Color color = new Color() { Val = "FFFFFF" };
                    runProps.Append(color);

                    // Very small font size
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

        /// <summary>
        /// Adds a paragraph containing hidden keywords to the last page of a Word document.
        /// </summary>
        /// <remarks>This method appends a paragraph with the specified keywords to the end of the
        /// document. The text is styled to be nearly invisible by setting the font color to white and the font size to
        /// 1pt. If the document does not have a main part or body, an <see cref="InvalidOperationException"/> is
        /// thrown.</remarks>
        /// <param name="filePath">The file path of the Word document to modify. Must be a valid, writable file path.</param>
        /// <param name="keywords">The keywords to insert as hidden text. Cannot be null or empty.</param>
        /// <exception cref="InvalidOperationException">Thrown if the document has no main part or the document body is null.</exception>
        private void AddInvisibleKeywordsToLastPage(string filePath, string keywords)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
            {
                MainDocumentPart mainPart = doc.MainDocumentPart
                    ?? throw new InvalidOperationException("Document has no main part");

                var body = mainPart.Document.Body ?? throw new InvalidOperationException("Document body is null");

                // 1. Force a new page (Optional, but ensures keywords are isolated)
                // You might choose to skip this if you don't want a truly 'blank' page
                // body.Append(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));

                // 2. Create the hidden text paragraph
                Paragraph para = new Paragraph();

                // Style the paragraph to be at the bottom (optional, but good practice)
                ParagraphProperties paraProps = new ParagraphProperties();
                Justification justification = new Justification() { Val = JustificationValues.Left };
                paraProps.Append(justification);
                para.Append(paraProps);

                Run run = new Run();
                RunProperties runProps = new RunProperties();

                // Set color to white for near invisibility
                Color color = new Color() { Val = "FFFFFF" };
                runProps.Append(color);

                // Set very small font size (1pt)
                FontSize fontSize = new FontSize() { Val = "2" };
                runProps.Append(fontSize);

                run.Append(runProps);
                run.Append(new Text(keywords) { Space = SpaceProcessingModeValues.Preserve });

                para.Append(run);

                // 3. Append the paragraph to the body of the document (last page)
                body.Append(para);

                mainPart.Document.Save();
            }
        }

        /// <summary>
        /// Handles the Click event of the "Clear" button, resetting all input fields and controls to their default
        /// states.
        /// </summary>
        /// <remarks>This method clears all text fields, resets checkboxes to their default values,
        /// updates labels to their initial states, and hides the loader icon. A confirmation message is displayed to
        /// inform the user that the fields have been cleared.</remarks>
        /// <param name="sender">The source of the event, typically the "Clear" button.</param>
        /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCompany.Text = string.Empty;
            txtPosition.Text = string.Empty;
            txtKeywords.Text = string.Empty;
            txtCVPath.Text = string.Empty;
            txtRelevantQANum.Text = "5";
            chkConvertToPdf.Checked = false;
            chkWriteToFooter.Checked = true; // Reset to default (footer)
            lblKeywords.Text = "Job Description:"; // CHANGE 2: Reset label text
            lblStatus.Text = string.Empty;
            picLoader.Visible = false;

            MessageBox.Show("All fields have been cleared.", "Clear", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
