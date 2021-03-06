﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Geomystery.ViewModel
{
    public class ViewModel : NotifyProperty
    {
        public ViewModel()
        {
            this.Theme= !APPDATA.app_data.ISNIGHT ? ElementTheme.Light : ElementTheme.Dark;
        }

        public ElementTheme Theme
        {
            get
            {
                return _theme;
            }
            set
            {
                _theme = value;
                OnPropertyChanged();
            }
        }

        public bool? AreChecked
        {
            set
            {
                _areChecked = value;
                Theme = value == false ? ElementTheme.Light : ElementTheme.Dark;
                OnPropertyChanged();
            }
            get
            {
                return _areChecked;
            }
        }

        private bool? _areChecked = true;

        private ElementTheme _theme = ElementTheme.Light;
    }

}
