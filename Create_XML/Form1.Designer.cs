namespace Create_XML
{
    partial class Form_Create_xml
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_create = new System.Windows.Forms.Button();
            this.txt_docno = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_create
            // 
            this.btn_create.Location = new System.Drawing.Point(63, 38);
            this.btn_create.Name = "btn_create";
            this.btn_create.Size = new System.Drawing.Size(279, 23);
            this.btn_create.TabIndex = 0;
            this.btn_create.Text = "Gen";
            this.btn_create.UseVisualStyleBackColor = true;
            this.btn_create.Click += new System.EventHandler(this.Btn_create_Click);
            // 
            // txt_docno
            // 
            this.txt_docno.Location = new System.Drawing.Point(63, 12);
            this.txt_docno.Name = "txt_docno";
            this.txt_docno.Size = new System.Drawing.Size(279, 20);
            this.txt_docno.TabIndex = 1;
            this.txt_docno.Text = "BNA-01190410027";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Docno : ";
            // 
            // Form_Create_xml
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 70);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_docno);
            this.Controls.Add(this.btn_create);
            this.Name = "Form_Create_xml";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_create;
        private System.Windows.Forms.TextBox txt_docno;
        private System.Windows.Forms.Label label1;
    }
}

