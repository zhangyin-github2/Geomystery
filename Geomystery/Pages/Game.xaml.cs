using Geomystery.Award;
using Geomystery.Controllers.Geometry;
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
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class Game : Page
    {
        Vector2 maxHeightWidth;

        List<UserTool> userTools;
        Level localLevel = new Level();
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
            GameDiscribe.Visibility = Visibility.Collapsed;
            double kw, kh;
            kh = Window.Current.Bounds.Height / 1080;
            kw = Window.Current.Bounds.Width / 1920;
            double k = Math.Min(kw, kh);
            GameId.FontSize = Math.Max(32 * k, 12);
            GameName.FontSize = Math.Max(28 * k, 12);
        }
        private ViewModel.ViewModel View { set; get; } = new ViewModel.ViewModel();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var x = localLevel = e.Parameter as Level;
            GameId.Text = x.ID.ToString();
            GameName.Text = x.name;
            GameImage.Source = new BitmapImage(new Uri(x.cover, UriKind.Absolute));
            GameDiscribe.Text = x.intro;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //controller = new Controllers.Geometry.Controllers();
            //controller.PreInitialized(LevelLoader.GetLevel(1));                 //第一关的控制器
            controller = LevelLoader.GetLevel(1);

            controller.outputCoordinates[0].WindowHeight = (float)canvas1.ActualHeight;
            controller.outputCoordinates[0].WindowWidth = (float)canvas1.ActualWidth;
            maxHeightWidth = new Vector2((float)canvas1.ActualWidth, (float)canvas1.ActualHeight);
            //text1.Text = maxHeightWidth.X.ToString() + " | " + maxHeightWidth.Y.ToString();
            listView1.SelectedIndex = 2;
            controller.historyDfaList.Clear();
            redo.IsEnabled = controller.CanRedo();
            undo.IsEnabled = controller.CanUndo();
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
            /*
            args.DrawingSession.DrawText("click me", center, Colors.Red);
            for (int i = 0; i < plist.Count; i++)
            {
                args.DrawingSession.DrawCircle(plist[i], 5, Color.FromArgb(255, 0, 0, 0));
            }
            */
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
                            args.DrawingSession.DrawCircle(pointList[i].viewPoint, OutputPoint.scopeLength, pointList[i].selectedLineColor);
                        }
                        else
                        {
                            args.DrawingSession.FillCircle(pointList[i].viewPoint, OutputPoint.scopeLength, pointList[i].fillColor);
                            args.DrawingSession.DrawCircle(pointList[i].viewPoint, OutputPoint.scopeLength, pointList[i].lineColor);
                        }
                    }
                }
            }

        }

        private async void canvas1_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Point pxy = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
            Vector2 p = pxy.ToVector2();
            MainPage.debugTxt.Text = p.X.ToString() + " | " + p.Y.ToString();

            controller.PointerPressed((UserTool)listView1.SelectedItem, sender, e);

            redo.IsEnabled = controller.CanRedo();
            undo.IsEnabled = controller.CanUndo();

            int flag = 0;
            for (int i = 0; i < controller.coordinate.pointSetList.Count; i++)
            {
                if (controller.coordinate.pointSetList[i] is Line)
                {
                    Line line = controller.coordinate.pointSetList[i] as Line;
                    if ((line.p1.id == 1 && line.p2.id == 2) || (line.p2.id == 1 && line.p1.id == 2))
                    {
                        flag++;
                    }
                    if ((line.p1.id == 1 && line.p2.id == 3) || (line.p2.id == 1 && line.p1.id == 3))
                    {
                        flag++;
                    }
                    if ((line.p1.id == 2 && line.p2.id == 3) || (line.p2.id == 2 && line.p1.id == 3))
                    {
                        flag++;
                    }
                }
            }
            if (flag == 3)
            {
                LevelSucceedDialog lsd = new LevelSucceedDialog();
                await lsd.ShowAsync();
                MainPage.MainFrame.Navigate(typeof(SelectChapter));
            }
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            //controller.outputCoordinates[0].geometryList.Clear();
            controller = null;
            controller = LevelLoader.GetLevel(1);

            controller.historyDfaList.Clear();
            controller.outputCoordinates[0].refreshCanvas(canvas1);
            redo.IsEnabled = controller.CanRedo();
            undo.IsEnabled = controller.CanUndo();
        }

        private void undo_Click(object sender, RoutedEventArgs e)
        {
            controller.Undo();
            redo.IsEnabled = controller.CanRedo();
            undo.IsEnabled = controller.CanUndo();
        }

        private void redo_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
