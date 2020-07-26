using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VetProject
{
    class NumberOnly
    {
        public static void setNumber(object sender, KeyEventArgs e, Control c)
        {
            if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Space && e.KeyCode != Keys.Enter && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right)
            {
                try
                {
                    if (e.KeyValue < 48 || e.KeyValue > 57)
                    {
                        e.Handled = true;

                        if (c.Text.Length < 0)
                        {
                            return;
                        }
                        else
                        {
                            c.Text = c.Text.Substring(0, c.Text.Length - 1);
                        }
                        MessageBox.Show("ປ້ອນສະເພາະຕົວເລກ");
                    }
                }
                catch (Exception)
                {
                    
                }
               
            }
        }
    }
}
