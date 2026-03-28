namespace RadarPNCP
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            webView22 = new Microsoft.Web.WebView2.WinForms.WebView2();
            progressBar1 = new ProgressBar();
            labelProgresso = new Label();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            ((System.ComponentModel.ISupportInitialize)webView22).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Location = new Point(1, 1);
            webView21.Name = "webView21";
            webView21.Size = new Size(801, 423);
            webView21.TabIndex = 1;
            webView21.ZoomFactor = 1D;
            // 
            // webView22
            // 
            webView22.AllowExternalDrop = true;
            webView22.BackColor = Color.Cyan;
            webView22.CreationProperties = null;
            webView22.DefaultBackgroundColor = Color.White;
            webView22.Location = new Point(0, 0);
            webView22.Name = "webView22";
            webView22.Size = new Size(804, 453);
            webView22.TabIndex = 3;
            webView22.ZoomFactor = 1D;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(0, 441);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(804, 12);
            progressBar1.TabIndex = 0;
            // 
            // labelProgresso
            // 
            labelProgresso.AutoSize = true;
            labelProgresso.Location = new Point(1, 426);
            labelProgresso.Name = "labelProgresso";
            labelProgresso.Size = new Size(65, 15);
            labelProgresso.TabIndex = 2;
            labelProgresso.Text = "Iniciando...";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(progressBar1);
            panel1.Controls.Add(labelProgresso);
            panel1.Controls.Add(webView22);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(804, 451);
            panel1.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(804, 451);
            Controls.Add(panel1);
            Controls.Add(webView21);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new Size(820, 490);
            MinimumSize = new Size(820, 490);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            ((System.ComponentModel.ISupportInitialize)webView22).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView22;
        private ProgressBar progressBar1;
        private Label labelProgresso;
        private Panel panel1;
    }
}
