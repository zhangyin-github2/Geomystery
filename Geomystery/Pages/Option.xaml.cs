using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Geomystery
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Option : Page
    {

        public Option()
        {
            this.InitializeComponent();
            View = new ViewModel.ViewModel();
            NightMode.IsChecked = APPDATA.ISNIGHT;
            Fullscreen.IsChecked = !APPDATA.ISFULLSCREEN;
        }

        private ViewModel.ViewModel View { set; get; } = new ViewModel.ViewModel();

        private void NightMode_Click(object sender, RoutedEventArgs e)
        {
            APPDATA.ISNIGHT = !APPDATA.ISNIGHT;
            View.Theme = !APPDATA.ISNIGHT ? ElementTheme.Light : ElementTheme.Dark;
        }

        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }

        private void Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            bool isInFullScreenMode = view.IsFullScreenMode;
            APPDATA.ISFULLSCREEN = isInFullScreenMode;
            if (isInFullScreenMode)
            {
                view.ExitFullScreenMode();
            }
            else
            {
                view.TryEnterFullScreenMode();
            }
        }
    }
}
