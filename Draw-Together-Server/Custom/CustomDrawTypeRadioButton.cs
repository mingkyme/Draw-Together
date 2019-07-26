using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Draw_Together_Server.Custom
{
    class CustomDrawTypeRadioButton : RadioButton
    {
        private InkCanvasEditingMode drawType;
        public InkCanvasEditingMode DrawType
        {
            get
            {
                return drawType;
            }
            set
            {
                drawType = value;
            }
        }
    }
}
