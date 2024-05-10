using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Threading;

namespace libPLC
{
    /// <summary>
    /// Interaction logic for inputNumeric.xaml
    /// </summary>
    ///
    public interface iInputPad
    {
        object senderOb { get; set; }
        TextBox TextBoxVal { get; set; }
        object DataContext { get; set; }
        UserControl ctrl { get; }

        dragPopup Popup { get; set; }
    }

    public partial class inputNumeric : UserControl, iInputPad
    {
        dragPopup popup;
        public dragPopup Popup
        {
            get { return popup; }
            set
            {
                popup = value;
                popup.Width = this.Width;
                popup.Height = this.Height;  
            }
        }

        public UserControl ctrl { get { return this; } }
        public TextBox TextBoxVal { set { textBoxVal = value; } get { return textBoxVal; } }

        public object senderOb { get; set; }

        bool start = false;

        public inputNumeric()
        {
            InitializeComponent();
           
        }

        private void setValue()
        {
            // bool b = Regex.IsMatch(textBoxVal.Text, "^[0-9_-]+$");
            bool b  = textBoxVal.Text.All(c => CheckAllowedChars(c));

            if (b)
            {
                Object dT = (Object)new DataTable().Compute(textBoxVal.Text.Replace(',', '.'), null);
                if (dT != null)
                {
                    string oldText = textBoxVal.Text;
                    textBoxVal.Text = dT.ToString();
                    textBoxVal.SelectionStart = textBoxVal.Text.Length;
                    textBoxVal.Focus();
          //          Console.WriteLine("dT.ToString() " + dT.ToString());
                    if (oldText != textBoxVal.Text) return;
                }
            }
            BindingExpression be = textBoxVal.GetBindingExpression(TextBox.TextProperty);
            if (be != null)
                be.UpdateSource();
            else
            {
                TextBox tb = senderOb as TextBox;
                tb.Text = textBoxVal.Text;
                Console.WriteLine("textboxiin kirjoitus");
            }




            /*
            Binding myBinding = BindingOperations.GetBinding(textBoxVal, TextBox.TextProperty);
            if (MainWindow.popups.ContainsKey(myBinding.Path.Path))
            {
                Console.WriteLine("Popup " + myBinding.Path.Path + " suljettu");
                MainWindow.popups.Remove(myBinding.Path.Path);
            }*/
            Popup.IsOpen = false;


        }

        private void setValue_Click(object sender, RoutedEventArgs e)
        {
            setValue();
        }

       

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Popup.IsOpen = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int ot = textBoxVal.SelectionStart;

            string cT = button.Content.ToString();

            if (textBoxVal.SelectionLength == 0)
            {
                string stM = textBoxVal.Text.Insert(ot, cT);
                textBoxVal.Text = stM;
                textBoxVal.Focus();
                textBoxVal.SelectionStart = ot + 1;
            }
            else
            {
                string stM = textBoxVal.Text.Remove(ot, textBoxVal.SelectionLength);
                stM = stM.Insert(ot, cT);
                textBoxVal.Text = stM;
            }
        }

        private void buttonBackspace_Click(object sender, RoutedEventArgs e)
        {
            int selCount  = textBoxVal.SelectedText.Count();
            if (selCount == 0 && textBoxVal.SelectionStart > 0)
            {
                int oldCaret = textBoxVal.SelectionStart - 1;
                string removedStr = textBoxVal.Text.Remove(textBoxVal.SelectionStart - 1, 1);
                textBoxVal.Text = removedStr;
                textBoxVal.SelectionStart = oldCaret;
                textBoxVal.Focus();
            }
            else
            {
                int oldCaret = textBoxVal.SelectionStart;
                string removedStr = textBoxVal.Text.Remove(textBoxVal.SelectionStart, selCount);
                textBoxVal.Text = removedStr;
                textBoxVal.SelectionStart = oldCaret;
                textBoxVal.Focus();
            }
        }

        private void textBoxVal_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!start)
            {
                textBoxVal.SelectionStart = textBoxVal.Text.Length;
                textBoxVal.Focus();
                start = true;
            }
        }

        private void textBoxVal_KeyDown(object sender, KeyEventArgs e)
        {
            if ( e.Key == Key.Return )
                setValue();
        }

        private void textBoxVal_Loaded(object sender, RoutedEventArgs e)
        {
            textBoxVal.Focus();
            Keyboard.Focus(textBoxVal);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Keyboard.Focus(textBoxVal);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
         //   Console.WriteLine("Test");
            textBoxVal.Focus();
            Keyboard.Focus(textBoxVal);
        }

        private void textBoxVal_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.ContextIdle,
                 new Action(delegate ()
                {
                    textBoxVal.Focus();
                    Keyboard.Focus(textBoxVal);
                }));         
        }


        private static bool CheckAllowedChars(char uc)
        {
            switch (uc)
            {
                case ',':
                case '-':
                case '+':
                case '*':
                case '/':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return true;
                default:
                    return false;
            }
        }

        private void buttonkeyboad_Click(object sender, RoutedEventArgs e)
        {
            Binding myBinding = BindingOperations.GetBinding(textBoxVal, TextBox.TextProperty);
            inputAlpha alpaPad = new inputAlpha();
            if (myBinding != null)
            {
                Binding newBinding = new Binding(myBinding.Path.Path);
                newBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
                alpaPad.textBoxVal.SetBinding(TextBox.TextProperty, newBinding);
                alpaPad.DataContext = textBoxVal.DataContext;
            }
            else
            {
                alpaPad.textBoxVal.Text = textBoxVal.Text;
                alpaPad.senderOb = senderOb;
            }
            dragPopup popupNew = new dragPopup();
            popupControl popCtrl = new popupControl(alpaPad);
            popCtrl.Popup = popupNew;
            alpaPad.Popup = popupNew;
            popupNew.Width = popCtrl.Width;
            popupNew.Height = popCtrl.Height;
            popupNew.Child = popCtrl;
            popupNew.Placement = PlacementMode.Absolute;
            Point pos = this.PointToScreen(new Point(0, 0));
            popupNew.HorizontalOffset = pos.X - this.Width;
            popupNew.VerticalOffset = pos.Y-30;
            Console.WriteLine("X : " + pos.X);
            Console.WriteLine("Y : " + pos.Y);
         //   popupNew.PopupAnimation = PopupAnimation.Fade;
            popupNew.IsOpen = true;

            popup.IsOpen = false;
        }

        private void ButtonV_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            textBoxVal.Text = b.Content.ToString();
            setValue();
        }
    }
}
