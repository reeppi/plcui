﻿using System;
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
using System.Timers;
using System.Windows.Threading;

namespace ui
{
    /// <summary>
    /// Interaction logic for plcChart.xaml
    /// </summary>
    public partial class plcScope : UserControl
    {
        public plcScope()
        {
            InitializeComponent();

            TimeLine = new List<valEntry>();
            ValueLines = new List<valLine>();
            MouseLine = new Line();
        }


        public class valLine 
        {
            public Point P1 { get; set; }
            public Point P2 { get; set; }
            public double Val { get; set; }
            public Point ClosestPoint;
            public double Dist { get; set; }
        }

        Line MouseLine { get; set; } 
        List<valLine> ValueLines { get; set; }

        public int RefreshRate { get; set; }

        List<valEntry> TimeLine { get; set; }
        bool autoYfactor = false;
        public class valEntry
        {
            public long Time { get; set; }
            public double Val { get; set; }
        }

        public static readonly DependencyProperty inputProperty = DependencyProperty.Register("Input", typeof(iTagObj), typeof(plcScope), new FrameworkPropertyMetadata(IsTagInPropertyChanged));
        public iTagObj Input
        {
            get { return (iTagObj)GetValue(inputProperty); }
            set { SetValue(inputProperty, value); }
        }
        private static void IsTagInPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            plcScope ctrl = d as plcScope;
            ctrl.setTag((iTagObj)e.NewValue);
        }
        public void setTag(iTagObj tagObj)
        {
            if (tagObj != null)
            {
                Max = tagObj.MaxVal.ChangeType<double>();
                Min = tagObj.MinVal.ChangeType<double>();

                if (tagObj.OType == typeof(bool))
                    Max = 1;
                else if (Max == 0 && Min == 0)
                {
                    Max = 1;
                    autoYfactor = true;
                }
            }

            if (TimeScale == 0) TimeScale = 10000;

            xFactor = this.Width / TimeScale;
            yFactor = (this.Height-4) / (Max-Min);


            Binding myBinding = BindingOperations.GetBinding(this, inputProperty);
            Binding newBinding = new Binding(myBinding.Path.Path + ".Val");
            BindingOperations.SetBinding(this, plcScope.inputValProperty, newBinding);
        }

        public double TimeScale { get; set;  }

        public bool Stop { get; set;  }

