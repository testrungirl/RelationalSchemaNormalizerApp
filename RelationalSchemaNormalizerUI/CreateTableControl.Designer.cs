namespace RelationalSchemaNormalizerUI
{
    partial class CreateTableControl
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            label1 = new Label();
            dataGridView1 = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewComboBoxColumn();
            Column3 = new DataGridViewComboBoxColumn();
            label2 = new Label();
            tableName = new TextBox();
            createTableBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 26F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(821, 47);
            label1.Name = "label1";
            label1.Size = new Size(454, 70);
            label1.TabIndex = 0;
            label1.Text = "Create New Table";
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3 });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.GridColor = SystemColors.ButtonFace;
            dataGridView1.Location = new Point(226, 266);
            dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 14F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.RowTemplate.Height = 45;
            dataGridView1.Size = new Size(1749, 673);
            dataGridView1.TabIndex = 1;
            dataGridView1.CellValidating += dataGridView1_CellValidating;
            dataGridView1.EditingControlShowing += dataGridView1_EditingControlShowing;
            dataGridView1.RowValidating += dataGridView1_RowValidating;
            // 
            // Column1
            // 
            Column1.HeaderText = "Attribute";
            Column1.MinimumWidth = 8;
            Column1.Name = "Column1";
            // 
            // Column2
            // 
            Column2.HeaderText = "Data Type";
            Column2.Items.AddRange(new object[] { "string", "boolean", "char", "datetime", "double", "float", "guid", "int" });
            Column2.MinimumWidth = 8;
            Column2.Name = "Column2";
            // 
            // Column3
            // 
            Column3.HeaderText = "Key Atrribute";
            Column3.Items.AddRange(new object[] { "true", "false" });
            Column3.MinimumWidth = 8;
            Column3.Name = "Column3";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label2.Location = new Point(227, 185);
            label2.Name = "label2";
            label2.Size = new Size(188, 38);
            label2.TabIndex = 2;
            label2.Text = "Table Name: ";
            label2.TextAlign = ContentAlignment.BottomCenter;
            // 
            // tableName
            // 
            tableName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableName.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            tableName.Location = new Point(420, 179);
            tableName.Name = "tableName";
            tableName.Size = new Size(537, 45);
            tableName.TabIndex = 3;
            tableName.Validating += tableName_Validating;
            // 
            // createTableBtn
            // 
            createTableBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            createTableBtn.AutoSize = true;
            createTableBtn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            createTableBtn.BackColor = Color.CornflowerBlue;
            createTableBtn.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            createTableBtn.Location = new Point(1327, 980);
            createTableBtn.Name = "createTableBtn";
            createTableBtn.Size = new Size(394, 48);
            createTableBtn.TabIndex = 4;
            createTableBtn.Text = "Create Table with Attributes";
            createTableBtn.UseVisualStyleBackColor = false;
            createTableBtn.Click += createTableBtn_Click;
            // 
            // CreateTableControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = SystemColors.ActiveCaption;
            Controls.Add(createTableBtn);
            Controls.Add(tableName);
            Controls.Add(label2);
            Controls.Add(dataGridView1);
            Controls.Add(label1);
            Name = "CreateTableControl";
            Size = new Size(2198, 1147);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private DataGridView dataGridView1;
        private Label label2;
        private TextBox tableName;
        private Button createTableBtn;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewComboBoxColumn Column2;
        private DataGridViewComboBoxColumn Column3;
    }
}
