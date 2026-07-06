using cvApp.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using Spire.Doc;
using System.Text; 
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using Xceed.Words.NET;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using Document = Spire.Doc.Document;
using Image = System.Drawing.Image;
using StringReplaceTextOptions = Xceed.Document.NET.StringReplaceTextOptions;

namespace cvApp
{

    public partial class MainForm : Form
    {
        //#region Variable declaration and constructor initialization
        //private readonly IConfiguration _config;
        //private readonly AIModel _aiModel;
        //private string outputFolder = string.Empty;
        //private string sanitizedCompany = string.Empty;
        //private string sanitizedPosition = string.Empty;
        //private readonly CoverLetter? oCoverLetter;

        //// Used when no company/organization was provided by the user (e.g. recruiter posts that don't name the company)
        //private const string NotMentionedCompanyText = "Not mentioned";
        //private const string NotMentionedFolderToken = "notmentioned";

        //public MainForm(IConfiguration config)
        //{
        //    InitializeComponent();
        //    picLoader.Image = Image.FromFile("loader-wait.gif");
        //    _config = config;

        //    /// cv/resume section innitialization
        //    var modelIndex = Convert.ToInt32(_config["ModelIndex"]);
        //    var numberOfQA = _config["InitialFAQNumber"];
        //    List<AIModel>? oModelList = _config.GetSection("AIModels").Get<List<AIModel>>();
        //    _aiModel = oModelList![modelIndex];
        //    txtRelevantQANum.Text = numberOfQA;

        //    // Default checkbox states: Missing Skills and Fully AI Generated Cover Letter are ON by default
        //    chkMissingSkills.Checked = true;
        //    chkAIGenCoverLetter.Checked = true;

        //    // Job Description input toggle: off by default (typed text active, JD PDF controls disabled)
        //    chkJobDescPdf.CheckedChanged += chkJobDescPdf_CheckedChanged;
        //    chkJobDescPdf.Checked = false;
        //    chkJobDescPdf_CheckedChanged(chkJobDescPdf, EventArgs.Empty); // apply the initial enabled/disabled state

        //    /// cover letter section innitialization
        //    oCoverLetter = _config.GetSection("CoverLetter").Get<CoverLetter>();
        //    if (oCoverLetter != null)
        //    {
        //        string rawAddress = oCoverLetter.AddressTo ?? "Hiring Manager";
        //        string formattedAddress = rawAddress.Replace(".", Environment.NewLine);
        //        txtAddressTo.Text = formattedAddress;
        //        txtSalutation.Text = oCoverLetter.Salutation ?? string.Empty;
        //        txtJobSource.Text = oCoverLetter.JobSource ?? string.Empty;
        //        txtJobPosition.Text = oCoverLetter.Position ?? string.Empty;
        //        // Company/Location is left blank by default; only used if the user explicitly types a location
        //        txtJobCompanyLoc.Text = string.Empty;
        //        txtSkills.Text = oCoverLetter.Skills ?? string.Empty;
        //        chkClientOrg.Checked = oCoverLetter.Organization ?? false;
        //        chkLetterToPdf.Checked = oCoverLetter.ConvertToPdf ?? false;
        //    }

        //}
        //#endregion

        //#region All methods
        ///// <summary>
        ///// Handles the Click event of the Browse button, allowing the user to select a file.
        ///// </summary>
        //private void btnBrowse_Click(object sender, EventArgs e)
        //{
        //    using (OpenFileDialog ofd = new OpenFileDialog())
        //    {
        //        ofd.Filter = "Word Documents (*.docx)|*.docx|All Files (*.*)|*.*";
        //        ofd.Title = "Select CV File";

        //        if (ofd.ShowDialog() == DialogResult.OK)
        //        {
        //            txtCVPath.Text = ofd.FileName;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Handles the Click event of the JD Pdf Browse button, allowing the user to point to a
        ///// job description PDF instead of pasting the text directly.
        ///// </summary>
        //private void btnJdPdfBrowse_Click(object sender, EventArgs e)
        //{
        //    using (OpenFileDialog ofd = new OpenFileDialog())
        //    {
        //        ofd.Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
        //        ofd.Title = "Select Job Description PDF";

        //        if (ofd.ShowDialog() == DialogResult.OK)
        //        {
        //            txtJdPdf.Text = ofd.FileName;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Creates and sets the output directory based on sanitized company and position names, ensuring the directory
        ///// exists.
        ///// </summary>
        //private void SetUpOutputDirectory()
        //{
        //    // 1. Sanitize the inputs
        //    sanitizedCompany = SanitizeFolderName(txtCompany.Text);
        //    if (string.IsNullOrWhiteSpace(sanitizedCompany))
        //    {
        //        sanitizedCompany = NotMentionedFolderToken;
        //    }
        //    sanitizedPosition = SanitizeFolderName(txtPosition.Text);

        //    // 2. Build the folder name and path
        //    string folderName = $"{sanitizedCompany}_{sanitizedPosition}";
        //    string parentDirectory = Path.GetDirectoryName(txtCVPath.Text) ?? Directory.GetCurrentDirectory();
        //    outputFolder = Path.Combine(parentDirectory, folderName);

        //    // 3. Ensure the folder exists
        //    if (!Directory.Exists(outputFolder))
        //    {
        //        Directory.CreateDirectory(outputFolder);
        //    }
        //}

        ///// <summary>
        ///// Toggles which Job Description input is active. When "Job Description PDF?" is checked, the JD
        ///// PDF path/browse controls are enabled and the free-text box is disabled - and vice versa. Only
        ///// one source can feed the process at a time.
        ///// </summary>
        //private void chkJobDescPdf_CheckedChanged(object sender, EventArgs e)
        //{
        //    bool usePdf = chkJobDescPdf.Checked;
        //    txtJdPdf.Enabled = usePdf;
        //    btnJdPdfBrowse.Enabled = usePdf;
        //    txtJobDescription.Enabled = !usePdf;
        //}

        ///// <summary>
        ///// Returns the job description text to use for the current operation, based on which source is
        ///// currently active (see chkJobDescPdf). Text mode returns whatever's typed/scraped in the box;
        ///// PDF mode extracts text from the browsed JD PDF.
        ///// </summary>
        //private string GetCurrentJobDescriptionText()
        //{
        //    if (chkJobDescPdf.Checked)
        //    {
        //        if (!string.IsNullOrWhiteSpace(txtJdPdf.Text) && File.Exists(txtJdPdf.Text))
        //        {
        //            return ExtractTextFromPdf(txtJdPdf.Text);
        //        }
        //        return string.Empty;
        //    }

        //    return txtJobDescription.Text.Trim();
        //}

        ///// <summary>
        ///// Handles the click event of the "Process" button, performing a series of operations to customize a CV and
        ///// generate supplementary documents such as job description, job description related technical question and answer.
        ///// </summary>
        //private async void btnProcess_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        bool hasJobDescriptionSource = chkJobDescPdf.Checked
        //            ? (!string.IsNullOrWhiteSpace(txtJdPdf.Text) && File.Exists(txtJdPdf.Text))
        //            : !string.IsNullOrWhiteSpace(txtJobDescription.Text);

        //        if (string.IsNullOrWhiteSpace(txtPosition.Text) || !hasJobDescriptionSource ||
        //            string.IsNullOrWhiteSpace(txtCVPath.Text) || !File.Exists(txtCVPath.Text))
        //        {
        //            MessageBox.Show("Please fill Position, provide a Job Description (typed text or a browsed JD PDF), " +
        //                "and select a valid CV file. Company Name is optional.", "Validation Error",
        //                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }


        //        if (chkIsQAGenerate.Checked && !IsPositiveIntAtLeastOne(txtRelevantQANum.Text))
        //        {
        //            MessageBox.Show("Please enter a valid number (1 or more) for Q&A to generate.", "Validation Error",
        //                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            txtRelevantQANum.Focus();
        //            return;
        //        }

        //        // 1. Check if the input is a URL then read the JD and paste into txtJobDescription otherwise paste the JD into the txtJobDescription directly.
        //        // Only relevant in text mode - when the JD PDF checkbox is on, this box is disabled anyway.
        //        string input = chkJobDescPdf.Checked ? string.Empty : txtJobDescription.Text.Trim();
        //        bool isUrl = !string.IsNullOrWhiteSpace(input) &&
        //                     Uri.TryCreate(input, UriKind.Absolute, out Uri uriResult)
        //                     && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        //        if (isUrl)
        //        {
        //            picLoader.Visible = true;
        //            lblStatus.Text = "Reading job link... Please wait.";
        //            lblStatus.ForeColor = System.Drawing.Color.Blue;
        //            Application.DoEvents();

        //            // Attempt to read the URL using AI service. Here I have used Gemini model. You can use the model based on your need.
        //            string scrapedDescription = await ExtractFullJobDescriptionFromUrlAsync(input);

        //            if (scrapedDescription.StartsWith("ERROR:"))
        //            {
        //                picLoader.Visible = false;
        //                lblStatus.Text = "Read Failed.";
        //                lblStatus.ForeColor = System.Drawing.Color.Red;
        //                MessageBox.Show("Job profile or description can't be read. Please copy and paste the description.", "Link Error",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return;
        //            }

