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
        }

        private ViewModel.ViewModel View { set; get; } = new ViewModel.ViewModel();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           

            controller = new Controllers.Geometry.Controllers(1);
            controller.PreInitialized(LevelLoader.GetLevel(1));
            controller.outputCoordinates[0].WindowHeight = (float)canvas1.ActualHeight;
            controller.outputCoordinates[0].WindowWidth = (float)canvas1.ActualWidth;
            maxHeightWidth = new Vector2((float)canvas1.ActualWidth, (float)canvas1.ActualHeight);
            //text1.Text = maxHeightWidth.X.ToString() + " | " + maxHeightWidth.Y.ToString();
            listView1.SelectedIndex = 2;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            canvas1.RemoveFromVisualTree();
            canvas1 = null;
        }

        private void canvas1_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            var draw = args.DrawingSession;
            /*
            args.DrawingSession.DrawText("click me", center, Colors.Red);
            for (int i = 0; i < plist.Count; i++)
            {
                args.DrawingSession.DrawCircle(plist[i], 5, Color.FromArgb(255, 0, 0, 0));
            }
            */
            Rect rect = new Rect(0, 0, maxHeightWidth.X, maxHeightWidth.Y);
            args.DrawingSession.DrawRectangle(rect, Colors.Black);
            var geoList = controller.outputCoordinates[0].geometryList;
            if (geoList != null)
            {
                for (int i = 0; i < geoList.Count; i++)
                {
                    if (geoList[i] is OutputCircle)
                    {
                        var realCircle = geoList[i] as OutputCircle;
                        args.DrawingSession.DrawCircle(realCircle.center, realCircle.radius, realCircle.lineColor, realCircle.thickness);

                        if (realCircle.circle.center.isSelected)
                        {
                            args.DrawingSession.FillCircle(realCircle.circle.center.resultPoint.viewPoint, OutputPoint.scopeLength, realCircle.circle.center.resultPoint.selectedFillColor);
                            args.DrawingSession.DrawCircle(realCircle.circle.center.resultPoint.viewPoint, OutputPoint.scopeLength, realCircle.circle.center.resultPoint.selectedLineColor);
                        }
                        else
                        {
                            args.DrawingSession.FillCircle(realCircle.circle.center.resultPoint.viewPoint, OutputPoint.scopeLength, realCircle.circle.center.resultPoint.fillColor);
                            args.DrawingSession.DrawCircle(realCircle.circle.center.resultPoint.viewPoint, OutputPoint.scopeLength, realCircle.circle.center.resultPoint.lineColor);
                        }
                        if (realCircle.circle.radius.isSelected)
                        {
                            args.DrawingSession.FillCircle(realCircle.circle.radius.resultPoint.viewPoint, OutputPoint.scopeLength, realCircle.circle.radius.resultPoint.selectedFillColor);
                            args.DrawingSession.DrawCircle(realCircle.circle.radius.resultPoint.viewPoint, OutputPoint.scopeLength, realCircle.circle.radius.resultPoint.selectedLineColor);
                        }
                        else
                        {
                            args.DrawingSession.FillCircle(realCircle.circle.radius.resultPoint.viewPoint, OutputPoint.scopeLength, realCircle.circle.radius.resultPoint.fillColor);
                            args.DrawingSession.DrawCircle(realCircle.circle.radius.resultPoint.viewPoint, OutputPoint.scopeLength, realCircle.circle.radius.resultPoint.lineColor);
                        }
                    }
                    else if (geoList[i] is OutputLine)
                    {
                        var realLine = geoList[i] as OutputLine;
                        args.DrawingSession.DrawLine(realLine.p1, realLine.p2, realLine.lineColor, realLine.thickness);

                        if (realLine.line.p1.isSelected)
                        {
                            args.DrawingSession.FillCircle(realLine.line.p1.resultPoint.viewPoint, OutputPoint.scopeLength, realLine.line.p1.resultPoint.selectedFillColor);
                            args.DrawingSession.DrawCircle(realLine.line.p1.resultPoint.viewPoint, OutputPoint.scopeLength, realLine.line.p1.resultPoint.selectedLineColor);
                        }
                        else
                        {
                            args.DrawingSession.FillCircle(realLine.line.p1.resultPoint.viewPoint, OutputPoint.scopeLength, realLine.line.p1.resultPoint.fillColor);
                            args.DrawingSession.DrawCircle(realLine.line.p1.resultPoint.viewPoint, OutputPoint.scopeLength, realLine.line.p1.resultPoint.lineColor);
                        }
                        if (realLine.line.p2.isSelected)
                        {
                            args.DrawingSession.FillCircle(realLine.line.p2.resultPoint.viewPoint, OutputPoint.scopeLength, realLine.line.p1.resultPoint.selectedFillColor);
                            args.DrawingSession.DrawCircle(realLine.line.p2.resultPoint.viewPoint, OutputPoint.scopeLength, realLine.line.p1.resultPoint.selectedLineColor);
                        }
                        else
                        {
                            args.DrawingSession.FillCircle(realLine.line.p2.resultPoint.viewPoint, OutputPoint.scopeLength, realLine.line.p1.resultPoint.fillColor);
                            args.DrawingSession.DrawCircle(realLine.line.p2.resultPoint.viewPoint, OutputPoint.scopeLength, realLine.line.p1.resultPoint.lineColor);
                        }
                    }
                    else if (geoList[i] is OutputPoint)
                    {
                        var realPoint = geoList[i] as OutputPoint;
                        if (realPoint.point.isSelected)
                        {
                            args.DrawingSession.FillCircle(realPoint.viewPoint, OutputPoint.scopeLength, realPoint.selectedFillColor);
                            args.DrawingSession.DrawCircle(realPoint.viewPoint, OutputPoint.scopeLength, realPoint.selectedLineColor);
                        }
                        else
                        {
                            args.DrawingSession.FillCircle(realPoint.viewPoint, OutputPoint.scopeLength, realPoint.fillColor);
                            args.DrawingSession.DrawCircle(realPoint.viewPoint, OutputPoint.scopeLength, realPoint.lineColor);
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
            controller.outputCoordinates[0].geometryList.Clear();
            controller = new Controllers.Geometry.Controllers(1);
            controller.PreInitialized(LevelLoader.GetLevel(1));
        }
    }
}
