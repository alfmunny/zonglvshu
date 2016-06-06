using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using Norne_Beta.UIElements;


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

    public partial class MainWindow
    {
        private BrushConverter bc = new BrushConverter();

        private void NewProject(Grid parentGrid)
        {
            ProjectWindow pw = new ProjectWindow(this);
            parentGrid.Children.Add(pw);
        }

        public void addVerticalTemplate(Canvas canvas, Point p)
        {
            VerticalTemplate vt = new VerticalTemplate();
            canvas.Children.Add(vt);
            Canvas.SetLeft(vt, p.X);
            Canvas.SetTop(vt, p.Y);
        }

        public void addHorizontalTemplate(Canvas canvas, Point p)
        {
            HorizontalTemplate ht = new HorizontalTemplate(this);
            canvas.Children.Add(ht);
            Canvas.SetLeft(ht, p.X);
            Canvas.SetTop(ht, p.Y);
        }

        public void addVerticalTemplateToGrid(Grid grid, Point point)
        {
            VerticalTemplate vt = new VerticalTemplate();
            grid.Children.Add(vt);
        }

        public void addHorizontalTemplateToGrid(Grid grid, Point point)
        {
            HorizontalTemplate ht = new HorizontalTemplate(this);
            grid.Children.Add(ht);
        }

        /*
        public void addButtonToDockPanel(DockPanel dp)
        {
            //Button btn = new Button() { Content = "Button" };
            BaseButton btn = new BaseButton();
            dp.Children.Add(btn);
            DockPanel.SetDock(btn, Dock.Top);
        }
        */

        public void addTextBoxToDockPanel(DockPanel dp)
        {
            TextBox newTextBox = new TextBox() { Text = "New Text", };
            dp.Children.Add(newTextBox);
        }

        /*
        public void addTextPanelToDockPanel(DockPanel dp)
        {

            TextPanel tp = new TextPanel();
            dp.Children.Add(tp);
            DockPanel.SetDock(tp, Dock.Top);
        }
        */

        public void addVerticalTemplateToDockPanel(DockPanel dp)
        {
            VerticalTemplate vt = new VerticalTemplate();
            dp.Children.Add(vt);
            DockPanel.SetDock(vt, Dock.Top);
        }

        public void addHorizontalTemplateDockPanel(MainWindow win, DockPanel dp)
        {
            HorizontalTemplate ht = new HorizontalTemplate(win);
            dp.Children.Add(ht);
            DockPanel.SetDock(ht, Dock.Top);
        }

    }

    class ProjectGenerator
    {
        public void BuildProject(Grid projectGrid)
        {

        }
    }
}
