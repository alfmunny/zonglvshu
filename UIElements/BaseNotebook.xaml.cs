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

namespace Norne_Beta.UIElements
{
    /// <summary>
    /// Interaction logic for BaseNotebook.xaml
    /// </summary>
    public partial class BaseNotebook : ElementControl
    {
        public BaseNotebook(MainWindow win, TemplateControl parentTemplate, string label)
            :base(win, parentTemplate)
        {
            InitializeComponent();
        }
    }
}
