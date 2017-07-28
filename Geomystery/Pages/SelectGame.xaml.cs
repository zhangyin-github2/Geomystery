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
    public sealed partial class SelectGame : Page
    {
        private ViewModel.ViewModel View { set; get; } = new ViewModel.ViewModel();
        public ObservableCollection<Level> levels;
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
            levels = Level.getLevels();
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            GridInListView.Add((Grid)sender);
        }
        public List<Grid> GridInListView = new List<Grid>();
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
        private void levelbord_ItemClick(object sender, ItemClickEventArgs e)
        {
            var x = e.ClickedItem as Level;
            MainPage.debugTxt.Text = x.cover.ToString();
            if (x.unlocked == 0) return;
            MainPage.MainFrame.Navigate(typeof(Game),x);
            APPDATA.app_data.MoveTo(AppPage.GamePage);
            APPDATA.app_data.CURRENT_PAGE = AppPage.GamePage;
        }
    }

    
}
