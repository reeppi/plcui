﻿#pragma checksum "..\..\..\input\inputNumeric.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "BA572F2CEDA29BD111CC7517DCC08048B8E03BC5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
using libPLC;


namespace libPLC {
    
    
    /// <summary>
    /// inputNumeric
    /// </summary>
    public partial class inputNumeric : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 30 "..\..\..\input\inputNumeric.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textBoxVal;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\input\inputNumeric.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button buttonkeyboad;
        
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
            System.Uri resourceLocater = new System.Uri("/libPLC;component/input/inputnumeric.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\input\inputNumeric.xaml"
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
            
            #line 8 "..\..\..\input\inputNumeric.xaml"
            ((libPLC.inputNumeric)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            
            #line 8 "..\..\..\input\inputNumeric.xaml"
            ((libPLC.inputNumeric)(target)).IsVisibleChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.UserControl_IsVisibleChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.textBoxVal = ((System.Windows.Controls.TextBox)(target));
            
            #line 30 "..\..\..\input\inputNumeric.xaml"
            this.textBoxVal.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.textBoxVal_TextChanged);
            
            #line default
            #line hidden
            
            #line 30 "..\..\..\input\inputNumeric.xaml"
            this.textBoxVal.KeyDown += new System.Windows.Input.KeyEventHandler(this.textBoxVal_KeyDown);
            
            #line default
            #line hidden
            
            #line 30 "..\..\..\input\inputNumeric.xaml"
            this.textBoxVal.Loaded += new System.Windows.RoutedEventHandler(this.textBoxVal_Loaded);
            
            #line default
            #line hidden
            
            #line 30 "..\..\..\input\inputNumeric.xaml"
            this.textBoxVal.IsVisibleChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.textBoxVal_IsVisibleChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 31 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.buttonBackspace_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 34 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 35 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 36 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 37 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 38 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 39 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 40 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 41 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 42 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 43 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 44 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            
            #line 45 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 46 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 47 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            
            #line 48 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 19:
            this.buttonkeyboad = ((System.Windows.Controls.Button)(target));
            
            #line 49 "..\..\..\input\inputNumeric.xaml"
            this.buttonkeyboad.Click += new System.Windows.RoutedEventHandler(this.buttonkeyboad_Click);
            
            #line default
            #line hidden
            return;
            case 20:
            
            #line 50 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonV_Click);
            
            #line default
            #line hidden
            return;
            case 21:
            
            #line 51 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonV_Click);
            
            #line default
            #line hidden
            return;
            case 22:
            
            #line 53 "..\..\..\input\inputNumeric.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.setValue_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

