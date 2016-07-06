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

        private TablePanel tp;

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
            f.WriteLine("from norne.template.sky2016.util import *");
            f.WriteLine("from norne.template.sky2016.baseinsert import SimpleBaseUI, SimpleBaseCtrl, MultiBaseCtrl, SimpleBaseGfx, MultiBaseGfx, OneShotBaseCtrl, ToggleBaseCtrl, HighlightScriptGfx");
            f.WriteLine("");
        }
        
        public void InitUI()
        {
            string className = string.Format("class {0}({1}):", t.GetUIClassName(), t.ParentClass);
            f.WriteLine(className);
            f.WriteLine("\tname = \"{0}\"", t.TemplateName);
            f.WriteLine("\tlabel = \"{0}\"", t.TemplateLabel);
            f.WriteLine("\tdef __init__(self, parent, wxid, project, *args, **kwargs):");
            f.WriteLine("\t\tself.globals_dct.update(globals())");
            f.WriteLine("\t\tSimpleBaseUI.__init__(self, parent, wxid, project, *args, **kwargs)");
        }

        public void SetupController()
        {
            f.WriteLine("\tdef setup_control(self):");
            for (int i = 0; i < t.StateMachines.Count; i++)
            {
                BaseControl x = t.StateMachines[i];
                f.WriteLine("\t\tself.{0} = {1}(self, -1, \"{2}\", self.project, get_content_callback=self.get_content, statemachine=self.{3})", 
                     x.BtnName, x.ParentControl, x.ControlName, x.StateMachineName);

            }
        }

        public void SetupStateMachine()
        {
            f.WriteLine("\tdef setup_statemachine(self):");
            foreach (BaseControl item in t.StateMachines)
            {
                f.WriteLine("\t\tself.{0} = {1}(self.project)", item.StateMachineName, item.GfxClassName);
            }
        }

        public void CreateUI()
        {
            f.WriteLine("\tdef create_ui(self):");
            AddTempalte(t);
            f.WriteLine("");
        }

        private void AddTempalte(HorizontalTemplate t)
        {
            if (t.Elements.Count() == 0)
            {
                f.WriteLine("\t\tpass");
                return;
            }
            else
            {
                foreach (ElementControl ele in t.Elements)
                {
                    f.WriteLine("\t\tself.ctrl_obj.add_element({0})", ele.GetUICode());
                }
            }
        }


        public void WriteController()
        {

        }

        public void WriteMultipleStateMachines()
        {
            foreach (BaseControl bc in t.StateMachines)
            {
                WriteStateMachine(bc);
            }
        }

        public void WriteStateMachine(BaseControl bc)
        {
            string className = string.Format("class {0}({1}):", bc.GfxClassName, bc.ParentGfx);
            f.WriteLine(className);
            f.WriteLine("\tdef evaluate_content(self):");
            f.WriteLine("\t\tself.scene_name = \"{0}\"", bc.SceneName);
            if (bc.HasHighlights)
            {
                f.WriteLine("\t\tself.has_highlights= self.content.get(\"has_highlights\")");
            }
            WriteControlObject();
            WriteSetContent();
            if (t.HasHighlights)
            {
                WriteSetHighlights();
            }

            f.WriteLine("");
        }

        public void WriteSetHighlights()
        {
            if (tp != null)
            {
                f.WriteLine("\tdef setup_highlights(self):");
                f.WriteLine("\t\tprefix = \"{0}\"", t.HighlightPrefix);
                f.WriteLine("\t\tlin_cnt = {0}", tp.PyCodeTableCount.ToString());
                f.WriteLine("\t\thighlights = self.set_single_highlights(prefix, lin_cnt)");
                f.WriteLine("\t\tself.lin_cnt = {0}", tp.PyCodeTableCount.ToString());
                f.WriteLine("\t\tself.highlights = {\"onair\": highlights}");
                f.WriteLine("\t\tself.set_onair_highlights(self.content[\"tbl_{0}\"], {1}, {2}, lin_cnt)", tp.LabelID, tp.HighlightLabelIndex, tp.HighlightCheckBoxIndex);
            }
        }

        public void WriteSetContent()
        {
            f.WriteLine("\tdef set_content(self):");
            AddContent(t);
            f.WriteLine("\t\tpass");
        }

        public void WriteControlObject()
        {
            f.WriteLine("\tdef define_ctrl_plugin(self):");
            f.WriteLine("\t\tself.ctrl_plugin = viz.VizGroup(\"object\", self.scene)");
        }

        private void Writeline(string code, int num)
        {
            for (int i = 0; i < num; i++)
            {
                f.Write("\t");
            }
            f.WriteLine(code);
        }

        private void AddContent(HorizontalTemplate t)
        {
            foreach(ElementControl e in t.Elements)
            {
                if (e.GetContentCode() == null)
                {
                    f.WriteLine("\t\tself.set_value('', '')");
                    break;
                }

                //TODO: optimize the set_content and GetContentCode
                foreach (string item in e.GetContentCode())
                {
                    if (e.NorneType != ElementType.DockPanel && e.ControlObject == "")
                    {
                        continue; 
                    }
                    int index = e.GetContentCode().IndexOf(item);
                    if (e.NorneType == ElementType.BaseTable && index == 0)
                    {
                        if(((TablePanel)e).HasHighlights)
                        {
                            tp = (TablePanel)e;
                        }

                        f.WriteLine("\t\tself.set_table_col({0})", item);
                    }
                    else
                    {
                        f.WriteLine("\t\tself.set_value({0})", item);
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
