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
using libPLC;
using System.Windows.Threading;
using System.Timers;
using System.Windows.Media.Animation;

namespace libPLC
{
    /// <summary>
    /// Interaction logic for plcSlider.xaml
    /// </summary>
    /// 


    public partial class plcSlider : UserControl
    {
        public plcSlider()
        {
            InitializeComponent();
            slider.PreviewMouseDown += Slider_PreviewMouseDown;
            slider.PreviewMouseMove += Slider_PreviewMouseMove;
            slider.PreviewMouseUp += Slider_PreviewMouseUp;


        }

        public static readonly DependencyProperty inputProperty = DependencyProperty.Register("Input", typeof(iTagObj), typeof(plcSlider), new FrameworkPropertyMetadata(IsTagInPropertyChanged));
        public iTagObj Input
        {
            get { return (iTagObj)GetValue(inputProperty); }
            set { SetValue(inputProperty, value); }
        }
        private static void IsTagInPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            plcSlider ctrl = d as plcSlider;
             ctrl.setTag((iTagObj)e.NewValue);
        }

        public  void setTag(iTagObj tagObj)
        {
           (this.Content as FrameworkElement).DataContext = this;

            if (tagObj != null)
            {
                Max = tagObj.MaxVal.ChangeType<double>();
                Min = tagObj.MinVal.ChangeType<double>();
            }

            Binding myBinding = BindingOperations.GetBinding(this, inputProperty);
            Binding newBinding = new Binding(myBinding.Path.Path + ".Val");
            BindingOperations.SetBinding(this, plcSlider.inputValProperty, newBinding);

        }

        public static readonly DependencyProperty inputValProperty = DependencyProperty.Register("InputVal", typeof(object), typeof(plcSlider), new FrameworkPropertyMetadata(IsInputValPropertyChanged));
        public object InputVal
        {
            get { return (object)GetValue(inputValProperty); }
            set { SetValue(inputValProperty, value); }
        }
        private static void IsInputValPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            plcSlider ctrl = d as plcSlider;
            ctrl.setInputVal((object)e.NewValue);
    
        }
        public void setInputVal(object tag)
        {
            if (tag == null) return;

            if (slider.Width == 0) slider.Width = 50;
    
            Double tagV = 0;
            Double.TryParse(tag.ToString(), out tagV);
            double w = sliderControl.Width - slider.Width;
            double wFact = w / (  Max - Min ) ;

            double newPos = (tagV - Min) * wFact;


            if (Pressed)
            {
                Canvas.SetLeft(slider, newPos);
                double newVal = GetNewVal();
            }
          else
            {
                if (newPos >= w)
                    newPos = w;
                if (newPos < 0)
                    newPos = 0;

                DoubleAnimation moveAni;
                moveAni = new DoubleAnimation(newPos, TimeSpan.FromSeconds(0.2));
                moveAni.FillBehavior = FillBehavior.Stop;
                slider.BeginAnimation(Canvas.LeftProperty, moveAni);
                Canvas.SetLeft(slider, newPos);
                ValueText.Text = String.Format("{0:0.#}", tagV);
                checkMinMax();

            }
           
        }

        public void checkMinMax ()
        {
            dynamic Val, MaxVal, MinVal;
            Val = Convert.ChangeType(Input.Val, Input.OType);
            MaxVal = Convert.ChangeType(Input.MaxVal, Input.OType);
            MinVal = Convert.ChangeType(Input.MinVal, Input.OType);
            if ((MaxVal != 0 && Val > MaxVal) || Val < MinVal)
                ValueText.Foreground = Brushes.Red;
            else
                ValueText.Foreground = Brushes.Black;

        }

        double GetNewVal()
        {
            double leftX = Canvas.GetLeft(slider);
            double w = sliderControl.Width - slider.Width;
            double wFact = (Max - Min) / w;
            double newVal = (leftX * wFact) + Min;
            ValueText.Text = String.Format("{0:0.#}", newVal);
            return newVal;
        }


        private void MoveAni_Completed(object sender, EventArgs e)
        {
            Console.WriteLine("Animation Completed!!!!!!!!!!!!!!!!!");
        }

        double sliderW;
        public double SliderW
        {
            get { return sliderW; }
            set { sliderW = value; }
        }

        double max;
        public double Max
        {
            get { return max; }
            set { max = value; }
        }

        double min;
        public double Min
        {
            get { return min; }
            set { min = value; }
        }

        bool Pressed; 
        Double oldMouseX, oldMouseY;

        private void Slider_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!Pressed) return;

            double oldLeftX = Canvas.GetLeft(slider);

            Point mousePoint = Mouse.GetPosition(this);
            Double deltaX = (mousePoint.X - oldMouseX);
            Double deltaY = (mousePoint.Y - oldMouseY);

            double newPosX = deltaX + oldLeftX;
            if (newPosX < 0) newPosX = 0;
            if ( (newPosX + slider.Width ) > sliderControl.Width) newPosX = sliderControl.Width - slider.Width;

   
           Canvas.SetLeft(slider, newPosX);

           double newVal = GetNewVal();

            if (deltaX != 0 || deltaY != 0)
                mouseMoved = true;

            oldMouseX = mousePoint.X;
            oldMouseY = mousePoint.Y;

        }

        bool mouseMoved = false;


        private void Slider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mouseMoved)
            {
                double newVal = GetNewVal();
                Input.Val = Convert.ChangeType(newVal, Input.OType);
                slider.ReleaseMouseCapture();
                Pressed = false;
                checkMinMax();
            }
            else
            {
                Pressed = false;
                slider.ReleaseMouseCapture();
                sliderMouseUpMove();
            }
        }

    
        private void sliderControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (SliderW == 0)
                slider.Width = 50;
        }

        private void sliderMouseUpMove()
        {
            Point mouseP = Mouse.GetPosition(bgRect);
            double leftX = mouseP.X - (slider.Width / 2);
            if (leftX < 0) leftX = 0;
            if (leftX > sliderControl.Width - slider.Width) leftX = sliderControl.Width - slider.Width;
            double w = sliderControl.Width - slider.Width;
            double wFact = (Max - Min) / w;
            double newVal = (leftX * wFact) + Min;
            Input.Val = Convert.ChangeType(newVal, Input.OType);

        }

        private void moveSliderUp(object sender, MouseButtonEventArgs e)
        {
            sliderMouseUpMove();
        }

        private void Slider_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseMoved = false;
            Pressed = true;
            Point oldMousePoint = Mouse.GetPosition(this);
            oldMouseX = oldMousePoint.X;
            oldMouseY = oldMousePoint.Y;
            slider.CaptureMouse();

 

        }
    }

   
}