        //            // SUCCESS: Automatically paste the read description into the field (txtJobDescription.Text) after reading the URL
        //            txtJobDescription.Text = scrapedDescription;
        //            lblStatus.Text = "Description extracted successfully!";
        //            lblStatus.ForeColor = System.Drawing.Color.ForestGreen;
        //            Application.DoEvents();
        //        }

        //        // Show loader and status
        //        picLoader.Visible = true;
        //        lblStatus.Text = "Please wait. Work in progress...";
        //        lblStatus.ForeColor = System.Drawing.Color.Blue;
        //        Application.DoEvents();

        //        // 2. Setup Shared Directory
        //        SetUpOutputDirectory();

        //        picLoader.Visible = true;
        //        lblStatus.Text = "Folder prepared: " + outputFolder;
        //        lblStatus.ForeColor = System.Drawing.Color.Blue;
        //        Application.DoEvents();

        //        // 3. Create Job Description Document using shared global variables
        //        string jdFileName = $"JobDescription_{sanitizedCompany}_{sanitizedPosition}.pdf";
        //        string jdFilePath = Path.Combine(outputFolder, jdFileName);

        //        // Resolve the job description content based on which source is active (see chkJobDescPdf).
        //        // This is kept separate from txtJobDescription.Text so the text box's own content is never overwritten below.
        //        if (chkJobDescPdf.Checked)
        //        {
        //            lblStatus.Text = "Extracting text from JD PDF...";
        //            Application.DoEvents();
        //        }
        //        string jobDescriptionInput = GetCurrentJobDescriptionText();

        //        string positionText = txtPosition.Text.Trim();
        //        string companyText = txtCompany.Text.Trim(); // may be empty -> treated as "Not mentioned"

        //        // Pass 'true' for convertToPdf as per the request to make it PDF
        //        CreateJobDescriptionDocument(jdFilePath, positionText, companyText, jobDescriptionInput, true);
        //        lblStatus.Text = $"Job Description document created: {jdFileName}";
        //        Application.DoEvents();

        //        // 4. If chkAddSkillToCv is checked then Gemini will read and extract keywords to embed into the CV/Resume.
        //        // NOTE: unlike before, the extracted keywords are no longer written back into the Job Description text area -
        //        // that box always keeps whatever the user typed/pasted (or the URL-scraped text).
        //        bool isAddSkillToDoc = chkAddSkillToCv.Checked;
        //        string extractedKeywords = string.Empty;
        //        if (isAddSkillToDoc)
        //        {
        //            extractedKeywords = await ExtractKeywordsAsync($"Company: {companyText}, Position: {positionText}\n{jobDescriptionInput}");

        //            if (extractedKeywords.StartsWith("ERROR:"))
        //            {
        //                picLoader.Visible = false;
        //                lblStatus.Text = "Keyword extraction failed.";
        //                lblStatus.ForeColor = System.Drawing.Color.Red;
        //                MessageBox.Show("AI model failed to extract the keyword", "Link Error",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return;
        //            }

        //            lblStatus.Text = $"Keywords extracted by {_aiModel.ModelName}. Processing CV/Resume...";
        //            Application.DoEvents();
        //        }

        //        // The ProcessCV now saves to the new folder
        //        string finalCVPath = ProcessCV(txtCVPath.Text, companyText, positionText, extractedKeywords, chkConvertToPdf.Checked, outputFolder, isAddSkillToDoc);
        //        lblStatus.Text = $"CV processed and saved to: {finalCVPath}";
        //        Application.DoEvents();

        //        // 4b. Missing Skills - independent of "Add skills to the CV/Resume". Compares the resume's
        //        // technical skills against the job description/JD PDF and lists what's missing.
        //        if (chkMissingSkills.Checked)
        //        {
        //            lblStatus.Text = "Analyzing missing skills...";
        //            Application.DoEvents();

        //            string resumeTextForGap = ExtractResumeText(txtCVPath.Text);
        //            string missingSkills = await AnalyzeMissingSkillsAsync(resumeTextForGap, jobDescriptionInput);

        //            if (missingSkills.StartsWith("ERROR:"))
        //            {
        //                lblStatus.Text = "Missing skills analysis failed.";
        //                lblStatus.ForeColor = System.Drawing.Color.Red;
        //                MessageBox.Show("AI model failed to analyze the missing skills.", "Link Error",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //            else
        //            {
        //                txtMissingSkills.Text = missingSkills;
        //            }
        //            Application.DoEvents();
        //        }

        //        // 5. Generate Relevant Q&A Document
        //        bool isGenerateQA = chkIsQAGenerate.Checked;
        //        if (isGenerateQA)
        //        {
        //            lblStatus.Text = $"Generating {Convert.ToInt32(txtRelevantQANum.Text)} Q&A pairs with {_aiModel.ModelName}...";
        //            Application.DoEvents();

        //            string qaDocumentContent = await GenerateTechnicalQnA(positionText, jobDescriptionInput, Convert.ToInt32(txtRelevantQANum.Text));

        //            if (qaDocumentContent.StartsWith("ERROR:"))
        //            {
        //                picLoader.Visible = false;
        //                lblStatus.Text = "Generation technical QA failed";
        //                lblStatus.ForeColor = System.Drawing.Color.Red;
        //                MessageBox.Show("AI model failed to generate the technical QA", "Link Error",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return;
        //            }

        //            string qaFileName = $"InterviewQA_{sanitizedCompany}_{sanitizedPosition}.pdf";
        //            string qaFilePath = Path.Combine(outputFolder, qaFileName);

        //            CreateDocumentFromText(qaFilePath, qaDocumentContent);
        //            lblStatus.Text = $"Q&A document created: {qaFileName}";
        //            Application.DoEvents();
        //        }

        //        // ------------------------------------------------------------------

        //        // Final status update
        //        picLoader.Visible = false;
        //        lblStatus.Text = "✓ Finished! All files have been created successfully in the folder.";
        //        lblStatus.ForeColor = System.Drawing.Color.Green;

        //        // Open the output folder for the user
        //        System.Diagnostics.Process.Start("explorer.exe", outputFolder);

        //        MessageBox.Show("CV customized and supplementary documents created successfully!", "Success",
        //            MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        lblStatus.Text = $"Error: {ex.Message}";
        //        lblStatus.ForeColor = System.Drawing.Color.Red;
        //        MessageBox.Show($"Error: {ex.Message}", "Error",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        picLoader.Visible = false;
        //        picLoader.Refresh();
        //    }
        //}

        ///// <summary>
        ///// Sanitizes a folder name by removing invalid characters and replacing spaces and non-alphanumeric characters
        ///// with underscores.
        ///// </summary>
        //private string SanitizeFolderName(string name)
        //{
        //    // Remove invalid path characters and replace spaces/non-alphanumeric with underscore
        //    string invalidChars = Regex.Escape(new string(Path.GetInvalidPathChars()) + new string(Path.GetInvalidFileNameChars()));
        //    string regex = string.Format(@"[{0} ]", invalidChars);

        //    // Abbreviate and sanitize
        //    string abbreviated = name.Replace(" ", ""); // Removes spaces for abbreviation effect

        //    return Regex.Replace(abbreviated, regex, "_").ToLowerInvariant();
        //}

        ///// <summary>
        ///// Creates a job description document in DOCX format and optionally converts it to a PDF.
        ///// If no company name was provided, "Not mentioned" is written in its place.
        ///// </summary>
        //private void CreateJobDescriptionDocument(string outputPath, string position, string company, string jobDescription, bool convertToPdf)
        //{
        //    string displayCompany = string.IsNullOrWhiteSpace(company) ? NotMentionedCompanyText : company;

        //    // The outputPath will be the PDF path. We determine the temporary DOCX path.
        //    string docxPath = Path.Combine(Path.GetDirectoryName(outputPath),
        //        Path.GetFileNameWithoutExtension(outputPath) + ".docx");

        //    using (WordprocessingDocument doc = WordprocessingDocument.Create(docxPath, WordprocessingDocumentType.Document))
        //    {
        //        MainDocumentPart mainPart = doc.AddMainDocumentPart();
        //        mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(new DocumentFormat.OpenXml.Wordprocessing.Body());

        //        // Position - Bold
        //        Paragraph pPosition = new Paragraph(new Run(
        //            new RunProperties(new Bold()),
        //            new Text(position) { Space = SpaceProcessingModeValues.Preserve }));
        //        mainPart.Document.Body.Append(pPosition);

        //        // Company Name (or "Not mentioned" if left blank)
        //        Paragraph pCompany = new Paragraph(new Run(new Text(displayCompany) { Space = SpaceProcessingModeValues.Preserve }));
        //        mainPart.Document.Body.Append(pCompany);

        //        // Two New Lines (Empty Paragraphs)
        //        mainPart.Document.Body.Append(new Paragraph());
        //        mainPart.Document.Body.Append(new Paragraph());

        //        // Job Description
        //        string formattedJD = jobDescription.Replace("\r\n", "\v").Replace("\n", "\v").Replace("\r", "\v");

        //        Paragraph pJD = new Paragraph();
        //        foreach (string line in formattedJD.Split('\v'))
        //        {
        //            pJD.Append(new Run(new Text(line) { Space = SpaceProcessingModeValues.Preserve }));
        //            pJD.Append(new Run(new DocumentFormat.OpenXml.Wordprocessing.Break()));
        //        }
        //        mainPart.Document.Body.Append(pJD);

        //        mainPart.Document.Save();
        //    }

        //    ConvertDocxToPdf(docxPath, outputPath);
        //    File.Delete(docxPath);
        //}

