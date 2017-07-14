using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Geomystery.Pages;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Geomystery.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        private void Game_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SelectChapter));
        }

        private void Freestyle_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Freestyle));
        }

        private void Option_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Option));
        }

        private void Achievement_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Achievement));
        }
    }
}
