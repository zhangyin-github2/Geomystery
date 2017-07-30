using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Geomystery
{
    public enum Direction { Up = 0, Down = 1, Left = 2, Right = 3 };
    public enum NotificationAudioNames
    {
        Default,
        IM,
        Mail,
        Reminder,
        SMS,
        Looping_Alarm,
        Looping_Alarm2,
        Looping_Alarm3,
        Looping_Alarm4,
        Looping_Alarm5,
        Looping_Alarm6,
        Looping_Alarm7,
        Looping_Alarm8,
        Looping_Alarm9,
        Looping_Alarm10,
        Looping_Call,
        Looping_Call2,
        Looping_Call3,
        Looping_Call4,
        Looping_Call5,
        Looping_Call6,
        Looping_Call7,
        Looping_Call8,
        Looping_Call9,
        Looping_Call10,
    }
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
        public static void ShowToastNotification(string assetsImageFileName, string text, NotificationAudioNames audioName)
        {
            // 1. create element
            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            // 2. provide text
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(text));

            // 3. provide image
            XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", $"ms-appx:///assets/{assetsImageFileName}");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "logo");

            // 4. duration
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "short");

            // 5. audio
            XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", $"ms-winsoundevent:Notification.{audioName.ToString().Replace("_", ".")}");
            toastNode.AppendChild(audio);

            // 6. app launch parameter
            //((XmlElement)toastNode).SetAttribute("launch", "{\"type\":\"toast\",\"param1\":\"12345\",\"param2\":\"67890\"}");

            // 7. send toast
            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
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
        private static ResourceLoader _loader;
        public static readonly Dictionary<string, string> ResourceCache = new Dictionary<string, string>();
        private static ResourceLoader CurrentResourceLoader
        {
            get { return _loader ?? (_loader = ResourceLoader.GetForCurrentView("Resources")); }
        }
        public static void refresh()
        {
            _loader = ResourceLoader.GetForCurrentView("Resources");
            ResourceCache.Clear();
        }
        

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
