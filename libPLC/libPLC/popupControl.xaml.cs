using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for popupControl.xaml
    /// </summary>
    public partial class popupControl : UserControl
    {
        UserControl Uc { get; set; }

        dragPopup popup;
        public dragPopup Popup
        {
            get { return popup; }
            set { popup = value; }
        }


        public popupControl(UserControl uc_)
        {
            InitializeComponent();
            Uc = uc_;
        }

        private void mainGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Border border = new Border();
            border.BorderThickness = new Thickness(1);
            border.Child = Uc;

            Grid.SetRow(border, 1);
            Grid.SetColumnSpan(border, 2);
            mainGrid.Children.Add(border);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            inputNumeric iN = Uc as inputNumeric;
            Binding myBinding = BindingOperations.GetBinding(iN.textBoxVal, TextBox.TextProperty);

            inputAlpha alpaPad = new inputAlpha();
            if (myBinding != null)
            {
                Binding newBinding = new Binding(myBinding.Path.Path);
                newBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
                alpaPad.textBoxVal.SetBinding(TextBox.TextProperty, newBinding);
                alpaPad.DataContext = iN.DataContext;
            }
            else
            {
                alpaPad.textBoxVal.Text = iN.textBoxVal.Text;
                alpaPad.senderOb = iN.senderOb;
            }

            dragPopup popupNew = new dragPopup();
            popupControl popCtrl = new popupControl(alpaPad);
            popCtrl.Popup = popupNew;
            alpaPad.Popup = popupNew;
            popupNew.Width = popCtrl.Width;
            popupNew.Height = popCtrl.Height;
            popupNew.Child = popCtrl;
            popupNew.Placement = PlacementMode.MousePoint;
            popupNew.IsOpen = true;

            popup.IsOpen = false;



        }
    }
}
