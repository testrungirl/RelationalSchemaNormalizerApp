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
            label1 = new Label();
            dataGridView1 = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            label2 = new Label();
            tableName = new TextBox();
            createTableBtn = new Button();
            StatusStrip1 = new StatusStrip();
            systemStatus = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            StatusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(821, 47);
            label1.Name = "label1";
            label1.Size = new Size(245, 38);
            label1.TabIndex = 0;
            label1.Text = "Create New Table";
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3 });
            dataGridView1.GridColor = SystemColors.ButtonFace;
            dataGridView1.Location = new Point(226, 219);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
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
            Column2.MinimumWidth = 8;
            Column2.Name = "Column2";
            // 
            // Column3
            // 
            Column3.HeaderText = "Key Atrribute";
            Column3.MinimumWidth = 8;
            Column3.Name = "Column3";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(226, 160);
            label2.Name = "label2";
            label2.Size = new Size(113, 25);
            label2.TabIndex = 2;
            label2.Text = "Table Name: ";
            // 
            // tableName
            // 
            tableName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tableName.Location = new Point(419, 154);
            tableName.Name = "tableName";
            tableName.Size = new Size(537, 31);
            tableName.TabIndex = 3;
            tableName.Validating += tableName_Validating;
            // 
            // createTableBtn
            // 
            createTableBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            createTableBtn.AutoSize = true;
            createTableBtn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            createTableBtn.BackColor = Color.CornflowerBlue;
            createTableBtn.Location = new Point(1754, 937);
            createTableBtn.Name = "createTableBtn";
            createTableBtn.Size = new Size(238, 35);
            createTableBtn.TabIndex = 4;
            createTableBtn.Text = "Create Table with Atrributes";
            createTableBtn.UseVisualStyleBackColor = false;
            createTableBtn.Click += createTableBtn_Click;
            // 
            // StatusStrip1
            // 
            StatusStrip1.BackColor = SystemColors.ActiveCaption;
            StatusStrip1.ImageScalingSize = new Size(24, 24);
            StatusStrip1.Items.AddRange(new ToolStripItem[] { systemStatus });
            StatusStrip1.Location = new Point(0, 1108);
            StatusStrip1.Name = "StatusStrip1";
            StatusStrip1.Size = new Size(2198, 39);
            StatusStrip1.TabIndex = 5;
            StatusStrip1.Text = "Ready";
            // 
            // systemStatus
            // 
            systemStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            systemStatus.Name = "systemStatus";
            systemStatus.Size = new Size(83, 32);
            systemStatus.Text = "Ready";
            // 
            // CreateTableControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            BackColor = SystemColors.ActiveCaption;
            Controls.Add(StatusStrip1);
            Controls.Add(createTableBtn);
            Controls.Add(tableName);
            Controls.Add(label2);
            Controls.Add(dataGridView1);
            Controls.Add(label1);
            Name = "CreateTableControl";
            Size = new Size(2198, 1147);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            StatusStrip1.ResumeLayout(false);
            StatusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private DataGridView dataGridView1;
        private Label label2;
        private TextBox tableName;
        private Button createTableBtn;
        private StatusStrip StatusStrip1;
        private ToolStripStatusLabel systemStatus;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
    }
}
