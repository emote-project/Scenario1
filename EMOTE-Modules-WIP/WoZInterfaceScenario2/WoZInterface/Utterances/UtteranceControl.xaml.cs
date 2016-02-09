using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Skene;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace WOZInterface.Utterances
{
    public partial class UtteranceControl : UserControl
    {
        /// <summary>
        /// Time to complete a combo before it resets in milliseconds
        /// </summary>
        private const int COMBO_DELAY = 5000;
        private const double MINIMUM_INTERVAL_BETWEEN_CONSECUTIVE_UTTERANCES = 1000;

        private DateTime lastTimeSentUtterance = new DateTime();

        public class UtteranceSayEventArgs
        {
            public string Utterance { get; set; }
            public UtteranceSayEventArgs(string utterance)
            {
                Utterance = utterance;
            }
        }

        public ListView mListView = new ListView();
        UtteranceLibrary utteranceLibrary;

        List<string> selectedUtterances;     // the utterances for the selected subcategory
        List<string> showedUtterances;      // the same utterances, transformed to be more readable on the gui (used as itemssource)

        string selectedCategory;
        string selectedSubcategory;
        DateTime lastStrokeTime = new DateTime(1,1,1);
        int keyComboStep = 0;   // defines which part of the combo we reached, in other words if the number pressed is for the category, subcategory or for the utterance

        Dictionary<RadioButton, string> categoryReference = new Dictionary<RadioButton, string>();
        Dictionary<RadioButton, string> subcategoryReference = new Dictionary<RadioButton, string>();

        public UtteranceControl() {
            InitializeComponent();
            txtCombo.Text = "";
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Initialize();
            }
        }

        public void Initialize()
        {
            mListView.Foreground = Brushes.White;
            mListView.Background = Brushes.Transparent;

            utteranceLibrary = new UtteranceLibrary();
            string parentDirectory = System.IO.Path.GetDirectoryName(System.IO.Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName);
            string fileName = parentDirectory + "\\utterances.xlsx";
            if (System.IO.File.Exists(fileName)) {
                utteranceLibrary.LoadLibrary(fileName);
            } else {
                throw new System.IO.FileNotFoundException("Can't find "+fileName);
            }
            DrawUtterancesCategoriesMenu();
            utterancesList.Items.Clear();

        }


        #region Drawing fancy menus and buttons

        private void DrawUtterancesCategoriesMenu()
        {
            Categories.Children.Clear();
            Subcategories.Children.Clear();
            int i = 0;
            categoryReference.Clear();
            foreach (string name in utteranceLibrary.Categories.Keys)
            {
                int mode = (i==0?1:(i==utteranceLibrary.Categories.Keys.Count-1?2:0));
                string nameWithShortcut = i < 10 ? i + 1 + " - " + name : name;
                RadioButton catMenuButton = DrawUttCategoryButton(name, nameWithShortcut, mode);
                categoryReference.Add(catMenuButton, name);
                catMenuButton.Checked += catMenuButton_Checked;
                catMenuButton.GroupName = "Categories";
                Categories.Children.Add(catMenuButton);
                i++;
            }
        }

        private void DrawUtterancesSubcategoriesMenu(List<string> subcategories)
        {
            Subcategories.Children.Clear();
            subcategoryReference.Clear();
            int i=0;
            foreach (string name in subcategories)
            {
                int mode = (i==0?1:(i==subcategories.Count-1?2:0));
                string nameWithShortcut = i < 10 ? i + 1 + " - " + name : name;
                RadioButton subcatMenuButton = DrawUttCategoryButton(name, nameWithShortcut, mode);
                subcategoryReference.Add(subcatMenuButton, name);
                subcatMenuButton.Checked += subcatMenuButton_Checked;
                subcatMenuButton.GroupName = "Subcategories";
                Subcategories.Children.Add(subcatMenuButton);
                i++;
            }
        }

        private RadioButton DrawUttCategoryButton(string name, string label, int mode = 0)    // 0 = normal, 1 = left side, 2 = right side
        {
            RadioButton catMenuButton = new RadioButton();
            string template = "ToggleButtonMid";
            if (mode == 1) template = "ToggleButtonLeft";
            if (mode == 2) template = "ToggleButtonRight";
            catMenuButton.SetResourceReference(Control.TemplateProperty, template);
            catMenuButton.Padding = new Thickness(8, 2, 8, 2);
            catMenuButton.Content = label;
            //if (!name.Equals("-"))
            //    catMenuButton.Name = name;
            //else
                //catMenuButton.Name = "generic";
            return catMenuButton;
        }

        void catMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            List<string> subcat = new List<string>();
            selectedCategory = categoryReference[(RadioButton)sender];
            if (utteranceLibrary.Categories.Keys.Contains(selectedCategory))
            {
                subcat = utteranceLibrary.Categories[selectedCategory];
                DrawUtterancesSubcategoriesMenu(subcat);
                Dispatcher.Invoke((Action)delegate()
                {
                    utterancesList.ItemsSource = new List<string>();
                    utterancesList.Items.Refresh();
                });
            }
            else
            {
                throw new Exception("Category not found exception: "+selectedCategory);
            }
        }

        void subcatMenuButton_Checked(object sender, RoutedEventArgs e)
        {
            selectedSubcategory = subcategoryReference[(RadioButton)sender];
            if (selectedSubcategory.Equals("generic")) selectedSubcategory = "-";
            RefreshUtterances();
        }

        #endregion

        public void RefreshUtterances()
        {
            selectedUtterances = utteranceLibrary.FilterUtterances(selectedCategory, selectedSubcategory);
            showedUtterances = new List<string>();
            for (int i =0; i<selectedUtterances.Count; i++)
            {
                string utterance = selectedUtterances[i];
                utterance = CleanUtterance(utterance);
                showedUtterances.Add(i + 1 + " | " + utterance);
            }
            utterancesList.ItemsSource = showedUtterances;
            utterancesList.Items.Refresh();
        }

        private void utterancesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (utterancesList.SelectedItem != null)
                freespeechtext.Text = selectedUtterances[utterancesList.SelectedIndex];
        }

        // Clean the utterance from all the tags for an easier reading
        private string CleanUtterance(string utterance)
        {
            string cleaned = utterance;

            Regex regex = new Regex(@"\\.+?\\");
            cleaned = regex.Replace(utterance, "");
            regex = new Regex(@"\<.+?\>");
            cleaned = regex.Replace(cleaned, "");

            return cleaned;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Say(freespeechtext.Text,"-");
        }
        
        private void utterancesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Say(selectedUtterances[utterancesList.SelectedIndex], selectedCategory+"."+selectedSubcategory);
        }

        private async void Say(string utterance, string category)
        {
            if (DateTime.Now.Subtract(lastTimeSentUtterance).Duration().TotalMilliseconds > MINIMUM_INTERVAL_BETWEEN_CONSECUTIVE_UTTERANCES)
            {
                WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.ReplaceTagsAndPerform(utterance, category);
                lastTimeSentUtterance = DateTime.Now;
                double timeToWait = 0;
                while (timeToWait >= 0)
                {
                    timeToWait = MINIMUM_INTERVAL_BETWEEN_CONSECUTIVE_UTTERANCES - (DateTime.Now.Subtract(lastTimeSentUtterance).Duration().TotalMilliseconds);
                    btnSay.Content = Math.Round(timeToWait/1000,2) + "..";
                    await Task.Delay(50);
                }
                btnSay.Content = "Say";
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CancelUtterance("");
        }

        private void CancelUtterance(string id)
        {
            //client.UCPublisher.CancelUtterance(id);
            WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.CancelUtterance(id);
        }

        public void keypressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                ResetCombo();

            

            // Extracting the digit corresponding to the numeral key pressed (it avoids a switch with a mapping from the key to the number)
            int index = -1;
            if (e.Key.ToString().Length == 2 && e.Key.ToString().StartsWith("D"))
            {
                index = int.Parse(e.Key.ToString().Substring(1, 1)) - 1;
            }

            if (index != -1)
            {
                // If it's still in time to complete the combo
                if (DateTime.Now.Subtract(lastStrokeTime).TotalMilliseconds < COMBO_DELAY)
                {
                    keyComboStep++;
                    if (keyComboStep > 2) keyComboStep = 2;
                }
                else // If it waited too long, the combo gets reset
                {
                    keyComboStep = 0;
                }


                lastStrokeTime = DateTime.Now;
                ResetComboAfterDeleyExpires();
                
                switch (keyComboStep)
                {
                    case 0:
                        var categories = utteranceLibrary.Categories.Keys.ToArray();
                        if (categories.Count() > index)
                        {
                            selectedCategory = categories.ToArray()[index];
                            ((RadioButton)Categories.Children[index]).IsChecked = true;
                            txtCombo.Text = index + 1 + "...";
                        }
                        else
                        {
                            keyComboStep--;
                        }
                        break;
                    case 1:
                        var subcategories = utteranceLibrary.Categories[selectedCategory];
                        if (subcategories.Count() > index)
                        {
                            selectedSubcategory = subcategories[index];
                            ((RadioButton)Subcategories.Children[index]).IsChecked = true;
                            txtCombo.Text = txtCombo.Text.Substring(0, 1) +"-"+ (index + 1);// +"...";
                        }
                        else
                        {
                            keyComboStep--;
                        }
                        break;
                    case 2:
                        if (utterancesList.Items.Count > index)
                        {
                            utterancesList.SelectedIndex = index;
                            //keyComboStep = 0;
                            string tmp = txtCombo.Text.Substring(0, 3) +"-"+ (index + 1) + "";
                            Say(selectedUtterances[utterancesList.SelectedIndex], selectedCategory + "." + selectedSubcategory);
                            //ResetCombo();
                        }
                        else
                        {
                            keyComboStep--;
                        }
                        break;
                }
            }
            
        }

        private async void ResetComboAfterDeleyExpires()
        {
            int barUpdatesNumber = 20;
            DateTime currentStrokeTime = lastStrokeTime;
            ComboProgressBar.Maximum = COMBO_DELAY;
            for (int i = COMBO_DELAY / barUpdatesNumber; i < COMBO_DELAY; i = i + COMBO_DELAY / barUpdatesNumber)
            {
                await Task.Delay(COMBO_DELAY / barUpdatesNumber);
                if (currentStrokeTime.Equals(lastStrokeTime))    // if we already pressed a new key, we ignore this method call since a new one was created while awaiting.
                {
                    ComboProgressBar.Value = i;
                }
                else {
                    break;
                }
            }
            if (currentStrokeTime.Equals(lastStrokeTime))
            {
                ResetCombo();
            }
        }

        private void ResetCombo()
        {
            keyComboStep = 0;
            //txtCombo.Text = "";
            ComboProgressBar.Value = 100;
            lastStrokeTime = new DateTime(1, 1, 1);
        }

        private void utterancesList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                Say(selectedUtterances[utterancesList.SelectedIndex], selectedCategory + "." + selectedSubcategory);
            }
        }


        

       

    }

}
