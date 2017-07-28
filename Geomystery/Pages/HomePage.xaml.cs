using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Geomystery.Pages;
using Geomystery.Models;
using Windows.UI.Xaml.Media.Animation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Geomystery.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private ViewModel.ViewModel View { set; get; } = new ViewModel.ViewModel();
        public HomePage()
        {
            this.InitializeComponent();
            LetLogoRotation();
            //hexagonStoryboard.Begin();
            View = new ViewModel.ViewModel();
            if (!APPDATA.app_data.Views.Contains(View))
            {
                APPDATA.app_data.Views.Add(View);
            }
        }

        void LetLogoRotation()
        {
            //var x = new CompositeTransform();
            hexagon.RenderTransform = new CompositeTransform();
            square.RenderTransform = new CompositeTransform();
            triangle.RenderTransform = new CompositeTransform();
            circle2.Projection = new PlaneProjection();

            var turnSB = new Storyboard();

            double t = 60;

            var doubleAnim = new DoubleAnimationUsingKeyFrames();
            doubleAnim.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromMilliseconds(0), Value = 0 });
            doubleAnim.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromSeconds(t), Value = 360 });

            var doubleAnim2 = new DoubleAnimationUsingKeyFrames();
            doubleAnim2.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromMilliseconds(0), Value = 0 });
            doubleAnim2.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromSeconds(t), Value = -540 });

            var doubleAnim3 = new DoubleAnimationUsingKeyFrames();
            doubleAnim3.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromMilliseconds(0), Value = 0 });
            doubleAnim3.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromSeconds(t), Value = 0 });

            var doubleAnim302 = new DoubleAnimationUsingKeyFrames();
            doubleAnim302.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromMilliseconds(0), Value = 1 });
            doubleAnim302.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromSeconds(0.3 * t), Value = 1 });
            doubleAnim302.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromSeconds(0.4 * t), Value = 0 });
            doubleAnim302.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromSeconds(0.6 * t), Value = 0 });
            doubleAnim302.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromSeconds(0.7 * t), Value = 1 });
            doubleAnim302.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromSeconds(t), Value = 1 });

            var doubleAnim4 = new DoubleAnimationUsingKeyFrames();
            doubleAnim4.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromMilliseconds(0), Value = 0 });
            doubleAnim4.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromSeconds(0.5*t), Value = 1800 });
            doubleAnim4.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromSeconds(t), Value = 0 });

            var doubleAnim5 = new DoubleAnimationUsingKeyFrames();
            doubleAnim5.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromMilliseconds(0), Value = 0 });
            doubleAnim5.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromSeconds(t), Value = 540 });

            turnSB.Children.Add(doubleAnim);
            turnSB.Children.Add(doubleAnim2);
            turnSB.Children.Add(doubleAnim3);
            turnSB.Children.Add(doubleAnim302);
            turnSB.Children.Add(doubleAnim4);
            turnSB.Children.Add(doubleAnim5);

            Storyboard.SetTarget(doubleAnim, hexagon.RenderTransform);
            Storyboard.SetTarget(doubleAnim2, square.RenderTransform);
            Storyboard.SetTarget(doubleAnim3, triangle.RenderTransform);
            Storyboard.SetTarget(doubleAnim302, triangle);
            Storyboard.SetTarget(doubleAnim4, circle2.Projection);
            Storyboard.SetTarget(doubleAnim5, circle2.Projection);

            Storyboard.SetTargetProperty(doubleAnim, "(CompositeTransform.Rotation)");
            Storyboard.SetTargetProperty(doubleAnim2, "(CompositeTransform.Rotation)");
            Storyboard.SetTargetProperty(doubleAnim3, "(CompositeTransform.Rotation)");
            Storyboard.SetTargetProperty(doubleAnim302, "Opacity");
            Storyboard.SetTargetProperty(doubleAnim4, "(PlaneProjection.RotationX)");
            Storyboard.SetTargetProperty(doubleAnim5, "(PlaneProjection.RotationZ)");

            turnSB.RepeatBehavior = RepeatBehavior.Forever;
            turnSB.Begin();
        }

        private void Game_Click(object sender, RoutedEventArgs e)
        {
            BGMPlayer.PlayButton();
            APPDATA.app_data.MoveTo(AppPage.SelectChapterPage);
        }

        private void Freestyle_Click(object sender, RoutedEventArgs e)
        {
            BGMPlayer.PlayButton();
            APPDATA.app_data.MoveTo(AppPage.FreeStylePage);
        }

        private void Option_Click(object sender, RoutedEventArgs e)
        {
            BGMPlayer.PlayButton();
            APPDATA.app_data.MoveTo(AppPage.OptionPage);
            //Frame.Navigate(typeof(Option));
        }

        private void Achievement_Click(object sender, RoutedEventArgs e)
        {
            BGMPlayer.PlayButton();
            APPDATA.app_data.MoveTo(AppPage.AchievementPage);
            //Frame.Navigate(typeof(Achievement));
        }

        private void Page_LayoutUpdated(object sender, object e)
        {
            double w, h;
            h = Window.Current.Bounds.Height;
            w = Window.Current.Bounds.Width;
            circle2.Height = 600 * Math.Min(h / 1080.0,w/1920.0);
            circle2.Width = circle2.Height;
        }
    }
}
