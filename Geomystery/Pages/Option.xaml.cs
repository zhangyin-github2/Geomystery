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
            View = new ViewModel.ViewModel();
            this.InitializeComponent();
        }

        private ViewModel.ViewModel View { set; get; } = new ViewModel.ViewModel();

        private void NightMode_Click(object sender, RoutedEventArgs e)
        {
            if (NightMode.IsChecked == true)
            {
                View.Theme = ElementTheme.Dark;
                //(Application.Current.Resources["fg"] as SolidColorBrush).Color = Color.FromArgb(255, r, g, b);
                //(Application.Current.Resources["bg"] as SolidColorBrush).Color = Color.FromArgb(255, r, g, b);
            } 
            else
            {
                View.Theme = ElementTheme.Light;
                //(Application.Current.Resources["fg"] as SolidColorBrush).Color = Color.FromArgb(255, r, g, b);
                //(Application.Current.Resources["bg"] as SolidColorBrush).Color = Color.FromArgb(255, r, g, b);
            }
                
        }

        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(About));
        }
    }
}
