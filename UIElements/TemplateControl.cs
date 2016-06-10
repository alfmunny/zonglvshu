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
using System.Reflection;

namespace Norne_Beta.UIElements
{

    public partial class TemplateControl : UserControl
    {
        public string TemplateName { get; set; }
        public string UIClassName{ get; set; }
        public string GfxClassName{ get; set; }
        public string TemplateLabel { get; set; }
        public string ParentClass { get; set; }
        public string ParentControl { get; set; }
        public string ParentGfx { get; set; }
        public string SceneName { get; set; }
        public string ProjectName { get; set; }
        public string FilePath { get; set; }

        public List<ElementControl> Elements;
        private string label = "line";

        public TemplateControl()
        {
            this.TemplateName = "TemplateName";
            this.UIClassName = this.TemplateName + "UI";
            this.GfxClassName = this.TemplateName + "Gfx";
            this.TemplateLabel = "Label";
            this.ParentClass = "SimpleBaseUI";
            this.ParentControl = "SimpleBaseCtrl";
            this.ParentGfx = "SimpleBaseGfx";
            this.Elements = new List<ElementControl>();

            this.SceneName = "";
            this.FilePath= "";
        }

        private string GetLabelID()
        {
            return this.label + (this.Size + 1).ToString();
        }

        public enum Action
        {
            Add
        }

        public enum Target
        {
            ToDockPanel
        }

        public int Size
        {
            get
            {
                return this.Elements.Count();
            }
        }

        public void RemoveElement(ElementControl ele)
        {
            this.Elements.Remove(ele);
        }

        public void AddElementsToDockPanel(MainWindow win, TemplateControl parentTemplate, string elementType, DockPanel dp)
        {
            Type t = this.GetType();
            string methodName = GetMethodName(Action.Add, elementType, Target.ToDockPanel);
            if (methodName != null)
            {
                MethodInfo method = t.GetMethod(methodName);
                object[] parameters = new object[] { win, parentTemplate, dp };
                object ele = method.Invoke(this, parameters);
                this.Elements.Add(ele as ElementControl);
            }
            else
            {
                Console.WriteLine("{0} can't be added to the template", elementType);
            }
        }

        public string GetMethodName(Enum action, string label, Enum taget)
        {
            string methodName = action.ToString() + label + taget.ToString();
            return methodName;
        }

        public ElementControl AddButtonToDockPanel(MainWindow win, TemplateControl parentTemplate, DockPanel dp)
        {
            string label = GetLabelID();
            BaseButton btn = new BaseButton(win, parentTemplate, label);
            dp.Children.Add(btn);
            DockPanel.SetDock(btn, Dock.Top);
            return btn;
        }

        /*
        public void AddTextBoxToDockPanel(DockPanel dp)
        {
            TextBox newTextBox = new TextBox() { Text = "New Text", };
            dp.Children.Add(newTextBox);
        }
        */

        public ElementControl AddTextPanelToDockPanel(MainWindow win, TemplateControl parentTemplate, DockPanel dp)
        {
            string label = GetLabelID();
            TextPanel tp = new TextPanel(win, parentTemplate, label);
            dp.Children.Add(tp);
            DockPanel.SetDock(tp, Dock.Top);
            return tp;
        }

        public ElementControl AddTableToDockPanel(MainWindow win, TemplateControl parentTemplate, DockPanel dp)
        {
            string label = GetLabelID();
            TablePanel tp = new TablePanel(win, parentTemplate, label);
            dp.Children.Add(tp);
            DockPanel.SetDock(tp, Dock.Top);
            return tp;
        }

        public ElementControl AddDockPanelToDockPanel(MainWindow win, TemplateControl parentTemplate, DockPanel dp)
        {

            string label = GetLabelID();
            BaseDockPanel bdp = new BaseDockPanel(win, parentTemplate, label);
            dp.Children.Add(bdp);
            DockPanel.SetDock(bdp, Dock.Top);
            return bdp;
        }

        public virtual void LoadElements()
        {
            Console.WriteLine("Please implement the method of LoadTemplate");
        }

        public virtual void ClearElements()
        {
            this.Elements.Clear();
        }

        public virtual void AddElement(ElementControl element)
        {
            Console.WriteLine("Please implement the method of AddElement");
        }

    }
}

