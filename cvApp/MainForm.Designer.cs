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
            lblCompany = new Label();
            txtCompany = new TextBox();
            lblPosition = new Label();
            txtPosition = new TextBox();
            lblKeywords = new Label();
            txtKeywords = new TextBox();
            lblCVPath = new Label();
            txtCVPath = new TextBox();
            btnBrowse = new Button();
            chkConvertToPdf = new CheckBox();
            chkWriteToFooter = new CheckBox();
            btnProcess = new Button();
            picLoader = new PictureBox();
            lblStatus = new Label();
            lblRelevantQANum = new Label();
            txtRelevantQANum = new TextBox();
            btnClear = new Button();
            ((System.ComponentModel.ISupportInitialize)picLoader).BeginInit();
            SuspendLayout();
            // 
            // lblCompany
            // 
            lblCompany.AutoSize = true;
            lblCompany.Location = new Point(20, 20);
            lblCompany.Name = "lblCompany";
            lblCompany.Size = new Size(100, 15);
            lblCompany.TabIndex = 0;
            lblCompany.Text = "Company Name :";
            // 
            // txtCompany
            // 
            txtCompany.Location = new Point(180, 17);
            txtCompany.Name = "txtCompany";
            txtCompany.Size = new Size(480, 23);
            txtCompany.TabIndex = 1;
            // 
            // lblPosition
            // 
            lblPosition.AutoSize = true;
            lblPosition.Location = new Point(20, 60);
            lblPosition.Name = "lblPosition";
            lblPosition.Size = new Size(53, 15);
            lblPosition.TabIndex = 2;
            lblPosition.Text = "Position:";
            // 
            // txtPosition
            // 
            txtPosition.Location = new Point(180, 57);
            txtPosition.Name = "txtPosition";
            txtPosition.Size = new Size(480, 23);
            txtPosition.TabIndex = 3;
            // 
            // lblKeywords
            // 
            lblKeywords.AutoSize = true;
            lblKeywords.Location = new Point(20, 100);
            lblKeywords.Name = "lblKeywords";
            lblKeywords.Size = new Size(91, 15);
            lblKeywords.TabIndex = 4;
            lblKeywords.Text = "Job Description:";
            // 
            // txtKeywords
            // 
            txtKeywords.Location = new Point(180, 100);
            txtKeywords.Multiline = true;
            txtKeywords.Name = "txtKeywords";
            txtKeywords.ScrollBars = ScrollBars.Vertical;
            txtKeywords.Size = new Size(480, 120);
            txtKeywords.TabIndex = 5;
            // 
            // lblCVPath
            // 
            lblCVPath.AutoSize = true;
            lblCVPath.Location = new Point(20, 240);
            lblCVPath.Name = "lblCVPath";
            lblCVPath.Size = new Size(73, 15);
            lblCVPath.TabIndex = 6;
            lblCVPath.Text = "CV File Path:";
            // 
            // txtCVPath
            // 
            txtCVPath.Location = new Point(180, 240);
            txtCVPath.Name = "txtCVPath";
            txtCVPath.ReadOnly = true;
            txtCVPath.Size = new Size(390, 23);
            txtCVPath.TabIndex = 7;
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(580, 238);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(80, 28);
            btnBrowse.TabIndex = 8;
            btnBrowse.Text = "Browse...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // chkConvertToPdf
            // 
            chkConvertToPdf.AutoSize = true;
            chkConvertToPdf.Location = new Point(20, 280);
            chkConvertToPdf.Name = "chkConvertToPdf";
            chkConvertToPdf.Size = new Size(106, 19);
            chkConvertToPdf.TabIndex = 9;
            chkConvertToPdf.Text = "Convert to PDF";
            chkConvertToPdf.UseVisualStyleBackColor = true;
            // 
            // chkWriteToFooter
            // 
            chkWriteToFooter.AutoSize = true;
            chkWriteToFooter.Checked = true;
            chkWriteToFooter.CheckState = CheckState.Checked;
            chkWriteToFooter.Location = new Point(20, 305);
            chkWriteToFooter.Name = "chkWriteToFooter";
            chkWriteToFooter.Size = new Size(267, 19);
            chkWriteToFooter.TabIndex = 10;
            chkWriteToFooter.Text = "Write keywords to Footer (default) / Last Page";
            chkWriteToFooter.UseVisualStyleBackColor = true;
            // 
            // btnProcess
            // 
            btnProcess.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnProcess.Location = new Point(239, 374);
            btnProcess.Name = "btnProcess";
            btnProcess.Size = new Size(140, 35);
            btnProcess.TabIndex = 13;
            btnProcess.Text = "Process CV";
            btnProcess.UseVisualStyleBackColor = true;
            btnProcess.Click += btnProcess_Click;
            // 
            // picLoader
            // 
            picLoader.Location = new Point(314, 415);
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
            lblStatus.Location = new Point(20, 486);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(744, 40);
            lblStatus.TabIndex = 16;
            lblStatus.TextAlign = ContentAlignment.TopCenter;
            // 
            // lblRelevantQANum
            // 
            lblRelevantQANum.AutoSize = true;
            lblRelevantQANum.Location = new Point(20, 336);
            lblRelevantQANum.Name = "lblRelevantQANum";
            lblRelevantQANum.Size = new Size(139, 15);
            lblRelevantQANum.TabIndex = 11;
            lblRelevantQANum.Text = "Relevant Q&A to Generate:";
            // 
            // txtRelevantQANum
            // 
            txtRelevantQANum.Location = new Point(180, 333);
            txtRelevantQANum.Name = "txtRelevantQANum";
            txtRelevantQANum.Size = new Size(50, 23);
            txtRelevantQANum.TabIndex = 12;
            txtRelevantQANum.Text = "5";
            // 
            // btnClear
            // 
            btnClear.Font = new Font("Segoe UI", 10F);
            btnClear.Location = new Point(399, 374);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(140, 35);
            btnClear.TabIndex = 14;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 550);
            Controls.Add(lblRelevantQANum);
            Controls.Add(txtRelevantQANum);
            Controls.Add(lblCompany);
            Controls.Add(txtCompany);
            Controls.Add(lblPosition);
            Controls.Add(txtPosition);
            Controls.Add(lblKeywords);
            Controls.Add(txtKeywords);
            Controls.Add(lblCVPath);
            Controls.Add(txtCVPath);
            Controls.Add(btnBrowse);
            Controls.Add(chkConvertToPdf);
            Controls.Add(btnProcess);
            Controls.Add(picLoader);
            Controls.Add(lblStatus);
            Controls.Add(btnClear);
            Controls.Add(chkWriteToFooter);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CV Customizer Tool";
            ((System.ComponentModel.ISupportInitialize)picLoader).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCompany;
        private TextBox txtCompany;
        private Label lblPosition;
        private TextBox txtPosition;
        private Label lblKeywords;
        private TextBox txtKeywords;
        private Label lblCVPath;
        private TextBox txtCVPath;
        private Button btnBrowse;
        private CheckBox chkConvertToPdf;
        private CheckBox chkWriteToFooter;
        private Button btnProcess;
        private PictureBox picLoader;
        private Label lblStatus;
        private Label lblRelevantQANum; // NEW
        private TextBox txtRelevantQANum; // NEW
        private Button btnClear; // NEW
    }
}
