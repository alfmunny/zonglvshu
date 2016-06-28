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
    /// Interaction logic for ElementPropertyWindow.xaml
    /// </summary>
    public partial class ElementPropertyWindow : Window
    {
        private ElementControl ele;

        public ElementPropertyWindow(ElementControl ele)
        {
            InitializeComponent();
            this.ele = ele;
        }

    private void button_Click(object sender, RoutedEventArgs e)
    {
        ele.Content = Label.Text;
        this.Close();
    }
    }
}
