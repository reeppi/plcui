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
    /// Interaction logic for previewControl.xaml
    /// </summary>
    public partial class previewControl : UserControl
    {
        public static readonly DependencyProperty dataHasChangedProperty = DependencyProperty.Register("DataHasChanged", typeof(bool), typeof(previewControl), new FrameworkPropertyMetadata(dataHasChanged));
        public bool DataHasChanged { get { return (bool)GetValue(dataHasChangedProperty); } set { SetValue(dataHasChangedProperty, value); } }


        public static readonly DependencyProperty recipeProperty = DependencyProperty.Register("RecipeControl", typeof(recipeControl), typeof(previewControl), new FrameworkPropertyMetadata(currentRecipeChanged));
        public recipeControl RecipeControl { get { return (recipeControl)GetValue(recipeProperty); } set { SetValue(recipeProperty, value); } }

        public bool Abs { get; set; }

        private static void dataHasChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            previewControl ctrl = d as previewControl;
            if ((bool)e.NewValue)
                ctrl.drawPreview();
        }

        private static void currentRecipeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            previewControl ctrl = d as previewControl;
          //  ctrl.readPath((string)e.NewValue);
        }

        public previewControl()
        {
            InitializeComponent();
        }

        private void drawPreview()
        {
            dataModel dm = this.DataContext as dataModel;

            Console.WriteLine("Draw Preview : W=" + this.Width + " " + this.Height);
            if (RecipeControl != null)
                RecipeControl.DataHasChanged = false;
            plcdata plcdata = new plcdata(RecipeControl.dataGrid,null);
            pCanvas.Children.Clear();

            if (plcdata.data.Count == 0) return;

            double maxVal = plcdata.data.Max(x => x.pos);
            double xFact = this.Width / maxVal;
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;

            SolidColorBrush blueBrush = new SolidColorBrush();
            blueBrush.Color = Colors.Blue;

            SolidColorBrush whiteBrush = new SolidColorBrush();
            whiteBrush.Color = Colors.White;

            SolidColorBrush graykBrush = new SolidColorBrush();
            graykBrush.Color = Colors.Gray;

            foreach (plcDataEntry entry in plcdata.data)
            {
                entry.textblock = new TextBlock();
                if ( !Abs )
                    entry.textblock.Text = entry.rel.ToString();
                else
                    entry.textblock.Text = entry.pos.ToString();

                entry.textblock.Foreground = blackBrush;

                if ( RecipeControl.selIndexes != null )
                {
                    if ( RecipeControl.selIndexes.Contains(entry.index))
                        entry.textblock.Foreground = blueBrush;
                }

                entry.textblock.FontSize = 12;
                Size msrSize = new Size(200, 200);
                entry.textblock.Measure(msrSize);
                Size si = entry.textblock.DesiredSize;
                entry.textWidth = si.Width;
            }

            for  (int i=0; i<plcdata.data.Count; i++)
            {
                plcDataEntry plcEntry = plcdata.data[i];

                int lvl = 1;
                for (int x = i + 1; x < plcdata.data.Count; x++)
                {
                    plcDataEntry plcEntryD = plcdata.data[x];
                    if ( plcEntry.textWidth + plcEntry.pos*xFact + plcEntry.textblock.FontSize/2   > plcEntryD.pos*xFact)
                    {
                        plcEntryD.textLevel = lvl;
                        lvl++;
                        i++;
                    }
                }
           }

      
            foreach ( plcDataEntry plcEntry in plcdata.data )
            {

                Line line = new Line();
                line.StrokeThickness = 1;
                line.Stroke = graykBrush;
                line.Y1 = plcEntry.textblock.FontSize * plcEntry.textLevel;
                line.Y2 = this.Height;
                pCanvas.Children.Add(line);
                Canvas.SetLeft(line, plcEntry.pos * xFact);
                Canvas.SetLeft(plcEntry.textblock, plcEntry.pos * xFact);
                Canvas.SetTop(plcEntry.textblock, plcEntry.textLevel* plcEntry.textblock.FontSize);
                pCanvas.Children.Add(plcEntry.textblock);
            }

            TextBlock holesText = new TextBlock();
            holesText.Text = "="+plcdata.data.Count.ToString();
            if (Abs)
                holesText.Text += " (ABS)";
            holesText.Foreground = whiteBrush;
            holesText.Background = blackBrush;
            holesText.FontSize = 12;
            Size msrSize1 = new Size(200, 200);
            holesText.Measure(msrSize1);
            Size si1 = holesText.DesiredSize;

            Canvas.SetLeft(holesText, 0 );
            Canvas.SetTop(holesText, this.Height - si1.Height + 2 );
            pCanvas.Children.Add(holesText);

        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (RecipeControl != null)
                Console.WriteLine(" Preview Control "+ RecipeControl.FilePath);
        }

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Abs = !Abs;
            drawPreview();
        }
    }
}
