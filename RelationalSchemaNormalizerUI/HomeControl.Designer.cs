using RelationalSchemaNormalizerLibrary.Interfaces;

namespace RelationalSchemaNormalizerUI
{
    partial class HomeControl
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
        private void InitializeComponent(IAppDBService appDbService)
        {
            tableControl1 = new TableControl();
            tablesControl1 = new TablesControl(appDbService);
            SuspendLayout();
            // 
            // tableControl1
            // 
            tableControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableControl1.AutoSize = true;
            tableControl1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableControl1.BackColor = SystemColors.ActiveCaption;
            tableControl1.Location = new Point(238, 51);
            tableControl1.Name = "tableControl1";
            tableControl1.Size = new Size(1661, 1091);
            tableControl1.TabIndex = 0;
            tableControl1.Visible = false;
            // 
            // tablesControl1
            // 
            tablesControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tablesControl1.BackColor = SystemColors.ActiveCaption;
            tablesControl1.Location = new Point(238, 51);
            tablesControl1.Name = "tablesControl1";
            tablesControl1.Size = new Size(1661, 1091);
            tablesControl1.TabIndex = 1;
            // 
            // HomeControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            Controls.Add(tablesControl1);
            Controls.Add(tableControl1);
            Name = "HomeControl";
            Size = new Size(2200, 1207);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableControl tableControl1;
        private TablesControl tablesControl1;
    }
}
