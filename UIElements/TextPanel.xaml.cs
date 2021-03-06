﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Newtonsoft.Json.Linq;

namespace Norne_Beta.UIElements
{
    /// <summary>
    /// Interaction logic for NorneUIElements.xaml
    /// </summary>
    public partial class TextPanel : ElementControl 
    {

        public string LabelName
        {
            get
            {
                return this.Label.Content.ToString();
            }
            set
            {
                this.Label.Content = value;
            }
        }

        public string Text {
            get
            {
                return this.TextBox.Text;
            }
            set
            {
                this.TextBox.Text = value;
            }
        }
        public bool IsUpperCase { get; set; }

        public TextPanel(MainWindow win, TemplateControl parentTemplate, string label)
            :base(win, parentTemplate)
        {
            InitializeComponent();
            LabelID = label;
            Init();
        }

        public override ElementControl GetCopy()
        {
            TextPanel copy = new TextPanel(mw, ParentTemplate, "lineCopy");
            copy.LabelName = this.LabelName;
            copy.Text = this.Text;
            copy.ControlObject = this.ControlObject;
            return copy;
        }

        private void Init()
        {
            NorneType = ElementType.TextPanel;
            ShortLabel = "txt";
            this.Label.Content = LabelID;
            IsUpperCase = false;
            //LabelName = this.Label.Content.ToString();
            //Text = this.TextBox.Text;
        }

        public void SetLabel(string label)
        {
            LabelName = label;
        }

        public void SetText(string text)
        {
            Text = text;
        }

        public override void LoadContent(JArray parameters)
        {
            this.SetLabel((string)parameters[0]);
            this.SetText((string)parameters[1]);
            if (parameters.Count == 3)
            {
                IsUpperCase = ((string)parameters[2] == "True") ? true : false;
            }
            else
            {
                IsUpperCase = false;
            }
            SetProperty();
        }

        private void Label_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string[] properties = { "LabelName", "Text", nameof(IsUpperCase), "ControlObject", "LabelID" };
            SetTargetProperties(properties);
            mw._propertyGrid.SelectedObject = this;
        }

        public override string GetUIParameters()
        {
            string upper = (IsUpperCase == true) ? "True" : "False";
            String ret = String.Format("[\"{0}\",\"{1}\", {2}]", this.Label.Content.ToString(), this.TextBox.Text.ToString(), upper);
            return ret;
        }

        public override List<string> GetContentCode()
        {
            List<string> code = new List<string>();
            code.Add(String.Format("\"{0}\", self.content[\"txt_{1}\"]", ControlObject, LabelID));
            return code;
        }

        public override void SetProperty()
        {
            this.Label.Content = ReplaceUnderlines(LabelName);
        }

    }
}
