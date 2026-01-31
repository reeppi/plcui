

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

namespace ui
{
    /// <summary>
    /// Interaction logic for plcPath.xaml
    /// </summary>
    public partial class plcPath : UserControl
    {
        public plcPath()
        {
            InitializeComponent();
            Points = new List<Point>();
        }

        List<Point> Points { get; set; }
        bool autoXFactor = false;
        bool autoYFactor = false;

        #region InputX Property
        public static readonly DependencyProperty inputXProperty =
            DependencyProperty.Register("InputX", typeof(iTagObj), typeof(plcPath),
            new FrameworkPropertyMetadata(IsInputXPropertyChanged));

        public iTagObj InputX
        {
            get { return (iTagObj)GetValue(inputXProperty); }
            set { SetValue(inputXProperty, value); }
        }

        private static void IsInputXPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            plcPath ctrl = d as plcPath;
            ctrl.setInputX((iTagObj)e.NewValue);
        }

        public void setInputX(iTagObj tagObj)
        {
            if (tagObj != null)
            {
                XMax = tagObj.MaxVal.ChangeType<double>();
                XMin = tagObj.MinVal.ChangeType<double>();

                if (tagObj.OType == typeof(bool))
                    XMax = 1;
                else if (XMax == 0 && XMin == 0)
                {
                    autoXFactor = true;
                    XMax = 1;
                    XMin = 0;
                }
            }

            if (XScale == 0) XScale = XMax - XMin;

            if (XScale > 0)
                xFactor = (this.Width - 4) / XScale;

            // Bind to value
            Binding myBinding = BindingOperations.GetBinding(this, inputXProperty);
            if (myBinding != null)
            {
                Binding newBinding = new Binding(myBinding.Path.Path + ".Val");
                BindingOperations.SetBinding(this, plcPath.inputXValProperty, newBinding);
            }
        }
        #endregion

        #region InputY Property
        public static readonly DependencyProperty inputYProperty =
            DependencyProperty.Register("InputY", typeof(iTagObj), typeof(plcPath),
            new FrameworkPropertyMetadata(IsInputYPropertyChanged));

        public iTagObj InputY
        {
            get { return (iTagObj)GetValue(inputYProperty); }
            set { SetValue(inputYProperty, value); }
        }

