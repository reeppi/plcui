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
using System.Windows.Media.Animation;

namespace libPLC
{
    /// <summary>
    /// Interaction logic for plcGauge.xaml
    /// </summary>
    public partial class plcGauge : UserControl
    {

        public Canvas NeedleCanvas { get; set;  }
        public TextBlock textblock { get; set;  }

        public plcGauge()
        {
            InitializeComponent();

      
        }

        double CenterX { get; set;}
        double CenterY { get; set; }

        public static readonly DependencyProperty inputProperty = DependencyProperty.Register("Input", typeof(iTagObj), typeof(plcGauge), new FrameworkPropertyMetadata(IsTagInPropertyChanged));
        public iTagObj Input
        {
            get { return (iTagObj)GetValue(inputProperty); }
            set { SetValue(inputProperty, value); }
        }
        private static void IsTagInPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            plcGauge ctrl = d as plcGauge;
            ctrl.setTag((iTagObj)e.NewValue);
        }
        public void setTag(iTagObj tagObj)
        {
            Binding myBinding = BindingOperations.GetBinding(this, inputProperty);
            Binding newBinding = new Binding(myBinding.Path.Path + ".Val");
            BindingOperations.SetBinding(this, plcGauge.inputValProperty, newBinding);
        }

        public static readonly DependencyProperty inputValProperty = DependencyProperty.Register("InputVal", typeof(object), typeof(plcGauge), new FrameworkPropertyMetadata(IsInputValPropertyChanged));
        public object InputVal
        {
            get { return (object)GetValue(inputValProperty); }
            set { SetValue(inputValProperty, value); }
        }
        private static void IsInputValPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            plcGauge ctrl = d as plcGauge;
            ctrl.setInputVal((object)e.NewValue);
        }


        public void setInputVal(object tag)
        {
            if (tag == null) return;

            Double MinV = Input.MinVal.ChangeType<double>();
            Double MaxV = Input.MaxVal.ChangeType<double>();
            double fact = 270 / (MaxV  - MinV);
            Double tagV = tag.ChangeType<double>();
     
            if (NeedleCanvas == null) LoadControl();
            DoubleAnimation moveAni;

            double newVal = 0;
            if (( (tagV-MinV) * fact) > 270)
                newVal = 270 + 45;
            else
                newVal = ( (tagV-MinV) * fact) + 45;
            moveAni = new DoubleAnimation(newVal, TimeSpan.FromSeconds(0.2));
            RotateTransform rotateTransform1 = new RotateTransform(oldVal);
            NeedleCanvas.RenderTransform = rotateTransform1;
            rotateTransform1.BeginAnimation(RotateTransform.AngleProperty, moveAni);

            string valTxt = String.Format("{0:0.#%}", (tagV-MinV)/(MaxV - MinV));
            bool error = false;
            if (tagV > MaxV && MaxV != 0) error = true;
            setText(valTxt,error);

            oldVal = newVal;
            
        }

        double oldVal=0;

        int logger = 0;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if ( NeedleCanvas == null)
                LoadControl();

            logger++;
            Console.WriteLine("logger :" + logger);
        }

        public int MLine { get; set; }

        public Brush GaugeColor { get; set; }

 
        public void LoadControl()
        {
            Console.WriteLine("GAUGE: Load Control");
            double gaugeSize = Math.Min(this.Width, this.Height);
            double needleLen = (gaugeSize) / 2 - 5;
            CenterX = this.Width / 2;
            CenterY = this.Height / 2;
            NeedleCanvas = new Canvas();

            //Big circle
            Ellipse ellipse = new Ellipse();
            ellipse.Stroke = Brushes.Black;
            ellipse.Width = gaugeSize;
            ellipse.Height = gaugeSize;
            Canvas.SetLeft(ellipse, this.Width / 2 - (gaugeSize / 2));
            Canvas.SetTop(ellipse, this.Height / 2 - (gaugeSize / 2));
            ellipse.PreviewMouseDown += gaugeMouseDown;


            mainCanvas.Children.Add(ellipse);

            //Small Center circle 
            double cSize = 10;
            ellipse = new Ellipse();
            ellipse.Stroke = Brushes.Black;
            ellipse.Fill = Brushes.Black;
            ellipse.Width = cSize;
            ellipse.Height = cSize;
            Canvas.SetLeft(ellipse, this.Width / 2 - (cSize / 2));
            Canvas.SetTop(ellipse, this.Height / 2 - (cSize / 2));
            mainCanvas.Children.Add(ellipse);

            //Arc 
            double arcSize = needleLen - 8;
            Path path = new Path();
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();

            double startX = CenterX + arcSize * Math.Cos(Math.PI / 180 * (45 + 90));
            double startY = CenterY + arcSize * Math.Sin(Math.PI / 180 * (45 + 90));
            double endX = CenterX + arcSize * Math.Cos(Math.PI / 180 * 45);
            double endY = CenterY + arcSize * Math.Sin(Math.PI / 180 * 45);

            pathFigure.StartPoint = new Point( startX, startY);
            ArcSegment arc = new ArcSegment();
            arc.Point = new Point(endX, endY);
            arc.Size = new Size(arcSize, arcSize);
            arc.IsLargeArc = true;
            arc.SweepDirection = SweepDirection.Clockwise;
            pathFigure.Segments.Add(arc);
            pathGeometry.Figures.Add(pathFigure);
            path.StrokeThickness = 20;
            path.PreviewMouseDown += gaugeMouseDown;


            if (GaugeColor == null)
                path.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF8C9DA6"));
            else
                path.Stroke = GaugeColor;
            path.Data = pathGeometry;
            mainCanvas.Children.Add(path);

            //Measurelines
            double count = 10;
            if (MLine > 0 ) count = MLine;
            double incAngle = 270 / count;
            for (int i = 1; i < count; i++)
            {
                Line mLine = new Line();
                mLine.X1 = CenterX + (needleLen - 18) * Math.Cos(Math.PI / 180 * (45 + 90 + incAngle * i));
                mLine.Y1 = CenterY + (needleLen - 18) * Math.Sin(Math.PI / 180 * (45 + 90 + incAngle * i));
                mLine.X2 = CenterX + (needleLen + 2) * Math.Cos(Math.PI / 180 * (45 + 90 + incAngle * i));
                mLine.Y2 = CenterY + (needleLen + 2) * Math.Sin(Math.PI / 180 * (45 + 90 + incAngle * i));
                mLine.Stroke = Brushes.White;
                mLine.PreviewMouseDown += gaugeMouseDown;
                mainCanvas.Children.Add(mLine);
            }

            textblock = new TextBlock();
            textblock.FontSize = (double)this.FindResource("defaultFontSize");
            textblock.Foreground = Brushes.Black;
            setText("0",false);
            mainCanvas.Children.Add(textblock);
            Canvas.SetTop(textblock, this.Height-textblock.FontSize-10);

            Path NeedleLinePath = new Path();
            pathGeometry = new PathGeometry();
            pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(0, 0);
            PolyLineSegment pLine  = new PolyLineSegment();
            pLine.Points.Add( new Point(5,0));
            pLine.Points.Add(new Point(0, needleLen));
            pLine.Points.Add(new Point(-5, 0));
            pLine.Points.Add(new Point(0, 0));
    
            pathFigure.Segments.Add(pLine);
            pathGeometry.Figures.Add(pathFigure);
            NeedleLinePath.Data = pathGeometry;
            NeedleLinePath.PreviewMouseDown += gaugeMouseDown;
            NeedleLinePath.StrokeThickness = 1;
            NeedleLinePath.Stroke = Brushes.Black;
            NeedleLinePath.Fill = Brushes.Black;
            NeedleCanvas.Children.Add(NeedleLinePath);
            mainCanvas.Children.Add(NeedleCanvas);
            Canvas.SetLeft(NeedleCanvas, CenterX);
            Canvas.SetTop(NeedleCanvas, CenterY);

            RotateTransform rotateTransform2 = new RotateTransform(45, 0, 0);
            NeedleCanvas.RenderTransform = rotateTransform2;

        }

        private void gaugeMouseDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("--- ");
            double X, Y;
            X = e.GetPosition(this).X - CenterX;
            Y = e.GetPosition(this).Y - CenterY;

            Double MinV = Input.MinVal.ChangeType<double>();
            Double MaxV = Input.MaxVal.ChangeType<double>();

            double angle = Math.Atan2(Y,X);
            Console.WriteLine("Angle " + (angle * 180 / Math.PI).ToString());
            if (angle < 0) angle += Math.PI*2;

            double angle2 = ((angle) * 180 / Math.PI) - 45 - 90;
            if (angle2 < 0) angle2 = 360 + angle2;

            double pr = angle2 / 270;
         

            double newValue = pr * (MaxV-MinV);
            Input.Val = Convert.ChangeType( (newValue+MinV), Input.OType);
        }

        public void setText(string value, bool error)
        {
            textblock.Text = value;
            Size msrSize = new Size(200, 200);
            textblock.Measure(msrSize);
            Size si = textblock.DesiredSize;
            Canvas.SetLeft(textblock, CenterX - (si.Width / 2));

            if ( error )
                textblock.Foreground = Brushes.Red;
            else
                textblock.Foreground = Brushes.Black;

        }
           
    }
}