        ///// <summary>
        ///// Creates a PDF document from the specified text content and saves it to the specified output path.
        ///// </summary>
        //private void CreateDocumentFromText(string outputPath, string content)
        //{
        //    string docxPath = Path.Combine(Path.GetDirectoryName(outputPath),
        //        Path.GetFileNameWithoutExtension(outputPath) + ".docx");

        //    using (WordprocessingDocument doc = WordprocessingDocument.Create(docxPath, WordprocessingDocumentType.Document))
        //    {
        //        MainDocumentPart mainPart = doc.AddMainDocumentPart();
        //        mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(new DocumentFormat.OpenXml.Wordprocessing.Body());

        //        string[] lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        //        foreach (string line in lines)
        //        {
        //            Paragraph p = new Paragraph();
        //            Run r = new Run();
        //            r.Append(new Text(line) { Space = SpaceProcessingModeValues.Preserve });
        //            p.Append(r);

        //            p.Append(new Run(new DocumentFormat.OpenXml.Wordprocessing.Break()));

        //            if (line.Trim().EndsWith("?") || line.Trim().StartsWith("Q:"))
        //            {
        //                r.RunProperties = new RunProperties(new Bold());
        //            }

        //            mainPart.Document.Body.Append(p);
        //        }

        //        mainPart.Document.Save();
        //    }

        //    string pdfPath = outputPath.Replace(".docx", ".pdf");
        //    ConvertDocxToPdf(docxPath, pdfPath);
        //    File.Delete(docxPath);
        //}

        ///// <summary>
        ///// Processes a CV file by appending metadata or parameter values, optionally converting it to PDF, and saving it to a specified
        ///// output folder.
        ///// </summary>
        //private string ProcessCV(string sourcePath, string company, string position, string keywords, bool convertToPdf, string outputFolder, bool isAddSkillToDoc)
        //{
        //    string baseFileName = Path.GetFileNameWithoutExtension(sourcePath);
        //    string sanitizedCompanyLocal = SanitizeFileName(company);
        //    if (string.IsNullOrWhiteSpace(sanitizedCompanyLocal))
        //    {
        //        sanitizedCompanyLocal = NotMentionedFolderToken;
        //    }
        //    string sanitizedPositionLocal = SanitizeFileName(position);

        //    string newBaseName = $"{baseFileName}_{sanitizedCompanyLocal}_{sanitizedPositionLocal}";
        //    string newDocxFileName = $"{newBaseName}.docx";
        //    string newFilePath = Path.Combine(outputFolder, newDocxFileName);

        //    File.Copy(sourcePath, newFilePath, true);

        //    if (isAddSkillToDoc)
        //    {
        //        AddInvisibleKeywordsToLastPage(newFilePath, keywords);
        //    }

        //    string outputPath = newFilePath;

        //    if (convertToPdf)
        //    {
        //        string pdfFileName = $"{newBaseName}.pdf";
        //        outputPath = Path.Combine(outputFolder, pdfFileName);
        //        ConvertDocxToPdf(newFilePath, outputPath);
        //        File.Delete(newFilePath);
        //    }

        //    return outputPath;
        //}

        ///// <summary>
        ///// Uses Gemini's URL context capabilities to read a webpage and return only the job description text.
        ///// </summary>
        //private async Task<string> ExtractFullJobDescriptionFromUrlAsync(string url)
        //{
        //    try
        //    {
        //        var googleAI = new GoogleAI(apiKey: _aiModel.ApiKey);
        //        var model = googleAI.GenerativeModel(model: _aiModel.ModelName);

        //        var prompt = $@"
        //            Access this URL: {url}
        //            Your task: Extract the full Job Description, Responsibilities, and Requirements.

        //            Strict Rules:
        //            1. If you encounter a login wall (like LinkedIn login), or if you cannot access the page content, respond ONLY with the word 'FAILED'.
        //            2. If successful, return ONLY the plain text of the job description. Do not add any greetings or explanations.
        //        ";

        //        var response = await model.GenerateContent(prompt);
        //        string result = response.Text.Trim();

        //        if (result.Contains("FAILED") || string.IsNullOrWhiteSpace(result))
        //        {
        //            return "ERROR: Manual paste required.";
        //        }

        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        return "ERROR: Link inaccessible.";
        //    }
        //}

        ///// <summary>
        ///// Extracts keywords from the provided job description, including technical skills, soft skills, and industry
        ///// terms - used to embed into the CV/Resume when "Add skills to the CV/Resume" is checked.
        ///// </summary>
        //private async Task<string> ExtractKeywordsAsync(string jobDescription)
        //{
        //    if (string.IsNullOrWhiteSpace(_aiModel.ApiKey))
        //    {
        //        return $"ERROR: {_aiModel.ModelName} API Key not set. Please update MainForm.cs.";
        //    }

        //    try
        //    {
        //        var googleAI = new GoogleAI(apiKey: _aiModel.ApiKey);
        //        var model = googleAI.GenerativeModel(model: _aiModel.ModelName);

        //        var prompt = $@"
        //            You are an expert keyword extraction tool. 
        //            Analyze the following job description to extract the most important technical skills, 
        //            soft skills, and industry terms.

        //            Return the extracted keywords as a single string, with each keyword 
        //            separated **ONLY** by a comma and a single space (e.g., 'C#, .NET, Agile, Scrum Master'). 
        //            Do not include any other text, explanations, or formatting (like bullet points or list numbers).

        //            Job Description to Analyze:
        //            ---
        //            {jobDescription}
        //            ---
        //        ";

        //        var response = await model.GenerateContent(prompt);
        //        return response.Text.Trim().Trim('"', '\'').Trim();
        //    }
        //    catch (Exception ex)
        //    {
        //        return $"ERROR: API Exception during keyword extraction: {ex.Message}";
        //    }
        //}

        ///// <summary>
        ///// Compares the candidate's resume against the job description/JD PDF and returns the technical
        ///// skills that appear in the job description but are missing from the resume.
        ///// </summary>
        //private async Task<string> AnalyzeMissingSkillsAsync(string resumeText, string jobDescription)
        //{
        //    if (string.IsNullOrWhiteSpace(_aiModel.ApiKey))
        //    {
        //        return $"ERROR: {_aiModel.ModelName} API Key not set. Please update MainForm.cs.";
        //    }

        //    try
        //    {
        //        var googleAI = new GoogleAI(apiKey: _aiModel.ApiKey);
        //        var model = googleAI.GenerativeModel(model: _aiModel.ModelName);

        //        var prompt = $@"
        //            You are an expert technical recruiter.
        //            Compare the candidate's resume against the job description below and identify ONLY the
        //            technical skills, tools, frameworks, or technologies mentioned in the job description that
        //            are NOT already present anywhere in the candidate's resume (the missing / gap skills).

        //            Return the result as a single comma-separated list only (e.g. 'Kubernetes, GraphQL, Terraform').
        //            If there are no missing skills, return exactly 'None'.
        //            Do not include any explanation, headers, or extra text.

        //            Candidate Resume:
        //            ---
        //            {resumeText}
        //            ---

        //            Job Description:
        //            ---
        //            {jobDescription}
        //            ---
        //        ";

        //        var response = await model.GenerateContent(prompt);
        //        return response.Text.Trim().Trim('"', '\'').Trim();
        //    }
        //    catch (Exception ex)
        //    {
        //        return $"ERROR: API Exception during missing skills analysis: {ex.Message}";
        //    }
        //}

        ///// <summary>
        ///// Uses AI to write the main body paragraphs of a cover letter based on the candidate's resume and
        ///// the job description, tailored to the given position/address/salutation.
        ///// </summary>
        //private async Task<string> GenerateCoverLetterBodyAsync(string resumeText, string jobDescription, string position, string addressTo, string salutation)
        //{
        //    if (string.IsNullOrWhiteSpace(_aiModel.ApiKey))
        //    {
        //        return $"ERROR: {_aiModel.ModelName} API Key not set. Please update MainForm.cs.";
        //    }

        //    try
        //    {
        //        var googleAI = new GoogleAI(apiKey: _aiModel.ApiKey);
        //        var model = googleAI.GenerativeModel(model: _aiModel.ModelName);

        //        var prompt = $@"
        //            You are an expert professional cover letter writer.
        //            Write ONLY the main body of a cover letter (2 to 4 paragraphs) for the position of
        //            '{position}', addressed to '{addressTo}'.

        //            Base the content on the candidate's resume and the job description below, highlighting
        //            the most relevant experience, achievements, and skills that match the job requirements.

        //            Strict rules:
        //            - Do NOT include the salutation ('{salutation}') - it will be added separately.
        //            - Do NOT include a closing line or signature (no 'Sincerely', no name).
        //            - Separate each paragraph with a blank line.
        //            - Return plain text only, no markdown formatting.

        //            Candidate Resume:
        //            ---
        //            {resumeText}
        //            ---

        //            Job Description:
        //            ---
        //            {jobDescription}
        //            ---
        //        ";

        //        var response = await model.GenerateContent(prompt);
        //        return response.Text.Trim();
        //    }
        //    catch (Exception ex)
        //    {
        //        return $"ERROR: API Exception during cover letter generation: {ex.Message}";
        //    }
        //}

