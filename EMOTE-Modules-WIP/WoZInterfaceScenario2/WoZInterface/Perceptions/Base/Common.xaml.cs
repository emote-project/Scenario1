using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
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

namespace WOZInterface.Perceptions.Base
{
    public class CommonViewModel : ViewModelBase
    {
        private string title = "NoNamedPerception";
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                NotifyPropertyChanged("Title");
            }
        }
        private Style leftStyle;
        public Style LeftStyle
        {
            get
            {
                return leftStyle;
            }
            set
            {
                leftStyle = value;
                NotifyPropertyChanged("LeftStyle");
            }
        }
        private Style rightStyle;
        public Style RightStyle
        {
            get
            {
                return rightStyle;
            }
            set
            {
                rightStyle = value;
                NotifyPropertyChanged("RightStyle");
            }
        }
        private string leftDescription;
        public string LeftDescription
        {
            get
            {
                return leftDescription;
            }
            set
            {
                leftDescription = value;
                NotifyPropertyChanged("LeftDescription");
            }
        }
        private string rightDescription;
        public string RightDescription
        {
            get
            {
                return rightDescription;
            }
            set
            {
                rightDescription = value;
                NotifyPropertyChanged("RightDescription");
            }
        }
        private double progressBarLeft;
        public double ProgressBarLeft
        {
            get
            {
                return progressBarLeft;
            }
            set
            {
                progressBarLeft = value;
                NotifyPropertyChanged("ProgressBarLeft");
            }
        }
        private double progressBarRight;
        public double ProgressBarRight
        {
            get
            {
                return progressBarRight;
            }
            set
            {
                progressBarRight = value;
                NotifyPropertyChanged("ProgressBarRight");
            }
        }
        
    }

    public enum Icons
    {
        NaoEyesOff,
        NaoEyesOn,
        ScreenLeft,
        ScreenRight,
        Elsewhere,
        ActiveSpeakerON,
        ActiveSpeakerOFF
    }

    /// <summary>
    /// Interaction logic for Common.xaml
    /// </summary>
    public partial class Common : UserControl
    {        
        CommonViewModel data;
        public Common()
        {
            InitializeComponent();
            data = this.DataContext as CommonViewModel;
            //Test();
        }

        public void SetTitle(string title)
        {
            data.Title = title;
        }
        public void SetLeftIcon(Icons icon)
        {
            data.LeftStyle = GetStyle(icon);
        }
        public void SetRightIcon(Icons icon)
        {
            data.RightStyle = GetStyle(icon);
        }
        public void SetLeftDescription(string description)
        {
            data.LeftDescription = description;
        }
        public void SetRightDescription(string description)
        {
            data.RightDescription = description;
        }
        public void SetLeftProgressBar(double value)
        {
            data.ProgressBarLeft = value;
        }
        public void SetRightProgressBar(double value)
        {
            data.ProgressBarRight = value;
        }

        private Style GetStyle(Icons icon)
        {
            Style style = Resources[icon.ToString()] as Style;
            if (style == null)
            {
                throw new Exception("Can't find the style for icon: " + icon);
            }
            return style;
        }

        public void Test()
        {
            data = this.DataContext as CommonViewModel;
            data.LeftStyle = Resources["NaoEyesOff"] as Style;
            data.RightStyle = Resources["NaoEyesOff"] as Style;
            data.Title = "new";
        }
    }
}
