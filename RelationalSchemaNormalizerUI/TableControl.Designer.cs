namespace RelationalSchemaNormalizerUI
{
    partial class TableControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableName = new Label();
            addRecords = new Button();
            dataGridView1 = new DataGridView();
            tableLayoutPanel2 = new TableLayoutPanel();
            button2 = new Button();
            twoNFBtn = new Button();
            threeNFBtn = new Button();
            functDepText = new TextBox();
            statusStrip1 = new StatusStrip();
            systemStatus = new ToolStripStatusLabel();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(784, 29);
            label1.Margin = new Padding(3, 7, 3, 0);
            label1.Name = "label1";
            label1.Size = new Size(86, 38);
            label1.TabIndex = 1;
            label1.Text = "Table";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackgroundImageLayout = ImageLayout.None;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.8149F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49.1851F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 251F));
            tableLayoutPanel1.Controls.Add(tableName, 1, 0);
            tableLayoutPanel1.Controls.Add(addRecords, 2, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Location = new Point(60, 19);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(1970, 89);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // tableName
            // 
            tableName.Anchor = AnchorStyles.Left;
            tableName.AutoSize = true;
            tableName.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tableName.Location = new Point(876, 29);
            tableName.Margin = new Padding(3, 7, 3, 0);
            tableName.Name = "tableName";
            tableName.Size = new Size(172, 38);
            tableName.TabIndex = 3;
            tableName.Text = "Table Name";
            // 
            // addRecords
            // 
            addRecords.Anchor = AnchorStyles.None;
            addRecords.BackColor = Color.CornflowerBlue;
            addRecords.Location = new Point(1727, 20);
            addRecords.Name = "addRecords";
            addRecords.Size = new Size(234, 49);
            addRecords.TabIndex = 2;
            addRecords.Text = "Bulk Upload from CSV";
            addRecords.UseVisualStyleBackColor = false;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.GridColor = SystemColors.ButtonFace;
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(1964, 629);
            dataGridView1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(dataGridView1, 0, 0);
            tableLayoutPanel2.Controls.Add(functDepText, 0, 1);
            tableLayoutPanel2.Location = new Point(60, 139);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 78.81166F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 21.1883411F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(1970, 806);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button2.BackColor = Color.CornflowerBlue;
            button2.Location = new Point(1796, 951);
            button2.Name = "button2";
            button2.Size = new Size(234, 62);
            button2.TabIndex = 4;
            button2.Text = "Check Functional Dependencies";
            button2.UseVisualStyleBackColor = false;
            // 
            // twoNFBtn
            // 
            twoNFBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            twoNFBtn.BackColor = Color.CornflowerBlue;
            twoNFBtn.Location = new Point(1297, 951);
            twoNFBtn.Name = "twoNFBtn";
            twoNFBtn.Size = new Size(234, 62);
            twoNFBtn.TabIndex = 5;
            twoNFBtn.Text = "Conver to 2 NF";
            twoNFBtn.UseVisualStyleBackColor = false;
            twoNFBtn.Visible = false;
            // 
            // threeNFBtn
            // 
            threeNFBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            threeNFBtn.BackColor = Color.CornflowerBlue;
            threeNFBtn.Location = new Point(1546, 951);
            threeNFBtn.Name = "threeNFBtn";
            threeNFBtn.Size = new Size(234, 62);
            threeNFBtn.TabIndex = 6;
            threeNFBtn.Text = "Convert to 3 NF";
            threeNFBtn.UseVisualStyleBackColor = false;
            threeNFBtn.Visible = false;
            // 
            // functDepText
            // 
            functDepText.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            functDepText.BackColor = SystemColors.GradientInactiveCaption;
            functDepText.Location = new Point(3, 650);
            functDepText.Multiline = true;
            functDepText.Name = "functDepText";
            functDepText.ReadOnly = true;
            functDepText.ScrollBars = ScrollBars.Horizontal;
            functDepText.Size = new Size(1964, 153);
            functDepText.TabIndex = 4;
            functDepText.Visible = false;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = SystemColors.ActiveCaption;
            statusStrip1.BackgroundImageLayout = ImageLayout.None;
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { systemStatus });
            statusStrip1.Location = new Point(0, 1052);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(2099, 39);
            statusStrip1.TabIndex = 7;
            statusStrip1.Text = "statusStrip1";
            // 
            // systemStatus
            // 
            systemStatus.AccessibleName = "systemStatus";
            systemStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            systemStatus.ImageAlign = ContentAlignment.TopRight;
            systemStatus.Name = "systemStatus";
            systemStatus.Size = new Size(83, 32);
            systemStatus.Text = "Ready";
            // 
            // TableControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            Controls.Add(statusStrip1);
            Controls.Add(threeNFBtn);
            Controls.Add(twoNFBtn);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(button2);
            Controls.Add(tableLayoutPanel1);
            Name = "TableControl";
            Size = new Size(2099, 1091);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TableLayoutPanel tableLayoutPanel1;
        private Button addRecords;
        private Label tableName;
        private DataGridView dataGridView1;
        private TableLayoutPanel tableLayoutPanel2;
        private Button button2;
        private TextBox functDepText;
        private Button twoNFBtn;
        private Button threeNFBtn;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel systemStatus;
    }
}
