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
            addRecordFromFileBtn = new Button();
            recordsFromDB = new DataGridView();
            tableLayoutPanel2 = new TableLayoutPanel();
            functDepText = new TextBox();
            funcDepenBtn = new Button();
            twoNFBtn = new Button();
            threeNFBtn = new Button();
            statusStrip1 = new StatusStrip();
            systemStatus = new ToolStripStatusLabel();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)recordsFromDB).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(776, 29);
            label1.Margin = new Padding(3, 7, 3, 0);
            label1.Name = "label1";
            label1.Size = new Size(94, 38);
            label1.TabIndex = 1;
            label1.Text = "Table:";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.BackgroundImageLayout = ImageLayout.None;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.8149F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 49.1851F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 251F));
            tableLayoutPanel1.Controls.Add(tableName, 1, 0);
            tableLayoutPanel1.Controls.Add(addRecordFromFileBtn, 2, 0);
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
            // addRecordFromFileBtn
            // 
            addRecordFromFileBtn.Anchor = AnchorStyles.None;
            addRecordFromFileBtn.AutoSize = true;
            addRecordFromFileBtn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            addRecordFromFileBtn.BackColor = Color.CornflowerBlue;
            addRecordFromFileBtn.Location = new Point(1744, 27);
            addRecordFromFileBtn.Name = "addRecordFromFileBtn";
            addRecordFromFileBtn.Size = new Size(199, 35);
            addRecordFromFileBtn.TabIndex = 2;
            addRecordFromFileBtn.Text = "Bulk Upload from CSV";
            addRecordFromFileBtn.UseVisualStyleBackColor = false;
            addRecordFromFileBtn.Click += addRecordFromFileBtn_Click;
            // 
            // recordsFromDB
            // 
            recordsFromDB.AllowUserToAddRows = false;
            recordsFromDB.AllowUserToDeleteRows = false;
            recordsFromDB.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            recordsFromDB.BackgroundColor = SystemColors.ButtonHighlight;
            recordsFromDB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            recordsFromDB.GridColor = SystemColors.ButtonFace;
            recordsFromDB.Location = new Point(3, 3);
            recordsFromDB.Name = "recordsFromDB";
            recordsFromDB.ReadOnly = true;
            recordsFromDB.RowHeadersWidth = 62;
            recordsFromDB.Size = new Size(1574, 827);
            recordsFromDB.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 390F));
            tableLayoutPanel2.Controls.Add(functDepText, 1, 0);
            tableLayoutPanel2.Controls.Add(recordsFromDB, 0, 0);
            tableLayoutPanel2.Location = new Point(60, 139);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 78.81166F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(1970, 833);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // functDepText
            // 
            functDepText.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            functDepText.BackColor = SystemColors.GradientInactiveCaption;
            functDepText.Location = new Point(1583, 3);
            functDepText.Multiline = true;
            functDepText.Name = "functDepText";
            functDepText.ReadOnly = true;
            functDepText.ScrollBars = ScrollBars.Horizontal;
            functDepText.Size = new Size(384, 827);
            functDepText.TabIndex = 5;
            functDepText.Visible = false;
            // 
            // funcDepenBtn
            // 
            funcDepenBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            funcDepenBtn.AutoSize = true;
            funcDepenBtn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            funcDepenBtn.BackColor = Color.CornflowerBlue;
            funcDepenBtn.Location = new Point(1776, 1014);
            funcDepenBtn.Name = "funcDepenBtn";
            funcDepenBtn.Size = new Size(271, 35);
            funcDepenBtn.TabIndex = 4;
            funcDepenBtn.Text = "Check Functional Dependencies";
            funcDepenBtn.UseVisualStyleBackColor = false;
            funcDepenBtn.Click += funcDepenBtn_Click;
            // 
            // twoNFBtn
            // 
            twoNFBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            twoNFBtn.AutoSize = true;
            twoNFBtn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            twoNFBtn.BackColor = Color.CornflowerBlue;
            twoNFBtn.Location = new Point(1389, 1014);
            twoNFBtn.Name = "twoNFBtn";
            twoNFBtn.Size = new Size(142, 35);
            twoNFBtn.TabIndex = 5;
            twoNFBtn.Text = "Conver to 2 NF";
            twoNFBtn.UseVisualStyleBackColor = false;
            twoNFBtn.Visible = false;
            // 
            // threeNFBtn
            // 
            threeNFBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            threeNFBtn.AutoSize = true;
            threeNFBtn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            threeNFBtn.BackColor = Color.CornflowerBlue;
            threeNFBtn.Location = new Point(1594, 1014);
            threeNFBtn.Name = "threeNFBtn";
            threeNFBtn.Size = new Size(148, 35);
            threeNFBtn.TabIndex = 6;
            threeNFBtn.Text = "Convert to 3 NF";
            threeNFBtn.UseVisualStyleBackColor = false;
            threeNFBtn.Visible = false;
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
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = SystemColors.ActiveCaption;
            Controls.Add(statusStrip1);
            Controls.Add(threeNFBtn);
            Controls.Add(twoNFBtn);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(funcDepenBtn);
            Controls.Add(tableLayoutPanel1);
            Name = "TableControl";
            Size = new Size(2099, 1091);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)recordsFromDB).EndInit();
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
        private Button addRecordFromFileBtn;
        private Label tableName;
        private DataGridView recordsFromDB;
        private TableLayoutPanel tableLayoutPanel2;
        private Button funcDepenBtn;
        private Button twoNFBtn;
        private Button threeNFBtn;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel systemStatus;
        private TextBox functDepText;
    }
}
