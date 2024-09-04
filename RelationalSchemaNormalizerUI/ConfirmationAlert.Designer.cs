namespace RelationalSchemaNormalizerUI
{
    partial class ConfirmationAlert
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
            label1 = new Label();
            yesBtn = new Button();
            noBtn = new Button();
            statusTextBox = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Black", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(49, 85);
            label1.Name = "label1";
            label1.Size = new Size(559, 32);
            label1.TabIndex = 0;
            label1.Text = "Are you sure you want to normalize this table?";
            // 
            // yesBtn
            // 
            yesBtn.BackColor = Color.DarkGreen;
            yesBtn.DialogResult = DialogResult.Yes;
            yesBtn.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            yesBtn.Location = new Point(218, 134);
            yesBtn.Name = "yesBtn";
            yesBtn.Size = new Size(112, 34);
            yesBtn.TabIndex = 1;
            yesBtn.Text = "Yes";
            yesBtn.UseVisualStyleBackColor = false;
            // 
            // noBtn
            // 
            noBtn.BackColor = Color.Red;
            noBtn.DialogResult = DialogResult.Cancel;
            noBtn.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            noBtn.Location = new Point(377, 134);
            noBtn.Name = "noBtn";
            noBtn.Size = new Size(112, 34);
            noBtn.TabIndex = 2;
            noBtn.Text = "No";
            noBtn.UseVisualStyleBackColor = false;
            // 
            // statusTextBox
            // 
            statusTextBox.BackColor = SystemColors.Control;
            statusTextBox.BorderStyle = BorderStyle.None;
            statusTextBox.Location = new Point(49, 12);
            statusTextBox.Multiline = true;
            statusTextBox.Name = "statusTextBox";
            statusTextBox.ReadOnly = true;
            statusTextBox.ScrollBars = ScrollBars.Vertical;
            statusTextBox.Size = new Size(553, 58);
            statusTextBox.TabIndex = 3;
            // 
            // ConfirmationAlert
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(637, 191);
            Controls.Add(statusTextBox);
            Controls.Add(noBtn);
            Controls.Add(yesBtn);
            Controls.Add(label1);
            Name = "ConfirmationAlert";
            Text = "ConfirmationAlert";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button yesBtn;
        private Button noBtn;
        private TextBox statusTextBox;
    }
}