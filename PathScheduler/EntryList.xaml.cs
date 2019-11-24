using PathScheduler.Helpers;
using PathScheduler.Models;
using System.Data;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;

namespace PathScheduler
{
    /// <summary>
    /// Logika interakcji dla klasy EntryList.xaml
    /// </summary>
    public partial class EntryList : Window
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="dataSource">Reference to the data source.</param>
        public EntryList()
        {
            InitializeComponent();

            this.Focus();
            this.listViewEntries.ItemsSource = MapDataSource.Points;
        }

        /// <summary>
        /// On acceptButton click. Hides the window, without destroying it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// On addButton click. Adds a new point.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.textBoxName.Text == "")
            {
                return;
            }

            bool result = MapDataSource.AddPoint(new MapPoint
            {
                Name = this.textBoxName.Text,
                CoordX = this.upDownX.Value == null ? 0 : (int)this.upDownX.Value,
                CoordY = this.upDownY.Value == null ? 0 : (int)this.upDownY.Value
            });

            if (!result)
            {
                this.ClearInputControls();
                return;
            }

            MapDataSource.GenerateDistanceMatrix();
            // rerender the view
            this.listViewEntries.Items.Refresh();
        }

        private void viewMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            MapDataSource.ShowDistanceMatrixDialog();
        }

        /// <summary>
        /// Clears point input controls.
        /// </summary>
        private void ClearInputControls()
        {
            this.textBoxName.Text = "";
            this.upDownX.Value = 0;
            this.upDownY.Value = 0;
        }

        /// <summary>
        /// Called when window visibility was changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Hidden)
            {
                this.ClearInputControls();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (e.Cancel == false)
            {
                e.Cancel = true;
            }
            this.Hide();
        }
    }
}
