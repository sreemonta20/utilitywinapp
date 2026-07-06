namespace cvApp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            picLoader = new PictureBox();
            lblStatus = new Label();
            grpBoxCVResume = new GroupBox();
            chkJobDescPdf = new CheckBox();
            chkMissingSkills = new CheckBox();
            chkIsQAGenerate = new CheckBox();
            lblRelevantQANum = new Label();
            txtRelevantQANum = new TextBox();
            chkConvertToPdf = new CheckBox();
            chkAddSkillToCv = new CheckBox();
            txtMissingSkills = new TextBox();
            lblMissingSkills = new Label();
            btnJdPdfBrowse = new Button();
            lblJDPdf = new Label();
            txtJdPdf = new TextBox();
            lblCompany = new Label();
            txtCompany = new TextBox();
            lblPosition = new Label();
            txtPosition = new TextBox();
            lblJobDescription = new Label();
            txtJobDescription = new TextBox();
            lblCVPath = new Label();
            txtCVPath = new TextBox();
            btnBrowse = new Button();
            btnProcess = new Button();
            btnClear = new Button();
            grpBoxCoverLetter = new GroupBox();
            chkAIGenCoverLetter = new CheckBox();
            btnBrowseCoverLetter = new Button();
            txtCoverLetterPath = new TextBox();
            lblCoverLetterPath = new Label();
            chkLetterToPdf = new CheckBox();
            chkClientOrg = new CheckBox();
            txtSkills = new TextBox();
            btnLetterClear = new Button();
            btnProcCoverLetter = new Button();
            lblSkills = new Label();
            lblJobCompanyLoc = new Label();
            txtJobCompanyLoc = new TextBox();
            lblJobPosition = new Label();
            txtJobPosition = new TextBox();
            lblJobSource = new Label();
            txtJobSource = new TextBox();
            lblSalutation = new Label();
            txtSalutation = new TextBox();
            txtAddressTo = new TextBox();
            lblLetterTo = new Label();
            dtpDate = new DateTimePicker();
            lblDate = new Label();
            ((System.ComponentModel.ISupportInitialize)picLoader).BeginInit();
            grpBoxCVResume.SuspendLayout();
            grpBoxCoverLetter.SuspendLayout();
            SuspendLayout();
            // 
            // picLoader
            // 
            picLoader.Location = new Point(473, 548);
            picLoader.Name = "picLoader";
            picLoader.Size = new Size(140, 40);
            picLoader.SizeMode = PictureBoxSizeMode.Zoom;
            picLoader.TabIndex = 13;
            picLoader.TabStop = false;
            picLoader.Visible = false;
            // 
            // lblStatus
            // 
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.Green;
            lblStatus.Location = new Point(12, 600);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1063, 40);
            lblStatus.TabIndex = 16;
            lblStatus.TextAlign = ContentAlignment.TopCenter;
            // 
            // grpBoxCVResume
            // 
            grpBoxCVResume.Controls.Add(chkJobDescPdf);
            grpBoxCVResume.Controls.Add(chkMissingSkills);
            grpBoxCVResume.Controls.Add(chkIsQAGenerate);
            grpBoxCVResume.Controls.Add(lblRelevantQANum);
            grpBoxCVResume.Controls.Add(txtRelevantQANum);
            grpBoxCVResume.Controls.Add(chkConvertToPdf);
            grpBoxCVResume.Controls.Add(chkAddSkillToCv);
            grpBoxCVResume.Controls.Add(txtMissingSkills);
            grpBoxCVResume.Controls.Add(lblMissingSkills);
            grpBoxCVResume.Controls.Add(btnJdPdfBrowse);
            grpBoxCVResume.Controls.Add(lblJDPdf);
            grpBoxCVResume.Controls.Add(txtJdPdf);
            grpBoxCVResume.Controls.Add(lblCompany);
            grpBoxCVResume.Controls.Add(txtCompany);
            grpBoxCVResume.Controls.Add(lblPosition);
            grpBoxCVResume.Controls.Add(txtPosition);
            grpBoxCVResume.Controls.Add(lblJobDescription);
            grpBoxCVResume.Controls.Add(txtJobDescription);
            grpBoxCVResume.Controls.Add(lblCVPath);
            grpBoxCVResume.Controls.Add(txtCVPath);
            grpBoxCVResume.Controls.Add(btnBrowse);
            grpBoxCVResume.Controls.Add(btnProcess);
            grpBoxCVResume.Controls.Add(btnClear);
            grpBoxCVResume.Location = new Point(11, 4);
            grpBoxCVResume.Name = "grpBoxCVResume";
            grpBoxCVResume.Size = new Size(548, 528);
            grpBoxCVResume.TabIndex = 22;
            grpBoxCVResume.TabStop = false;
            grpBoxCVResume.Text = "CV / Resume";
            // 
            // chkJobDescPdf
            // 
            chkJobDescPdf.AutoSize = true;
            chkJobDescPdf.Location = new Point(147, 65);
            chkJobDescPdf.Name = "chkJobDescPdf";
            chkJobDescPdf.Size = new Size(128, 19);
            chkJobDescPdf.TabIndex = 59;
            chkJobDescPdf.Text = "Job Description Pdf";
            chkJobDescPdf.UseVisualStyleBackColor = true;
            chkJobDescPdf.CheckedChanged += chkJobDescPdf_CheckedChanged;
            // 
            // chkMissingSkills
            // 
            chkMissingSkills.AutoSize = true;
            chkMissingSkills.Location = new Point(333, 31);
            chkMissingSkills.Name = "chkMissingSkills";
            chkMissingSkills.Size = new Size(96, 19);
            chkMissingSkills.TabIndex = 58;
            chkMissingSkills.Text = "Missing Skills";
            chkMissingSkills.UseVisualStyleBackColor = true;
            // 
            // chkIsQAGenerate
            // 
            chkIsQAGenerate.AutoSize = true;
            chkIsQAGenerate.Location = new Point(15, 65);
            chkIsQAGenerate.Name = "chkIsQAGenerate";
            chkIsQAGenerate.Size = new Size(93, 19);
            chkIsQAGenerate.TabIndex = 57;
            chkIsQAGenerate.Text = "Generate Q&A";
            chkIsQAGenerate.UseVisualStyleBackColor = true;
            chkIsQAGenerate.CheckedChanged += chkIsQAGenerate_CheckedChanged;
            // 
            // lblRelevantQANum
            // 
            lblRelevantQANum.AutoSize = true;
            lblRelevantQANum.Location = new Point(333, 65);
            lblRelevantQANum.Name = "lblRelevantQANum";
            lblRelevantQANum.Size = new Size(139, 15);
            lblRelevantQANum.TabIndex = 55;
            lblRelevantQANum.Text = "Relevant Q&A to Generate:";
            lblRelevantQANum.Visible = false;
            // 
            // txtRelevantQANum
            // 
            txtRelevantQANum.Location = new Point(487, 65);
            txtRelevantQANum.Name = "txtRelevantQANum";
            txtRelevantQANum.Size = new Size(50, 23);
            txtRelevantQANum.TabIndex = 56;
            txtRelevantQANum.Visible = false;
            // 
            // chkConvertToPdf
            // 
            chkConvertToPdf.AutoSize = true;
            chkConvertToPdf.Location = new Point(15, 31);
            chkConvertToPdf.Name = "chkConvertToPdf";
            chkConvertToPdf.Size = new Size(106, 19);
            chkConvertToPdf.TabIndex = 53;
            chkConvertToPdf.Text = "Convert to PDF";
            chkConvertToPdf.UseVisualStyleBackColor = true;
            // 
            // chkAddSkillToCv
            // 
            chkAddSkillToCv.AutoSize = true;
            chkAddSkillToCv.Checked = true;
            chkAddSkillToCv.CheckState = CheckState.Checked;
            chkAddSkillToCv.Location = new Point(147, 31);
            chkAddSkillToCv.Name = "chkAddSkillToCv";
            chkAddSkillToCv.Size = new Size(175, 19);
            chkAddSkillToCv.TabIndex = 54;
            chkAddSkillToCv.Text = "Add skills to the CV/Resume";
            chkAddSkillToCv.UseVisualStyleBackColor = true;
            // 
            // txtMissingSkills
            // 
            txtMissingSkills.Location = new Point(147, 378);
            txtMissingSkills.Multiline = true;
            txtMissingSkills.Name = "txtMissingSkills";
            txtMissingSkills.ScrollBars = ScrollBars.Vertical;
            txtMissingSkills.Size = new Size(390, 63);
            txtMissingSkills.TabIndex = 51;
            // 
            // lblMissingSkills
            // 
            lblMissingSkills.AutoSize = true;
            lblMissingSkills.Location = new Point(15, 393);
            lblMissingSkills.Name = "lblMissingSkills";
            lblMissingSkills.Size = new Size(80, 15);
            lblMissingSkills.TabIndex = 50;
            lblMissingSkills.Text = "Missing Skills:";
            // 
            // btnJdPdfBrowse
            // 
            btnJdPdfBrowse.Location = new Point(457, 337);
            btnJdPdfBrowse.Name = "btnJdPdfBrowse";
            btnJdPdfBrowse.Size = new Size(80, 28);
            btnJdPdfBrowse.TabIndex = 49;
            btnJdPdfBrowse.Text = "Browse...";
            btnJdPdfBrowse.UseVisualStyleBackColor = true;
            btnJdPdfBrowse.Click += btnJdPdfBrowse_Click;
            // 
            // lblJDPdf
            // 
            lblJDPdf.AutoSize = true;
            lblJDPdf.Location = new Point(15, 340);
            lblJDPdf.Name = "lblJDPdf";
            lblJDPdf.Size = new Size(43, 15);
            lblJDPdf.TabIndex = 47;
            lblJDPdf.Text = "JD Pdf:";
            // 
            // txtJdPdf
            // 
            txtJdPdf.Location = new Point(147, 340);
            txtJdPdf.Name = "txtJdPdf";
            txtJdPdf.ReadOnly = true;
            txtJdPdf.Size = new Size(281, 23);
            txtJdPdf.TabIndex = 48;
            // 
            // lblCompany
            // 
            lblCompany.AutoSize = true;
            lblCompany.Location = new Point(15, 102);
            lblCompany.Name = "lblCompany";
            lblCompany.Size = new Size(100, 15);
            lblCompany.TabIndex = 19;
            lblCompany.Text = "Company Name :";
            // 
            // txtCompany
            // 
            txtCompany.Location = new Point(147, 99);
            txtCompany.Name = "txtCompany";
            txtCompany.Size = new Size(390, 23);
            txtCompany.TabIndex = 20;
            txtCompany.TextChanged += txtCompany_TextChanged;
            // 
            // lblPosition
            // 
            lblPosition.AutoSize = true;
            lblPosition.Location = new Point(15, 137);
            lblPosition.Name = "lblPosition";
            lblPosition.Size = new Size(53, 15);
            lblPosition.TabIndex = 22;
            lblPosition.Text = "Position:";
            // 
            // txtPosition
            // 
            txtPosition.Location = new Point(147, 134);
            txtPosition.Name = "txtPosition";
            txtPosition.Size = new Size(390, 23);
            txtPosition.TabIndex = 24;
            txtPosition.TextChanged += txtPosition_TextChanged;
            // 
            // lblJobDescription
            // 
            lblJobDescription.AutoSize = true;
            lblJobDescription.Location = new Point(15, 170);
            lblJobDescription.Name = "lblJobDescription";
            lblJobDescription.Size = new Size(91, 15);
            lblJobDescription.TabIndex = 26;
            lblJobDescription.Text = "Job Description:";
            // 
            // txtJobDescription
            // 
            txtJobDescription.Location = new Point(147, 170);
            txtJobDescription.Multiline = true;
            txtJobDescription.Name = "txtJobDescription";
            txtJobDescription.ScrollBars = ScrollBars.Vertical;
            txtJobDescription.Size = new Size(390, 120);
            txtJobDescription.TabIndex = 29;
            // 
            // lblCVPath
            // 
            lblCVPath.AutoSize = true;
            lblCVPath.Location = new Point(15, 303);
            lblCVPath.Name = "lblCVPath";
            lblCVPath.Size = new Size(120, 15);
            lblCVPath.TabIndex = 31;
            lblCVPath.Text = "CV/Resume File Path:";
            // 
            // txtCVPath
            // 
            txtCVPath.Location = new Point(147, 303);
            txtCVPath.Name = "txtCVPath";
            txtCVPath.ReadOnly = true;
            txtCVPath.Size = new Size(281, 23);
            txtCVPath.TabIndex = 32;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(457, 299);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(80, 28);
            btnBrowse.TabIndex = 35;
            btnBrowse.Text = "Browse...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // btnProcess
            // 
            btnProcess.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnProcess.Location = new Point(237, 467);
            btnProcess.Name = "btnProcess";
            btnProcess.Size = new Size(140, 35);
            btnProcess.TabIndex = 44;
            btnProcess.Text = "Process CV";
            btnProcess.UseVisualStyleBackColor = true;
            btnProcess.Click += btnProcess_Click;
            // 
            // btnClear
            // 
            btnClear.Font = new Font("Segoe UI", 10F);
            btnClear.Location = new Point(397, 467);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(140, 35);
            btnClear.TabIndex = 45;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // grpBoxCoverLetter
            // 
            grpBoxCoverLetter.Controls.Add(chkAIGenCoverLetter);
            grpBoxCoverLetter.Controls.Add(btnBrowseCoverLetter);
            grpBoxCoverLetter.Controls.Add(txtCoverLetterPath);
            grpBoxCoverLetter.Controls.Add(lblCoverLetterPath);
            grpBoxCoverLetter.Controls.Add(chkLetterToPdf);
            grpBoxCoverLetter.Controls.Add(chkClientOrg);
            grpBoxCoverLetter.Controls.Add(txtSkills);
            grpBoxCoverLetter.Controls.Add(btnLetterClear);
            grpBoxCoverLetter.Controls.Add(btnProcCoverLetter);
            grpBoxCoverLetter.Controls.Add(lblSkills);
            grpBoxCoverLetter.Controls.Add(lblJobCompanyLoc);
            grpBoxCoverLetter.Controls.Add(txtJobCompanyLoc);
            grpBoxCoverLetter.Controls.Add(lblJobPosition);
            grpBoxCoverLetter.Controls.Add(txtJobPosition);
            grpBoxCoverLetter.Controls.Add(lblJobSource);
            grpBoxCoverLetter.Controls.Add(txtJobSource);
            grpBoxCoverLetter.Controls.Add(lblSalutation);
            grpBoxCoverLetter.Controls.Add(txtSalutation);
            grpBoxCoverLetter.Controls.Add(txtAddressTo);
            grpBoxCoverLetter.Controls.Add(lblLetterTo);
            grpBoxCoverLetter.Controls.Add(dtpDate);
            grpBoxCoverLetter.Controls.Add(lblDate);
            grpBoxCoverLetter.Location = new Point(566, 4);
            grpBoxCoverLetter.Name = "grpBoxCoverLetter";
            grpBoxCoverLetter.Size = new Size(509, 528);
            grpBoxCoverLetter.TabIndex = 23;
            grpBoxCoverLetter.TabStop = false;
            grpBoxCoverLetter.Text = "Cover Letter";
            // 
            // chkAIGenCoverLetter
            // 
            chkAIGenCoverLetter.AutoSize = true;
            chkAIGenCoverLetter.Location = new Point(17, 23);
            chkAIGenCoverLetter.Name = "chkAIGenCoverLetter";
            chkAIGenCoverLetter.Size = new Size(189, 19);
            chkAIGenCoverLetter.TabIndex = 55;
            chkAIGenCoverLetter.Text = "Fully AI Generated Cover Letter";
            chkAIGenCoverLetter.UseVisualStyleBackColor = true;
            chkAIGenCoverLetter.CheckedChanged += chkAIGenCoverLetter_CheckedChanged;
            // 
            // btnBrowseCoverLetter
            // 
            btnBrowseCoverLetter.Location = new Point(404, 47);
            btnBrowseCoverLetter.Name = "btnBrowseCoverLetter";
            btnBrowseCoverLetter.Size = new Size(80, 28);
            btnBrowseCoverLetter.TabIndex = 54;
            btnBrowseCoverLetter.Text = "Browse...";
            btnBrowseCoverLetter.UseVisualStyleBackColor = true;
            btnBrowseCoverLetter.Click += btnBrowseCoverLetter_Click;
            // 
            // txtCoverLetterPath
            // 
            txtCoverLetterPath.Location = new Point(136, 49);
            txtCoverLetterPath.Name = "txtCoverLetterPath";
            txtCoverLetterPath.ReadOnly = true;
            txtCoverLetterPath.Size = new Size(252, 23);
            txtCoverLetterPath.TabIndex = 53;
            // 
            // lblCoverLetterPath
            // 
            lblCoverLetterPath.AutoSize = true;
            lblCoverLetterPath.Location = new Point(17, 54);
            lblCoverLetterPath.Name = "lblCoverLetterPath";
            lblCoverLetterPath.Size = new Size(67, 15);
            lblCoverLetterPath.TabIndex = 52;
            lblCoverLetterPath.Text = "Letter Path:";
            // 
            // chkLetterToPdf
            // 
            chkLetterToPdf.AutoSize = true;
            chkLetterToPdf.Location = new Point(282, 420);
            chkLetterToPdf.Name = "chkLetterToPdf";
            chkLetterToPdf.Size = new Size(106, 19);
            chkLetterToPdf.TabIndex = 51;
            chkLetterToPdf.Text = "Convert to PDF";
            chkLetterToPdf.UseVisualStyleBackColor = true;
            // 
            // chkClientOrg
            // 
            chkClientOrg.AutoSize = true;
            chkClientOrg.Location = new Point(136, 420);
            chkClientOrg.Name = "chkClientOrg";
            chkClientOrg.Size = new Size(102, 19);
            chkClientOrg.TabIndex = 50;
            chkClientOrg.Text = "Organization ?";
            chkClientOrg.UseVisualStyleBackColor = true;
            // 
            // txtSkills
            // 
            txtSkills.Location = new Point(136, 343);
            txtSkills.Multiline = true;
            txtSkills.Name = "txtSkills";
            txtSkills.ScrollBars = ScrollBars.Vertical;
            txtSkills.Size = new Size(348, 63);
            txtSkills.TabIndex = 49;
            // 
            // btnLetterClear
            // 
            btnLetterClear.Font = new Font("Segoe UI", 10F);
            btnLetterClear.Location = new Point(424, 467);
            btnLetterClear.Name = "btnLetterClear";
            btnLetterClear.Size = new Size(57, 35);
            btnLetterClear.TabIndex = 48;
            btnLetterClear.Text = "Clear";
            btnLetterClear.UseVisualStyleBackColor = true;
            btnLetterClear.Click += btnLetterClear_Click;
            // 
            // btnProcCoverLetter
            // 
            btnProcCoverLetter.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnProcCoverLetter.Location = new Point(302, 467);
            btnProcCoverLetter.Name = "btnProcCoverLetter";
            btnProcCoverLetter.Size = new Size(116, 35);
            btnProcCoverLetter.TabIndex = 48;
            btnProcCoverLetter.Text = "Process Letter";
            btnProcCoverLetter.UseVisualStyleBackColor = true;
            btnProcCoverLetter.Click += btnProcCoverLetter_Click;
            // 
            // lblSkills
            // 
            lblSkills.AutoSize = true;
            lblSkills.Location = new Point(17, 343);
            lblSkills.Name = "lblSkills";
            lblSkills.Size = new Size(36, 15);
            lblSkills.TabIndex = 39;
            lblSkills.Text = "Skills:";
            // 
            // lblJobCompanyLoc
            // 
            lblJobCompanyLoc.AutoSize = true;
            lblJobCompanyLoc.Location = new Point(17, 307);
            lblJobCompanyLoc.Name = "lblJobCompanyLoc";
            lblJobCompanyLoc.Size = new Size(113, 15);
            lblJobCompanyLoc.TabIndex = 37;
            lblJobCompanyLoc.Text = "Company/Location:";
            // 
            // txtJobCompanyLoc
            // 
            txtJobCompanyLoc.Location = new Point(136, 304);
            txtJobCompanyLoc.Name = "txtJobCompanyLoc";
            txtJobCompanyLoc.Size = new Size(348, 23);
            txtJobCompanyLoc.TabIndex = 36;
            // 
            // lblJobPosition
            // 
            lblJobPosition.AutoSize = true;
            lblJobPosition.Location = new Point(17, 267);
            lblJobPosition.Name = "lblJobPosition";
            lblJobPosition.Size = new Size(53, 15);
            lblJobPosition.TabIndex = 35;
            lblJobPosition.Text = "Position:";
            // 
            // txtJobPosition
            // 
            txtJobPosition.Location = new Point(136, 264);
            txtJobPosition.Name = "txtJobPosition";
            txtJobPosition.Size = new Size(348, 23);
            txtJobPosition.TabIndex = 34;
            // 
            // lblJobSource
            // 
            lblJobSource.AutoSize = true;
            lblJobSource.Location = new Point(17, 228);
            lblJobSource.Name = "lblJobSource";
            lblJobSource.Size = new Size(67, 15);
            lblJobSource.TabIndex = 33;
            lblJobSource.Text = "Job Source:";
            // 
            // txtJobSource
            // 
            txtJobSource.Location = new Point(136, 225);
            txtJobSource.Name = "txtJobSource";
            txtJobSource.Size = new Size(348, 23);
            txtJobSource.TabIndex = 32;
            // 
            // lblSalutation
            // 
            lblSalutation.AutoSize = true;
            lblSalutation.Location = new Point(17, 189);
            lblSalutation.Name = "lblSalutation";
            lblSalutation.Size = new Size(63, 15);
            lblSalutation.TabIndex = 31;
            lblSalutation.Text = "Salutation:";
            // 
            // txtSalutation
            // 
            txtSalutation.Location = new Point(136, 186);
            txtSalutation.Name = "txtSalutation";
            txtSalutation.Size = new Size(348, 23);
            txtSalutation.TabIndex = 30;
            // 
            // txtAddressTo
            // 
            txtAddressTo.Location = new Point(136, 123);
            txtAddressTo.Multiline = true;
            txtAddressTo.Name = "txtAddressTo";
            txtAddressTo.ScrollBars = ScrollBars.Vertical;
            txtAddressTo.Size = new Size(348, 47);
            txtAddressTo.TabIndex = 29;
            // 
            // lblLetterTo
            // 
            lblLetterTo.AutoSize = true;
            lblLetterTo.Location = new Point(17, 126);
            lblLetterTo.Name = "lblLetterTo";
            lblLetterTo.Size = new Size(67, 15);
            lblLetterTo.TabIndex = 25;
            lblLetterTo.Text = "Address To:";
            // 
            // dtpDate
            // 
            dtpDate.Location = new Point(136, 86);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(200, 23);
            dtpDate.TabIndex = 24;
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Location = new Point(17, 89);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(34, 15);
            lblDate.TabIndex = 23;
            lblDate.Text = "Date:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1087, 647);
            Controls.Add(grpBoxCoverLetter);
            Controls.Add(picLoader);
            Controls.Add(lblStatus);
            Controls.Add(grpBoxCVResume);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CV/Resume Customizer Tool";
            ((System.ComponentModel.ISupportInitialize)picLoader).EndInit();
            grpBoxCVResume.ResumeLayout(false);
            grpBoxCVResume.PerformLayout();
            grpBoxCoverLetter.ResumeLayout(false);
            grpBoxCoverLetter.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private PictureBox picLoader;
        private Label lblStatus;
        private GroupBox grpBoxCVResume;
        private Label lblCompany;
        private TextBox txtCompany;
        private Label lblPosition;
        private TextBox txtPosition;
        private Label lblJobDescription;
        private TextBox txtJobDescription;
        private Label lblCVPath;
        private TextBox txtCVPath;
        private Button btnBrowse;
        private Button btnProcess;
        private Button btnClear;
        private GroupBox grpBoxCoverLetter;
        private DateTimePicker dtpDate;
        private Label lblDate;
        private TextBox txtAddressTo;
        private Label lblLetterTo;
        private Label lblSalutation;
        private TextBox txtSalutation;
        private Button btnLetterClear;
        private Button btnProcCoverLetter;
        private Label lblSkills;
        private Label lblJobCompanyLoc;
        private TextBox txtJobCompanyLoc;
        private Label lblJobPosition;
        private TextBox txtJobPosition;
        private Label lblJobSource;
        private TextBox txtJobSource;
        private TextBox txtSkills;
        private CheckBox chkClientOrg;
        private CheckBox chkLetterToPdf;
        private Label lblCoverLetterPath;
        private TextBox txtCoverLetterPath;
        private Button btnBrowseCoverLetter;
        private Label lblJDPdf;
        private TextBox txtJdPdf;
        private Button btnJdPdfBrowse;
        private Label lblMissingSkills;
        private TextBox txtMissingSkills;
        private CheckBox chkAIGenCoverLetter;
        private CheckBox chkMissingSkills;
        private CheckBox chkIsQAGenerate;
        private Label lblRelevantQANum;
        private TextBox txtRelevantQANum;
        private CheckBox chkConvertToPdf;
        private CheckBox chkAddSkillToCv;
        private CheckBox chkJobDescPdf;
    }
}
