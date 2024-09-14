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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
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
            verifyNormalizationBtn = new Button();
            orignalTable = new Button();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)recordsFromDB).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left;
            label1.Font = new Font("Segoe UI", 26F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(3, 12);
            label1.Margin = new Padding(3, 7, 3, 0);
            label1.Name = "label1";
            label1.Size = new Size(339, 96);
            label1.TabIndex = 1;
            label1.Text = "Table:";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.BackgroundImageLayout = ImageLayout.None;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21.53558F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 78.46442F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 367F));
            tableLayoutPanel1.Controls.Add(tableName, 1, 0);
            tableLayoutPanel1.Controls.Add(addRecordFromFileBtn, 2, 0);
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Location = new Point(60, 19);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(1970, 114);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // tableName
            // 
            tableName.Anchor = AnchorStyles.Left;
            tableName.Font = new Font("Segoe UI", 26F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tableName.Location = new Point(348, 12);
            tableName.Margin = new Padding(3, 7, 3, 0);
            tableName.Name = "tableName";
            tableName.Size = new Size(1230, 96);
            tableName.TabIndex = 3;
            tableName.Text = "Table Name";
            // 
            // addRecordFromFileBtn
            // 
            addRecordFromFileBtn.Anchor = AnchorStyles.None;
            addRecordFromFileBtn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            addRecordFromFileBtn.BackColor = Color.CornflowerBlue;
            addRecordFromFileBtn.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            addRecordFromFileBtn.Location = new Point(1619, 24);
            addRecordFromFileBtn.Name = "addRecordFromFileBtn";
            addRecordFromFileBtn.Size = new Size(333, 65);
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
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            recordsFromDB.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            recordsFromDB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            recordsFromDB.DefaultCellStyle = dataGridViewCellStyle2;
            recordsFromDB.GridColor = SystemColors.ButtonFace;
            recordsFromDB.Location = new Point(3, 3);
            recordsFromDB.Name = "recordsFromDB";
            recordsFromDB.ReadOnly = true;
            recordsFromDB.RowHeadersWidth = 62;
            recordsFromDB.RowTemplate.Height = 45;
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
            functDepText.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
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
            funcDepenBtn.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            funcDepenBtn.Location = new Point(580, 993);
            funcDepenBtn.Name = "funcDepenBtn";
            funcDepenBtn.Size = new Size(441, 48);
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
            twoNFBtn.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            twoNFBtn.Location = new Point(263, 993);
            twoNFBtn.Name = "twoNFBtn";
            twoNFBtn.Size = new Size(316, 48);
            twoNFBtn.TabIndex = 5;
            twoNFBtn.Text = "View Tables in 2nd NF";
            twoNFBtn.UseVisualStyleBackColor = false;
            twoNFBtn.Visible = false;
            twoNFBtn.Click += twoNFBtn_Click;
            // 
            // threeNFBtn
            // 
            threeNFBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            threeNFBtn.AutoSize = true;
            threeNFBtn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            threeNFBtn.BackColor = Color.CornflowerBlue;
            threeNFBtn.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            threeNFBtn.Location = new Point(1564, 993);
            threeNFBtn.Name = "threeNFBtn";
            threeNFBtn.Size = new Size(318, 48);
            threeNFBtn.TabIndex = 6;
            threeNFBtn.Text = "View Tables in 3rd NF ";
            threeNFBtn.UseVisualStyleBackColor = false;
            threeNFBtn.Visible = false;
            threeNFBtn.Click += threeNFBtn_Click;
            // 
            // verifyNormalizationBtn
            // 
            verifyNormalizationBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            verifyNormalizationBtn.AutoSize = true;
            verifyNormalizationBtn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            verifyNormalizationBtn.BackColor = Color.CornflowerBlue;
            verifyNormalizationBtn.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            verifyNormalizationBtn.Location = new Point(1253, 993);
            verifyNormalizationBtn.Name = "verifyNormalizationBtn";
            verifyNormalizationBtn.Size = new Size(239, 48);
            verifyNormalizationBtn.TabIndex = 7;
            verifyNormalizationBtn.Text = "Normalize Table";
            verifyNormalizationBtn.UseVisualStyleBackColor = false;
            verifyNormalizationBtn.Visible = false;
            verifyNormalizationBtn.Click += verifyNormalizationBtn_Click;
            // 
            // orignalTable
            // 
            orignalTable.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            orignalTable.AutoSize = true;
            orignalTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            orignalTable.BackColor = Color.CornflowerBlue;
            orignalTable.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            orignalTable.Location = new Point(1052, 993);
            orignalTable.Name = "orignalTable";
            orignalTable.Size = new Size(180, 48);
            orignalTable.TabIndex = 8;
            orignalTable.Text = "Initial Table";
            orignalTable.UseVisualStyleBackColor = false;
            orignalTable.Visible = false;
            orignalTable.Click += orignalTable_Click;
            // 
            // TableControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = SystemColors.ActiveCaption;
            Controls.Add(orignalTable);
            Controls.Add(verifyNormalizationBtn);
            Controls.Add(threeNFBtn);
            Controls.Add(twoNFBtn);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(funcDepenBtn);
            Controls.Add(tableLayoutPanel1);
            Name = "TableControl";
            Size = new Size(2099, 1091);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)recordsFromDB).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
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
        private TextBox functDepText;
        private Button verifyNormalizationBtn;
        private Button orignalTable;
    }
}
