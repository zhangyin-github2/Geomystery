using Geomystery.Award;
using Geomystery.Controllers.Geometry;
using Geomystery.Models;
using Geomystery.Models.Geometry;
using Geomystery.Pages;
using Geomystery.Views.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Geomystery
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Game : Page
    {
        Vector2 maxHeightWidth;

        List<UserTool> userTools;
        Level localLevel = new Level();
        bool Imopen = false;
        Geomystery.Controllers.Geometry.Controllers controller;

        public Game()
        {
            this.InitializeComponent();
            View = new ViewModel.ViewModel();
            if (!APPDATA.app_data.Views.Contains(View))
            {
                APPDATA.app_data.Views.Add(View);
            }
                 
            userTools = UserToolsManager.GetInstance().GetTools();
            init();
        }
        void init()
        {
            double kw, kh;
            kh = Window.Current.Bounds.Height / 1080;
            kw = Window.Current.Bounds.Width / 1920;
            double k = Math.Min(kw, kh);
            GameId.FontSize = Math.Max(36 * k, 12);
            GameName.FontSize = Math.Max(28 * k, 12);
            openImpic.Height = Math.Max(32 * k, 12);
            GameDiscribe.FontSize = Math.Max(24 * k, 8);
            if (APPDATA.app_data.HAVEDONE < localLevel.ID) 
                openIm_Click(openIm , new RoutedEventArgs());
            else
            {
                coverG.Visibility = Visibility.Collapsed;
            }
        }
        private ViewModel.ViewModel View { set; get; } = new ViewModel.ViewModel();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var x = localLevel = e.Parameter as Level;
            GameId.Text = x.ID.ToString();
            GameName.Text = x.name;
            GameImage.Source = new BitmapImage(new Uri(x.cover, UriKind.Absolute));
            GameDiscribe.Text = x.Discribe;

            controller = LevelLoader.GetLevel(x.ID);
            for(int i = 0; i < controller.givenConditionsId.Count; i++)
            {
                Models.Geometry.Geometry geometry = controller.coordinate.GetGeometryById(controller.givenConditionsId[i]);
                if(geometry is Point2)
                {
                    Point2 p2 = geometry as Point2;
                    p2.resultPoint.lineColor = Color.FromArgb(255, 201, 84, 191);
                }
            }
            init();

            this.Loaded += delegate { this.Focus(FocusState.Programmatic); };
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //controller = new Controllers.Geometry.Controllers();
            //controller.PreInitialized(LevelLoader.GetLevel(1));                 //第一关的控制器
            //controller = LevelLoader.GetLevel(1);

            controller.outputCoordinates[0].WindowHeight = (float)canvas1.ActualHeight;
            controller.outputCoordinates[0].WindowWidth = (float)canvas1.ActualWidth;
            maxHeightWidth = new Vector2((float)canvas1.ActualWidth, (float)canvas1.ActualHeight);
            //text1.Text = maxHeightWidth.X.ToString() + " | " + maxHeightWidth.Y.ToString();
            listView1.SelectedIndex = 2;
            controller.historyDfaList.Clear();
            redo.IsEnabled = controller.CanRedo();
            undo.IsEnabled = controller.CanUndo();

            controller.missionSuccess += success;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            canvas1.RemoveFromVisualTree();
            canvas1 = null;
        }
        
        private void canvas1_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            if (controller == null || !controller.isIniialized) return;
            var draw = args.DrawingSession;
            //Rect rect = new Rect(0, 0, maxHeightWidth.X, maxHeightWidth.Y);
            //args.DrawingSession.DrawRectangle(rect, Colors.Black);
            List<OutputGeometry> pointSetList = controller.outputCoordinates[0].outputPointSetList;
            if (pointSetList != null)
            {
                for (int i = 0; i < pointSetList.Count; i++)
                {
                    if (pointSetList[i].isVisible)
                    {
                        if (pointSetList[i] is OutputCircle)
                        {
                            var realCircle = pointSetList[i] as OutputCircle;
                            if (realCircle.circle.isSelected)
                            {
                                args.DrawingSession.DrawCircle(realCircle.center, realCircle.radius, realCircle.selectedLineColor, realCircle.thickness);
                            }
                            else
                            {
                                args.DrawingSession.DrawCircle(realCircle.center, realCircle.radius, realCircle.lineColor, realCircle.thickness);
                            }
                        }
                        else if (pointSetList[i] is OutputLine)
                        {
                            var realLine = pointSetList[i] as OutputLine;
                            if (realLine.line.isSelected)
                            {
                                args.DrawingSession.DrawLine(realLine.p1, realLine.p2, realLine.selectedLineColor, realLine.thickness);
                            }
                            else
                            {
                                args.DrawingSession.DrawLine(realLine.p1, realLine.p2, realLine.lineColor, realLine.thickness);
                            }
                        }
                    }
                }
            }

            List<OutputPoint> pointList = controller.outputCoordinates[0].outputPointList;
            if (pointList != null)
            {
                for (int i = 0; i < pointList.Count; i++)
                {
                    if (pointList[i].isVisible)
                    {
                        if (pointList[i].point.isSelected)
                        {
                            args.DrawingSession.FillCircle(pointList[i].viewPoint, OutputPoint.scopeLength, pointList[i].selectedFillColor);
                            args.DrawingSession.DrawCircle(pointList[i].viewPoint, OutputPoint.scopeLength, pointList[i].selectedLineColor, pointList[i].thickness);
                        }
                        else
                        {
                            args.DrawingSession.FillCircle(pointList[i].viewPoint, OutputPoint.scopeLength, pointList[i].fillColor);
                            args.DrawingSession.DrawCircle(pointList[i].viewPoint, OutputPoint.scopeLength, pointList[i].lineColor, pointList[i].thickness);
                        }
                    }
                }
            }

        }

        private void canvas1_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Point pxy = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
            Vector2 p = pxy.ToVector2();
            MainPage.debugTxt.Text = p.X.ToString() + " | " + p.Y.ToString();

            controller.PointerPressed((UserTool)listView1.SelectedItem, sender, e);

            redo.IsEnabled = controller.CanRedo();
            undo.IsEnabled = controller.CanUndo();

        }

        public async void success()
        {
            BGMPlayer.PlayButton8();
            LevelSucceedDialog lsd = new LevelSucceedDialog();
            APPDATA.app_data.HAVEDONE = Math.Max(APPDATA.app_data.HAVEDONE, localLevel.ID);
            var ach = APPDATA.app_data.ACHIEVEMENT;
            APPDATA.app_data.setAchievement();
            for(int i=0;i<6;i++)
            {
                if(ach[i]=='0'&& APPDATA.app_data.ACHIEVEMENT[i]=='1')
                {
                    CONST.ShowToastNotification("Square150x150Logo.scale-200.png", AppResources.GetString("isunlock") , NotificationAudioNames.Default);
                }
            }

            var res = await lsd.ShowAsync();
            if (res.ToString() == "Primary")
            {
                //APPDATA.app_data.MoveTo(AppPage.GamePage,localLevel);
            }
            else
            {
                if (localLevel.ID % 9 == 0)
                {
                    APPDATA.app_data.MoveTo(AppPage.SelectChapterPage);
                    var c = SelectGame.localChapter;
                    if (c.ID >= APPDATA.app_data.Chapters.Count) return;
                    APPDATA.app_data.Chapters[c.ID].unlocked = 1;
                    return;
                }
                var levels = Level.getLevels(SelectGame.localChapter.ID);
                foreach (var l in levels)
                {
                    if (l.ID == localLevel.ID + 1)
                    {
                        APPDATA.app_data.MoveTo(AppPage.GamePage,l);
                    }
                }
            }
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            BGMPlayer.PlayButton9();
            //controller.outputCoordinates[0].geometryList.Clear();
            controller = null;
            controller = LevelLoader.GetLevel(localLevel.ID);

            controller.historyDfaList.Clear();
            controller.outputCoordinates[0].refreshCanvas(canvas1);
            redo.IsEnabled = controller.CanRedo();
            undo.IsEnabled = controller.CanUndo();

            for (int i = 0; i < controller.givenConditionsId.Count; i++)
            {
                Models.Geometry.Geometry geometry = controller.coordinate.GetGeometryById(controller.givenConditionsId[i]);
                if (geometry is Point2)
                {
                    Point2 p2 = geometry as Point2;
                    p2.resultPoint.lineColor = Color.FromArgb(255, 201, 84, 191);
                }
            }

            controller.outputCoordinates[0].refreshGeometrys();         //刷新
            controller.missionSuccess += success;
        }

        private void undo_Click(object sender, RoutedEventArgs e)
        {
            BGMPlayer.PlayButton5();
            controller.Undo();
            redo.IsEnabled = controller.CanRedo();
            undo.IsEnabled = controller.CanUndo();
        }

        private void redo_Click(object sender, RoutedEventArgs e)
        {
            BGMPlayer.PlayButton5();
            controller.Redo();
            redo.IsEnabled = controller.CanRedo();
            undo.IsEnabled = controller.CanUndo(); ;
        }

        private void canvas1_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            var wheelDelta = e.GetCurrentPoint(canvas1).Properties.MouseWheelDelta;
            int step = wheelDelta / 10;
            if (step > 0)
            {
                controller.outputCoordinates[0].unitLength *= (1.0f + 0.01f * step);
            }
            else if (step < 0 && controller.outputCoordinates[0].unitLength > 1)
            {
                float newUL = controller.outputCoordinates[0].unitLength * (1.0f + 0.01f * step);
                if (newUL >= 1) controller.outputCoordinates[0].unitLength = newUL;
                else controller.outputCoordinates[0].unitLength = 1;
            }
            controller.outputCoordinates[0].refreshGeometrys();         //刷新
        }

        private void Page_LayoutUpdated(object sender, object e)
        {
            double kw, kh;
            kh = Window.Current.Bounds.Height/1080;
            kw = Window.Current.Bounds.Width/1920;
            GameIm.Width = 800 * Math.Min( kw,kh);
            GameIm.Height = GameIm.Width * 0.618;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double kw, kh;
            kh = Window.Current.Bounds.Height / 1080;
            kw = Window.Current.Bounds.Width / 1920;
            double k = Math.Min(kw, kh);
            GameId.FontSize = Math.Max(36 * k,12);
            GameName.FontSize = Math.Max(28 * k, 12);
            openIm.FontSize = Math.Max(28 * k, 12);
            GameDiscribe.FontSize = Math.Max(24 * k, 8);
            titleG.Width = 500 * kw;

            if (controller != null)
            {
                controller.outputCoordinates[0].WindowHeight = (float)canvas1.ActualHeight;
                controller.outputCoordinates[0].WindowWidth = (float)canvas1.ActualWidth;
                controller.outputCoordinates[0].refreshGeometrys();         //刷新
            }
            
        }

        private async void openIm_Click(object sender, RoutedEventArgs e)
        {
            var back = GameIm;
            back.RenderTransform = new CompositeTransform();

            var storyBoard = new Storyboard();
            var extendAnimation1 = new DoubleAnimation();

            double from = -back.Width, to = 0;

            var extendAnimation2 = new DoubleAnimation();
            if (Imopen)
            {
                from = 0;
                to = -back.Width;
                extendAnimation2 = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(1)), From = 1, To = 0, EnableDependentAnimation = true };

            }
            else
            {
                coverG.Visibility = Visibility.Visible;
                extendAnimation2 = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(0.5)), From = 0, To = 1, EnableDependentAnimation = true };
            }

            extendAnimation1 = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(1)), From = from, To = to, EnableDependentAnimation = true };

            Storyboard.SetTarget(extendAnimation1, back);
            Storyboard.SetTarget(extendAnimation2, coverG);
            Storyboard.SetTargetProperty(extendAnimation1, "(UIElement.RenderTransform).(CompositeTransform .TranslateY)");
            Storyboard.SetTargetProperty(extendAnimation2, "Opacity");

            storyBoard.Children.Add(extendAnimation1);
            storyBoard.Children.Add(extendAnimation2);
            storyBoard.Begin();

            Imopen = !Imopen;

            await Task.Delay(500);
            coverG.Visibility = Imopen ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Page_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            var k = e.Key;
            if (k == Windows.System.VirtualKey.F12) success();
        }
    }
}
