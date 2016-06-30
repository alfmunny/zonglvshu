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
using System.ComponentModel;
using System.Reflection;

namespace Norne_Beta.UIElements
{
    /// <summary>
    /// Interaction logic for VerticalTemplate.xaml
    /// </summary>
    public partial class VerticalTemplate : TemplateControl 
    {

        public VerticalTemplate()
        {
            InitializeComponent();
            this.TemplateName = "";
            this.ParentControl = CtrlType.SimpleBaseCtrl;
            this.ParentClass = "SimpleBaseUI";
            this.ParentGfx = GfxType.SimpleBaseGfx;
        }

        public void DockPanel_Drop(object sender, DragEventArgs e)
        {
            DockPanel dp = sender as DockPanel;
            string label = (string)e.Data.GetData(DataFormats.StringFormat);
            Type t = this.GetType();
            string methodName = GetMethodName(Action.Add, label, Target.ToDockPanel);
            MethodInfo method = t.GetMethod(methodName);
            object[] parameters = new object[] { dp };
            method.Invoke(this, parameters);
        }


        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.Parent != null)
            {
                ((Panel)this.Parent).Children.Remove(this);
            }
        }

        private void MenuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            TemplatePropertyWindow tpw = new TemplatePropertyWindow(this);
            tpw.Show();
        }
    }
}
