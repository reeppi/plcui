//using System;
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
using System.Text.RegularExpressions;
using System;
using System.Globalization;

namespace libPLC
{

    public class minMaxValidator : ValidationRule
    {

        public minMaxValidator()
        {

        }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Console.WriteLine("Validation goes ");
            int age = 0;
            try
            {
                if (((string)value).Length > 0)
                {
                    age = Int32.Parse((String)value);
                    if (age < 5)
                        return new ValidationResult(false, "Numero liian pieni");
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }
            return ValidationResult.ValidResult;
        }
    }

    public class editTextBox : TextBox
    {

        public editTextBox()
        {
            this.PreviewMouseDown += EditTextBox_PreviewMouseDown;
            this.TextChanged += EditTextBox_TextChanged;

        }

        private void EditTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Input == null) return;
            dynamic Val, MaxVal, MinVal;
            Val = Convert.ChangeType(Input.Val, Input.OType);
            MaxVal = Convert.ChangeType(Input.MaxVal, Input.OType);
            MinVal = Convert.ChangeType(Input.MinVal, Input.OType);
            if ( ( MaxVal != 0 && Val > MaxVal ) || Val < MinVal)
                this.Background = Brushes.Red;
            else
                this.Background = null;

     
        }

        public enum inputType
        {
            numeric, alpha, param
        }

        public static readonly DependencyProperty inputProperty = DependencyProperty.Register("Input", typeof(iTagObj), typeof(editTextBox), new FrameworkPropertyMetadata(IsTagInPropertyChanged));
        public iTagObj Input
        {
            get { return (iTagObj)GetValue(inputProperty); }
            set { SetValue(inputProperty, value); }
        }
        private static void IsTagInPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            editTextBox ctrl = d as editTextBox;
            ctrl.setTag((iTagObj)e.NewValue);
        }

        public void setTag(iTagObj tagObj)
        {
            Binding myBinding = BindingOperations.GetBinding(this, editTextBox.inputProperty);
           
            Binding newBinding = new Binding(myBinding.Path.Path+".Val");
            Console.WriteLine("myBinding " + myBinding.Path.Path+".Val");

            newBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            minMaxValidator rules = new minMaxValidator();
            newBinding.ValidationRules.Clear();
            newBinding.ValidationRules.Add(rules);
  
            this.SetBinding(TextBox.TextProperty, newBinding);

        }


        public static readonly DependencyProperty plcNameProperty = DependencyProperty.Register("PlcName", typeof(string), typeof(editTextBox));
        public string PlcName { get { return (string)GetValue(plcNameProperty); } set { SetValue(plcNameProperty, value); } }


        public inputType InputType { get; set; }


        private void EditTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
          //  dataModel dm = this.DataContext as dataModel;
          //  if (!dm.Touch) return;
            TextBox tb = sender as TextBox;
            Binding myBinding = BindingOperations.GetBinding(tb, TextBox.TextProperty);

            editTextBox tbE = sender as editTextBox;

            string title = "";

            iInputPad numPad;
            if (InputType == inputType.numeric)
                numPad = new inputNumeric();
            else if (InputType == inputType.param)
                numPad = new inputParam();
            else 
                numPad = new inputAlpha();

            if (myBinding != null)
            {
                Binding newBinding = new Binding(myBinding.Path.Path);
                newBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
                numPad.TextBoxVal.SetBinding(TextBox.TextProperty, newBinding);
                numPad.DataContext = tb.DataContext; // Huom tähän mahdollinen korjaus Datacontexti 
                title = myBinding.Path.Path;
            } 
            else
            {
                if (tb.Tag != null && tbE.PlcName != null)
                {
                    Console.WriteLine("TAGI AVATAAN");
                    string tag = tb.Tag.ToString();
                    string plc = tbE.PlcName;
                    string bindStr = "plc[" + plc + "].tags[" + tag + "].Val";
                    Binding newBinding = new Binding(bindStr);
                    newBinding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
                    numPad.TextBoxVal.SetBinding(TextBox.TextProperty, newBinding);
                    Page pg = helper.FindParentPage(tb);
                    if ( pg != null)
                    numPad.DataContext = pg.DataContext;
                    title = bindStr;
                }
                else
                {
                    numPad.TextBoxVal.Text = tb.Text;
                    numPad.senderOb = sender;
                }
            }
           // string plc1 = "";
            string pattern = "\\[(.*?)\\]";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(title);
            foreach (Match match in matches)
            {
                foreach (Capture capture in match.Groups[1].Captures)
                    title = capture.ToString();
            }

            /*
            if (matches.Count == 2)
                foreach (Capture capture in matches[0].Groups[1].Captures)
                    plc1 =  " PLC:"+capture.ToString();*/

            dragPopup popup = new dragPopup();
            popupControl popCtrl = new popupControl(numPad.ctrl);
            popCtrl.title.Text = title;
            popCtrl.Popup = popup;
            numPad.Popup = popup;
            popup.Width = popCtrl.Width;
            popup.Height = popCtrl.Height;
            popup.Child = popCtrl;
            popup.Placement = PlacementMode.MousePoint;
            //  popup.PopupAnimation = PopupAnimation.Fade;
            popup.IsOpen = true;

        }
    }
}
