using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Norne_Beta.UIElements;

namespace PythonLib 
{
    class PyWriter
    {
        private StreamWriter f;
        private VizController vc;
        private HorizontalTemplate t;

        public PyWriter(StreamWriter file, HorizontalTemplate t)
        {
            this.f = file;
            this.t = t;
            vc = new VizController(t.TemplateLabel);
        }

        public void Import()
        {
            f.WriteLine("import wx;");
            f.WriteLine("from norne.device import viz");
            f.WriteLine("from norne.template.base import tool");
            f.WriteLine("from norne.template.sky2016.util import BaseTable");
            f.WriteLine("from norne.template.sky2016.baseinsert import SimpleBaseUI, SimpleBaseCtrl, MultiBaseCtrl, SimpleBaseGfx, MultiBaseGfx, OneShotBaseCtrl, ToggleBaseCtrl");
            f.WriteLine("");
        }
        
        public void InitUI()
        {
            string className = string.Format("class {0}({1}):", t.UIClassName, t.ParentClass);
            f.WriteLine(className);
            f.WriteLine("   name = \"{0}\"", t.TemplateName);
            f.WriteLine("   label = \"{0}\"", t.TemplateLabel);
            f.WriteLine("   def __init__(self, parent, wxid, project, *args, **kwargs):");
            f.WriteLine("       self.globals_dct.update(globals())");
            f.WriteLine("       SimpleBaseUI.__init__(self, parent, wxid, project, *args, **kwargs)");
        }

        public void SetupController()
        {
            f.WriteLine("   def setup_control(self):");
            f.WriteLine("       self.btn_show = {0}(self, -1, \"{1}\", self.project, get_content_callback=self.get_content, statemachine=self.{2})", vc.Controller, vc.Label, vc.StateMachine);
        }

        public void SetupStateMachine()
        {
            f.WriteLine("   def setup_statemachine(self):");
            f.WriteLine("       self.statemachine = {0}Gfx(self.project)", t.TemplateName);
        }

        public void CreateUI()
        {
            f.WriteLine("   def create_ui(self):");
            AddTempalte(t);
            f.WriteLine("");
        }

        private void AddTempalte(HorizontalTemplate t)
        {
            if (t.Elements.Count() == 0)
            {
                f.WriteLine("       pass");
                return;
            }
            else
            {
                foreach (ElementControl ele in t.Elements)
                {
                    f.WriteLine("       self.ctrl_obj.add_element({0})", ele.GetUICode());
                }
            }
        }


        public void WriteController()
        {

        }

        public void WriteStateMachine()
        {
            string className = string.Format("class {0}({1}):", t.GfxClassName, t.ParentGfx);
            f.WriteLine(className);
            f.WriteLine("   def evaluate_content(self):");
            f.WriteLine("       self.scene_name = \"{0}\"", t.SceneName);
            WriteSetContent();
            f.WriteLine("");
        }

        public void WriteSetContent()
        {
            f.WriteLine("   def set_content(self):");
            AddContent(t);
            f.WriteLine("       pass");
        }

        private void AddContent(HorizontalTemplate t)
        {
            foreach(ElementControl e in t.Elements)
            {
                if (e.GetContentCode() == null)
                {
                    f.WriteLine("       self.set_value('', '')");
                    break;
                }

                foreach (string item in e.GetContentCode())
                {
                    if (e.Type == e.Elements.Table)
                    {
                        f.WriteLine("       self.set_table_value({0})", item);
                    }
                    else
                    {
                        f.WriteLine("       self.set_value({0})", item);
                    }
                }
            }
        }

        
    }

    class VizController
    {
        public string Label { get; set; }
        public string StateMachine { get; set; }
        public string Controller{ get; set; }

        public VizController(string label)
        {
            this.Label = label;
            this.StateMachine = "statemachine";
            this.Controller = "SimpleBaseCtrl";
        }
    }
}
