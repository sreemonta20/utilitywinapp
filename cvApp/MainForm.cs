using cvApp.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using Spire.Doc;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions; // For new sanitization
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xceed.Words.NET;
using CheckBox = System.Windows.Forms.CheckBox;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using Document = Spire.Doc.Document;
using Image = System.Drawing.Image;
using StringReplaceTextOptions = Xceed.Document.NET.StringReplaceTextOptions;

namespace cvApp
{

    public partial class MainForm : Form
    {
        #region Variable declaration and constructor initialization
        private readonly IConfiguration _config;
        private readonly AIModel _aiModel;
        private string outputFolder = string.Empty;
        private string sanitizedCompany = string.Empty;
        private string sanitizedPosition = string.Empty;
        private readonly CoverLetter? oCoverLetter;
        public MainForm(IConfiguration config)
        {
            InitializeComponent();
            picLoader.Image = Image.FromFile("loader-wait.gif");
            _config = config;

            /// cv/resume section innitialization
            var modelIndex = Convert.ToInt32(_config["ModelIndex"]);
            var numberOfQA = _config["InitialFAQNumber"];
            List<AIModel>? oModelList = _config.GetSection("AIModels").Get<List<AIModel>>();
            _aiModel = oModelList![modelIndex];
            txtRelevantQANum.Text = numberOfQA;

            /// cover letter section innitialization
            oCoverLetter = _config.GetSection("CoverLetter").Get<CoverLetter>();
            if (oCoverLetter != null)
            {
                string rawAddress = oCoverLetter.AddressTo ?? string.Empty;
                string formattedAddress = rawAddress.Replace(".", Environment.NewLine);
                txtAddressTo.Text = formattedAddress;
                txtSalutation.Text = oCoverLetter.Salutation ?? string.Empty;
                txtJobSource.Text = oCoverLetter.JobSource ?? string.Empty;
                txtJobPosition.Text = oCoverLetter.Position ?? string.Empty;
                txtJobCompanyLoc.Text = oCoverLetter.CompanyLocation ?? string.Empty;
                txtSkills.Text = oCoverLetter.Skills ?? string.Empty;
                chkClientOrg.Checked = oCoverLetter.Organization ?? false;
                chkLetterToPdf.Checked = oCoverLetter.ConvertToPdf ?? false;
            }

        }
        #endregion

        #region All methods
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
        /// Creates and sets the output directory based on sanitized company and position names, ensuring the directory
        /// exists.
        /// </summary>
        private void SetUpOutputDirectory()
        {
            // 1. Sanitize the inputs
            sanitizedCompany = SanitizeFolderName(txtCompany.Text);
            sanitizedPosition = SanitizeFolderName(txtPosition.Text);

            // 2. Build the folder name and path
            string folderName = $"{sanitizedCompany}_{sanitizedPosition}";
            string parentDirectory = Path.GetDirectoryName(txtCVPath.Text) ?? Directory.GetCurrentDirectory();
            outputFolder = Path.Combine(parentDirectory, folderName);

            // 3. Ensure the folder exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }
        }

