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
            var geoList = controller.outputCoordinates[0].geometryList;
            if (geoList != null)
            {
                for (int i = 0; i < geoList.Count; i++)
                {
                    if (geoList[i] is OutputCircle)
                    {
                        var realCircle = geoList[i] as OutputCircle;
                        if (realCircle.circle.isSelected)
                        {
                            args.DrawingSession.DrawCircle(realCircle.center, realCircle.radius, realCircle.selectedLineColor, realCircle.thickness);
                        }
                        else
                        {
                            args.DrawingSession.DrawCircle(realCircle.center, realCircle.radius, realCircle.lineColor, realCircle.thickness);
                        }

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
                        if (realLine.line.isSelected)
                        {
                            args.DrawingSession.DrawLine(realLine.p1, realLine.p2, realLine.selectedLineColor, realLine.thickness);
                        }
                        else
                        {
                            args.DrawingSession.DrawLine(realLine.p1, realLine.p2, realLine.lineColor, realLine.thickness);
                        }

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

        private void undo_Click(object sender, RoutedEventArgs e)
        {
            controller.Undo();
            redo.IsEnabled = controller.CanRedo();
            undo.IsEnabled = controller.CanUndo();
        }
    }
}
