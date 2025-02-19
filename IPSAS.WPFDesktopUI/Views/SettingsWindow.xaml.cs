﻿using IPSAS.WPFDesktopUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IPSAS.WPFDesktopUI.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly SettingsViewModel settingsViewModel;

        public SettingsWindow(SettingsViewModel settingsViewModel)
        {
            InitializeComponent();
            this.settingsViewModel = settingsViewModel;
            DataContext = settingsViewModel;
        }
    }
}
