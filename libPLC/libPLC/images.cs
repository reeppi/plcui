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
using System.Windows.Media.Animation;

namespace libPLC
{
    public static class classData
    {
        public static classRes res = null;
    }

    public enum imgI
    {
        none,
        cylinder,
        down,
        fluid,
        fw,
        fwTurn,
        fwY,
        hataseis,
        indicator,
        indicatorSmall,
        in1,
        out1,
        reference,
        run,
        rw,
        rwTurn,
        rwY,
        start,
        up
    }


    public class classRes
    {
        public Dictionary<imgI, bmpEntry> images;

        public classRes()
        {
            images = new Dictionary<imgI, bmpEntry>();
            images.Add(imgI.cylinder, new bmpEntry("img/cylinderOff.png", "img/cylinderON.png"));
            images.Add(imgI.down, new bmpEntry("img/downOff.png", "img/downON.png"));
            images.Add(imgI.fluid, new bmpEntry("img/fluidOff.png", "img/fluidON.png"));
            images.Add(imgI.fw, new bmpEntry("img/fwOff.png", "img/fwON.png"));
            images.Add(imgI.fwTurn, new bmpEntry("img/fwTurnOff.png", "img/fwTurnON.png"));
            images.Add(imgI.fwY, new bmpEntry("img/fwYOff.png", "img/fwYON.png"));
            images.Add(imgI.hataseis, new bmpEntry("img/hataseisON.png", "img/hataseisOff.png"));
            images.Add(imgI.indicator, new bmpEntry("img/indicatorOff.png", "img/indicatorOn.png"));
            images.Add(imgI.indicatorSmall, new bmpEntry("img/indicatorOffSmall.png", "img/indicatorOnSmall.png"));
            images.Add(imgI.in1, new bmpEntry("img/inOff.png", "img/inON.png"));
            images.Add(imgI.out1, new bmpEntry("img/outOff.png", "img/outON.png"));
            images.Add(imgI.reference, new bmpEntry("img/refOff.png", "img/refON.png"));
            images.Add(imgI.run, new bmpEntry("img/runOff.png", "img/runON.png"));
            images.Add(imgI.rw, new bmpEntry("img/rwOff.png", "img/rwON.png"));
            images.Add(imgI.rwTurn, new bmpEntry("img/rwTurnOff.png", "img/rwTurnON.png"));
            images.Add(imgI.rwY, new bmpEntry("img/rwYOff.png", "img/rwYON.png"));
            images.Add(imgI.start, new bmpEntry("img/startOff.png", "img/startON.png"));
            images.Add(imgI.up, new bmpEntry("img/upOff.png", "img/upON.png"));
        }


        public class bmpEntry
        {
            public BitmapImage imgOn { get; set; }
            public BitmapImage imgOff { get; set; }

            public bmpEntry(string uriOffStr, string uriOnStr)
            {
                Uri fileUriOn = new Uri(uriOnStr, UriKind.Relative);
                Uri fileUriOff = new Uri(uriOffStr, UriKind.Relative);
                imgOn = new BitmapImage(fileUriOn);
                imgOff = new BitmapImage(fileUriOff);
            }
        }
    }




}
