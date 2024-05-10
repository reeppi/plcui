using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace libPLC
{
    /// <summary>
    /// Interaction logic for plcIndicator.xaml
    /// </summary>
    /// 
  
    public partial class plcImage : UserControl
    {
        // double widthImage, heightImage;
        bool small;
        ImageSource imgOff = null;
        ImageSource imgOn = null;

        imgI img;
        public imgI Img { get
            {
                return img;
            }
                set {
                img = value;
                setImages();
            }
        } 

        public bool Small
        {
            get { return small; }
            set {
                small = value;
                setImages();
            }
        }

        public ImageSource ImgOff
        {
            get { return imgOff; }
            set
            {
                imgOff = value;
                indicatorImageOff.Source = imgOff;
                setImages();
            }
        }

        public ImageSource ImgOn
        {
            get { return imgOn; }
            set
            {
                imgOn = value;
                indicatorImageOn.Source = imgOn;
                setImages();
            }
        }

        public bool Input
        {
            get { return (bool)GetValue(inputProperty); }
            set {
                Console.WriteLine("Writing Input :" + value);
                setSize();
                changeValue(value);
                SetValue(inputProperty, value);
            }
        }

        public static readonly DependencyProperty inputProperty = DependencyProperty.Register("Input", typeof(bool), typeof(plcImage), new FrameworkPropertyMetadata(IsInputPropertyChanged));

        private static void IsInputPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Console.WriteLine("IsInputPropertyChanged");
            plcImage ctrl = d as plcImage;
            ctrl.changeValue((bool)e.NewValue);
        }


        public plcImage() : base()
        {
            InitializeComponent();
        }

        public void changeValue(bool setVal)
        {
            double time = 0.2;
            DoubleAnimation aniOn, aniOff;
            if (setVal)
            {
                indicatorImageOn.Opacity = 0;
                indicatorImageOff.Opacity = 1;
                aniOn = new DoubleAnimation(1, TimeSpan.FromSeconds(time));
                indicatorImageOn.BeginAnimation(Image.OpacityProperty, aniOn);
                aniOff = new DoubleAnimation(0, TimeSpan.FromSeconds(time));
                indicatorImageOff.BeginAnimation(Image.OpacityProperty, aniOff);
            }
            else
            {
                //indicatorImageOn.Opacity = 1;
                //indicatorImageOff.Opacity = 0;
                aniOn = new DoubleAnimation(0, TimeSpan.FromSeconds(time));
                indicatorImageOn.BeginAnimation(Image.OpacityProperty, aniOn);
                aniOff = new DoubleAnimation(1, TimeSpan.FromSeconds(time));
                indicatorImageOff.BeginAnimation(Image.OpacityProperty, aniOff);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetValue(inputProperty, !Input);
        }

        private void setImages()
        {
            if (classData.res == null)
                classData.res = new classRes();

            if (imgOff == null)
            {
                if (!Small)
                    indicatorImageOff.Source = classData.res.images[imgI.indicator].imgOff;
                else
                    indicatorImageOff.Source = classData.res.images[imgI.indicatorSmall].imgOff;
            }
            if (imgOn == null)
            {
                if (!Small)
                    indicatorImageOn.Source = classData.res.images[imgI.indicator].imgOn;
                else
                    indicatorImageOn.Source = classData.res.images[imgI.indicatorSmall].imgOn;
            }

            if (Img != imgI.none && imgOn == null)
                indicatorImageOn.Source = classData.res.images[Img].imgOn;

            if (Img != imgI.none && imgOff == null)
                indicatorImageOff.Source = classData.res.images[Img].imgOff;

            setSize();
        }

        private void setSize()
        {
            if (Input)
            {
                this.Width = indicatorImageOn.Source.Width;
                this.Height = indicatorImageOn.Source.Height;
            }
            else
            {
                this.Width = indicatorImageOff.Source.Width;
                this.Height = indicatorImageOff.Source.Height;
            }
        }

        private void indicatorImageOn_Loaded(object sender, RoutedEventArgs e)
        {
            setImages();
        }
    }
}
