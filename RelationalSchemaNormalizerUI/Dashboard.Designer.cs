namespace RelationalSchemaNormalizerUI
{
    partial class Dashboard
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
            panel1 = new Panel();
            createBtn = new Button();
            homeBtn = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(createBtn);
            panel1.Controls.Add(homeBtn);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(135, 1023);
            panel1.TabIndex = 0;
            // 
            // createBtn
            // 
            createBtn.Location = new Point(12, 352);
            createBtn.Name = "createBtn";
            createBtn.Size = new Size(107, 69);
            createBtn.TabIndex = 2;
            createBtn.Text = "Create Table";
            createBtn.UseVisualStyleBackColor = true;
            // 
            // homeBtn
            // 
            homeBtn.Location = new Point(12, 274);
            homeBtn.Name = "homeBtn";
            homeBtn.Size = new Size(112, 34);
            homeBtn.TabIndex = 1;
            homeBtn.Text = "Home";
            homeBtn.UseVisualStyleBackColor = true;
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.MenuHighlight;
            ClientSize = new Size(2182, 1128);
            Controls.Add(panel1);
            Name = "Dashboard";
            Text = "Dashboard";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button createBtn;
        private Button homeBtn;
    }
}
