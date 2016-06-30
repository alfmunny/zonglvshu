﻿using System;
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
using System.Reflection;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace Norne_Beta.UIElements
{
    /// <summary>
    /// Interaction logic for HorizontalTemplate.xaml
    /// </summary>
    public partial class HorizontalTemplate: TemplateControl
    {
        public MainWindow mw;

        public HorizontalTemplate(MainWindow win)
        {
            InitializeComponent();
            mw = win;
            _dockPanel = this.MainPanel;
        }

        public void DockPanel_Drop(object sender, DragEventArgs e)
        {
            DockPanel dp = sender as DockPanel;
            string elementName = (string)e.Data.GetData(DataFormats.StringFormat);
            AddElementToDockPanel(mw, elementName);
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

        private void MenuItemPaste_Click(object sender, RoutedEventArgs e)
        {
            ElementControl x = mw.ElementToCopy;

            if(x != null)
            {
                this._dockPanel.Children.Add(x);
                this.Elements.Add(x);
                x.LabelID = this.GetLabelID();
                DockPanel.SetDock(x, Dock.Top);
                mw.ElementToCopy  = null;
            }
            else
            {
                MessageBox.Show("There is no available ui element to paste, please copy at first");
            }
        }
        private void TemplateControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetTargetProperties(new string[]
            {
                nameof(TemplateName),
                nameof(TemplateLabel),
                nameof(ParentControl),
                nameof(ParentGfx),
                nameof(SceneName),
                nameof(ContinuesLeft),
            });

            mw._propertyGrid.SelectedObject = this;
        }

        public void SetTargetProperties(string[] properties)
        {
            mw._propertyGrid.PropertyDefinitions.Clear();
            PropertyDefinition item = new PropertyDefinition();
            item.TargetProperties = properties;
            mw._propertyGrid.PropertyDefinitions.Add(item);
        }

        private void MenuItemLoad_Click(object sender, RoutedEventArgs e)
        {
            this.mw.ParseTemplate(this);
        }

        public override ElementControl AddElement(string elementType)
        {
            ElementControl e =  AddElementToDockPanel(mw, elementType);
            return e;
        }

        public override void ClearElements()
        {
            base.ClearElements();
            MainPanel.Children.Clear();
        }

    }
}
