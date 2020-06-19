using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VetProject
{
    class SwitchLanguage
    {
        public static InputLanguage la, en;
        public static void setLanguage()
        {
            foreach (InputLanguage l in InputLanguage.InstalledInputLanguages)
            {
                if (l.LayoutName.Contains("US"))
                {
                    en = l;
                }
                if (l.LayoutName.Contains("Lao"))
                {
                    la = l;
                }
            }
        }
    }
}