        /// <summary>
        /// Handles the click event of the "Process" button, performing a series of operations to customize a CV and
        /// generate supplementary documents such as job description, job description related technical question and answer.
        /// </summary>
        /// <remarks>This method validates user input, creates an output folder, generates a job
        /// description document, extracts keywords using an AI service, processes the CV, and generates a relevant FAQ
        /// document. The generated files are saved in the output folder, and the folder is opened for the user upon
        /// successful completion.</remarks>
        /// <param name="sender">The source of the event, typically the "Process" button.</param>
        /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
        private async void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCompany.Text) || string.IsNullOrWhiteSpace(txtPosition.Text) ||
                    string.IsNullOrWhiteSpace(txtJobDescription.Text) || string.IsNullOrWhiteSpace(txtCVPath.Text) ||
                    !File.Exists(txtCVPath.Text))
                {
                    MessageBox.Show("Please fill all fields and select a valid CV file.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                if (chkIsQAGenerate.Checked && !IsPositiveIntAtLeastOne(txtRelevantQANum.Text))
                {
                    MessageBox.Show("Please enter a valid number (1 or more) for Q&A to generate.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRelevantQANum.Focus();
                    return;
                }

                //if (!int.TryParse(txtRelevantQANum.Text, out int qaCount) || qaCount < 1)
                //{
                //    MessageBox.Show("Please enter a valid number (1 or more) for Q&A to generate.", "Validation Error",
                //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    txtRelevantQANum.Focus();
                //    return;
                //}

                // 1. Check if the input is a URL then read the JD and paste into txtJobDescription otherwise paste the JD into the txtJobDescription directly.
                string input = txtJobDescription.Text.Trim();
                bool isUrl = Uri.TryCreate(input, UriKind.Absolute, out Uri uriResult)
                             && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (isUrl)
                {
                    picLoader.Visible = true;
                    lblStatus.Text = "Reading job link... Please wait.";
                    lblStatus.ForeColor = System.Drawing.Color.Blue;
                    Application.DoEvents();

                    // Attempt to read the URL using AI service. Here I have used Gemini model. You can use the model based on your need.
                    string scrapedDescription = await ExtractFullJobDescriptionFromUrlAsync(input);

                    if (scrapedDescription.StartsWith("ERROR:"))
                    {
                        picLoader.Visible = false;
                        lblStatus.Text = "Read Failed.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        MessageBox.Show("Job profile or description can't be read. Please copy and paste the description.", "Link Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // SUCCESS: Automatically paste the read description into the field (txtJobDescription.Text) after reading the URL
                    txtJobDescription.Text = scrapedDescription;
                    lblStatus.Text = "Description extracted successfully!";
                    lblStatus.ForeColor = System.Drawing.Color.ForestGreen;
                    Application.DoEvents();
                }

                // Show loader and status
                picLoader.Visible = true;
                lblStatus.Text = "Please wait. Work in progress...";
                lblStatus.ForeColor = System.Drawing.Color.Blue;
                Application.DoEvents();

                //// 2. Create Folder
                //string sanitizedCompany = SanitizeFolderName(txtCompany.Text);
                //string sanitizedPosition = SanitizeFolderName(txtPosition.Text);
                //string folderName = $"{sanitizedCompany}_{sanitizedPosition}";
                //string parentDirectory = Path.GetDirectoryName(txtCVPath.Text) ?? Directory.GetCurrentDirectory();
                //string outputFolder = Path.Combine(parentDirectory, folderName);

                //// Create the folder if it doesn't exist
                //Directory.CreateDirectory(outputFolder);
                //lblStatus.Text = "Folder created: " + outputFolder;
                //Application.DoEvents();

                // 2. Setup Shared Directory (Replaces your previous folder creation block)
                SetUpOutputDirectory();

                picLoader.Visible = true;
                lblStatus.Text = "Folder prepared: " + outputFolder;
                lblStatus.ForeColor = System.Drawing.Color.Blue;
                Application.DoEvents();

                // 3. Create Job Description Document using shared global variables
                string jdFileName = $"JobDescription_{sanitizedCompany}_{sanitizedPosition}.pdf"; // Changed extension to .pdf
                string jdFilePath = Path.Combine(outputFolder, jdFileName);

                string jobDescriptionInput = txtJobDescription.Text.Trim();
                string positionText = txtPosition.Text.Trim();
                string companyText = txtCompany.Text.Trim();

                // Pass 'true' for convertToPdf as per the request to make it PDF
                CreateJobDescriptionDocument(jdFilePath, positionText, companyText, jobDescriptionInput, true);
                lblStatus.Text = $"Job Description document created: {jdFileName}";
                Application.DoEvents();

                // 4. If chkAddSkillToCv is checked then Gemini will read and extract keywords
                bool isAddSkillToDoc = chkAddSkillToCv.Checked; // NEW: Get choice
                string extractedKeywords = string.Empty;
                if (isAddSkillToDoc)
                {
                    // Get Keywords from Gemini (you can use any tool) and Process CV (Existing Logic) if chkisAddSkillToDoc is checked
                    extractedKeywords = await ExtractKeywordsAsync($"Company: {companyText}, Position: {positionText}\n{jobDescriptionInput}");

                    if (extractedKeywords.StartsWith("ERROR:"))
                    {
                        //throw new Exception(extractedKeywords);
                        picLoader.Visible = false;
                        lblStatus.Text = "Keyword extraction failed.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        MessageBox.Show("AI model failed to extract the keyword", "Link Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Update the UI with AI keywords
                    txtJobDescription.Text = extractedKeywords;

                    // CHANGE: Set label to "Keywords"
                    lblJobDescription.Text = "Keywords:";
                    lblStatus.Text = $"Keywords extracted by {_aiModel.ModelName}. Processing CV/Resume...";
                    Application.DoEvents();
                }

                // The ProcessCV now saves to the new folder
                string finalCVPath = ProcessCV(txtCVPath.Text, companyText, positionText, extractedKeywords, chkConvertToPdf.Checked, outputFolder, isAddSkillToDoc); // NEW: Pass choice
                lblStatus.Text = $"CV processed and saved to: {finalCVPath}";
                Application.DoEvents();

                // 5. Generate Relevant Q&A Document
                bool isGenerateQA = chkIsQAGenerate.Checked;
                if (isGenerateQA)
                {
                    lblStatus.Text = $"Generating {Convert.ToInt32(txtRelevantQANum.Text)} Q&A pairs with {_aiModel.ModelName}...";
                    Application.DoEvents();

                    string qaDocumentContent = await GenerateTechnicalQnA(positionText, jobDescriptionInput, Convert.ToInt32(txtRelevantQANum.Text));

                    if (qaDocumentContent.StartsWith("ERROR:"))
                    {
                        //throw new Exception(qaDocumentContent);
                        picLoader.Visible = false;
                        lblStatus.Text = "Generation technical QA failed";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        MessageBox.Show("AI model failed to generate the technical QA", "Link Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Enforce PDF for Q&A document
                    // Changed extension to .pdf
                    string qaFileName = $"InterviewQA_{sanitizedCompany}_{sanitizedPosition}.pdf";
                    string qaFilePath = Path.Combine(outputFolder, qaFileName);

                    // CreateDocumentFromText now handles the DOCX-to-PDF conversion internally
                    CreateDocumentFromText(qaFilePath, qaDocumentContent);
                    lblStatus.Text = $"Q&A document created: {qaFileName}";
                    Application.DoEvents();
                }

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
                // Explicitly use OpenXML's Body**
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(new DocumentFormat.OpenXml.Wordprocessing.Body());

                // Position - Bold
                Paragraph pPosition = new Paragraph(new Run(
                    new RunProperties(new Bold()),
                    new Text(position) { Space = SpaceProcessingModeValues.Preserve }));
                mainPart.Document.Body.Append(pPosition);

                // Company Name
                Paragraph pCompany = new Paragraph(new Run(new Text(company) { Space = SpaceProcessingModeValues.Preserve }));
                mainPart.Document.Body.Append(pCompany);

                // Two New Lines (Empty Paragraphs)
                mainPart.Document.Body.Append(new Paragraph());
                mainPart.Document.Body.Append(new Paragraph());

                // Job Description
                // Replace common line breaks with Word breaks
                string formattedJD = jobDescription.Replace("\r\n", "\v").Replace("\n", "\v").Replace("\r", "\v");

                Paragraph pJD = new Paragraph();
                foreach (string line in formattedJD.Split('\v'))
                {
                    pJD.Append(new Run(new Text(line) { Space = SpaceProcessingModeValues.Preserve }));

                    // Explicitly use OpenXML's Break**
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
        /// Processes a CV file by appending metadata or parameter values, optionally converting it to PDF, and saving it to a specified
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
        /// <param name="isAddSkillToDoc">A value indicating whether the keywords should be embedded in the footer of the CV file. If <see
        /// langword="false"/>, the keywords will be embedded on the last page of the document.</param>
        /// <returns>The full path to the processed file. If <paramref name="convertToPdf"/> is <see langword="true"/>, the path
        /// will point to the generated PDF file; otherwise, it will point to the modified DOCX file.</returns>
        private string ProcessCV(string sourcePath, string company, string position, string keywords, bool convertToPdf, string outputFolder, bool isAddSkillToDoc)
        {
            string baseFileName = Path.GetFileNameWithoutExtension(sourcePath);
            string sanitizedCompany = SanitizeFileName(company);
            string sanitizedPosition = SanitizeFileName(position);

            string newBaseName = $"{baseFileName}_{sanitizedCompany}_{sanitizedPosition}";
            string newDocxFileName = $"{newBaseName}.docx";
            string newFilePath = Path.Combine(outputFolder, newDocxFileName); // Save to the new folder

            File.Copy(sourcePath, newFilePath, true);

            if (isAddSkillToDoc)
            {
                //AddInvisibleKeywordsToFooter(newFilePath, keywords);
                AddInvisibleKeywordsToLastPage(newFilePath, keywords);
            }

            string outputPath = newFilePath;

            if (convertToPdf)
            {
                string pdfFileName = $"{newBaseName}.pdf";

                // Save PDF to the new folder
                outputPath = Path.Combine(outputFolder, pdfFileName);

                ConvertDocxToPdf(newFilePath, outputPath);

                // Clean up the temporary DOCX file
                File.Delete(newFilePath);
            }

            return outputPath;
        }

        /// <summary>
        /// Uses Gemini's URL context capabilities to read a webpage and return only the job description text.
        /// </summary>
        private async Task<string> ExtractFullJobDescriptionFromUrlAsync(string url)
        {
            try
            {
                var googleAI = new GoogleAI(apiKey: _aiModel.ApiKey);
                // Using Flash for faster web reading
                var model = googleAI.GenerativeModel(model: _aiModel.ModelName);

                var prompt = $@"
                    Access this URL: {url}
                    Your task: Extract the full Job Description, Responsibilities, and Requirements.
            
                    Strict Rules:
                    1. If you encounter a login wall (like LinkedIn login), or if you cannot access the page content, respond ONLY with the word 'FAILED'.
                    2. If successful, return ONLY the plain text of the job description. Do not add any greetings or explanations.
                ";

                var response = await model.GenerateContent(prompt);
                string result = response.Text.Trim();

                if (result.Contains("FAILED") || string.IsNullOrWhiteSpace(result))
                {
                    return "ERROR: Manual paste required.";
                }

                return result;
            }
            catch (Exception)
            {
                return "ERROR: Link inaccessible.";
            }
        }

        /// <summary>
        /// Extracts keywords from the provided job description, including technical skills, soft skills, and industry 
        /// in case if Add skill to cv is checked
        /// terms.
        /// </summary>
        /// <remarks>This method uses the Gemini API to analyze the job description and extract relevant
        /// keywords. Ensure that the <c>GeminiApiKey</c> is properly configured before calling this method.</remarks>
        /// <param name="jobDescription">The job description to analyze. This parameter must not be null, empty, or consist solely of whitespace.</param>
        /// <returns>A comma-separated string of extracted keywords (e.g., "C#, .NET, Agile, Scrum Master"). If the Gemini API
        /// key is not set, or an exception occurs during processing, an error message is returned.</returns>
        private async Task<string> ExtractKeywordsAsync(string jobDescription)
        {
            if (string.IsNullOrWhiteSpace(_aiModel.ApiKey))
            {
                return $"ERROR: {_aiModel.ModelName} API Key not set. Please update MainForm.cs.";
            }

            try
            {
                var googleAI = new GoogleAI(apiKey: _aiModel.ApiKey);
                var model = googleAI.GenerativeModel(model: _aiModel.ModelName);

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
            if (string.IsNullOrWhiteSpace(_aiModel.ApiKey))
            {
                return $"ERROR: {_aiModel.ModelName} API Key not set. Please update MainForm.cs.";
            }

            try
            {
                var googleAI = new GoogleAI(apiKey: _aiModel.ApiKey);
                var model = googleAI.GenerativeModel(model: _aiModel.ModelName); // Use a more capable model for Q&A

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
            txtJobDescription.Text = string.Empty;
            txtCVPath.Text = string.Empty;
            chkConvertToPdf.Checked = false;
            chkAddSkillToCv.Checked = true; // Reset to default (footer)
            chkIsQAGenerate.Checked = false;
            lblRelevantQANum.Visible = false;
            txtRelevantQANum.Visible = false;
            txtRelevantQANum.Text = "";
            lblJobDescription.Text = "Job Description:"; // CHANGE 2: Reset label text
            lblStatus.Text = string.Empty;
            picLoader.Visible = false;

            MessageBox.Show("All fields have been cleared.", "Clear", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void chkIsQAGenerate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsQAGenerate.Checked)
            {
                lblRelevantQANum.Visible = true;
                txtRelevantQANum.Visible = true;
                txtRelevantQANum.Text = _config["InitialFAQNumber"];
            }
            else
            {
                lblRelevantQANum.Visible = false;
                txtRelevantQANum.Visible = false;
                txtRelevantQANum.Text = "";
            }
        }
        #endregion

        #region Helper methods
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
        /// Determines whether the specified string represents an integer greater than or equal to one.
        /// </summary>
        /// <param name="text">The string to validate as a positive integer.</param>
        /// <returns>True if the string is a valid integer greater than or equal to one; otherwise, false.</returns>
        private bool IsPositiveIntAtLeastOne(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return int.TryParse(text.Trim(), out var n) && n >= 1;
        }
        #endregion

        private void btnProcCoverLetter_Click(object sender, EventArgs e)
        {
            try
            {
                string templatePath = txtCoverLetterPath.Text; // Path from the Browse button
                if (string.IsNullOrEmpty(templatePath) || !File.Exists(templatePath))
                {
                    MessageBox.Show("Please select a valid template file.");
                    return;
                }

                picLoader.Visible = true;
                lblStatus.Text = "Processing Cover Letter...";
                lblStatus.ForeColor = System.Drawing.Color.Blue;
                Application.DoEvents();

                //// 1. Define Output Path (Folder where CV/Job description resides)
                //string outputFolder = Path.GetDirectoryName(txtCVPath.Text); // Assuming txtCVPath holds the target folder
                //string fileName = $"Cover_Letter_{DateTime.Now:yyyyMMdd_HHmmss}.docx";
                //string targetFilePath = Path.Combine(outputFolder, fileName);

                // 1. Ensure the shared folder is ready
                SetUpOutputDirectory();

                //// 2. Copy Template to the target folder
                //File.Copy(templatePath, targetFilePath, true);

                // 2. Define target file path
                string fileName = $"Cover_Letter_{sanitizedCompany}_{sanitizedPosition}.docx";
                string targetFilePath = Path.Combine(outputFolder, fileName);

                // 3. Copy Template to the target folder
                File.Copy(templatePath, targetFilePath, true);

                // 4. Perform Search and Replace using a Library (e.g., Xceed.Words.NET / DocX)
                using (var document = DocX.Load(targetFilePath))
                {
                    document.ReplaceText(new StringReplaceTextOptions
                    {
                        SearchValue = "{Date}",
                        NewValue = dtpDate.Value.ToString("MMMM dd, yyyy")
                    });
                    document.ReplaceText(new StringReplaceTextOptions
                    {
                        SearchValue = "{AddressTo}",
                        NewValue = txtAddressTo.Text
                    });
                    document.ReplaceText(new StringReplaceTextOptions
                    {
                        SearchValue = "{Salutation}",
                        NewValue = txtSalutation.Text
                    });
                    document.ReplaceText(new StringReplaceTextOptions
                    {
                        SearchValue = "{JobSource}",
                        NewValue = txtJobSource.Text
                    });
                    document.ReplaceText(new StringReplaceTextOptions
                    {
                        SearchValue = "{JobPosition}",
                        NewValue = txtJobPosition.Text
                    });
                    document.ReplaceText(new StringReplaceTextOptions
                    {
                        SearchValue = "{JobCompanyLoc}",
                        NewValue = txtJobCompanyLoc.Text
                    });
                    document.ReplaceText(new StringReplaceTextOptions
                    {
                        SearchValue = "{Skills}",
                        NewValue = txtSkills.Text
                    });

                    // Conditional replacement for ClientOrg
                    string orgText = chkClientOrg.Checked ? "organization" : "client";
                    document.ReplaceText(new StringReplaceTextOptions
                    {
                        SearchValue = "{ClientOrg}",
                        NewValue = orgText
                    });

                    document.Save();
                }

                // 5. Handle PDF Conversion if checked
                if (chkLetterToPdf.Checked)
                {
                    string pdfPath = Path.ChangeExtension(targetFilePath, ".pdf");
                    ConvertDocxToPdf(targetFilePath, pdfPath);
                    // Optionally delete the docx after conversion
                    // File.Delete(targetFilePath); 
                }

                picLoader.Visible = false;
                lblStatus.Text = "✓ Creating Cover Letter Finished!";
                lblStatus.ForeColor = System.Drawing.Color.Green;
                System.Diagnostics.Process.Start("explorer.exe", outputFolder);

                MessageBox.Show("Cover letter created successfully!", "Success",
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

        private void btnBrowseCoverLetter_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Word Documents (*.docx)|*.docx|All Files (*.*)|*.*";
                ofd.Title = "Select Cover Letter Template";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtCoverLetterPath.Text = ofd.FileName;
                }
            }
        }

        private void btnLetterClear_Click(object sender, EventArgs e)
        {
            // Reset global variables
            outputFolder = string.Empty;
            sanitizedCompany = string.Empty;
            sanitizedPosition = string.Empty;

            if (oCoverLetter != null)
            {
                string rawAddress = oCoverLetter.AddressTo ?? string.Empty;
                string formattedAddress = rawAddress.Replace(".", Environment.NewLine);
                txtAddressTo.Text = formattedAddress;
                txtSalutation.Text = oCoverLetter.Salutation ?? string.Empty;
                txtJobSource.Text = oCoverLetter.JobSource ?? string.Empty;
                txtJobPosition.Text = oCoverLetter.Position ?? string.Empty;
                txtJobCompanyLoc.Text = oCoverLetter.CompanyLocation ?? string.Empty;
                txtSkills.Text = oCoverLetter.Skills ?? string.Empty;
                chkClientOrg.Checked = oCoverLetter.Organization ?? false;
                chkLetterToPdf.Checked = oCoverLetter.ConvertToPdf ?? false;
            }
            MessageBox.Show("All fields have been cleared.", "Clear", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
