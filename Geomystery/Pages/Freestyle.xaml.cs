using Geomystery.Controllers.Geometry;
using Geomystery.Views.Geometry;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
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
    public sealed partial class Freestyle : Page
    {
        Vector2 maxHeightWidth;

        CanvasCommandList cl = null;

        Random rnd = new Random();

        //List<Vector2> plist;

        List<UserTool> userTools;

        Geomystery.Controllers.Geometry.Controllers controller;

        Stack<OutputLine> undolist, redolist;

        public Freestyle()
        {
            this.InitializeComponent();
            View = new ViewModel.ViewModel();
            if (!APPDATA.app_data.Views.Contains(View))
            {
                APPDATA.app_data.Views.Add(View);
            }
            userTools = UserToolsManager.GetInstance().GetTools();
            controller = new Controllers.Geometry.Controllers(1);
        }

        private ViewModel.ViewModel View { set; get; } = new ViewModel.ViewModel();


        private Vector2 RndPosition()
        {
            double x = rnd.NextDouble() * 500f;
            double y = rnd.NextDouble() * 500f;
            return new Vector2((float)x, (float)y);
        }

        private float RndRadius()
        {
            return (float)rnd.NextDouble() * 150f;
        }

        private byte RndByte()
        {
            return (byte)rnd.Next(256);
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
            //Rect rect = new Rect(0, 0, maxHeightWidth.X, maxHeightWidth.Y);
            //args.DrawingSession.DrawRectangle(rect,Colors.Black);
            List<OutputGeometry> pointSetList = controller.outputCoordinates[0].outputPointSetList;
            if (pointSetList != null)
            {
                for (int i = 0; i < pointSetList.Count; i++)
                {
                    if(pointSetList[i].isVisible)
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
            if(pointList != null)
            {
                for(int i = 0; i < pointList.Count; i++)
                {
                    if(pointList[i].isVisible)
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

        private void canvas1_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            cl = new CanvasCommandList(sender);
            using (CanvasDrawingSession clds = cl.CreateDrawingSession())
            {
                clds.DrawText("Demo", new Vector2((float)Frame.Height / 2, (float)Frame.Width / 2), Colors.Red);
                for (int i = 0; i < 100; i++)
                {
                    clds.DrawText("Hello, World!", RndPosition(), Color.FromArgb(255, RndByte(), RndByte(), RndByte()));
                    clds.DrawCircle(RndPosition(), RndRadius(), Color.FromArgb(255, RndByte(), RndByte(), RndByte()));
                }
            }
        }

        private void canvas1_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Point pxy = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
            Vector2 p = pxy.ToVector2();
            MainPage.debugTxt.Text = p.X.ToString() + " | " + p.Y.ToString();

            controller.PointerPressed((UserTool)toolList.SelectedItem, sender, e);

            redo.IsEnabled = controller.CanRedo();
            undo.IsEnabled = controller.CanUndo();
        }

        private void canvas1_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ;
        }

        private void canvas1_PointerMoved(object sender, PointerRoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //plist = new List<Vector2>();
            controller.outputCoordinates[0].WindowHeight = (float)canvas1.ActualHeight;
            controller.outputCoordinates[0].WindowWidth = (float)canvas1.ActualWidth;
            maxHeightWidth = new Vector2((float)canvas1.ActualWidth, (float)canvas1.ActualHeight);
            MainPage.debugTxt.Text = maxHeightWidth.X.ToString() + " | " + maxHeightWidth.Y.ToString();
            toolList.SelectedIndex = 2;
            redo.IsEnabled = controller.CanRedo();
            undo.IsEnabled = controller.CanUndo();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            canvas1.RemoveFromVisualTree();
            canvas1 = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MainPage.debugTxt.Text = "helloWorld";
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            controller = new Controllers.Geometry.Controllers(1);
            controller.outputCoordinates[0].WindowHeight = (float)canvas1.ActualHeight;
            controller.outputCoordinates[0].WindowWidth = (float)canvas1.ActualWidth;
        }

        private void redo_Click(object sender, RoutedEventArgs e)
        {
            controller.Redo();
            redo.IsEnabled = controller.CanRedo();
            undo.IsEnabled = controller.CanUndo();
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

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            controller.outputCoordinates[0].WindowHeight = (float)canvas1.ActualHeight;
            controller.outputCoordinates[0].WindowWidth = (float)canvas1.ActualWidth;
            controller.outputCoordinates[0].refreshGeometrys();         //刷新
        }

        private void undo_Click(object sender, RoutedEventArgs e)
        {
            controller.Undo();
            redo.IsEnabled = controller.CanRedo();
            undo.IsEnabled = controller.CanUndo();
        }
    }
}
