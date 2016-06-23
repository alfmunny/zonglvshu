import wx;
from norne.device import viz
from norne.template.base import tool
from norne.template.sky2016.util import BaseTable, TextFieldPanel
from norne.template.sky2016.baseinsert import SimpleBaseUI, SimpleBaseCtrl, MultiBaseCtrl, SimpleBaseGfx, MultiBaseGfx, OneShotBaseCtrl, ToggleBaseCtrl
class TemplateNameUI(SimpleBaseUI):
   name = "TemplateName"
   label = "Label"
   def __init__(self, parent, wxid, project, *args, **kwargs):
       self.globals_dct.update(globals())
       SimpleBaseUI.__init__(self, parent, wxid, project, *args, **kwargs)
   def setup_control(self):
       self.btn_show = SimpleBaseCtrl(self, -1, "Label", self.project, get_content_callback=self.get_content, statemachine=self.statemachine)
   def setup_statemachine(self):
       self.statemachine = TemplateNameGfx(self.project)
   def create_ui(self):
       self.ctrl_obj.add_element("line3", "BaseTable", [self.project, [[wx.TextCtrl, None], [LogoAssetChoice, None], [wx.TextCtrl, BaseTable.TEAM]], ["Column 1" , "Logo" , "Team"], 3])

class TemplateNameGfx(SimpleBaseGfx):
   def evaluate_content(self):
       self.scene_name = "Right"
   def set_content(self):
       pass

class ThatTemplateNameUI(SimpleBaseUI):
   name = "ThatTemplateName"
   label = "Label"
   def __init__(self, parent, wxid, project, *args, **kwargs):
       self.globals_dct.update(globals())
       SimpleBaseUI.__init__(self, parent, wxid, project, *args, **kwargs)
   def setup_control(self):
       self.btn_show = SimpleBaseCtrl(self, -1, "Label", self.project, get_content_callback=self.get_content, statemachine=self.statemachine)
   def setup_statemachine(self):
       self.statemachine = ThatTemplateNameGfx(self.project)
   def create_ui(self):
       self.ctrl_obj.add_element("line1", "TextFieldPanel,Label", [["LALA", "HAHA"],["AIJOSDIAJS"]])
       self.ctrl_obj.add_element("line2", "TextFieldPanel", ["HA@@#1", "HAHA"])
       self.ctrl_obj.add_element("line3", "TextFieldPanel", ["HA@@#1", "HAHA"])
       pass
class ThatTemplateNameGfx(SimpleBaseGfx):
   def evaluate_content(self):
       self.scene_name = "This is a"
   def set_content(self):
       self.set_value("0001", self.content["txt_lin1_1"])
       self.set_value("0002", self.content["txt_line2_2"])
       self.set_table_col(self.content["tbl_line2"], [1000, ], 10)

       pass
class ThisTemplateNameUI(SimpleBaseUI):
   name = "ThisTemplateNameUI"
   label = "Label"
   def __init__(self, parent, wxid, project, *args, **kwargs):
       self.globals_dct.update(globals())
       SimpleBaseUI.__init__(self, parent, wxid, project, *args, **kwargs)
   def setup_control(self):
       self.btn_show = SimpleBaseCtrl(self, -1, "Label", self.project, get_content_callback=self.get_content, statemachine=self.statemachine)
   def setup_statemachine(self):
       self.statemachine = ThisTemplateNameUIGfx(self.project)
   def create_ui(self):
       self.ctrl_obj.add_element("line1", "Label,Text", [["line1"],[""]])
class ThisTemplateNameUIGfx(SimpleBaseGfx):
   def evaluate_content(self):
       self.scene_name = ""
   def set_content(self):
       self.set_value("", self.content["txt_line1"])
       pass
class CompleteNewUI(SimpleBaseUI):
   name = "CompleteNewUI"
   label = "Label"
   def __init__(self, parent, wxid, project, *args, **kwargs):
       self.globals_dct.update(globals())
       SimpleBaseUI.__init__(self, parent, wxid, project, *args, **kwargs)
   def setup_control(self):
       self.btn_show = SimpleBaseCtrl(self, -1, "Label", self.project, get_content_callback=self.get_content, statemachine=self.statemachine)
   def setup_statemachine(self):
       self.statemachine = CompleteNewUIGfx(self.project)
   def create_ui(self):
       self.ctrl_obj.add_element("line1", "Label,Text", [["line1"],[""]])
       self.ctrl_obj.add_element("line2", "Label,Text", [["line2"],[""]])
class CompleteNewUIGfx(SimpleBaseGfx):
   def evaluate_content(self):
       self.scene_name = ""
   def set_content(self):
       self.set_value("", self.content["txt_line1"])
       self.set_value("", self.content["txt_line2"])
       pass
class NameUI(SimpleBaseUI):
   name = "NameUI"
   label = "Label"
   def __init__(self, parent, wxid, project, *args, **kwargs):
       self.globals_dct.update(globals())
       SimpleBaseUI.__init__(self, parent, wxid, project, *args, **kwargs)
   def setup_control(self):
       self.btn_show = SimpleBaseCtrl(self, -1, "Label", self.project, get_content_callback=self.get_content, statemachine=self.statemachine)
   def setup_statemachine(self):
       self.statemachine = NameUIGfx(self.project)
   def create_ui(self):
       self.ctrl_obj.add_element("line1", "Label,Text", [["line1"],[""]])
class NameUIGfx(SimpleBaseGfx):
   def evaluate_content(self):
       self.scene_name = ""
   def set_content(self):
       self.set_value("", self.content["txt_line1"])
       pass
class NewSceneUI(SimpleBaseUI):
   name = "NewScene"
   label = "Label"
   def __init__(self, parent, wxid, project, *args, **kwargs):
       self.globals_dct.update(globals())
       SimpleBaseUI.__init__(self, parent, wxid, project, *args, **kwargs)
   def setup_control(self):
       self.btn_show = SimpleBaseCtrl(self, -1, "Label", self.project, get_content_callback=self.get_content, statemachine=self.statemachine)
   def setup_statemachine(self):
       self.statemachine = NewSceneGfx(self.project)
   def create_ui(self):
       self.ctrl_obj.add_element("line1", "Label,Text", [["line1"],[""]])
       self.ctrl_obj.add_element("line2", "Label,Text", [["line2"],[""]])
       self.ctrl_obj.add_element("line3", "Label,Text", [["line3"],[""]])
       self.ctrl_obj.add_element("line4", "Label,Text", [["line4"],[""]])
       self.ctrl_obj.add_element("line5", "BaseTable", [self.project, [[wx.TextCtrl, None], [wx.TextCtrl, None], [wx.TextCtrl, None]], ["Column 1" , "Column 2" , "Column 3"], 3])

class NewSceneGfx(SimpleBaseGfx):
   def evaluate_content(self):
       self.scene_name = ""
   def set_content(self):
       self.set_value("", self.content["txt_line1"])
       self.set_value("", self.content["txt_line2"])
       self.set_value("", self.content["txt_line3"])
       self.set_value("", self.content["txt_line4"])
       self.set_table_value(0, self.content["tbl_line5"])
       pass

