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

namespace WOZInterface.EyeBrows
{

    public class EyebrowsPerceptionViewModel : ViewModelBase
    {
        public double envAU2 = 0;
        public double EnvAU2
        {
            get
            {
                return envAU2;
            }
            set
            {
                envAU2 = value;
                NotifyPropertyChanged("EnvAU2");
            }
        }
        public double envAU4 = 0;
        public double EnvAU4
        {
            get
            {
                return envAU4;
            }
            set
            {
                envAU4 = value;
                NotifyPropertyChanged("EnvAU4");
            }
        }
        public double ecoAU2 = 0;
        public double EcoAU2
        {
            get
            {
                return ecoAU2;
            }
            set
            {
                ecoAU2 = value;
                NotifyPropertyChanged("EcoAU2");
            }
        }
        public double ecoAU4 = 0;
        public double EcoAU4
        {
            get
            {
                return ecoAU4;
            }
            set
            {
                ecoAU4 = value;
                NotifyPropertyChanged("EcoAU4");
            }
        }
    }

    public partial class EyebrowsPerception : UserControl
    {
        private EyebrowsPerceptionViewModel Data
        {
            get
            {
                return this.DataContext as EyebrowsPerceptionViewModel;
            }
        }
        public double EnvAU2
        {
            get
            {
                return Data.EnvAU2;
            }
            set
            {
                Data.EnvAU2 = value;
            }
        }
        public double EnvAU4
        {
            get
            {
                return Data.EnvAU4;
            }
            set
            {
                Data.EnvAU4 = value;
            }
        }
        public double EcoAU2
        {
            get
            {
                return Data.EcoAU2;
            }
            set
            {
                Data.EcoAU2 = value;
            }
        }
        public double EcoAU4
        {
            get
            {
                return Data.EcoAU4;
            }
            set
            {
                Data.EcoAU4 = value;
            }
        }

        public EyebrowsPerception()
        {
            InitializeComponent();
        }
    }
}
