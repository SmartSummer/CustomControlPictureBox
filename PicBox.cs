using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TriggerBigImage.Utils;

namespace CustomPictureBox
{
    public partial class PicBox : UserControl
    {
        public PicBox()
        {
            InitializeComponent();
           
        }

        public void InitalData() {

            Real2LogicW = (double)(double)this.PictureBox_Custom.Width / (double)this.Width;
            Real2LogicH = (double)(double)this.PictureBox_Custom.Height / (double)this.Height;
        }

        private double Real2LogicW;
        private double Real2LogicH;


        public int GetPicBoxWidth() {

            return this.PictureBox_Custom.Size.Width;
        }
        public int GetPicBoxHeight()
        {

            return this.PictureBox_Custom.Size.Height;
        }




        public Bitmap GetBMP() {

            try {

                if (isRectReady == true && this.PictureBox_Custom.Image != null)
                {
                    double scw, sch;
                    bool flag = GetScaleWH(out scw, out sch);

                    if (flag == true)
                    {
                        Point newS = new Point();
                        newS.X = (int)(_logic_start_Pont.X * scw);
                        newS.Y = (int)(_logic_start_Pont.Y * sch);

                        Point endS = new Point();
                        endS.X = (int)(_logic_end_Pont.X * scw);
                        endS.Y = (int)(_logic_end_Pont.Y * sch);

                        return ImageManager.GetModelBMP((Bitmap)this.PictureBox_Custom.Image, newS, endS);
                    }
                    
                }
                else
                    return null;
            }
            catch (Exception e) {


                return null;
            }
            return null;
        }



        #region 鼠标事件
        private void BX_MouseDown(object sender, MouseEventArgs e)
        {
            isDown = true;
            isRectReady = false;
            _startPoint = e.Location;

            _logic_start_Pont.X = _startPoint.X;
            _logic_start_Pont.Y =_startPoint.Y;
            Transfer(ref _logic_start_Pont);
        }

        private void Transfer(ref Point p) {

            ////p.X = (int)(p.X * Real2LogicW);
            ////p.Y = (int)(p.Y * Real2LogicH);
        }
        private void BX_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDown == true)
            {

                _endPoint = e.Location;
                _logic_end_Pont.X = _endPoint.X;
                _logic_end_Pont.Y = _endPoint.Y;
                Transfer(ref _logic_end_Pont);

                JudeBorder();
                PictureBoxDraw.DrawRect(ref this.PictureBox_Custom, _startPoint, _endPoint, new Pen(Brushes.Blue));
                isRectReady = true;
            }
            isDown = false;
        }

        private void BX_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDown == true)
            {
                _endPoint = e.Location;
                _logic_end_Pont.X = _endPoint.X;
                _logic_end_Pont.Y = _endPoint.Y;
                Transfer(ref _logic_end_Pont);
                JudeBorder();
                PictureBoxDraw.DrawRect(ref this.PictureBox_Custom, _startPoint, _endPoint, new Pen(Brushes.Blue));
                isRectReady = true;
            }
        }


        public Point _startPoint;
        public Point _endPoint;

        private Point _logic_start_Pont;
        private Point _logic_end_Pont;
        public bool isDown { get; set; }
        public bool isRectReady { get; set; }
        public double ScaleHeight;
        public double ScaleWidth;
        public bool GetScaleWH(out double scaleWidth,out double scaleHeight) {

            scaleWidth = 0.0;
            scaleHeight = 0.0;
            if (PictureBox_Custom.Image != null) {

                scaleWidth = (double)((double)PictureBox_Custom.Image.Width / (double)PictureBox_Custom.Size.Width);
                scaleHeight = (double)((double)PictureBox_Custom.Image.Height / (double)PictureBox_Custom.Size.Height);

                ScaleWidth = scaleWidth;
                ScaleWidth = scaleHeight;
                return true;
            }

            return false;
        }
        private void JudeBorder()
        {


            if (_endPoint.X > PictureBox_Custom.Size.Width)
            {

                _endPoint.X = PictureBox_Custom.Size.Width - 1;
            }

            if (_endPoint.Y > PictureBox_Custom.Size.Height)
            {

                _endPoint.Y = PictureBox_Custom.Size.Height - 1;
            }
        }
        #endregion



        public void SycShowRect() {

            if (this.PictureBox_Custom.InvokeRequired == true)
            {
                DrawRect dr = new DrawRect(PictureBoxDraw.DrawRect);
                this.PictureBox_Custom.Invoke(dr, this.PictureBox_Custom, _startPoint, _endPoint, new Pen(Brushes.Blue));
            }
            else {
                PictureBoxDraw.DrawRect(ref this.PictureBox_Custom, _startPoint, _endPoint, new Pen(Brushes.Blue));

            }

        }

        public void SycShowRect(Bitmap bmp)
        {

            if (this.PictureBox_Custom.InvokeRequired == true)
            {

                DrawRect dr = new DrawRect(PictureBoxDraw.DrawRect);
                this.PictureBox_Custom.Invoke(dr, this.PictureBox_Custom, _startPoint, _endPoint, new Pen(Brushes.Blue));
            }
            else
            {
                PictureBoxDraw.DrawRect(ref this.PictureBox_Custom, _startPoint, _endPoint, new Pen(Brushes.Blue));

            }

        }
        delegate void DrawRect(ref PictureBox pb, Point start, Point end, Pen p);

    }
}
