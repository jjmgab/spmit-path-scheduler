﻿using PathScheduler.Helpers;
using PathScheduler.Models;
using System.Windows;
using System.Windows.Input;

namespace PathScheduler
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Point data source.
        /// </summary>
        private PointDataSource<MapPoint> _dataSource;

        /// <summary>
        /// Handler to the EntryList window.
        /// </summary>
        private EntryList _entryListWindow;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            _dataSource = new MockupDataGen(30, -100, -100, 100, 100);
        }

        /// <summary>
        /// On entryListButton click. Opens the EntryList window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntryListButton_Click(object sender, RoutedEventArgs e)
        {
            if (_entryListWindow == null)
            {
                _entryListWindow = new EntryList(_dataSource);
            }
            this.IsEnabled = false;
            this._entryListWindow.ShowDialog();
            this.IsEnabled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
