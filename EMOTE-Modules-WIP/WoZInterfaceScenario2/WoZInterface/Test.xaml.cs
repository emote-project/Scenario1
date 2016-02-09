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
using System.Windows.Shapes;

namespace WOZInterface
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        public class ModelObject : ViewModelBase
        {
            string test = "asdkjh";
            public string Test {
                get {
                    return test;
                }
                set {
                    test = value;
                    NotifyPropertyChanged("Test");
                }
            }
         }

        

        public Test()
        {
            InitializeComponent();

            var model = new ModelObject()
            {
                Test = "ciao!"
            };

            this.DataContext = model;
        }
    }
}
