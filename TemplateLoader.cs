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

            foreach (JToken item in TemplateJson["ui"])
            {
                string labelID = (string)item["label_id"];
                JArray elements = (JArray)item["elements"];
                JArray paramters = (JArray)item["parameters"];

                for (int i = 0; i < elements.Count(); i++)
                {
                    if ((string)elements[i]== "TextFieldPanel")
                    {
                        TextPanel tp = new TextPanel(mw, template, labelID);
                        tp.SetLabel((string)paramters[i][0]);
                        tp.SetText((string)paramters[i][1]);
                        template.AddElement(tp);
                    }
                }
            }
        }

        public void LoadGfx()
        {

        }



    }
}
