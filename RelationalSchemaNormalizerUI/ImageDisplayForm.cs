using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RelationalSchemaNormalizerUI
{
    public partial class ImageDisplayForm: Form
    {
        private PictureBox pictureBox;

        public ImageDisplayForm(Image image)
        {
            InitializeComponent();
            SetImageAndSize(image);
        }

        private void SetImageAndSize(Image image)
        {
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            int maxWidth = (int)(screenBounds.Width * 0.8);
            int maxHeight = (int)(screenBounds.Height * 0.8);

            double widthRatio = (double)maxWidth / image.Width;
            double heightRatio = (double)maxHeight / image.Height;
            double ratio = Math.Min(widthRatio, heightRatio);

            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);

            this.ClientSize = new Size(newWidth, newHeight);
            pictureBox.Image = image;
        }
       
    }
}
