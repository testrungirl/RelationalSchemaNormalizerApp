﻿namespace RelationalSchemaNormalizerUI
{
    partial class ImageDisplayForm
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
            this.pictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();

            // 
            // pictureBox
            // 
            this.pictureBox.Dock = DockStyle.Fill;
            this.pictureBox.Location = new Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            // 
            // ImageDisplayForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.pictureBox);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ImageDisplayForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "ER Diagram";

            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion
    }
}