        private static void IsInputYPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            plcPath ctrl = d as plcPath;
            ctrl.setInputY((iTagObj)e.NewValue);
        }

        public void setInputY(iTagObj tagObj)
        {
            if (tagObj != null)
            {
                YMax = tagObj.MaxVal.ChangeType<double>();
                YMin = tagObj.MinVal.ChangeType<double>();

                if (tagObj.OType == typeof(bool))
                    YMax = 1;
                else if (YMax == 0 && YMin == 0)
                {
                    autoYFactor = true;
                    YMax = 1;
                    YMin = 0;
                }
            }

            if (YScale == 0) YScale = YMax - YMin;

            if (YScale > 0)
                yFactor = (this.Height - 4) / YScale;

            // Bind to value
            Binding myBinding = BindingOperations.GetBinding(this, inputYProperty);
            if (myBinding != null)
            {
                Binding newBinding = new Binding(myBinding.Path.Path + ".Val");
                BindingOperations.SetBinding(this, plcPath.inputYValProperty, newBinding);
            }
        }
        #endregion

        #region X and Y Value Properties
        public static readonly DependencyProperty inputXValProperty =
            DependencyProperty.Register("InputXVal", typeof(object), typeof(plcPath),
            new FrameworkPropertyMetadata(IsInputXValPropertyChanged));

        public object InputXVal
        {
            get { return (object)GetValue(inputXValProperty); }
            set { SetValue(inputXValProperty, value); }
        }

        private static void IsInputXValPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            plcPath ctrl = d as plcPath;
            ctrl.setInputXVal((object)e.NewValue);
        }

        public void setInputXVal(object val)
        {
            if (val == null) return;

            double dVal = val.ChangeType<double>();
            lastX = currentX;
            currentX = dVal;

            // Update bounds if auto-scaling
            if (autoXFactor && dVal > XMax)
                XMax = dVal;
            if (autoXFactor && dVal < XMin)
                XMin = dVal;

            CheckAndAddPointAndDraw();
        }

        public static readonly DependencyProperty inputYValProperty =
            DependencyProperty.Register("InputYVal", typeof(object), typeof(plcPath),
            new FrameworkPropertyMetadata(IsInputYValPropertyChanged));

        public object InputYVal
        {
            get { return (object)GetValue(inputYValProperty); }
            set { SetValue(inputYValProperty, value); }
        }

        private static void IsInputYValPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            plcPath ctrl = d as plcPath;
            ctrl.setInputYVal((object)e.NewValue);
        }

        public void setInputYVal(object val)
        {
            if (val == null) return;

            double dVal = val.ChangeType<double>();
            lastY = currentY;
            currentY = dVal;

            // Update bounds if auto-scaling
            if (autoYFactor && dVal > YMax)
                YMax = dVal;
            if (autoYFactor && dVal < YMin)
                YMin = dVal;

            CheckAndAddPointAndDraw();
        }
        #endregion

        private double currentX = double.NaN;
        private double currentY = double.NaN;
        private double lastX = double.NaN;
        private double lastY = double.NaN;
        private bool hasValidData = false; // Onko saatu ensimmäinen validi datapari

        private void CheckAndAddPointAndDraw()
        {
            // Tarkistetaan että molemmat arvot ovat validit
            if (double.IsNaN(currentX) || double.IsNaN(currentY))
                return;

            // Tarkistetaan onko arvot muuttuneet
            bool xChanged = double.IsNaN(lastX) || Math.Abs(currentX - lastX) > 0.001;
            bool yChanged = double.IsNaN(lastY) || Math.Abs(currentY - lastY) > 0.001;

            // Lisätään piste ja piirretään vain jos jompikumpi arvo on muuttunut
            if (xChanged || yChanged)
            {
                // Jos tämä on ensimmäinen validi piste, merkitään että nyt on dataa
                if (!hasValidData)
                {
                    hasValidData = true;
                    // Tyhjennetään lista aluksi
                    Points.Clear();
                }

                Points.Add(new Point(currentX, currentY));

                // Limit number of points
                int maxPoints = 1000;
                if (Points.Count > maxPoints)
                    Points.RemoveAt(0);

                // Piirretään välittömästi
                DrawPath();
            }
        }

        // Poistettu: Canvas mainCanvas = null; (nyt xaml:ssä määritelty)
        TextBlock leftUpText, leftDownText;
        TextBlock rightUpText, rightDownText;
        TextBlock posText;
        Size leftDownTextSize;

        double xFactor = 0;
        double yFactor = 0;

        // Uusi property: näytetäänkö siniset pisteet (Oletus: EI)
        private bool showBluePoints = false;
        public bool ShowBluePoints
        {
            get { return showBluePoints; }
            set
            {
                showBluePoints = value;
                DrawPath();
            }
        }

        private void DrawPath()
        {
            if (this.IsVisible)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
                {
                    drawPath();
                }));
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Timer poistettu kokonaan

            leftUpText = new TextBlock { Text = "0", FontSize = 10 };
            leftDownText = new TextBlock { Text = "0", FontSize = 10 };
            rightUpText = new TextBlock { Text = "0", FontSize = 10 };
            rightDownText = new TextBlock { Text = "0", FontSize = 10 };
            posText = new TextBlock { Text = "X: -, Y: -", FontSize = 10 };

            Size msrSize1 = new Size(200, 200);
            leftDownText.Measure(msrSize1);
            leftDownTextSize = leftDownText.DesiredSize;

            // Piirretään vain kerran latauksessa (tyhjä kuva)
            drawPath();
        }

        private void drawPath()
        {
            mainCanvas.Children.Clear();

            // Draw border
            Rectangle rect = new Rectangle();
            rect.Width = this.Width;
            rect.Height = this.Height;
            rect.Stroke = Brushes.Gray;
            mainCanvas.Children.Add(rect);

            // Update scaling factors
            if (autoXFactor && XMax > XMin)
            {
                XScale = XMax - XMin;
                if (XScale > 0)
                    xFactor = (this.Width - 4) / XScale;
            }

            if (autoYFactor && YMax > YMin)
            {
                YScale = YMax - YMin;
                if (YScale > 0)
                    yFactor = (this.Height - 4) / YScale;
            }
            else
            {
                // Y-axis grows upward (inverted)
                if (YScale > 0)
                    yFactor = -(this.Height - 4) / YScale;
            }

            // Draw grid lines (optional) - vain jos on dataa
            if (hasValidData)
                DrawGrid();

            // Draw path - vain jos on vähintään 2 pistettä
            if (Points.Count > 1)
            {
                Point? previousPoint = null;

                foreach (var point in Points)
                {
                    double x = 2 + (point.X - XMin) * xFactor;
                    double y = this.Height - 2 + (point.Y - YMin) * yFactor; // Y grows upward

                    if (previousPoint.HasValue)
                    {
                        // Draw line from previous point to current point
                        Line line = new Line();
                        line.Stroke = Brushes.DarkRed;
                        line.StrokeThickness = 1;
                        line.X1 = previousPoint.Value.X;
                        line.Y1 = previousPoint.Value.Y;
                        line.X2 = x;
                        line.Y2 = y;
                        mainCanvas.Children.Add(line);
                    }

                    // Draw small circle at point (optional)
                    if (showBluePoints)
                    {
                        Ellipse pointMarker = new Ellipse();
                        pointMarker.Width = 3;
                        pointMarker.Height = 3;
                        pointMarker.Fill = Brushes.Blue;
                        pointMarker.Stroke = Brushes.DarkBlue;
                        pointMarker.StrokeThickness = 1;

                        mainCanvas.Children.Add(pointMarker);
                        Canvas.SetLeft(pointMarker, x - 1.5);
                        Canvas.SetTop(pointMarker, y - 1.5);
                    }

                    previousPoint = new Point(x, y);
                }
            }
            else if (Points.Count == 1 && showBluePoints)
            {
                // Piirretään vain piste ilman viivaa (vain jos siniset pisteet päällä)
                var point = Points[0];
                double x = 2 + (point.X - XMin) * xFactor;
                double y = this.Height - 2 + (point.Y - YMin) * yFactor;

                Ellipse pointMarker = new Ellipse();
                pointMarker.Width = 3;
                pointMarker.Height = 3;
                pointMarker.Fill = Brushes.Blue;
                pointMarker.Stroke = Brushes.DarkBlue;
                pointMarker.StrokeThickness = 1;

                mainCanvas.Children.Add(pointMarker);
                Canvas.SetLeft(pointMarker, x - 1.5);
                Canvas.SetTop(pointMarker, y - 1.5);
            }

            // Draw current position marker (aina piirretään jos on validi arvo)
            if (!double.IsNaN(currentX) && !double.IsNaN(currentY))
            {
                double currentXPos = 2 + (currentX - XMin) * xFactor;
                double currentYPos = this.Height - 2 + (currentY - YMin) * yFactor;

                // Varmistetaan että piste on näytön sisällä
                currentXPos = Math.Max(2, Math.Min(this.Width - 2, currentXPos));
                currentYPos = Math.Max(2, Math.Min(this.Height - 2, currentYPos));

                Ellipse currentMarker = new Ellipse();
                currentMarker.Width = 6;
                currentMarker.Height = 6;
                currentMarker.Fill = Brushes.Red;
                currentMarker.Stroke = Brushes.DarkRed;
                currentMarker.StrokeThickness = 1;

                mainCanvas.Children.Add(currentMarker);
                Canvas.SetLeft(currentMarker, currentXPos - 3);
                Canvas.SetTop(currentMarker, currentYPos - 3);
            }

            // Update labels
            UpdateLabels();
        }

        private void DrawGrid()
        {
            // Draw center lines if zero is within range
            if (XMin <= 0 && XMax >= 0)
            {
                Line verticalLine = new Line();
                verticalLine.Stroke = Brushes.LightGray;
                verticalLine.StrokeThickness = 1;
                verticalLine.StrokeDashArray = new DoubleCollection() { 2, 2 };
                double xPos = 2 + (0 - XMin) * xFactor;
                verticalLine.X1 = xPos;
                verticalLine.Y1 = 0;
                verticalLine.X2 = xPos;
                verticalLine.Y2 = this.Height;
                mainCanvas.Children.Add(verticalLine);
            }

            if (YMin <= 0 && YMax >= 0)
            {
                Line horizontalLine = new Line();
                horizontalLine.Stroke = Brushes.LightGray;
                horizontalLine.StrokeThickness = 1;
                horizontalLine.StrokeDashArray = new DoubleCollection() { 2, 2 };
                double yPos = this.Height - 2 + (0 - YMin) * yFactor;
                horizontalLine.X1 = 0;
                horizontalLine.Y1 = yPos;
                horizontalLine.X2 = this.Width;
                horizontalLine.Y2 = yPos;
                mainCanvas.Children.Add(horizontalLine);
            }
        }

        private void UpdateLabels()
        {
            // Päivitetään arvot
            leftUpText.Text = string.Format("{0:0.##}", YMax);
            leftDownText.Text = string.Format("{0:0.##}", YMin);
            rightDownText.Text = string.Format("{0:0.##}", XMin);
            rightUpText.Text = string.Format("{0:0.##}", XMax);

            // Current position
            if (!double.IsNaN(currentX) && !double.IsNaN(currentY))
                posText.Text = string.Format("X: {0:0.##}, Y: {1:0.##}", currentX, currentY);
            else
                posText.Text = "X: -, Y: -";

            // Lisätään labelit takaisin canvasille
            mainCanvas.Children.Add(leftUpText);
            Canvas.SetLeft(leftUpText, 2);
            Canvas.SetTop(leftUpText, 2);

            mainCanvas.Children.Add(leftDownText);
            Canvas.SetLeft(leftDownText, 2);
            Canvas.SetTop(leftDownText, this.Height - leftDownTextSize.Height - 2);

            mainCanvas.Children.Add(rightUpText);
            Canvas.SetRight(rightUpText, 2);
            Canvas.SetTop(rightUpText, 2);

            mainCanvas.Children.Add(rightDownText);
            Canvas.SetRight(rightDownText, 2);
            Canvas.SetTop(rightDownText, this.Height - leftDownTextSize.Height - 2);

            mainCanvas.Children.Add(posText);
            Canvas.SetLeft(posText, this.Width / 2 - 30);
            Canvas.SetTop(posText, 2);
        }

        ContextMenu menu = null;

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                menu = new ContextMenu();

                // Tyhjennä viivat (mutta säilytä nykyinen sijainti)
                MenuItem clearLinesMenuItem = new MenuItem { Header = "Clear Path" };
                clearLinesMenuItem.Click += ClearLinesMenuItem_Click;
                menu.Items.Add(clearLinesMenuItem);

                // Näytä/piilota siniset pisteet
                MenuItem togglePointsMenuItem = new MenuItem();
                if (showBluePoints)
                    togglePointsMenuItem.Header = "Hide Blue Points";
                else
                    togglePointsMenuItem.Header = "Show Blue Points";
                togglePointsMenuItem.Click += TogglePointsMenuItem_Click;
                menu.Items.Add(togglePointsMenuItem);

                MenuItem closeMenuItem = new MenuItem { Header = "Close Menu" };
                closeMenuItem.Click += CloseMenuItem_Click;
                menu.Items.Add(closeMenuItem);

                menu.IsOpen = true;
            }
        }

        private void ClearLinesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Tyhjennetään vain polku (viivat ja siniset pisteet)
            // Säilytetään nykyinen sijainti (currentX, currentY)
            Points.Clear();

            // Piirretään välittömästi
            DrawPath();
            menu.IsOpen = false;
        }

        private void TogglePointsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowBluePoints = !ShowBluePoints;
            menu.IsOpen = false;
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            menu.IsOpen = false;
        }

        #region Properties
        public double XScale { get; set; }
        public double YScale { get; set; }
        public double XMax { get; set; } = 1;
        public double XMin { get; set; } = 0;
        public double YMax { get; set; } = 1;
        public double YMin { get; set; } = 0;
        #endregion
    }
}