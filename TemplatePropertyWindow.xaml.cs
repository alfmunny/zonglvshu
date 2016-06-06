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
using System.Windows.Shapes;
using Norne_Beta.UIElements;

namespace Norne_Beta
{
    /// <summary>
    /// Interaction logic for TemplatePropertyWindow.xaml
    /// </summary>
    public partial class TemplatePropertyWindow : Window
    {
        private TemplateControl temp;

        public TemplatePropertyWindow(TemplateControl temp)
        {
            InitializeComponent();
            this.temp = temp;
            TemplateName.Text = temp.TemplateName;
            ParentControl.Text = temp.ParentControl;
            ParentClass.Text = temp.ParentClass;
            ParentGfx.Text = temp.ParentGfx;
            Label.Text = temp.TemplateLabel;
            txtScene.Text = temp.SceneName;
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            temp.TemplateName = TemplateName.Text;
            temp.ParentClass = ParentClass.Text;
            temp.ParentControl = ParentControl.Text;
            temp.ParentGfx = ParentGfx.Text;
            temp.TemplateLabel = Label.Text;
            temp.SceneName = txtScene.Text;
            this.Close();
        }

    }
}
