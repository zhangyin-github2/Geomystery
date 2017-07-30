using Geomystery.Award;
using Geomystery.Models;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Geomystery
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SelectChapter : Page
    {
        private ViewModel.ViewModel View { set; get; } = new ViewModel.ViewModel();
        ObservableCollection<Chapter> Chapters = APPDATA.app_data.Chapters;
        List<TextBlock> ChpName = new List<TextBlock>(), ChpDiscribe = new List<TextBlock>();
        List<Grid> Grids = new List<Grid>();
        List<Button> Buttons = new List<Button>();
        public SelectChapter()
        {
            this.InitializeComponent();
            View = new ViewModel.ViewModel();
            if (!APPDATA.app_data.Views.Contains(View))
            {
                APPDATA.app_data.Views.Add(View);
            }
            init();
        }
        void init()
        {
            changeSize();
            for(int i =0;i<Buttons.Count;i++)
            {
                ImageBrush imbrush = new ImageBrush();
                imbrush.ImageSource = new BitmapImage(new Uri(Chapters[i].cover, UriKind.Absolute));
                Buttons[i].Background = imbrush;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            foreach(var x in Chapters)
            {
                if (APPDATA.app_data.HAVEDONE / 9 >= x.ID-1) x.unlocked = 1;
            }
        }

        private void ChpNameTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            var k = sender as TextBlock;
            ChpName.Add(k);
            changeSize();
        }

        private void ChpDiscibeTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            var k = sender as TextBlock;
            ChpDiscribe.Add(k);
            changeSize();
        }

        private void ChapterB_Loaded(object sender, RoutedEventArgs e)
        {
            var k = sender as Button;
            Buttons.Add(k);
            changeSize();
            for (int i = 0; i < Buttons.Count; i++)
            {
                ImageBrush imbrush = new ImageBrush();
                imbrush.ImageSource = new BitmapImage(new Uri(Chapters[i].cover, UriKind.Absolute));
                Buttons[i].Background = imbrush;
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Grids.Add(sender as Grid);
            changeSize();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            changeFonts(); 
            changeSize();
        }

        void changeFonts()
        {
            double w, h, k;
            h = Window.Current.Bounds.Height;
            w = Window.Current.Bounds.Width;
            k = Math.Min(w / 1920.0, h / 1080.0);
            foreach (var x in ChpName)
            {
                x.FontSize = 56 * k;
            }
            foreach (var x in ChpDiscribe)
            {
                x.FontSize = 24 * k;
            }          
        }

        void changeSize()
        {
            double w, h, k;
            h = Window.Current.Bounds.Height;
            w = Window.Current.Bounds.Width;
            k = Math.Min(w / 1920.0, h / 1080.0);
            foreach (var g in Grids)
            {
                g.Width = 1100 * k;
                g.Height = g.Width * (9 / 16.0);
            }
            foreach (var b in Buttons)
            {
                b.Width = 1200 * k;
                b.Height = b.Width * (9 / 16.0);
            }
            changeFonts();
        }

        private void Page_LayoutUpdated(object sender, object e)
        {
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            init();
        }

        private void Chapter_Click(object sender, RoutedEventArgs e)
        {
            BGMPlayer.PlayButton7();
            Chapter k = myFlip.SelectedItem as Chapter;
            if (k.unlocked==0) return;
            APPDATA.app_data.MoveTo(AppPage.SelectGamePage,k);
        }
    }
}
