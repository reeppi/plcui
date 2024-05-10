using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.Data;
using System.Windows.Controls.Primitives;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace libPLC
{
    public class dragPopup : Popup
    {
        Point _initialMousePosition;
        bool _isDragging;

        public dragPopup()
        {
       

        }
        protected override void OnInitialized(EventArgs e)
        {
            
            var contents = Child as FrameworkElement;

            contents.MouseLeftButtonDown += Child_MouseLeftButtonDown;
            contents.MouseLeftButtonUp += Child_MouseLeftButtonUp;
            contents.MouseMove += Child_MouseMove;

            this.AllowsTransparency = true;
        }

        private void Child_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            _initialMousePosition = e.GetPosition(null);

            Point pos = Child.PointToScreen(new Point(0, 0));
            Console.WriteLine("pos.X-:" + pos.X);
            Console.WriteLine("pos.Y-:" + pos.Y);


            /*
             this.HorizontalOffset = pos.X;
            this.VerticalOffset = pos.Y;
            Placement = PlacementMode.Absolute;
            this.IsOpen = false;
            this.IsOpen = true;
            */
            _isDragging = true;
            e.Handled = true;
            Console.WriteLine("Press");
        }

        private void Child_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                var currentPoint = e.GetPosition(null);
                HorizontalOffset = HorizontalOffset + (currentPoint.X - _initialMousePosition.X);
                VerticalOffset = VerticalOffset + (currentPoint.Y - _initialMousePosition.Y);
            }
        }

        private void Child_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                var element = sender as FrameworkElement;
                element.ReleaseMouseCapture();
                _isDragging = false;
                e.Handled = true;
            }


        }
    }

   
}