        ///// <summary>
        ///// Generates a list of technical interview questions and their concise answers based on the provided job
        ///// position and job description.
        ///// </summary>
        //private async Task<string> GenerateTechnicalQnA(string position, string jobDescription, int count)
        //{
        //    if (string.IsNullOrWhiteSpace(_aiModel.ApiKey))
        //    {
        //        return $"ERROR: {_aiModel.ModelName} API Key not set. Please update MainForm.cs.";
        //    }

        //    try
        //    {
        //        var googleAI = new GoogleAI(apiKey: _aiModel.ApiKey);
        //        var model = googleAI.GenerativeModel(model: _aiModel.ModelName);

        //        var prompt = $@"
        //            You are an expert technical interviewer assistant.
        //            Based on the following job position and job description, generate {count} highly relevant technical interview questions and their concise answers. 
        //            Format the output as a simple question-and-answer list, using 'Q:' for the question and 'A:' for the answer.
        //            Separate each Q&A pair with two empty lines.

        //            **Example Format:**
        //            Q: What is a deadlock in multithreading and how do you prevent it?
        //            A: A deadlock is a state where two or more threads are waiting for each other to release resources, leading to a standstill. Prevention methods include ordered resource acquisition, timeout locks, and avoiding nested locks.

        //            Job Position: {position}
        //            Job Description:
        //            ---
        //            {jobDescription}
        //            ---
        //        ";

        //        var response = await model.GenerateContent(prompt);
        //        return response.Text.Trim();
        //    }
        //    catch (Exception ex)
        //    {
        //        return $"ERROR: API Exception during Q&A generation: {ex.Message}";
        //    }
        //}


        //// ------------------------------------------------------------------
        //// Remaining Original Methods (SanitizeFileName, AddInvisibleKeywordsToFooter)
        //// ------------------------------------------------------------------

        //private string SanitizeFileName(string fileName)
        //{
        //    char[] invalidChars = Path.GetInvalidFileNameChars();
        //    string sanitized = new string(fileName
        //        .Where(c => !invalidChars.Contains(c))
        //        .ToArray());

        //    return sanitized.Replace(" ", "").ToLower();
        //}

        //private void AddInvisibleKeywordsToFooter(string filePath, string keywords)
        //{
        //    using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
        //    {
        //        MainDocumentPart mainPart = doc.MainDocumentPart
        //            ?? throw new InvalidOperationException("Document has no main part");

        //        var document = mainPart.Document;
        //        var body = document.Body ?? throw new InvalidOperationException("Document body is null");

        //        var sectionPropsList = body.Descendants<SectionProperties>().ToList();

        //        if (sectionPropsList.Count == 0)
        //        {
        //            var sectionProps = new SectionProperties();
        //            body.Append(sectionProps);
        //            sectionPropsList.Add(sectionProps);
        //        }

        //        foreach (var sectionProps in sectionPropsList)
        //        {
        //            FooterReference footerRef = sectionProps.Descendants<FooterReference>()
        //                .FirstOrDefault(r => r.Type?.Value == HeaderFooterValues.Default);

        //            FooterPart footerPart;
        //            if (footerRef != null)
        //            {
        //                footerPart = (FooterPart)mainPart.GetPartById(footerRef.Id!);
        //            }
        //            else
        //            {
        //                footerPart = mainPart.AddNewPart<FooterPart>();
        //                footerRef = new FooterReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(footerPart) };
        //                sectionProps.Append(footerRef);
        //            }

        //            Footer footer = footerPart.Footer ?? new Footer();
        //            footer.RemoveAllChildren();

        //            Paragraph para = new Paragraph();

        //            ParagraphProperties paraProps = new ParagraphProperties();
        //            Justification justification = new Justification() { Val = JustificationValues.Left };
        //            paraProps.Append(justification);
        //            para.Append(paraProps);

        //            Run run = new Run();
        //            RunProperties runProps = new RunProperties();

        //            Color color = new Color() { Val = "FFFFFF" };
        //            runProps.Append(color);

        //            FontSize fontSize = new FontSize() { Val = "2" };
        //            runProps.Append(fontSize);

        //            run.Append(runProps);
        //            run.Append(new Text(keywords) { Space = SpaceProcessingModeValues.Preserve });

        //            para.Append(run);
        //            footer.Append(para);

        //            footerPart.Footer = footer;
        //            footerPart.Footer.Save();
        //        }

        //        mainPart.Document.Save();
        //    }
        //}

        //private void AddInvisibleKeywordsToLastPage(string filePath, string keywords)
        //{
        //    using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
        //    {
        //        MainDocumentPart mainPart = doc.MainDocumentPart
        //            ?? throw new InvalidOperationException("Document has no main part");

        //        var body = mainPart.Document.Body ?? throw new InvalidOperationException("Document body is null");

        //        Paragraph para = new Paragraph();

        //        ParagraphProperties paraProps = new ParagraphProperties();
        //        Justification justification = new Justification() { Val = JustificationValues.Left };
        //        paraProps.Append(justification);
        //        para.Append(paraProps);

        //        Run run = new Run();
        //        RunProperties runProps = new RunProperties();

        //        Color color = new Color() { Val = "FFFFFF" };
        //        runProps.Append(color);

        //        FontSize fontSize = new FontSize() { Val = "2" };
        //        runProps.Append(fontSize);

        //        run.Append(runProps);
        //        run.Append(new Text(keywords) { Space = SpaceProcessingModeValues.Preserve });

        //        para.Append(run);

        //        body.Append(para);

        //        mainPart.Document.Save();
        //    }
        //}

        ///// <summary>
        ///// Handles the Click event of the "Clear" button, resetting all input fields and controls to their default
        ///// states.
        ///// </summary>
        //private void btnClear_Click(object sender, EventArgs e)
        //{
        //    txtCompany.Text = string.Empty;
        //    txtPosition.Text = string.Empty;
        //    txtJobDescription.Text = string.Empty;
        //    txtJdPdf.Text = string.Empty;
        //    chkJobDescPdf.Checked = false; // back to typed-text mode
        //    txtCVPath.Text = string.Empty;
        //    txtMissingSkills.Text = string.Empty;
        //    chkConvertToPdf.Checked = false;
        //    chkAddSkillToCv.Checked = true; // Reset to default (footer)
        //    chkMissingSkills.Checked = true; // Reset to default (on)
        //    chkIsQAGenerate.Checked = false;
        //    lblRelevantQANum.Visible = false;
        //    txtRelevantQANum.Visible = false;
        //    txtRelevantQANum.Text = "";
        //    lblJobDescription.Text = "Job Description:";
        //    lblStatus.Text = string.Empty;
        //    picLoader.Visible = false;

        //    MessageBox.Show("All fields have been cleared.", "Clear", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}

        //private void chkIsQAGenerate_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkIsQAGenerate.Checked)
        //    {
        //        lblRelevantQANum.Visible = true;
        //        txtRelevantQANum.Visible = true;
        //        txtRelevantQANum.Text = _config["InitialFAQNumber"];
        //    }
        //    else
        //    {
        //        lblRelevantQANum.Visible = false;
        //        txtRelevantQANum.Visible = false;
        //        txtRelevantQANum.Text = "";
        //    }
        //}
        //#endregion

        //#region Helper methods
        ///// <summary>
        ///// Converts a Word document (.docx) to a PDF file.
        ///// </summary>
        //private void ConvertDocxToPdf(string docxPath, string pdfPath)
        //{
        //    Document document = new Document();
        //    document.LoadFromFile(docxPath);
        //    document.SaveToFile(pdfPath, FileFormat.PDF);
        //}

        ///// <summary>
        ///// Determines whether the specified string represents an integer greater than or equal to one.
        ///// </summary>
        //private bool IsPositiveIntAtLeastOne(string? text)
        //{
        //    if (string.IsNullOrWhiteSpace(text))
        //        return false;

        //    return int.TryParse(text.Trim(), out var n) && n >= 1;
        //}

        ///// <summary>
        ///// Extracts all readable text out of a .docx resume/CV file - used both for the missing-skills
        ///// comparison and as context when generating an AI cover letter.
        ///// </summary>
        //private string ExtractResumeText(string docxPath)
        //{
        //    using (var document = DocX.Load(docxPath))
        //    {
        //        return document.Text;
        //    }
        //}

        ///// <summary>
        ///// Extracts plain text from a PDF job description file using PdfPig.
        ///// </summary>
        //private string ExtractTextFromPdf(string pdfPath)
        //{
        //    var sb = new StringBuilder();
        //    using (var pdf = PdfDocument.Open(pdfPath))
        //    {
        //        foreach (var page in pdf.GetPages())
        //        {
        //            sb.AppendLine(page.Text);
        //        }
        //    }
        //    return sb.ToString();
        //}

        ///// <summary>
        ///// Finds the paragraph containing the given placeholder (e.g. "{Content}") in a Xceed DocX document,
        ///// replaces it with one paragraph per blank-line-separated block of the supplied content, and removes
        ///// the original placeholder paragraph. Used for the fully AI-generated cover letter, where the body
        ///// can't simply be dropped in with a single inline text replace.
        ///// </summary>
        //private void ReplaceContentPlaceholderWithParagraphs(Xceed.Document.NET.Document document, string placeholder, string content)
        //{
        //    var target = document.Paragraphs.FirstOrDefault(p => p.Text.Contains(placeholder));
        //    if (target == null)
        //    {
        //        return;
        //    }

