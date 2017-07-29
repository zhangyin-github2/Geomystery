using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Geomystery
{
    public enum Direction { Up = 0, Down = 1, Left = 2, Right = 3 };
    public static class CONST
    {
        public static string volume1 = "\xE993";
        public static string volume2 = "\xE994";
        public static string volume3 = "\xE995";
        public static string mute = "\xE74F";
        public static string yes = "\x2714";
        public static string no = "\x2716";

        public static void GridMove(Grid back,Direction direction,double from=0,double to=0)
        {
            back.RenderTransform = new CompositeTransform();

            var storyBoard = new Storyboard();
            var extendAnimation1 = new DoubleAnimation();

            extendAnimation1 = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(0.5)), From = from, To = to, EnableDependentAnimation = true };

            Storyboard.SetTarget(extendAnimation1, back);
            if (direction<Direction.Left)
                Storyboard.SetTargetProperty(extendAnimation1, "(UIElement.RenderTransform).(CompositeTransform .TranslateY)");
            else
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
    public static class AppResources
    {
        private static ResourceLoader CurrentResourceLoader
        {
            get { return _loader ?? (_loader = ResourceLoader.GetForCurrentView("Resources")); }
        }

        private static ResourceLoader _loader;
        private static readonly Dictionary<string, string> ResourceCache = new Dictionary<string, string>();

        public static string GetString(string key)
        {
            string s;
            if (ResourceCache.TryGetValue(key, out s))
            {
                return s;
            }
            else
            {
                s = CurrentResourceLoader.GetString(key);
                ResourceCache[key] = s;
                return s;
            }
        }
    }
}
