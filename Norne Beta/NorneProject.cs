using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Norne_Beta
{
    class NorneProject
    {
        public void AddButton(Grid parentGrid)
        {
            Button btnShow = new Button();
            btnShow.Content = "this";
            parentGrid.Children.Add(btnShow);
        }
    } 

    class ProjectEditor
    {
        public void NewProject(Grid parentGrid)
        {
            TabControl project = new TabControl();
            project.HorizontalAlignment = HorizontalAlignment.Stretch;
            project.VerticalAlignment = VerticalAlignment.Stretch;
            project.Width = double.NaN;
            project.Height = double.NaN;
            TabItem pageMain = new TabItem();
            TabItem pageDebug = new TabItem();
            pageMain.Width = 60.0;
            pageMain.Height = 20.0;
            pageDebug.Width = 60.0;
            pageDebug.Height = 20.0;
            pageMain.Header = "Main";
            pageDebug.Header = "Debug";
            project.Items.Add(pageMain);
            project.Items.Add(pageDebug);
            parentGrid.Children.Add(project);
        }
    }

    class ProjectGenerator
    {
        public void BuildProject(Grid projectGrid)
        {

        }
    }
}
