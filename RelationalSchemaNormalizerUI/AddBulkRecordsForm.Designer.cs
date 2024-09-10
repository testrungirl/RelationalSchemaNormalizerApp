namespace RelationalSchemaNormalizerUI
{
    partial class AddBulkRecordsForm
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
            fileInput = new TextBox();
            addRecords = new Button();
            closeModalBtn = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.Location = new Point(12, 86);
            label1.Name = "label1";
            label1.Size = new Size(180, 32);
            label1.TabIndex = 0;
            label1.Text = "Select *csv File";
            // 
            // fileInput
            // 
            fileInput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            fileInput.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            fileInput.Location = new Point(226, 86);
            fileInput.Name = "fileInput";
            fileInput.ReadOnly = true;
            fileInput.Size = new Size(550, 39);
            fileInput.TabIndex = 1;
            // 
            // addRecords
            // 
            addRecords.BackColor = Color.CornflowerBlue;
            addRecords.DialogResult = DialogResult.Yes;
            addRecords.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            addRecords.Location = new Point(348, 169);
            addRecords.Name = "addRecords";
            addRecords.Size = new Size(249, 44);
            addRecords.TabIndex = 2;
            addRecords.Text = "Upload records";
            addRecords.UseVisualStyleBackColor = false;
            // 
            // closeModalBtn
            // 
            closeModalBtn.BackColor = Color.Red;
            closeModalBtn.DialogResult = DialogResult.Cancel;
            closeModalBtn.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            closeModalBtn.Location = new Point(652, 173);
            closeModalBtn.Name = "closeModalBtn";
            closeModalBtn.Size = new Size(124, 40);
            closeModalBtn.TabIndex = 3;
            closeModalBtn.Text = "Close";
            closeModalBtn.UseVisualStyleBackColor = false;
            // 
            // AddBulkRecordsForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(841, 247);
            Controls.Add(closeModalBtn);
            Controls.Add(addRecords);
            Controls.Add(fileInput);
            Controls.Add(label1);
            Name = "AddBulkRecordsForm";
            Text = "Add Bulk Records From *csv";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox fileInput;
        private Button addRecords;
        private Button closeModalBtn;
    }
}