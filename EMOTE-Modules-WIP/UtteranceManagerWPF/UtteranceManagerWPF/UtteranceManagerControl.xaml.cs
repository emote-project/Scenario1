using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace UtteranceManagerControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class EMOTEUtterancesManager : UserControl
    {
        public class PerformUtteranceEventArgs : EventArgs
        {
            public string Category { get; private set; }
            public string Utterance {get; private set;}
            public PerformUtteranceEventArgs(string utterance, string category){
                Utterance = utterance;
                Category = category;
            }
        }

        public delegate void CancelUtteranceHandler();
        public event EventHandler<PerformUtteranceEventArgs> PerformUtteranceClicked;
        public event CancelUtteranceHandler CancelUtterance;

        public bool AutoLoad
        {
            get
            {
                return (bool)ckbAutoload.IsChecked;
            }
            set
            {
                ckbAutoload.IsChecked = value;
            }
        }
        public string LibraryPath { get; private set; }
        
        UtteranceManager.UtteranceLibrary Library;


        public EMOTEUtterancesManager()
        {
            InitializeComponent();
        }

        private void btnLoadLibrary_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Microsoft Excel Worksheet|*.xls;*.xlsx|All Files|*.*";
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                LoadLibrary(openFileDialog.FileName);
            }
            
        }

        public void LoadLibrary(string fileName)
        {
            LibraryPath = fileName;
            Library = new UtteranceManager.UtteranceLibrary();
            Library.LoadLibrary(fileName);
            txtLibraryPath.Text = fileName;
            Properties.Settings.Default.lastOpenLibrary = fileName;
            Properties.Settings.Default.Save();
            UpdateLists();
            UpdateLists2();
        }

        private void UpdateLists2()
        {
            lstCategories2.ItemsSource = Library.Categories.Keys;
            if (lstCategories2.SelectedItem != null)
            {
                lstSubcategories2.ItemsSource = Library.FilterSubcategories((string)lstCategories2.SelectedItem);
                if (lstSubcategories2.SelectedItem != null)
                {
                    lstUtterances2.ItemsSource = Library.FilterUtterances((string)lstCategories2.SelectedItem, (string)lstSubcategories2.SelectedItem);
                    if (lstUtterances2.Items.Count > 0) lstUtterances2.SelectedIndex = 0;
                }
                else
                {
                    lstUtterances2.ItemsSource = null;
                }
            }
        }

        private void UpdateLists()
        {
            lstCategories.ItemsSource = Library.Categories.Keys;
            if (lstCategories.SelectedItem != null)
            {
                lstSubcategories.ItemsSource = Library.FilterSubcategories((string)lstCategories.SelectedItem);
                if (lstSubcategories.SelectedItem != null)
                {
                    lstUtterances.ItemsSource = Library.FilterUtterances((string)lstCategories.SelectedItem, (string)lstSubcategories.SelectedItem);
                    if (lstUtterances.Items.Count > 0) lstUtterances.SelectedIndex = 0;
                }
                else
                {
                    lstUtterances.ItemsSource = null;
                }
            }
        }

        private void lstCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateLists();
        }

        private void lstCategories2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateLists2();
        }

        private void lstSubcategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateLists();
        }

        private void lstSubcategories2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateLists2();
        }

        private void lstUtterances_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtUtterance.Text = (string)lstUtterances.SelectedItem;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (PerformUtteranceClicked != null) PerformUtteranceClicked(this,new PerformUtteranceEventArgs(txtUtterance.Text, lstCategories.SelectedItem + "." + lstSubcategories.SelectedItem));
        }

        private void lstUtterances2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtUtterance.Text = (string)lstUtterances2.SelectedItem;
        }

        private void lstUtterances_GotFocus(object sender, RoutedEventArgs e)
        {
            txtUtterance.Text = (string)lstUtterances.SelectedItem;
        }

        private void lstUtterances2_GotFocus(object sender, RoutedEventArgs e)
        {
            txtUtterance.Text = (string)lstUtterances2.SelectedItem;
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.lastOpenLibrary != "" && File.Exists(Properties.Settings.Default.lastOpenLibrary))
            {
                LoadLibrary(Properties.Settings.Default.lastOpenLibrary);
                AutoLoad = true;
            }
        }

        private void lstUtterances_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstUtterances.SelectedItem!=null)
            {
                txtUtterance.Text = (string)lstUtterances.SelectedItem;
                if (PerformUtteranceClicked != null) PerformUtteranceClicked(this, new PerformUtteranceEventArgs(txtUtterance.Text, lstCategories.SelectedItem + "." + lstSubcategories.SelectedItem));
            }
        }

        private void lstUtterances2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lstUtterances2.SelectedItem != null)
            {
                txtUtterance.Text = (string)lstUtterances2.SelectedItem;
                if (PerformUtteranceClicked != null) PerformUtteranceClicked(this, new PerformUtteranceEventArgs(txtUtterance.Text, lstCategories2.SelectedItem + "." + lstSubcategories2.SelectedItem));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (CancelUtterance != null) CancelUtterance();
        }

    }
}
