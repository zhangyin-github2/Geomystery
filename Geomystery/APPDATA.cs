using Geomystery.Award;
using Geomystery.Models;
using Geomystery.Pages;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Geomystery
{
    public enum AppPage {HomePage=0,OptionPage,AchievementPage,SelectChapterPage,SelectGamePage,GamePage,FreeStylePage,AboutPage};
    public class APPDATA
    {
        //这部分是需要系统保存在数据库中的配置
        public bool ISMUTE { get; set; }
        public bool ISNIGHT { get; set; }
        public bool ISFULLSCREEN { get; set; }
        public string LANGGUAGE { get; set; }
        public int HAVEDONE { get; set; }
        public double SFXVOLUME { get; set; }
        public double MUSICVOLUME { get; set; }

        //系统内部配置
        public ObservableCollection<Level> cp1 = Level.getLevels();
        public int GAMEMODE { get; set; }
        public List<ViewModel.ViewModel> Views;
        public AppPage CURRENT_PAGE { get; set; }
        public Grid MAINGRID { get; set; }
        public Button BACKBUTTON { get; set; }

        /// <summary>
        /// 单件模式实例体
        /// </summary>
        public static APPDATA app_data;

        public APPDATA()
        {
            Views = new List<ViewModel.ViewModel>();
            ISMUTE = false;
            ISNIGHT = false;
            ISFULLSCREEN=true;
            LANGGUAGE = Windows.Globalization.ApplicationLanguages.Languages[0];
            HAVEDONE = 0;
            MUSICVOLUME = 1;
            SFXVOLUME = 1;
            GAMEMODE = -1;
            MAINGRID = new Grid();
        }
        public static void SAVE()
        {
            string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "APPDATA.db");
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);
            conn.CreateTable<option_data>();// 创建 option_data 模型对应的表，如果已存在，则忽略该操作。
            var db = conn.Table<option_data>();
            var k = new option_data();
            if (db.Count() == 0) conn.Insert(k);
            else
            {
                conn.DeleteAll(typeof(option_data));
                conn.Insert(k);
            }
        }
        public static void LOAD()
        {
            string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "APPDATA.db");
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);
            conn.CreateTable<option_data>();// 创建 option_data 模型对应的表，如果已存在，则忽略该操作。
            var db = conn.Table<option_data>();
            if (db.Count() == 0) return;
            else
            {
                option_data k = db.First();
                app_data.ISFULLSCREEN = k.ISFULLSCREEN;
                app_data.ISMUTE = k.ISMUTE;
                app_data.ISNIGHT = k.ISNIGHT;
                app_data.HAVEDONE = k.HAVEDONE;
                app_data.LANGGUAGE = k.LANGGUAGE;
                app_data.SFXVOLUME = k.SFXVOLUME;
                app_data.MUSICVOLUME = k.MUSICVOLUME;
            }
        }
        public async void change_language()
        {
            if (app_data.LANGGUAGE != "en-US") app_data.LANGGUAGE = "en-US";
            else app_data.LANGGUAGE = "zh-CN";
           
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = app_data.LANGGUAGE;
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();
            await Task.Delay(50);
            update_grid();
        }
        public void setFullScreen()
        {
            ApplicationView current_window = ApplicationView.GetForCurrentView();
            app_data.ISFULLSCREEN = current_window.IsFullScreenMode;
            if (app_data.ISFULLSCREEN)
            {
                current_window.ExitFullScreenMode();
            }
            else
            {
                current_window.TryEnterFullScreenMode();
            }
            app_data.ISFULLSCREEN = current_window.IsFullScreenMode;
        }
        public void setNight()
        {
            app_data.ISNIGHT = !app_data.ISNIGHT;
            update_views();
        }
        public void setVolume(double x)
        {
             app_data.SFXVOLUME = BGMPlayer.MusicPlayer.MediaPlayer.Volume = 1.00 * x;
        }
        public void setBgmVolume(double x)
        {
            app_data.MUSICVOLUME = BGMPlayer.BgmPlayer.MediaPlayer.Volume = 1.00 * x;
        }
        public void setMute()
        {
            app_data.ISMUTE = !app_data.ISMUTE;
            BGMPlayer.setMute();
        }
        public void update_views()
        {
            foreach(var View in Views)
            {
                View.Theme = !app_data.ISNIGHT ? ElementTheme.Light : ElementTheme.Dark;
            }
        }
        public void update_grid()
        {
            var x = app_data.MAINGRID.Children;
            MainPage.debugTxt.Text = "";
            foreach (var c in x)
            {
                if(c is Frame)
                {
                    var k = c as Frame;
                    string name = k.Name;
                    switch (name)
                    {
                        case "myFrame": k.Navigate(typeof(HomePage)); break;
                        case "optionFrame": k.Navigate(typeof(Option)); break;
                        case "achievementFrame": k.Navigate(typeof(Achievement));  break;
                    }
                }
            }

        }
        async public Task<int> Reset()
        {
            var dialog = new ContentDialog()
            {
                Title = "警告",
                Content = "请确认您是否要重置游戏的所有进程？",
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消",
                FullSizeDesired = false,
            };
            dialog.PrimaryButtonClick += (_s, _e) => { };
            var res = await dialog.ShowAsync();
            if (res.ToString() != "Primary") return 0;
            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            app_data.ISMUTE = false;
            app_data.ISNIGHT = false;
            app_data.ISFULLSCREEN = true;
            app_data.HAVEDONE = 0;
            app_data.MUSICVOLUME = app_data.SFXVOLUME = 1;
            update_views();
            return 0;
        }
        public void MoveTo(AppPage to)
        {
            switch (to)
            {
                case AppPage.OptionPage:
                    if (CURRENT_PAGE == AppPage.HomePage) GridMove(Direction.Down);
                    else;
                    break;
                case AppPage.AchievementPage: GridMove(Direction.Right); break;
                case AppPage.HomePage:
                    if (CURRENT_PAGE == AppPage.OptionPage) GridMove(Direction.Up);
                    else if (CURRENT_PAGE == AppPage.AchievementPage) GridMove(Direction.Left);
                    else 
                    {
                        MainPage.MainFrame.Navigate(typeof(HomePage));
                    }
                    break;
                case AppPage.AboutPage:;break;
                case AppPage.FreeStylePage: MainPage.MainFrame.Navigate(typeof(Freestyle)); BGMPlayer.BgmPlayer.MediaPlayer.Pause();  break;
                case AppPage.SelectChapterPage: MainPage.MainFrame.Navigate(typeof(SelectChapter)); break;
                case AppPage.SelectGamePage: MainPage.MainFrame.Navigate(typeof(SelectGame)); break;
                case AppPage.GamePage: BGMPlayer.BgmPlayer.MediaPlayer.Pause(); break;
                default: return;
            }
            if (to != AppPage.HomePage) BACKBUTTON.Visibility = Visibility.Visible;
            else BACKBUTTON.Visibility = Visibility.Collapsed;
            if ((to != AppPage.GamePage && to != AppPage.FreeStylePage)) BGMPlayer.BgmPlayer.MediaPlayer.Play();
            app_data.CURRENT_PAGE = to;
        }
        /// <summary>
        /// 移动MAINGRID
        /// </summary>
        /// <param name="direction">
        /// 0上 1下 2左 3右
        /// </param>
        public void GridMove(Direction direction)
        {
            double from, to;
            double w, h;
            w = Window.Current.Bounds.Width; h = Window.Current.Bounds.Height;
            switch ((int)direction)
            {
                case 0:   from = -h ; to = 0  ;   break;
                case 1:   from = 0  ; to = -h ;   break;
                case 2:   from = -w ; to = 0  ;   break;
                case 3:   from = 0  ; to = -w ;   break;
                default:  from = 0  ; to = 0  ;   break;
            }
            CONST.GridMove(MAINGRID, direction,from,to);
        }

        public string show()
        {
            var ss = "";
            ss = ISMUTE.ToString() + " " + ISNIGHT.ToString() + " " + ISFULLSCREEN;
            return ss;
        }
    }
    public class option_data
    {
        public bool ISMUTE { get; set; }
        public bool ISNIGHT { get; set; }
        public bool ISFULLSCREEN { get; set; }
        public int HAVEDONE { get; set; }
        public string LANGGUAGE { get; set; }
        public double SFXVOLUME { get; set; }
        public double MUSICVOLUME { get; set; }
        public option_data()
        {
            ISMUTE = APPDATA.app_data.ISMUTE;
            ISNIGHT = APPDATA.app_data.ISNIGHT;
            ISFULLSCREEN = APPDATA.app_data.ISFULLSCREEN;
            HAVEDONE = APPDATA.app_data.HAVEDONE;
            LANGGUAGE = APPDATA.app_data.LANGGUAGE;
            SFXVOLUME = APPDATA.app_data.SFXVOLUME;
            MUSICVOLUME = APPDATA.app_data.MUSICVOLUME;
        }
    }
}
