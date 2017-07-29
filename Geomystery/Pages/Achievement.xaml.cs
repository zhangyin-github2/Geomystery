using Geomystery.Award;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Geomystery
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Achievement : Page
    {
        private ViewModel.ViewModel View { set; get; } = new ViewModel.ViewModel();
        
        public  ObservableCollection<Achievements> ach;
        public ObservableCollection<TextBlock> nameT = new ObservableCollection<TextBlock>(), disT = new ObservableCollection<TextBlock>(), lockT = new ObservableCollection<TextBlock>();
        public ObservableCollection<Grid> grids = new ObservableCollection<Grid>();
        public ObservableCollection<Image> ims = new ObservableCollection<Image>();
        public Achievement()
        {
            this.InitializeComponent();
            changefont();
            View = new ViewModel.ViewModel();
            if (!APPDATA.app_data.Views.Contains(View))
            {
                APPDATA.app_data.Views.Add(View);
            }
            ach = Achievements.GetAch(10);
            APPDATA.app_data.ACHIEVEMENTS = ach;
        }
        private void nameTloaded(object sender, RoutedEventArgs e)
        {
            var k = sender as TextBlock;
            nameT.Add(k);
        }
        private void disTloaded(object sender, RoutedEventArgs e)
        {
            var k = sender as TextBlock;
            disT.Add(k);

        }
        private void lockTloaded(object sender, RoutedEventArgs e)
        {
            var k = sender as TextBlock;
            lockT.Add(k);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var k = sender as Grid;
            grids.Add(k);
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            var k = sender as Image;
            ims.Add(k);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            changefont();
        }

        void changefont()
        {
            double w, h, k;
            h = Window.Current.Bounds.Height;
            w = Window.Current.Bounds.Width;
            k = Math.Min(w / 1920.0, h / 1080.0);
            foreach (var x in nameT)
            {
                x.FontSize = 28 * k;
            }
            foreach (var x in disT)
            {
                x.FontSize = 24 * k;
            }
            foreach (var x in lockT)
            {
                x.FontSize = 18 * k;
            }
            foreach(var x in grids)
            {
                x.Width = 750 * w / 1920.0;          
            }
            foreach(var x in ims)
            {
                x.Height = x.Width = 80 * k;
            }
        }
    }
}
