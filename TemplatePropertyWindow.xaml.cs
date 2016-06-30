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
            ParentClass.Text = temp.ParentClass;
            Label.Text = temp.TemplateLabel;
            txtScene.Text = temp.SceneName;
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            temp.TemplateName = TemplateName.Text;
            temp.UIClassName = TemplateName.Text + "UI";
            temp.GfxClassName = TemplateName.Text + "Gfx";
            temp.ParentClass = ParentClass.Text;
            temp.TemplateLabel = Label.Text;
            temp.SceneName = txtScene.Text;
            this.Close();
        }
    }
}
