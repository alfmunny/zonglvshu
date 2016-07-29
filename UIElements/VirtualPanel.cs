using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norne_Beta.UIElements
{
    class VirtualPanel : ElementControl
    {
        public VirtualPanel(MainWindow win, TemplateControl parentTemplate, string label)
            :base(win, parentTemplate)
        {
            LabelID = label;
            NorneType = ElementType.Panel;
        }

        public override string GetUIParameters()
        {
            String ret = String.Format("[wx.Size(-1,0)]");
            return ret;
        }
    }
}
