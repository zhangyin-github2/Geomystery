﻿using System;
using System.Collections.Generic;
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

// “内容对话框”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上进行了说明

namespace Geomystery.Pages
{
    public sealed partial class LevelSucceedDialog : ContentDialog
    {
        public LevelSucceedDialog()
        {
            this.InitializeComponent();
            Title = AppResources.GetString("Congratulations");
            passT.Text = AppResources.GetString("Problemsolved");
            PrimaryButtonText = AppResources.GetString("PLAYAGAIN");
            SecondaryButtonText = AppResources.GetString("NEXTLEVEL");
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }
}
