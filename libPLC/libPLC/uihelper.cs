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


namespace libPLC
{
    public class nav
    {
        /*
        public static void navigateButtonClick(object sender, RoutedEventArgs e)
        {
            string page = ((Button)sender).Tag.ToString();

            switch (page)
            {
                case "1": frame.Navigate(new Page1()); break;
                case "2": frame.Navigate(new Page2()); break;
            }
        }*/
    }

    public static class helper
    {
        public static Page FindParentPage(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            if (parent == null) return null;
            Page parentControl = parent as Page;
            if (parentControl != null)
            {
                return parentControl;
            }
            else
            {
                //use recursion until it reaches a Window
                return FindParentPage(parent);
            }
        }


        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }


    }
   



}