        //    string[] bodyParagraphs = content.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
        //    if (bodyParagraphs.Length == 0)
        //    {
        //        bodyParagraphs = new[] { content };
        //    }

        //    Xceed.Document.NET.Paragraph anchor = target;
        //    foreach (string para in bodyParagraphs)
        //    {
        //        anchor = anchor.InsertParagraphAfterSelf(para.Trim());
        //    }

        //    target.Remove(false);
        //}
        //#endregion

        //private async void btnProcCoverLetter_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string templatePath = txtCoverLetterPath.Text; // Path from the Browse button
        //        if (string.IsNullOrEmpty(templatePath) || !File.Exists(templatePath))
        //        {
        //            MessageBox.Show("Please select a valid template file.");
        //            return;
        //        }

        //        picLoader.Visible = true;
        //        lblStatus.Text = "Processing Cover Letter...";
        //        lblStatus.ForeColor = System.Drawing.Color.Blue;
        //        Application.DoEvents();

        //        // 1. Ensure the shared folder is ready
        //        SetUpOutputDirectory();

        //        // 2. Define target file path and copy the template over
        //        string fileName = $"Cover_Letter_{sanitizedCompany}_{sanitizedPosition}.docx";
        //        string targetFilePath = Path.Combine(outputFolder, fileName);
        //        File.Copy(templatePath, targetFilePath, true);

        //        if (chkAIGenCoverLetter.Checked)
        //        {
        //            // ---- Fully AI Generated Cover Letter ----
        //            // Uses the {Date}/{AddressTo}/{JobPosition}/{Salutation}/{Content} style template.
        //            if (string.IsNullOrWhiteSpace(txtCVPath.Text) || !File.Exists(txtCVPath.Text))
        //            {
        //                picLoader.Visible = false;
        //                MessageBox.Show("Please select a valid CV/Resume file (CV/Resume section) so the AI can tailor the cover letter.",
        //                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                return;
        //            }

        //            string jobDescriptionForLetter = GetCurrentJobDescriptionText();
        //            if (string.IsNullOrWhiteSpace(jobDescriptionForLetter))
        //            {
        //                picLoader.Visible = false;
        //                MessageBox.Show("Please provide a Job Description (typed text or browsed JD PDF) so the AI can tailor the cover letter.",
        //                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                return;
        //            }

        //            lblStatus.Text = $"Generating cover letter content with {_aiModel.ModelName}...";
        //            Application.DoEvents();

        //            string resumeTextForLetter = ExtractResumeText(txtCVPath.Text);
        //            string generatedBody = await GenerateCoverLetterBodyAsync(
        //                resumeTextForLetter,
        //                jobDescriptionForLetter,
        //                txtJobPosition.Text,
        //                txtAddressTo.Text,
        //                txtSalutation.Text);

        //            if (generatedBody.StartsWith("ERROR:"))
        //            {
        //                picLoader.Visible = false;
        //                lblStatus.Text = "AI cover letter generation failed.";
        //                lblStatus.ForeColor = System.Drawing.Color.Red;
        //                MessageBox.Show("AI model failed to generate the cover letter content.", "Generation Error",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return;
        //            }

        //            using (var document = DocX.Load(targetFilePath))
        //            {
        //                document.ReplaceText(new StringReplaceTextOptions { SearchValue = "{Date}", NewValue = dtpDate.Value.ToString("MMMM dd, yyyy") });
        //                document.ReplaceText(new StringReplaceTextOptions { SearchValue = "{AddressTo}", NewValue = txtAddressTo.Text });
        //                document.ReplaceText(new StringReplaceTextOptions { SearchValue = "{Salutation}", NewValue = txtSalutation.Text });
        //                document.ReplaceText(new StringReplaceTextOptions { SearchValue = "{JobPosition}", NewValue = txtJobPosition.Text });

        //                ReplaceContentPlaceholderWithParagraphs(document, "{Content}", generatedBody);

        //                document.Save();
        //            }
        //        }
        //        else
        //        {
        //            // ---- Existing template-based logic, unchanged ----
        //            using (var document = DocX.Load(targetFilePath))
        //            {
        //                document.ReplaceText(new StringReplaceTextOptions
        //                {
        //                    SearchValue = "{Date}",
        //                    NewValue = dtpDate.Value.ToString("MMMM dd, yyyy")
        //                });
        //                document.ReplaceText(new StringReplaceTextOptions
        //                {
        //                    SearchValue = "{AddressTo}",
        //                    NewValue = txtAddressTo.Text
        //                });
        //                document.ReplaceText(new StringReplaceTextOptions
        //                {
        //                    SearchValue = "{Salutation}",
        //                    NewValue = txtSalutation.Text
        //                });
        //                document.ReplaceText(new StringReplaceTextOptions
        //                {
        //                    SearchValue = "{JobSource}",
        //                    NewValue = txtJobSource.Text
        //                });
        //                document.ReplaceText(new StringReplaceTextOptions
        //                {
        //                    SearchValue = "{JobPosition}",
        //                    NewValue = txtJobPosition.Text
        //                });
        //                document.ReplaceText(new StringReplaceTextOptions
        //                {
        //                    SearchValue = "{JobCompanyLoc}",
        //                    NewValue = txtJobCompanyLoc.Text
        //                });
        //                document.ReplaceText(new StringReplaceTextOptions
        //                {
        //                    SearchValue = "{Skills}",
        //                    NewValue = txtSkills.Text
        //                });

        //                string orgText = chkClientOrg.Checked ? "organization" : "client";
        //                document.ReplaceText(new StringReplaceTextOptions
        //                {
        //                    SearchValue = "{ClientOrg}",
        //                    NewValue = orgText
        //                });

        //                document.Save();
        //            }
        //        }

        //        // 3. Handle PDF Conversion if checked
        //        if (chkLetterToPdf.Checked)
        //        {
        //            string pdfPath = Path.ChangeExtension(targetFilePath, ".pdf");
        //            ConvertDocxToPdf(targetFilePath, pdfPath);
        //        }

        //        picLoader.Visible = false;
        //        lblStatus.Text = "✓ Creating Cover Letter Finished!";
        //        lblStatus.ForeColor = System.Drawing.Color.Green;
        //        System.Diagnostics.Process.Start("explorer.exe", outputFolder);

        //        MessageBox.Show("Cover letter created successfully!", "Success",
        //            MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        lblStatus.Text = $"Error: {ex.Message}";
        //        lblStatus.ForeColor = System.Drawing.Color.Red;
        //        MessageBox.Show($"Error: {ex.Message}", "Error",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        // Belt and braces: no matter which branch/return above ran (validation failure, AI error,
        //        // success, or an exception), the loader always gets switched off and repainted immediately -
        //        // this is what was leaving the gif visibly "stuck" after the letter had actually finished.
        //        picLoader.Visible = false;
        //        picLoader.Refresh();
        //    }
        //}

        //private void btnBrowseCoverLetter_Click(object sender, EventArgs e)
        //{
        //    using (OpenFileDialog ofd = new OpenFileDialog())
        //    {
        //        ofd.Filter = "Word Documents (*.docx)|*.docx|All Files (*.*)|*.*";
        //        ofd.Title = "Select Cover Letter Template";

        //        if (ofd.ShowDialog() == DialogResult.OK)
        //        {
        //            txtCoverLetterPath.Text = ofd.FileName;
        //        }
        //    }
        //}

        //private void btnLetterClear_Click(object sender, EventArgs e)
        //{
        //    // Reset global variables
        //    outputFolder = string.Empty;
        //    sanitizedCompany = string.Empty;
        //    sanitizedPosition = string.Empty;

        //    if (oCoverLetter != null)
        //    {
        //        string rawAddress = oCoverLetter.AddressTo ?? "Hiring Manager";
        //        string formattedAddress = rawAddress.Replace(".", Environment.NewLine);
        //        txtAddressTo.Text = formattedAddress;
        //        txtSalutation.Text = oCoverLetter.Salutation ?? string.Empty;
        //        txtJobSource.Text = oCoverLetter.JobSource ?? string.Empty;
        //        txtJobPosition.Text = oCoverLetter.Position ?? string.Empty;
        //        txtJobCompanyLoc.Text = string.Empty;
        //        txtSkills.Text = oCoverLetter.Skills ?? string.Empty;
        //        chkClientOrg.Checked = oCoverLetter.Organization ?? false;
        //        chkLetterToPdf.Checked = oCoverLetter.ConvertToPdf ?? false;
        //    }
        //    chkAIGenCoverLetter.Checked = true; // Reset to default (on)
        //    MessageBox.Show("All fields have been cleared.", "Clear", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}

        //private void txtPosition_TextChanged(object sender, EventArgs e)
        //{
        //    txtJobPosition.Text = txtPosition.Text;
        //}

        //private void txtCompany_TextChanged(object sender, EventArgs e)
        //{
        //    // Address To is now fixed to "Hiring Manager" by default and Company/Location is left for the
        //    // user to fill in manually (e.g. with just a city/country) - so typing a company name here no
        //    // longer auto-populates either of those cover letter fields.
        //}

        #region Variable declaration and constructor initialization
        private readonly IConfiguration _config;
        private readonly AIModel _aiModel;
        private string outputFolder = string.Empty;
        private string sanitizedCompany = string.Empty;
        private string sanitizedPosition = string.Empty;
        private readonly CoverLetter? oCoverLetter;

