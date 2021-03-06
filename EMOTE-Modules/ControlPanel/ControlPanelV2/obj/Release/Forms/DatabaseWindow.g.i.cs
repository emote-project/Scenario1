﻿#pragma checksum "..\..\..\Forms\DatabaseWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "47294686A97A66256FE74B39640751F2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ControlPanel.Thalamus.UserControl;
using ControlPanel.viewModels;
using EmoteEvents;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace ControlPanel.Forms {
    
    
    /// <summary>
    /// DatabaseWindow
    /// </summary>
    public partial class DatabaseWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\..\Forms\DatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid MainGrid;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\Forms\DatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtFirstName;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\Forms\DatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMiddleName;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\Forms\DatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtLastName;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\Forms\DatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker DatePickerBirth;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\Forms\DatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbSex;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\Forms\DatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddButton;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\Forms\DatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveButton;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\Forms\DatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock DatabaseStatus;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\Forms\DatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ImportButton;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\Forms\DatabaseWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RefreshButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/ControlPanel;component/forms/databasewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Forms\DatabaseWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.MainGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 2:
            this.txtFirstName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.txtMiddleName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.txtLastName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.DatePickerBirth = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 6:
            this.cmbSex = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.AddButton = ((System.Windows.Controls.Button)(target));
            
            #line 73 "..\..\..\Forms\DatabaseWindow.xaml"
            this.AddButton.Click += new System.Windows.RoutedEventHandler(this.AddButton_OnClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.RemoveButton = ((System.Windows.Controls.Button)(target));
            
            #line 74 "..\..\..\Forms\DatabaseWindow.xaml"
            this.RemoveButton.Click += new System.Windows.RoutedEventHandler(this.RemoveButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.DatabaseStatus = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.ImportButton = ((System.Windows.Controls.Button)(target));
            
            #line 97 "..\..\..\Forms\DatabaseWindow.xaml"
            this.ImportButton.Click += new System.Windows.RoutedEventHandler(this.ImportButton_OnClick);
            
            #line default
            #line hidden
            return;
            case 11:
            this.RefreshButton = ((System.Windows.Controls.Button)(target));
            
            #line 98 "..\..\..\Forms\DatabaseWindow.xaml"
            this.RefreshButton.Click += new System.Windows.RoutedEventHandler(this.RefreshButton_OnClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

