using Imaginator.App.Data;
using Imaginator.App.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Imaginator.App
{
    public partial class MaskForm2 : Form
    {
        public MaskForm2()
        {
            InitializeComponent();
        }

        int CircleSize = 10;
        public Bitmap OriginalImage { get; set; }
        public ImageEdit ParentFrm { get; set; }
        public ImageFile SelectedMask { set; get; }
       
        public MaskForm2(ImageFile SourceImg, ImageEdit Parent)
        {
            InitializeComponent();
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            BtnReset.Click += resetButton_Click;
            numericUpDown1.ValueChanged += (a, b) => { CircleSize = (int)numericUpDown1.Value; };

            using (var ms = new MemoryStream(SourceImg.Content))
            {
                OriginalImage = new Bitmap(ms);
            }
            pictureBox1.Image = OriginalImage;
            BtnSave.Click += (a, b) => { SaveMask(); this.Close(); };
            this.ParentFrm = Parent;
            SelectedMask = SourceImg;
            //pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        void SaveMask()
        {
            var width = Math.Min(OriginalImage.Width, OriginalImage.Height);
            var newImg = new Bitmap(width, width);
            using (var newGraphic = System.Drawing.Graphics.FromImage(newImg))
            {
                newGraphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                newGraphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                newGraphic.Clear(System.Drawing.Color.Transparent);
                newGraphic.DrawImage(pictureBox1.Image, 0, 0, width, width);
                newGraphic.Flush();
            }
            var mem = new MemoryStream();
            newImg.MakeTransparent(Color.Fuchsia);
            newImg.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
            SelectedMask.Content = mem.ToArray();
            SelectedMask.FileName = "Mask " + SelectedMask.FileName;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        Point lastPoint = Point.Empty;//Point.Empty represents null for a Point object

        bool isMouseDown = new Boolean();//this is used to evaluate whether our mousebutton is down or not


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            lastPoint = e.Location;//we assign the lastPoint to the current mouse position: e.Location ('e' is from the MouseEventArgs passed into the MouseDown event)

            isMouseDown = true;//we set to true because our mouse button is down (clicked)

        }
        static SolidBrush brush = new SolidBrush(Color.Fuchsia);
        static Pen pen = new Pen(brush, 1);
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            if (isMouseDown == true)//check to see if the mouse button is down
            {

                using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                {//we need to create a Graphics object to draw on the picture box, its our main tool

                    //when making a Pen object, you can just give it color only or give it color and pen size

                    g.FillCircle(brush, e.Location.X, e.Location.Y, CircleSize);
                    g.DrawCircle(pen, e.Location.X, e.Location.Y, CircleSize);
                    g.SmoothingMode = SmoothingMode.Default;
                    //this is to give the drawing a more smoother, less sharper look
                    //g.Flush();
                }

                pictureBox1.Invalidate();//refreshes the picturebox

                lastPoint = e.Location;//keep assigning the lastPoint to the current mouse position

            }



        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

            isMouseDown = false;

            lastPoint = Point.Empty;

            //set the previous point back to null if the user lets go of the mouse button

        }

        private void resetButton_Click(object sender, EventArgs e)//our clearing button

        {

            if (pictureBox1.Image != null)
            {

                pictureBox1.Image = OriginalImage;

                Invalidate();

            }

        }


    }
  

}
