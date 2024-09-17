using RelationalSchemaNormalizerLibrary.Interfaces;
using RelationalSchemaNormalizerLibrary.Services;

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
        private void InitializeComponent(IAppDBService appDBService, IDynamicDBService dynamicDBService, INormalizerService normalizerService)
        {
            panel1 = new Panel();
            createBtn = new Button();
            homeBtn = new Button();
            createTableControl1 = new CreateTableControl(appDBService, dynamicDBService);
            homeControl1 = new HomeControl(appDBService, dynamicDBService, normalizerService);
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.Controls.Add(createBtn);
            panel1.Controls.Add(homeBtn);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(135, 1127);
            panel1.TabIndex = 0;
            // 
            // createBtn
            // 
            createBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            createBtn.AutoSize = true;
            createBtn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            createBtn.Location = new Point(12, 315);
            createBtn.Name = "createBtn";
            createBtn.Size = new Size(71, 35);
            createBtn.TabIndex = 2;
            createBtn.Text = "Table";
            createBtn.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            createBtn.UseVisualStyleBackColor = true;
            createBtn.Click += createBtn_Click;
            // 
            // homeBtn
            // 
            homeBtn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            homeBtn.AutoSize = true;
            homeBtn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            homeBtn.Location = new Point(12, 255);
            homeBtn.Name = "homeBtn";
            homeBtn.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            homeBtn.Size = new Size(71, 35);
            homeBtn.TabIndex = 1;
            homeBtn.Text = "Home";
            homeBtn.UseVisualStyleBackColor = true;
            homeBtn.Click += homeBtn_Click;
            // 
            // createTableControl1
            // 
            createTableControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            createTableControl1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            createTableControl1.BackColor = SystemColors.ActiveCaption;
            createTableControl1.Location = new Point(150, 12);
            createTableControl1.Name = "createTableControl1";
            createTableControl1.Size = new Size(2029, 1104);
            createTableControl1.TabIndex = 1;
            // 
            // homeControl1
            // 
            homeControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            homeControl1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            homeControl1.BackColor = SystemColors.ActiveCaption;
            homeControl1.Location = new Point(150, 12);
            homeControl1.Name = "homeControl1";
            homeControl1.Size = new Size(2029, 1104);
            homeControl1.TabIndex = 2;
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = SystemColors.MenuHighlight;
            ClientSize = new Size(2182, 1128);
            Controls.Add(homeControl1);
            Controls.Add(panel1);
            Controls.Add(createTableControl1);
            Name = "Dashboard";
            Text = "Dashboard";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button createBtn;
        private Button homeBtn;
        private CreateTableControl createTableControl1;
        private HomeControl homeControl1;
    }
}
