﻿#pragma checksum "..\..\..\Pages\LoginPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "AA8B7343BBD45CE1CFB57D7A467EAC53070F6E61"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Minesweeper.Pages;
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


namespace Minesweeper.Pages {
    
    
    /// <summary>
    /// LoginPage
    /// </summary>
    public partial class LoginPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 14 "..\..\..\Pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtBoxClumns;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtBoxRows;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\Pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtBoxMines;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtBlockTitle;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\Pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSubmit;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\Pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbkColumnError;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\Pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbkRowError;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\Pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tbkMineError;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\Pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlock;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\Pages\LoginPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlock1;
        
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
            System.Uri resourceLocater = new System.Uri("/Minesweeper;component/pages/loginpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\LoginPage.xaml"
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
            this.txtBoxClumns = ((System.Windows.Controls.TextBox)(target));
            
            #line 14 "..\..\..\Pages\LoginPage.xaml"
            this.txtBoxClumns.GotFocus += new System.Windows.RoutedEventHandler(this.txtBoxClumns_GotFocus);
            
            #line default
            #line hidden
            
            #line 14 "..\..\..\Pages\LoginPage.xaml"
            this.txtBoxClumns.LostFocus += new System.Windows.RoutedEventHandler(this.txtBoxClumns_LostFocus);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtBoxRows = ((System.Windows.Controls.TextBox)(target));
            
            #line 21 "..\..\..\Pages\LoginPage.xaml"
            this.txtBoxRows.LostFocus += new System.Windows.RoutedEventHandler(this.txtBoxRows_LostFocus);
            
            #line default
            #line hidden
            
            #line 21 "..\..\..\Pages\LoginPage.xaml"
            this.txtBoxRows.GotFocus += new System.Windows.RoutedEventHandler(this.txtBoxRows_GotFocus);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtBoxMines = ((System.Windows.Controls.TextBox)(target));
            
            #line 28 "..\..\..\Pages\LoginPage.xaml"
            this.txtBoxMines.GotFocus += new System.Windows.RoutedEventHandler(this.txtBoxMines_GotFocus);
            
            #line default
            #line hidden
            
            #line 28 "..\..\..\Pages\LoginPage.xaml"
            this.txtBoxMines.LostFocus += new System.Windows.RoutedEventHandler(this.txtBoxMines_LostFocus);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtBlockTitle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.btnSubmit = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\Pages\LoginPage.xaml"
            this.btnSubmit.Click += new System.Windows.RoutedEventHandler(this.button_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.tbkColumnError = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.tbkRowError = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.tbkMineError = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.textBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.textBlock1 = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