        // Used when no company/organization was provided by the user (e.g. recruiter posts that don't name the company)
        private const string NotMentionedCompanyText = "Not mentioned";
        private const string NotMentionedFolderToken = "notmentioned";

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

            // Default checkbox states: Missing Skills and Fully AI Generated Cover Letter are ON by default
            chkMissingSkills.Checked = true;
            chkAIGenCoverLetter.Checked = true;

            // Job Description input toggle: off by default (typed text active, JD PDF controls disabled)
            chkJobDescPdf.CheckedChanged += chkJobDescPdf_CheckedChanged;
            chkJobDescPdf.Checked = false;
            chkJobDescPdf_CheckedChanged(chkJobDescPdf, EventArgs.Empty); // apply the initial enabled/disabled state

            /// cover letter section innitialization
            oCoverLetter = _config.GetSection("CoverLetter").Get<CoverLetter>();
            if (oCoverLetter != null)
            {
                string rawAddress = oCoverLetter.AddressTo ?? "Hiring Manager";
                string formattedAddress = rawAddress.Replace(".", Environment.NewLine);
                txtAddressTo.Text = formattedAddress;
                txtSalutation.Text = oCoverLetter.Salutation ?? string.Empty;
                txtJobSource.Text = oCoverLetter.JobSource ?? string.Empty;
                txtJobPosition.Text = oCoverLetter.Position ?? string.Empty;
                // Company/Location is left blank by default; only used if the user explicitly types a location
                txtJobCompanyLoc.Text = string.Empty;
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
        /// Handles the Click event of the JD Pdf Browse button, allowing the user to point to a
        /// job description PDF instead of pasting the text directly.
        /// </summary>
        private void btnJdPdfBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                ofd.Title = "Select Job Description PDF";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtJdPdf.Text = ofd.FileName;
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
            if (string.IsNullOrWhiteSpace(sanitizedCompany))
            {
                sanitizedCompany = NotMentionedFolderToken;
            }
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
        /// Toggles which Job Description input is active. When "Job Description PDF?" is checked, the JD
        /// PDF path/browse controls are enabled and the free-text box is disabled - and vice versa. Only
        /// one source can feed the process at a time.
        /// </summary>
        private void chkJobDescPdf_CheckedChanged(object sender, EventArgs e)
        {
            bool usePdf = chkJobDescPdf.Checked;
            txtJdPdf.Enabled = usePdf;
            btnJdPdfBrowse.Enabled = usePdf;
            txtJobDescription.Enabled = !usePdf;
        }

        /// <summary>
        /// Returns the job description text to use for the current operation, based on which source is
        /// currently active (see chkJobDescPdf). Text mode returns whatever's typed/scraped in the box;
        /// PDF mode extracts text from the browsed JD PDF.
        /// </summary>
        private string GetCurrentJobDescriptionText()
        {
            if (chkJobDescPdf.Checked)
            {
                if (!string.IsNullOrWhiteSpace(txtJdPdf.Text) && File.Exists(txtJdPdf.Text))
                {
                    return ExtractTextFromPdf(txtJdPdf.Text);
                }
                return string.Empty;
            }

            return txtJobDescription.Text.Trim();
        }

        /// <summary>
        /// Runs an AI call with a hard timeout so a slow/stuck network request can never leave the loader
        /// spinning forever - if the API hasn't answered within the window, this returns an ERROR string
        /// instead of hanging the UI indefinitely.
        /// </summary>
        private static readonly TimeSpan AiCallTimeout = TimeSpan.FromSeconds(90);

        private async Task<string> RunWithTimeoutAsync(Func<Task<string>> apiCall, string timeoutMessage)
        {
            Task<string> callTask = apiCall();
            Task completed = await Task.WhenAny(callTask, Task.Delay(AiCallTimeout));

            if (completed != callTask)
            {
                return $"ERROR: {timeoutMessage}";
            }

            return await callTask;
        }

        /// <summary>
        /// Handles the click event of the "Process" button, performing a series of operations to customize a CV and
        /// generate supplementary documents such as job description, job description related technical question and answer.
        /// </summary>
        private async void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                bool hasJobDescriptionSource = chkJobDescPdf.Checked
                    ? (!string.IsNullOrWhiteSpace(txtJdPdf.Text) && File.Exists(txtJdPdf.Text))
                    : !string.IsNullOrWhiteSpace(txtJobDescription.Text);

                if (string.IsNullOrWhiteSpace(txtPosition.Text) || !hasJobDescriptionSource ||
                    string.IsNullOrWhiteSpace(txtCVPath.Text) || !File.Exists(txtCVPath.Text))
                {
                    MessageBox.Show("Please fill Position, provide a Job Description (typed text or a browsed JD PDF), " +
                        "and select a valid CV file. Company Name is optional.", "Validation Error",
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

                // 1. Check if the input is a URL then read the JD and paste into txtJobDescription otherwise paste the JD into the txtJobDescription directly.
                // Only relevant in text mode - when the JD PDF checkbox is on, this box is disabled anyway.
                string input = chkJobDescPdf.Checked ? string.Empty : txtJobDescription.Text.Trim();
                bool isUrl = !string.IsNullOrWhiteSpace(input) &&
                             Uri.TryCreate(input, UriKind.Absolute, out Uri uriResult)
                             && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (isUrl)
                {
                    picLoader.Visible = true;
                    lblStatus.Text = "Reading job link... Please wait.";
                    lblStatus.ForeColor = System.Drawing.Color.Blue;
                    Application.DoEvents();

                    // Attempt to read the URL using AI service. Here I have used Gemini model. You can use the model based on your need.
                    //string scrapedDescription = await RunWithTimeoutAsync(
                    //    () => ExtractFullJobDescriptionFromUrlAsync(input),
                    //    "Reading the job link timed out. Check your connection and try again, or paste the description manually.");
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

                // 2. Setup Shared Directory
                SetUpOutputDirectory();

                picLoader.Visible = true;
                lblStatus.Text = "Folder prepared: " + outputFolder;
                lblStatus.ForeColor = System.Drawing.Color.Blue;
                Application.DoEvents();

                // 3. Create Job Description Document using shared global variables
                string jdFileName = $"JobDescription_{sanitizedCompany}_{sanitizedPosition}.pdf";
                string jdFilePath = Path.Combine(outputFolder, jdFileName);

                // Resolve the job description content based on which source is active (see chkJobDescPdf).
                // This is kept separate from txtJobDescription.Text so the text box's own content is never overwritten below.
                if (chkJobDescPdf.Checked)
                {
                    lblStatus.Text = "Extracting text from JD PDF...";
                    Application.DoEvents();
                }
                string jobDescriptionInput = GetCurrentJobDescriptionText();

                string positionText = txtPosition.Text.Trim();
                string companyText = txtCompany.Text.Trim(); // may be empty -> treated as "Not mentioned"

                // Pass 'true' for convertToPdf as per the request to make it PDF
                CreateJobDescriptionDocument(jdFilePath, positionText, companyText, jobDescriptionInput, true);
                lblStatus.Text = $"Job Description document created: {jdFileName}";
                Application.DoEvents();

                // 4. If chkAddSkillToCv is checked then Gemini will read and extract keywords to embed into the CV/Resume.
                // NOTE: unlike before, the extracted keywords are no longer written back into the Job Description text area -
                // that box always keeps whatever the user typed/pasted (or the URL-scraped text).
                bool isAddSkillToDoc = chkAddSkillToCv.Checked;
                string extractedKeywords = string.Empty;
                if (isAddSkillToDoc)
                {
                    //extractedKeywords = await RunWithTimeoutAsync(
                    //    () => ExtractKeywordsAsync($"Company: {companyText}, Position: {positionText}\n{jobDescriptionInput}"),
                    //    "Keyword extraction timed out. Check your connection/API quota and try again.");
                    extractedKeywords = await ExtractKeywordsAsync($"Company: {companyText}, Position: {positionText}\n{jobDescriptionInput}");

                    if (extractedKeywords.StartsWith("ERROR:"))
                    {
                        picLoader.Visible = false;
                        lblStatus.Text = "Keyword extraction failed.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        MessageBox.Show("AI model failed to extract the keyword", "Link Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    lblStatus.Text = $"Keywords extracted by {_aiModel.ModelName}. Processing CV/Resume...";
                    Application.DoEvents();
                }

                // The ProcessCV now saves to the new folder
                string finalCVPath = ProcessCV(txtCVPath.Text, companyText, positionText, extractedKeywords, chkConvertToPdf.Checked, outputFolder, isAddSkillToDoc);
                lblStatus.Text = $"CV processed and saved to: {finalCVPath}";
                Application.DoEvents();

                // 4b. Missing Skills - independent of "Add skills to the CV/Resume". Compares the resume's
                // technical skills against the job description/JD PDF and lists what's missing.
                if (chkMissingSkills.Checked)
                {
                    lblStatus.Text = "Analyzing missing skills...";
                    Application.DoEvents();

                    string resumeTextForGap = ExtractResumeText(txtCVPath.Text);
                    //string missingSkills = await RunWithTimeoutAsync(
                    //    () => AnalyzeMissingSkillsAsync(resumeTextForGap, jobDescriptionInput),
                    //    "Missing skills analysis timed out after 90 seconds. Check your connection and API quota, then try again.");
                    string missingSkills = await AnalyzeMissingSkillsAsync(resumeTextForGap, jobDescriptionInput);

                    if (missingSkills.StartsWith("ERROR:"))
                    {
                        lblStatus.Text = "Missing skills analysis failed.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        MessageBox.Show("AI model failed to analyze the missing skills.", "Link Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        txtMissingSkills.Text = missingSkills;
                    }
                    Application.DoEvents();
                }

                // 5. Generate Relevant Q&A Document
                bool isGenerateQA = chkIsQAGenerate.Checked;
                if (isGenerateQA)
                {
                    lblStatus.Text = $"Generating {Convert.ToInt32(txtRelevantQANum.Text)} Q&A pairs with {_aiModel.ModelName}...";
                    Application.DoEvents();

                    //string qaDocumentContent = await RunWithTimeoutAsync(
                    //    () => GenerateTechnicalQnA(positionText, jobDescriptionInput, Convert.ToInt32(txtRelevantQANum.Text)),
                    //    "Q&A generation timed out. Check your connection/API quota and try again.");
                    string qaDocumentContent = await GenerateTechnicalQnA(positionText, jobDescriptionInput, Convert.ToInt32(txtRelevantQANum.Text));

                    if (qaDocumentContent.StartsWith("ERROR:"))
                    {
                        picLoader.Visible = false;
                        lblStatus.Text = "Generation technical QA failed";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        MessageBox.Show("AI model failed to generate the technical QA", "Link Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string qaFileName = $"InterviewQA_{sanitizedCompany}_{sanitizedPosition}.pdf";
                    string qaFilePath = Path.Combine(outputFolder, qaFileName);

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
                lblStatus.Text = $"Error: {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                picLoader.Visible = false;
                picLoader.Refresh();
            }
        }

        /// <summary>
        /// Sanitizes a folder name by removing invalid characters and replacing spaces and non-alphanumeric characters
        /// with underscores.
        /// </summary>
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
        /// If no company name was provided, "Not mentioned" is written in its place.
        /// </summary>
        private void CreateJobDescriptionDocument(string outputPath, string position, string company, string jobDescription, bool convertToPdf)
        {
            string displayCompany = string.IsNullOrWhiteSpace(company) ? NotMentionedCompanyText : company;

            // The outputPath will be the PDF path. We determine the temporary DOCX path.
            string docxPath = Path.Combine(Path.GetDirectoryName(outputPath),
                Path.GetFileNameWithoutExtension(outputPath) + ".docx");

            using (WordprocessingDocument doc = WordprocessingDocument.Create(docxPath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(new DocumentFormat.OpenXml.Wordprocessing.Body());

                // Position - Bold
                Paragraph pPosition = new Paragraph(new Run(
                    new RunProperties(new Bold()),
                    new Text(position) { Space = SpaceProcessingModeValues.Preserve }));
                mainPart.Document.Body.Append(pPosition);

                // Company Name (or "Not mentioned" if left blank)
                Paragraph pCompany = new Paragraph(new Run(new Text(displayCompany) { Space = SpaceProcessingModeValues.Preserve }));
                mainPart.Document.Body.Append(pCompany);

                // Two New Lines (Empty Paragraphs)
                mainPart.Document.Body.Append(new Paragraph());
                mainPart.Document.Body.Append(new Paragraph());

                // Job Description
                string formattedJD = jobDescription.Replace("\r\n", "\v").Replace("\n", "\v").Replace("\r", "\v");

                Paragraph pJD = new Paragraph();
                foreach (string line in formattedJD.Split('\v'))
                {
                    pJD.Append(new Run(new Text(line) { Space = SpaceProcessingModeValues.Preserve }));
                    pJD.Append(new Run(new DocumentFormat.OpenXml.Wordprocessing.Break()));
                }
                mainPart.Document.Body.Append(pJD);

                mainPart.Document.Save();
            }

            ConvertDocxToPdf(docxPath, outputPath);
            File.Delete(docxPath);
        }

        /// <summary>
        /// Creates a PDF document from the specified text content and saves it to the specified output path.
        /// </summary>
        private void CreateDocumentFromText(string outputPath, string content)
        {
            string docxPath = Path.Combine(Path.GetDirectoryName(outputPath),
                Path.GetFileNameWithoutExtension(outputPath) + ".docx");

            using (WordprocessingDocument doc = WordprocessingDocument.Create(docxPath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(new DocumentFormat.OpenXml.Wordprocessing.Body());

                string[] lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    Paragraph p = new Paragraph();
                    Run r = new Run();
                    r.Append(new Text(line) { Space = SpaceProcessingModeValues.Preserve });
                    p.Append(r);

                    p.Append(new Run(new DocumentFormat.OpenXml.Wordprocessing.Break()));

                    if (line.Trim().EndsWith("?") || line.Trim().StartsWith("Q:"))
                    {
                        r.RunProperties = new RunProperties(new Bold());
                    }

                    mainPart.Document.Body.Append(p);
                }

                mainPart.Document.Save();
            }

            string pdfPath = outputPath.Replace(".docx", ".pdf");
            ConvertDocxToPdf(docxPath, pdfPath);
            File.Delete(docxPath);
        }

        /// <summary>
        /// Processes a CV file by appending metadata or parameter values, optionally converting it to PDF, and saving it to a specified
        /// output folder.
        /// </summary>
        private string ProcessCV(string sourcePath, string company, string position, string keywords, bool convertToPdf, string outputFolder, bool isAddSkillToDoc)
        {
            string baseFileName = Path.GetFileNameWithoutExtension(sourcePath);
            string sanitizedCompanyLocal = SanitizeFileName(company);
            if (string.IsNullOrWhiteSpace(sanitizedCompanyLocal))
            {
                sanitizedCompanyLocal = NotMentionedFolderToken;
            }
            string sanitizedPositionLocal = SanitizeFileName(position);

            string newBaseName = $"{baseFileName}_{sanitizedCompanyLocal}_{sanitizedPositionLocal}";
            string newDocxFileName = $"{newBaseName}.docx";
            string newFilePath = Path.Combine(outputFolder, newDocxFileName);

            File.Copy(sourcePath, newFilePath, true);

            if (isAddSkillToDoc)
            {
                AddInvisibleKeywordsToLastPage(newFilePath, keywords);
            }

            string outputPath = newFilePath;

            if (convertToPdf)
            {
                string pdfFileName = $"{newBaseName}.pdf";
                outputPath = Path.Combine(outputFolder, pdfFileName);
                ConvertDocxToPdf(newFilePath, outputPath);
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
        /// terms - used to embed into the CV/Resume when "Add skills to the CV/Resume" is checked.
        /// </summary>
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
        /// Compares the candidate's resume against the job description/JD PDF and returns the technical
        /// skills that appear in the job description but are missing from the resume.
        /// </summary>
        private async Task<string> AnalyzeMissingSkillsAsync(string resumeText, string jobDescription)
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
                    You are an expert technical recruiter.
                    Compare the candidate's resume against the job description below and identify ONLY the
                    technical skills, tools, frameworks, or technologies mentioned in the job description that
                    are NOT already present anywhere in the candidate's resume (the missing / gap skills).

                    Return the result as a single comma-separated list only (e.g. 'Kubernetes, GraphQL, Terraform').
                    If there are no missing skills, return exactly 'None'.
                    Do not include any explanation, headers, or extra text.

                    Candidate Resume:
                    ---
                    {resumeText}
                    ---

                    Job Description:
                    ---
                    {jobDescription}
                    ---
                ";

                var response = await model.GenerateContent(prompt);
                return response.Text.Trim().Trim('"', '\'').Trim();
            }
            catch (Exception ex)
            {
                return $"ERROR: API Exception during missing skills analysis: {ex.Message}";
            }
        }

        /// <summary>
        /// Uses AI to write the main body paragraphs of a cover letter based on the candidate's resume and
        /// the job description, tailored to the given position/address/salutation.
        /// </summary>
        private async Task<string> GenerateCoverLetterBodyAsync(string resumeText, string jobDescription, string position, string addressTo, string salutation)
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
                    You are an expert professional cover letter writer.
                    Write ONLY the main body of a cover letter (2 to 4 paragraphs) for the position of
                    '{position}', addressed to '{addressTo}'.

                    Base the content on the candidate's resume and the job description below, highlighting
                    the most relevant experience, achievements, and skills that match the job requirements.

                    Strict rules:
                    - Do NOT include the salutation ('{salutation}') - it will be added separately.
                    - Do NOT include a closing line or signature (no 'Sincerely', no name).
                    - Separate each paragraph with a blank line.
                    - Return plain text only, no markdown formatting.

                    Candidate Resume:
                    ---
                    {resumeText}
                    ---

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
                return $"ERROR: API Exception during cover letter generation: {ex.Message}";
            }
        }

        /// <summary>
        /// Generates a list of technical interview questions and their concise answers based on the provided job
        /// position and job description.
        /// </summary>
        private async Task<string> GenerateTechnicalQnA(string position, string jobDescription, int count)
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
                    footer.RemoveAllChildren();

                    Paragraph para = new Paragraph();

                    ParagraphProperties paraProps = new ParagraphProperties();
                    Justification justification = new Justification() { Val = JustificationValues.Left };
                    paraProps.Append(justification);
                    para.Append(paraProps);

                    Run run = new Run();
                    RunProperties runProps = new RunProperties();

                    Color color = new Color() { Val = "FFFFFF" };
                    runProps.Append(color);

                    FontSize fontSize = new FontSize() { Val = "2" };
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

        private void AddInvisibleKeywordsToLastPage(string filePath, string keywords)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, true))
            {
                MainDocumentPart mainPart = doc.MainDocumentPart
                    ?? throw new InvalidOperationException("Document has no main part");

                var body = mainPart.Document.Body ?? throw new InvalidOperationException("Document body is null");

                Paragraph para = new Paragraph();

                ParagraphProperties paraProps = new ParagraphProperties();
                Justification justification = new Justification() { Val = JustificationValues.Left };
                paraProps.Append(justification);
                para.Append(paraProps);

                Run run = new Run();
                RunProperties runProps = new RunProperties();

                Color color = new Color() { Val = "FFFFFF" };
                runProps.Append(color);

                FontSize fontSize = new FontSize() { Val = "2" };
                runProps.Append(fontSize);

                run.Append(runProps);
                run.Append(new Text(keywords) { Space = SpaceProcessingModeValues.Preserve });

                para.Append(run);

                body.Append(para);

                mainPart.Document.Save();
            }
        }

        /// <summary>
        /// Handles the Click event of the "Clear" button, resetting all input fields and controls to their default
        /// states.
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCompany.Text = string.Empty;
            txtPosition.Text = string.Empty;
            txtJobDescription.Text = string.Empty;
            txtJdPdf.Text = string.Empty;
            chkJobDescPdf.Checked = false; // back to typed-text mode
            txtCVPath.Text = string.Empty;
            txtMissingSkills.Text = string.Empty;
            chkConvertToPdf.Checked = false;
            chkAddSkillToCv.Checked = true; // Reset to default (footer)
            chkMissingSkills.Checked = true; // Reset to default (on)
            chkIsQAGenerate.Checked = false;
            lblRelevantQANum.Visible = false;
            txtRelevantQANum.Visible = false;
            txtRelevantQANum.Text = "";
            lblJobDescription.Text = "Job Description:";
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
        private void ConvertDocxToPdf(string docxPath, string pdfPath)
        {
            Document document = new Document();
            document.LoadFromFile(docxPath);
            document.SaveToFile(pdfPath, FileFormat.PDF);
        }

        /// <summary>
        /// Determines whether the specified string represents an integer greater than or equal to one.
        /// </summary>
        private bool IsPositiveIntAtLeastOne(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return int.TryParse(text.Trim(), out var n) && n >= 1;
        }

        /// <summary>
        /// Extracts all readable text out of a .docx resume/CV file - used both for the missing-skills
        /// comparison and as context when generating an AI cover letter.
        /// </summary>
        private string ExtractResumeText(string docxPath)
        {
            using (var document = DocX.Load(docxPath))
            {
                return document.Text;
            }
        }

        /// <summary>
        /// Extracts plain text from a PDF job description file using PdfPig.
        /// </summary>
        private string ExtractTextFromPdf(string pdfPath)
        {
            var sb = new StringBuilder();
            using (var pdf = PdfDocument.Open(pdfPath))
            {
                foreach (var page in pdf.GetPages())
                {
                    sb.AppendLine(page.Text);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Finds the paragraph containing the given placeholder (e.g. "{Content}") in a Xceed DocX document,
        /// replaces it with one paragraph per blank-line-separated block of the supplied content, and removes
        /// the original placeholder paragraph. Used for the fully AI-generated cover letter, where the body
        /// can't simply be dropped in with a single inline text replace.
        /// </summary>
        private void ReplaceContentPlaceholderWithParagraphs(Xceed.Document.NET.Document document, string placeholder, string content)
        {
            var target = document.Paragraphs.FirstOrDefault(p => p.Text.Contains(placeholder));
            if (target == null)
            {
                return;
            }

            string[] bodyParagraphs = content.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (bodyParagraphs.Length == 0)
            {
                bodyParagraphs = new[] { content };
            }

            Xceed.Document.NET.Paragraph anchor = target;
            foreach (string para in bodyParagraphs)
            {
                anchor = anchor.InsertParagraphAfterSelf(para.Trim());
            }

            target.Remove(false);
        }
        #endregion

        private async void btnProcCoverLetter_Click(object sender, EventArgs e)
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

                // 1. Ensure the shared folder is ready
                SetUpOutputDirectory();

                // 2. Define target file path and copy the template over
                string fileName = $"Cover_Letter_{sanitizedCompany}_{sanitizedPosition}.docx";
                string targetFilePath = Path.Combine(outputFolder, fileName);
                File.Copy(templatePath, targetFilePath, true);

                if (chkAIGenCoverLetter.Checked)
                {
                    // ---- Fully AI Generated Cover Letter ----
                    // Uses the {Date}/{AddressTo}/{JobPosition}/{Salutation}/{Content} style template.
                    if (string.IsNullOrWhiteSpace(txtCVPath.Text) || !File.Exists(txtCVPath.Text))
                    {
                        picLoader.Visible = false;
                        MessageBox.Show("Please select a valid CV/Resume file (CV/Resume section) so the AI can tailor the cover letter.",
                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string jobDescriptionForLetter = GetCurrentJobDescriptionText();
                    if (string.IsNullOrWhiteSpace(jobDescriptionForLetter))
                    {
                        picLoader.Visible = false;
                        MessageBox.Show("Please provide a Job Description (typed text or browsed JD PDF) so the AI can tailor the cover letter.",
                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    lblStatus.Text = $"Generating cover letter content with {_aiModel.ModelName}...";
                    Application.DoEvents();

                    string resumeTextForLetter = ExtractResumeText(txtCVPath.Text);
                    //string generatedBody = await RunWithTimeoutAsync(
                    //    () => GenerateCoverLetterBodyAsync(
                    //        resumeTextForLetter,
                    //        jobDescriptionForLetter,
                    //        txtJobPosition.Text,
                    //        txtAddressTo.Text,
                    //        txtSalutation.Text),
                    //    "Cover letter generation timed out. Check your connection/API quota and try again.");
                    string generatedBody = await GenerateCoverLetterBodyAsync(
                        resumeTextForLetter,
                        jobDescriptionForLetter,
                        txtJobPosition.Text,
                        txtAddressTo.Text,
                        txtSalutation.Text);

                    if (generatedBody.StartsWith("ERROR:"))
                    {
                        picLoader.Visible = false;
                        lblStatus.Text = "AI cover letter generation failed.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        MessageBox.Show("AI model failed to generate the cover letter content.", "Generation Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    using (var document = DocX.Load(targetFilePath))
                    {
                        document.ReplaceText(new StringReplaceTextOptions { SearchValue = "{Date}", NewValue = dtpDate.Value.ToString("MMMM dd, yyyy") });
                        document.ReplaceText(new StringReplaceTextOptions { SearchValue = "{AddressTo}", NewValue = txtAddressTo.Text });
                        document.ReplaceText(new StringReplaceTextOptions { SearchValue = "{Salutation}", NewValue = txtSalutation.Text });
                        document.ReplaceText(new StringReplaceTextOptions { SearchValue = "{JobPosition}", NewValue = txtJobPosition.Text });

                        ReplaceContentPlaceholderWithParagraphs(document, "{Content}", generatedBody);

                        document.Save();
                    }
                }
                else
                {
                    // ---- Existing template-based logic, unchanged ----
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

                        string orgText = chkClientOrg.Checked ? "organization" : "client";
                        document.ReplaceText(new StringReplaceTextOptions
                        {
                            SearchValue = "{ClientOrg}",
                            NewValue = orgText
                        });

                        document.Save();
                    }
                }

                // 3. Handle PDF Conversion if checked
                if (chkLetterToPdf.Checked)
                {
                    string pdfPath = Path.ChangeExtension(targetFilePath, ".pdf");
                    ConvertDocxToPdf(targetFilePath, pdfPath);
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
                lblStatus.Text = $"Error: {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Belt and braces: no matter which branch/return above ran (validation failure, AI error,
                // success, or an exception), the loader always gets switched off and repainted immediately -
                // this is what was leaving the gif visibly "stuck" after the letter had actually finished.
                picLoader.Visible = false;
                picLoader.Refresh();
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
                string rawAddress = oCoverLetter.AddressTo ?? "Hiring Manager";
                string formattedAddress = rawAddress.Replace(".", Environment.NewLine);
                txtAddressTo.Text = formattedAddress;
                txtSalutation.Text = oCoverLetter.Salutation ?? string.Empty;
                txtJobSource.Text = oCoverLetter.JobSource ?? string.Empty;
                txtJobPosition.Text = oCoverLetter.Position ?? string.Empty;
                txtJobCompanyLoc.Text = string.Empty;
                txtSkills.Text = oCoverLetter.Skills ?? string.Empty;
                chkClientOrg.Checked = oCoverLetter.Organization ?? false;
                chkLetterToPdf.Checked = oCoverLetter.ConvertToPdf ?? false;
            }
            chkAIGenCoverLetter.Checked = true; // Reset to default (on)
            MessageBox.Show("All fields have been cleared.", "Clear", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtPosition_TextChanged(object sender, EventArgs e)
        {
            txtJobPosition.Text = txtPosition.Text;
        }

        private void txtCompany_TextChanged(object sender, EventArgs e)
        {
            // Address To is now fixed to "Hiring Manager" by default and Company/Location is left for the
            // user to fill in manually (e.g. with just a city/country) - so typing a company name here no
            // longer auto-populates either of those cover letter fields.
        }
    }
}
