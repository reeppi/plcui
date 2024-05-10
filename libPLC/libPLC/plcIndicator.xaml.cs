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
    public partial class plcIndicator : UserControl
    {
        public static readonly DependencyProperty inputProperty = DependencyProperty.Register("Input", typeof(iTagObj), typeof(plcIndicator));

        public iTagObj Input
        {
            get { return (iTagObj)GetValue(inputProperty); }
            set { SetValue(inputProperty, value); }
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
            get { return right; }
            set
            {
                right = value;
                if (right)
                {
                    indicator.SetValue(Grid.ColumnProperty, 1);
                    textbox.SetValue(Grid.ColumnProperty, 0);
                    gridi.ColumnDefinitions[0].Width = new GridLength(100, GridUnitType.Star);
                    gridi.ColumnDefinitions[1].Width = GridLength.Auto;
                    textbox.HorizontalAlignment = HorizontalAlignment.Right;
                }
            }
        }


        public plcIndicator()
        {
            InitializeComponent();
            (this.Content as FrameworkElement).DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            (this.Content as FrameworkElement).DataContext = this;
        }

    }
}
