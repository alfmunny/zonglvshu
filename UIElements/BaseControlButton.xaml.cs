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
    /// Interaction logic for BaseControlButton.xaml
    /// </summary>
    public partial class BaseControlButton : BaseControl 
    {
        private const string _inactiveColor = "#FF979BEE";
        private const string _activeColor = "#FFEC8992";


        public BaseControlButton(MainWindow win, TemplateControl parentTemplate)
            :base(win, parentTemplate)
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            this.button.Content = Label;
        }

        public override void SetProperty()
        {
            this.button.Content = Label;
        }

        private void button_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((BaseControlButton)ParentTemplate.ControlEditing).ChangeColor(_inactiveColor);
            this.ChangeColor(_activeColor);

            ParentTemplate.ControlEditing = this;
            ParentTemplate.LoadStateMachine();

            this.BasicProperty.Add(nameof(Label));
            SetTargetProperties(BasicProperty.ToArray());
            this.mw._propertyGrid.SelectedObject = this;
        }
        

        private void ChangeColor(string color)
        {
            var bc = new BrushConverter();
            this.button.Background = (Brush)bc.ConvertFrom(color);
        }
    }
}