        public static readonly DependencyProperty inputValProperty = DependencyProperty.Register("InputVal", typeof(object), typeof(plcScope), new FrameworkPropertyMetadata(IsInputValPropertyChanged));
        public object InputVal
        {
            get { return (object)GetValue(inputValProperty); }
            set { SetValue(inputValProperty, value); }
        }
        private static void IsInputValPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            plcScope ctrl = d as plcScope;
            ctrl.setInputVal((object)e.NewValue);
        }
        public void setInputVal(object val)
        {
            if (val == null) return;
            //dynamic tValue = Convert.ChangeType(val, Input.OType);
            if (Stop) return;
            double dVal= val.ChangeType<double>();
            TimeLine.Insert( 0, new valEntry { Time=DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond, Val = dVal });

            int ct = 100;
           if ( TimeLine.Count > ct)
                  TimeLine.RemoveRange(ct, TimeLine.Count- ct);
        }

        Canvas mainCanvas = null;

        TextBlock leftUpText, leftDownText;
        TextBlock rightUpText, rightDownText;
        TextBlock valText;
        Timer refreshTimer;

        Size leftDownTextSize;

        double xFactor = 0;
        double yFactor = 0;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if ( TimeLine.Count > 1 )
             TimeLine.RemoveRange(1, TimeLine.Count-1);

            if (mainCanvas != null) return;

            mainCanvas = new Canvas();

            mainCanvas.Children.Add(MouseLine);
           // Canvas.SetZIndex(MouseLine, -100);
            this.PreviewMouseMove += PlcScope_PreviewMouseMove; 

            this.AddChild(mainCanvas);
            
            refreshTimer = new Timer();
            refreshTimer.Interval = 30;
            refreshTimer.Elapsed += RefreshTimer_Elapsed;
            refreshTimer.Enabled = true;

            double fontSize=(double)this.FindResource("defaultFontSize");

            leftUpText = new TextBlock { Text = "00" };
            leftDownText = new TextBlock { Text = "00" };
            rightUpText = new TextBlock { Text = "00" };
            rightDownText = new TextBlock { Text = "00" };
            /*leftUpText.FontSize = fontSize;
            leftDownText.FontSize = fontSize;
            rightUpText.FontSize = fontSize;
            rightDownText.FontSize = fontSize;*/

            valText = new TextBlock { Text = "00" };


            Size msrSize1 = new Size(200, 200);
            leftDownText.Measure(msrSize1);
            leftDownTextSize = leftDownText.DesiredSize;


            lastTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            drawChart();

        }

        private void drawMouseLine(Point mousePt)
        {
            if (ValueLines.Count == 0) return;
            foreach (valLine ValLine in ValueLines)
                ValLine.Dist = helper.FindDistanceToSegment(mousePt, ValLine.P1, ValLine.P2, out ValLine.ClosestPoint);
           
            ValueLines.Sort(delegate (valLine p1, valLine p2) { return p1.Dist.CompareTo(p2.Dist); });
            valLine ValL = ValueLines.First();
            MouseLine.X1 = mousePt.X;
            MouseLine.Y1 = mousePt.Y;
            MouseLine.X2 = ValL.ClosestPoint.X;
            MouseLine.Y2 = ValL.ClosestPoint.Y;
            MouseLine.Stroke = Brushes.Gray;
            MouseLine.StrokeThickness = 1;

            rightUpText.Text = String.Format("{0:0.#}", ValL.Val);
            Size msrSize1 = new Size(200, 200);
            rightUpText.Measure(msrSize1);
            Canvas.SetLeft(rightUpText, this.Width - rightUpText.DesiredSize.Width-2);

            rightDownText.Text = String.Format("{0:0}", (ValL.ClosestPoint.X-this.Width) / xFactor);
            Size msrSize2 = new Size(200, 200);
            rightDownText.Measure(msrSize2);
            Canvas.SetLeft(rightDownText, this.Width - rightDownText.DesiredSize.Width -2);
            Canvas.SetTop(rightDownText, this.Height - rightDownText.DesiredSize.Height);
        }

        private void PlcScope_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!Stop) return;
            drawMouseLine(e.GetPosition(mainCanvas));
        }

        long lastTime = 0;


        private void drawChart()
        {
            long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            lastTime = startTime;
            mainCanvas.Children.Clear();
            Rectangle rect = new Rectangle();
            rect.Width = this.Width;
            rect.Height = this.Height;
            rect.Stroke = Brushes.Gray;
            mainCanvas.Children.Add(rect);

            Point pt = Mouse.GetPosition(this);
            if ( pt.X < this.Width && pt.X > 0 && pt.Y < this.Height && pt.Y > 0 )
            {
               drawMouseLine(pt);
               mainCanvas.Children.Add(MouseLine);
               mainCanvas.Children.Add(rightUpText);
               mainCanvas.Children.Add(rightDownText);
            }
            ValueLines.Clear();

            if ( autoYfactor )
            {
                if (TimeLine.Count > 0)
                {
                    if (TimeLine.First().Val > Max)
                        Max = TimeLine.First().Val;
                    if (TimeLine.First().Val < Min)
                        Min = TimeLine.First().Val;

                    yFactor = (this.Height - 4) / (Max - Min);

                }
            }

            bool last = false;
            double c = 0;
            if (Min < 0)
                c = Min;
            else
                c = 0;
            Line liCenterY = new Line();
            liCenterY.Stroke = Brushes.Silver;
            liCenterY.StrokeThickness = 2;
            liCenterY.StrokeDashArray = new DoubleCollection() { 2 };
            liCenterY.X1 = 0;
            liCenterY.Y1 = (this.Height - 2) +c * yFactor;
            liCenterY.X2 = this.Width;
            liCenterY.Y2 = (this.Height - 2) +c * yFactor;
            mainCanvas.Children.Add(liCenterY);


            for  (int i=0;i<TimeLine.Count;i++)
            {
                valEntry val = TimeLine[i];
                valEntry valNext = null;
                valEntry valPrev = null;

                if ( i > 0 )
                    valPrev = TimeLine[i - 1];

                if ( i < (TimeLine.Count -1))
                     valNext= TimeLine[i+1];

                double startX =0;
                double endX = 0;

                if (i == 0)
                    startX = this.Width;


                if (valPrev != null)
                {
                    startX = this.Width - (startTime - valPrev.Time) * xFactor;  
                }
         
               endX = this.Width - (startTime-val.Time) * xFactor;
                if (endX <= 0)
                {
                    endX = 0;
                    last = true;
                }
                double Y = (this.Height - 2) - (val.Val - Min) * yFactor;
                Line li = new Line();
                li.Stroke = Brushes.DarkRed;
                li.StrokeThickness = 1;
                li.X1 = startX;
                li.Y1 = Y;
                li.X2 = endX;
                li.Y2 = Y;
                mainCanvas.Children.Add(li);

                valLine ValLine = new valLine();
                ValLine.P1 = new Point(startX,Y);
                ValLine.P2 = new Point(endX, Y);
                ValLine.Val = val.Val;
                ValueLines.Add(ValLine);

                if (valPrev != null)
                {
                    Line liY = new Line();
                    liY.StrokeThickness = 1;
                    liY.Stroke = Brushes.DarkRed;
                    liY.X1 = startX;
                    liY.Y1 = (this.Height-2) - (val.Val-Min) * yFactor;
                    liY.X2 = startX;
                    liY.Y2 = (this.Height-2) - (valPrev.Val-Min) * yFactor;
                    mainCanvas.Children.Add(liY);
                }

                long endTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                if (last) break;
            }

            mainCanvas.Children.Add(leftUpText);
            Canvas.SetLeft(leftUpText, 2);
            leftUpText.Text = Max.ToString();

            mainCanvas.Children.Add(leftDownText);
            Canvas.SetLeft(leftDownText, 2);
            Canvas.SetTop(leftDownText, this.Height - leftDownTextSize.Height-2 );
            leftDownText.Text = Min.ToString();

            mainCanvas.Children.Add(valText);
            Canvas.SetLeft(valText, 2);
            Canvas.SetTop(valText, this.Height / 2 - leftDownTextSize.Height / 2-2 );
            if ( TimeLine.Count > 0 )
            valText.Text = String.Format("{0:0.#}", TimeLine.First().Val);




        }

        private void RefreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.IsVisible)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
                {
                    drawChart();
                }));
            }
        }

        ContextMenu menu = null ;

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            menu = new ContextMenu();
            MenuItem closeMenuItem  = new MenuItem { Header = "Close" };
            closeMenuItem.Click += CloseMenuItem_Click;

            MenuItem playStopMenuItem = new MenuItem ();
            if (Stop)
                playStopMenuItem.Header = "Play";
            else
                playStopMenuItem.Header = "Stop";
            playStopMenuItem.Click += PlayStopMenuItem_Click; ;

            menu.Items.Add(playStopMenuItem);
            menu.Items.Add(closeMenuItem);
            menu.IsOpen = true;
           

        }

        private void PlayStopMenuItem_Click(object sender, RoutedEventArgs e)
        {
           if ( Stop )
            {
                //   if (TimeLine.Count > 1)
                //     TimeLine.RemoveRange(1, TimeLine.Count - 1);
                TimeLine.Clear();
                Stop = false;
                refreshTimer.Start();
                setInputVal(Input.Val);
            }
           else
            {
                Stop = true;
                refreshTimer.Stop();
            //    mainCanvas.Children.Add(MouseLine);
            //    mainCanvas.Children.Add(rightUpText);
            }
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            menu.IsOpen = false;
        }

        public double Max { get; set; }

        public double Min { get; set; }

    }
}
