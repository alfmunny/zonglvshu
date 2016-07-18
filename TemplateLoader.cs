using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Norne_Beta.UIElements;
using System.Windows.Controls;

namespace Norne_Beta
{
    class TemplateLoader
    {

        public JObject TemplateJson { get; set; }
        private TemplateControl template;

        private MainWindow mw;

        public TemplateLoader(dynamic templateJson, MainWindow win, TemplateControl parentTemplate)
        {
            TemplateJson = templateJson;
            template = parentTemplate;
            mw = win;
        }

        public void LoadTemplate()
        {
            try
            {
                LoadUI();
                LoadGfx();
            }
            catch (Exception)
            {
                Console.WriteLine("Cant't Load Template");
            }
        }

        public void LoadUI()
        {
            template.ClearElements();
            template.LoadContent(TemplateJson);
            JArray content = (JArray)TemplateJson["content"];

            foreach (JToken item in TemplateJson["ui"])
            {
                string labelID = (string)item["label_id"];
                JArray elements = (JArray)item["elements"];
                JArray parameters = (JArray)item["parameters"];

                if (elements.Count > 1)
                {
                    BaseDockPanel dp = (BaseDockPanel)template.AddElementToDockPanel(mw, "DockPanel");
                    dp.LoadLabelID(labelID);
                    for (int i = 0; i < elements.Count(); i++)
                    {
                        string id;
                        string subLabelID = ((string)elements[i]).Split('|')[0];

                        if (! subLabelID.Contains("!"))
                        {
                            id = labelID + "_" + ((string)elements[i]).Split('|')[0];
                        }
                        else
                        {
                            id = subLabelID;
                        }
                        string elementsType = ((string)elements[i]).Split('|')[1];

                        ElementControl ele = dp.AddElementToDockPanel(mw, elementsType);
                        if( ele != null)
                        {
                            ele.LoadContent((JArray)parameters[0][i]);
                            ele.LoadLabelID(id);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < elements.Count(); i++)
                    {
                        ElementControl ele = template.AddElementToDockPanel(mw, (string)elements[i]);
                        if( ele != null)
                        {
                            ele.LoadContent((JArray)parameters[i]);
                            ele.LoadLabelID(labelID);
                            // TODO: Add LoadLabelID
                        }
                    }
                }
            
            }
        }

        public void LoadGfx()
        {
            template._controlPanel.Children.Clear();
            template.StateMachines.Clear();
            foreach (JProperty item in ((JObject)TemplateJson["statemachines"]).Properties())
            {
                BaseControlButton bcb = new BaseControlButton(mw, template);
                template.StateMachines.Add(bcb);
                template._controlPanel.Children.Add(bcb);
                DockPanel.SetDock(bcb, Dock.Top);
                bcb.LoadGfxContent(item);
            }
        }

    }
}
