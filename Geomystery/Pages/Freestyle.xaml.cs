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
        Vector2 center;

        CanvasCommandList cl = null;

        Random rnd = new Random();

        //List<Vector2> plist;

        List<UserTool> userTools;

        Geomystery.Controllers.Geometry.Controllers controller;

        public Freestyle()
        {
            this.InitializeComponent();
            View = new ViewModel.ViewModel();
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
            var geoList = controller.outputCoordinates[0].GeometryList;
            if(geoList != null)
            {
                for (int i = 0; i < geoList.Count; i++)
                {
                    if (geoList[i] is OutputPoint)
                    {
                        var realPoint = geoList[i] as OutputPoint;
                        if(realPoint.point.isSelected)
                        {
                            args.DrawingSession.FillCircle(realPoint.viewPoint, OutputPoint.scopeLength, realPoint.selectedFillColor);
                        }
                        else
                        {
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
            /*
            Point pxy = e.GetCurrentPoint((CanvasAnimatedControl)sender).Position;
            Vector2 p = pxy.ToVector2();
            bool flag1 = true;
            for (int i = 0; i <plist.Count; i++)
            {
                if ((plist[i] - p).Length() <= 5)
                {
                    plist.RemoveAt(i);
                    flag1 = false;
                    break;
                }
            }
            if (flag1)
            {
                plist.Add(p);
            }
            text1.Text = p.X.ToString() + " | " + p.Y.ToString();
            */

            controller.PointerPressed((UserTool)listView1.SelectedItem, sender, e);
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
            center = new Vector2((float)canvas1.ActualWidth / 2, (float)canvas1.ActualHeight / 2);
            text1.Text = center.X.ToString() + " | " + center.Y.ToString();
            listView1.SelectedIndex = 2;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            canvas1.RemoveFromVisualTree();
            canvas1 = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            text1.Text = "helloWorld";
        }

        
    }
}
