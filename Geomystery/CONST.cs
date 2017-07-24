using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Geomystery
{
    public static class CONST
    {
        public static string volume1 = "\xE993";
        public static string volume2 = "\xE994";
        public static string volume3 = "\xE995";
        public static string mute = "\xE74F";
        public static string yes = "\x2714";
        public static string no = "\x2716";

        public static void GridMove(Grid back,int direction,double from=0,double to=0)
        {
            back.RenderTransform = new CompositeTransform();

            var storyBoard = new Storyboard();
            var extendAnimation1 = new DoubleAnimation();

            extendAnimation1 = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(0.5)), From = from, To = to, EnableDependentAnimation = true };

            Storyboard.SetTarget(extendAnimation1, back);
            Storyboard.SetTargetProperty(extendAnimation1, "(UIElement.RenderTransform).(CompositeTransform .TranslateX)");
            storyBoard.Children.Add(extendAnimation1);
            storyBoard.Begin();
        } 
    }
    public class intToVisibilityConverter : IValueConverter
    {

        #region IValueConverter Members

        // Define the Convert method to change a DateTime object to 
        // a month string.
        public object Convert(object value, Type targetType,
            object parameter, string language)
        {
            // The value parameter is the data from the source object.
            var k = (int)value;
            var v = Visibility.Collapsed;
            if (k == 0) v = Visibility.Visible;
            // Return the month value to pass to the target.
            return v;
        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
