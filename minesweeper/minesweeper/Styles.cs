using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace minesweeper
{
    class Styles
    {
        public Style enabledDefaultStyle;

        public Style disabledDefaultStyle;

        public Styles()
        {
            createEnDefStyle();
            createDisDefStyle();
        }

        public void createEnDefStyle()
        {
    
            //    < Setter Property = "OverridesDefaultStyle" Value = "True" />
          
            //    < Setter Property = "Template" >
           
            //    < Setter.Value >
           
            //    < ControlTemplate TargetType = "Button" >
            
            //    < Border Name = "border"
            //BorderThickness = "1"
            //Padding = "4,2"
            //BorderBrush = "DarkGray"
            //CornerRadius = "3"
            //Background = "{TemplateBinding Background}" >
            //    < ContentPresenter HorizontalAlignment = "Center" VerticalAlignment = "Center" />
   
            //    </ Border >
   
            //    < ControlTemplate.Triggers >
   
            //    < Trigger Property = "IsMouseOver" Value = "True" >
      
            //    < Setter TargetName = "border" Property = "BorderBrush" Value = "Black" />

        }

        public void createDisDefStyle()
        {

        }
    }
}
