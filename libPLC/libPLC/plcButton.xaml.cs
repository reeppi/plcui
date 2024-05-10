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

namespace libPLC
{
    /// <summary>
    /// Interaction logic for plcButton.xaml
    /// </summary>
    public partial class plcButton : UserControl
    {
        public static readonly DependencyProperty inputProperty = DependencyProperty.Register("Input", typeof(iTagObj), typeof(plcButton),  new FrameworkPropertyMetadata(IsTagInPropertyChanged));
        public static readonly DependencyProperty outputProperty = DependencyProperty.Register("Output", typeof(iTagObj), typeof(plcButton), new FrameworkPropertyMetadata(IsTagOutPropertyChanged));

        bool bOutput = false;

        private static void IsTagOutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            plcButton ctrl = d as plcButton;
            ctrl.bOutput = true;
        }

        private static void IsTagInPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
         //   Console.WriteLine("IsTagInPropertyChanged ");
        }

        public static readonly DependencyProperty CTypeProperty = DependencyProperty.Register("CType", typeof(clickType), typeof(plcButton), new FrameworkPropertyMetadata(IsCTypeChanged));

        private static void IsCTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            plcButton ctrl = d as plcButton;
            ctrl.ChangeClickType((clickType)e.NewValue);

        }


        public void ChangeClickType(clickType ct)
        {
            if (CType == clickType.toggle)
            {
                buttonPlc.Click += PlcButton_Click;
            }
            if (CType == clickType.press)
            {
                buttonPlc.PreviewMouseDown += ButtonPlc_MouseDown;
                buttonPlc.PreviewMouseUp += ButtonPlc_MouseUp;
            }
        }


        public clickType CType
        {
            get { return (clickType)GetValue(CTypeProperty); }
            set {
                Console.WriteLine("CTYPE SETTER");
                SetValue(CTypeProperty, value);
            }
        }

        public iTagObj Input
        {
            get { return (iTagObj)GetValue(inputProperty); }
            set { SetValue(inputProperty, value); }
        }

        public iTagObj Output
        {
            get { return (iTagObj)GetValue(outputProperty); }
            set { SetValue(outputProperty, value); }
        }

        public enum clickType
        {
            none, toggle, press
        }

        public imgI Img
        {
            get { return indicator.Img; }
            set { indicator.Img = value;}
        }

        public ImageSource ImgOff
        {
            get { return indicator.ImgOff; }
            set { indicator.ImgOff = value; }
        }

        public ImageSource ImgOn
        {
            get { return indicator.ImgOn; }
            set { indicator.ImgOn = value; }
        }

        public string Text
        {
            get { return textbox.Text; }
            set { textbox.Text = value; }
        }

        public bool Small
        {
            get { return indicator.Small; }
            set { indicator.Small = value; }
        }

        bool right;
        public bool Right
        {
            get { return right;}
            set
            {
                right = value;
                if (right)
                {
                    indicator.SetValue(Grid.ColumnProperty, 1);
                    textbox.SetValue(Grid.ColumnProperty, 0);

                   gridi.ColumnDefinitions[0].Width =  new GridLength(100,GridUnitType.Star);
                   gridi.ColumnDefinitions[1].Width = GridLength.Auto;
                    textbox.HorizontalAlignment = HorizontalAlignment.Left;
                    textbox.TextAlignment = TextAlignment.Right;
                }
            }

        }




        public plcButton()
        {
            InitializeComponent();
           (this.Content as FrameworkElement).DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            (this.Content as FrameworkElement).DataContext = this;


        }

        private void ButtonPlc_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (bOutput)
                Output.Val = false;
            else
                Input.Val = false;
        }

        private void ButtonPlc_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (bOutput)
                Output.Val = true;
            else
                Input.Val = true;
        }

        private void PlcButton_Click(object sender, RoutedEventArgs e)
        {
            if (bOutput)
            {
                if (Output == null)
                {
                    Binding inputValBinding = BindingOperations.GetBinding(this, inputProperty);
                    MessageBox.Show("Variable " + inputValBinding.Path.Path + " not found");
                }
                else
                    Output.Val = !(bool)Output.Val;

            }
            else
            {
                if (Input == null)
                {
                    Binding inputValBinding = BindingOperations.GetBinding(this, inputProperty);
                    MessageBox.Show("Variable " + inputValBinding.Path.Path + " not found");
                }
                else
                    Input.Val = !(bool)Input.Val;

            }
        }
    }
}
