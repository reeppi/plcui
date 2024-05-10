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

namespace ui
{
    /// <summary>
    /// Interaction logic for navButton.xaml
    /// </summary>
    public partial class navButton : UserControl
    {
        public navButton()
        {
            InitializeComponent();

    
        }

        public static readonly DependencyProperty navServiceProperty = DependencyProperty.Register("NavService", typeof(NavigationService), typeof(navButton));
        public NavigationService NavService { get { return (NavigationService)GetValue(navServiceProperty); } set { SetValue(navServiceProperty, value); } }

        public static readonly DependencyProperty curPageProperty = DependencyProperty.Register("CurPage", typeof(string), typeof(navButton), new FrameworkPropertyMetadata(currentPageChanged));
        public string CurPage { get { return (string)GetValue(curPageProperty); } set { SetValue(curPageProperty, value); } }

        public string Page { get; set; }

        private static void currentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            navButton ctrl = d as navButton;
            string newValue = (string)e.NewValue;

            if (newValue == ctrl.Page)
                ctrl.nButton.Background = new SolidColorBrush(Colors.LightBlue);
            else
                ctrl.nButton.Background = new Button { Content = "New Button" }.Background;
        }

        public string Text
        {
            get { return this.nButton.Content.ToString(); }
            set { this.nButton.Content = value; }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (NavService == null)
            {
                Page pg = helper.FindParentPage(this);
                if (pg != null)
                    NavService = pg.NavigationService;
            }
            dataModel dm = this.DataContext as dataModel;
            dm.CurPage = Page;
            NavService.Navigate(MainWindow.Pages[Page]);

        }




    }
}
