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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Geomystery
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SelectGame : Page
    {
        private ViewModel.ViewModel View { set; get; } = new ViewModel.ViewModel();
        public ObservableCollection<Level> levels = new ObservableCollection<Level>();
        public List<Grid> GridInListView = new List<Grid>();
        public List<TextBlock> TextInListView = new List<TextBlock>();
        public static Chapter localChapter = new Chapter() ;
        public SelectGame()
        {
            this.InitializeComponent();
            View = new ViewModel.ViewModel();
            if (!APPDATA.app_data.Views.Contains(View))
            {
                APPDATA.app_data.Views.Add(View);
            }
            init();
        }
        public void init()
        {
            
            foreach(var x in levels)
            {
                if (x.ID - 1 <= APPDATA.app_data.HAVEDONE) x.unlocked = 1;
                else x.unlocked = 0;
            }
            double w, h;
            h = Window.Current.Bounds.Height;
            w = Window.Current.Bounds.Width;
            foreach (var x in GridInListView)
            {
                x.Width = 450 * w / 1920.0;
                x.Height = 250 * h / 1080.0;
            }
            var k = Math.Min(w / 1920.0, h / 1080.0);
            foreach (var x in TextInListView)
            {
                string ss = x.Text;
                if (!char.IsDigit(ss[0]))
                    x.FontSize = 32 * k;
                else x.FontSize = 36 * k;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var x = localChapter = e.Parameter as Chapter;
            levels = x.Levels;
            back.Source = new BitmapImage(new Uri(x.cover, UriKind.Absolute));
            init();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            GridInListView.Add((Grid)sender);
        }
        
        private void Page_LayoutUpdated(object sender, object e)
        {
            double w, h;
            h = Window.Current.Bounds.Height;
            w = Window.Current.Bounds.Width;
            foreach (var x in GridInListView)
            {
                x.Width = 450 * w / 1920.0;
                x.Height = 250 * h / 1080.0;
            }
        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w, h, k;
            h = Window.Current.Bounds.Height;
            w = Window.Current.Bounds.Width;
            k = Math.Min(w / 1920.0, h / 1080.0);
            foreach (var x in TextInListView)
            {
                string ss = x.Text;
                if (!char.IsDigit(ss[0]))
                    x.FontSize = 32 * k;
                else x.FontSize = 36 * k;
            }
        }
        private void levelbord_ItemClick(object sender, ItemClickEventArgs e)
        {
            var x = e.ClickedItem as Level;
            MainPage.debugTxt.Text = x.cover.ToString();
            if (x.unlocked == 0) return;
            APPDATA.app_data.MoveTo(AppPage.GamePage,x);
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            var x = sender as TextBlock;
            double w, h,k;
            h = Window.Current.Bounds.Height;
            w = Window.Current.Bounds.Width;
            k = Math.Min(w / 1920.0, h / 1080.0);
            string ss = x.Text;
            try
            {
                Convert.ToInt32(ss);
                x.FontSize = 36 * k;
            }
            catch
            {
                x.FontSize = 28 * k;
            }
            TextInListView.Add(x);
        }
    }
}
