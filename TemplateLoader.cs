using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Norne_Beta.UIElements;

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
            LoadUI();
            LoadGfx();
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
                    for (int i = 0; i < elements.Count(); i++)
                    {

                        string id = labelID + "_" + ((string)elements[i]).Split('|')[0];
                        string elementsType = ((string)elements[i]).Split('|')[1];

                        ElementControl ele = dp.AddElementToDockPanel(mw, elementsType);
                        if( ele != null)
                        {
                            ele.LoadContent((JArray)parameters[0][i]);
                            ele.LoadControlObject(content, id);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < elements.Count(); i++)
                    {
                        ElementControl ele = template.AddElement((string)elements[i]);
                        if( ele != null)
                        {
                            ele.LoadContent((JArray)parameters[i]);
                            ele.LoadControlObject(content, labelID);
                        }
                    }
                }
            
            }
        }

        public void LoadGfx()
        {

        }



    }
}
