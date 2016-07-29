import wx;
from norne.device import viz
from norne.template.base import tool
from norne.template.sky2016.util import BaseTable, TextFieldPanel
from norne.template.sky2016.baseinsert import SimpleBaseUI, SimpleBaseCtrl, MultiBaseCtrl, SimpleBaseGfx, MultiBaseGfx, \
    OneShotBaseCtrl, ToggleBaseCtrl


class TemplateNameUI(SimpleBaseUI):
    name = "TemplateName"
    label = "Label"

    def __init__(self, parent, wxid, project, *args, **kwargs):
        self.globals_dct.update(globals())
        SimpleBaseUI.__init__(self, parent, wxid, project, *args, **kwargs)

    def setup_control(self):
        self.btn_show = SimpleBaseCtrl(self, -1, "Label", self.project, get_content_callback=self.get_content,
                                       statemachine=self.statemachine)

    def setup_statemachine(self):
        self.statemachine = TemplateNameGfx(self.project)

    def create_ui(self):
        self.ctrl_obj.add_element("line3", "BaseTable", [self.project, [[wx.TextCtrl, None], [LogoAssetChoice, None],
                                                                        [wx.TextCtrl, BaseTable.TEAM]],
                                                         ["Column 1", "Logo", "Team"], 3])


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
        self.btn_show = SimpleBaseCtrl(self, -1, "Label000", self.project, get_content_callback=self.get_content,
                                       statemachine=self.statemachine)
        self.btn_show1 = SimpleBaseCtrl(self, -1, "Label111", self.project, get_content_callback=self.get_content,
                                        statemachine=self.statemachine1)

    def setup_statemachine(self):
        self.statemachine = ThatTemplateNameGfx(self.project)
        self.statemachine1 = ThatTemplateName1Gfx(self.project)

    def create_ui(self):
        self.ctrl_obj.add_element("line1", "TextFieldPanel,Label", [["LALA", "HAHA"], ["AIJOSDIAJS"]])
        self.ctrl_obj.add_element("line2", "TextFieldPanel", ["HA@@#1", "HAHA"])
        self.ctrl_obj.add_element("line3", "TextFieldPanel", ["HA@@#1", "HAHA"])
        pass


class ThatTemplateNameGfx(SimpleBaseGfx):
    def evaluate_content(self):
        self.scene_name = "This is a"
        self.custom_viz_dir = True

    def set_content(self):
        self.set_value("0001", self.content["txt_lin1_1"])
        self.set_value("0002", self.content["txt_line2_2"])
        self.set_value("0085", self.content["logo_line30_4"]["choice"])
        self.set_value("0090", self.content["logo_line30_4"]["logo"])
        self.set_value("0095", self.content["logo_line30_4"]["logo"])
        self.set_table_col(self.content["tbl_line2"], [1000, 1001], 10)
        self.set_value("0004", self.get_line_cnt(self.content["tbl_line2"], (1, 2), 1, 6))

        pass

    def setup_highlights(self):
        prefix = "H"
        lin_cnt = self.get_table_cnt(self.content["tbl_line3"], (), 1, 10)
        highlights = self.set_single_highlights(prefix, lin_cnt)
        self.highlights = {"onair": highlights}
        self.set_onair_highlights(self.content["tbl_line2"], 1, 2, lin_cnt)


class ThatTemplateName1Gfx(SimpleBaseGfx):
    def evaluate_content(self):
        self.scene_name = "This is a"

    def set_content(self):
        self.set_value("10000", self.content["txt_lin1_1"])
        self.set_value("0002", self.content["txt_line2_2"])
        self.set_value("0085", self.content["logo_line30_4"]["choice"])
        self.set_value("0090", self.content["logo_line30_4"]["logo"])
        self.set_value("0095", self.content["logo_line30_4"]["logo"])
        self.set_table_col(self.content["tbl_line2"], [1000, ], 10)
        self.set_value("0004", self.get_line_cnt(self.content["tbl_line2"], (1, 2), 1, 6))

        pass

    def setup_highlights(self):
        prefix = "H"
        lin_cnt = self.get_table_cnt(self.content["tbl_line3"], (), 1, 10)
        highlights = self.set_single_highlights(prefix, lin_cnt)
        self.highlights = {"onair": highlights}
        self.set_onair_highlights(self.content["tbl_line2"], 1, 2, lin_cnt)


class VBErg5Tab10UI(SimpleBaseUI):
    name = "VBErg5Tab10"
    label = "Label"

    def __init__(self, parent, wxid, project, *args, **kwargs):
        self.globals_dct.update(globals())
        SimpleBaseUI.__init__(self, parent, wxid, project, *args, **kwargs)

    def setup_control(self):
        self.btn_show = MultiBaseCtrl(self, -1, "Vb Erg Tab", self.project, get_content_callback=self.get_content,
                                      statemachine=self.statemachine)

    def setup_statemachine(self):
        self.statemachine = VBErg5Tab10Gfx(self.project)

    def create_ui(self):
        self.ctrl_obj.add_element("line1", "1|ToggleLogo,2|TextPanel,3|TextPanel,has_highlights!|CheckBox,5|CheckBox",
                                  [[True, ["logo_asset_manager", "diverse_asset_manager"], ], ["Titel", "", False],
                                   ["Zusatz", "", False], ["HL"], ["HG"], ])
        self.ctrl_obj.add_element("line2", "TextPanel", ["Untertitel", "", False])
        self.ctrl_obj.add_element("line6", "CheckBox", ["Tab Logo"])
        self.ctrl_obj.add_element("line3", "BaseTable", [self.project,
                                                         [[LogoAssetChoice, None], [wx.TextCtrl, BaseTable.TEAM],
                                                          [wx.TextCtrl, None], [wx.TextCtrl, BaseTable.TEAM],
                                                          [LogoAssetChoice, None], [wx.CheckBox, None]],
                                                         ["Logo Heim", "Team Heim", "Erg", "Team Gast", "Logo Gast",
                                                          "HL"],
                                                         5])
        self.ctrl_obj.add_element("line4", "BaseTable", [self.project,
                                                         [[wx.TextCtrl, None], [LogoAssetChoice, None],
                                                          [wx.TextCtrl, BaseTable.TEAM], [wx.TextCtrl, None],
                                                          [wx.TextCtrl, None], [wx.TextCtrl, None], [wx.TextCtrl, None],
                                                          [SelectionChoice, ["Kein", "Rot", ]], [wx.CheckBox, None]],
                                                         ["Rang", "Logo", "Team", "Spiele", "Siege", "Niederlagen",
                                                          "Pkt", "Relegation", "HL"],
                                                         10])
        self.ctrl_obj.add_element("line5", "1|TextPanel,2|CheckBox", [["Dropline", "", False], ["Sky HD Logo"], ])


class IBWetterUI(SimpleBaseUI):
    name = "IBWetter"
    label = "Label"

    def __init__(self, parent, wxid, project, *args, **kwargs):
        self.globals_dct.update(globals())
        SimpleBaseUI.__init__(self, parent, wxid, project, *args, **kwargs)

    def setup_control(self):
        self.btn_show = SimpleBaseCtrl(self, -1, "IB Wetter", self.project, get_content_callback=self.get_content,
                                       statemachine=self.statemachine)
        self.btn_show2 = ToggleBaseCtrl(self, -1, "CB Wetter", self.project, get_content_callback=self.get_content,
                                        statemachine=self.statemachine2)

    def setup_statemachine(self):
        self.statemachine = IBWetterGfx(self.project)
        self.statemachine2 = IBWetter2Gfx(self.project)

    def create_ui(self):
        self.ctrl_obj.add_element("line57", "6|ToggleLogo,2|TextPanel,3|TextPanel,4|Choice,has_highlights!|CheckBox",
                                  [[True, ["logo_asset_manager", "diverse_asset_manager"], ], ["Titel", "", True],
                                   ["Zusatz", "", True], [["Links", "Rechts", ]], ["HL"], ])
        self.ctrl_obj.add_element("line58", "TextPanel", ["Untertiel", "Wetterdaten", False])
        self.ctrl_obj.add_element("line59", "BaseTable", [self.project,
                                                          [[SelectionChoice,
                                                            ["Sonne", "Sonne Wolken", "Wechselhaft", "Leicht Bewoelkt",
                                                             "Stark Bewoelkt", "Regenwolken", "Regen", "Gewitter",
                                                             "Schnee", "Nacht Klar", "Nacht Bewoelkt", ]],
                                                           [wx.TextCtrl, None], [wx.TextCtrl, None],
                                                           [wx.CheckBox, None]],
                                                          ["Wetter", "Info", "Wert", "HL"],
                                                          3])
        self.ctrl_obj.add_additional_buttons(self.btn_show2, )


class IBWetterGfx(SimpleBaseGfx):
    def evaluate_content(self):
        self.scene_name = "IB_Wetter"
        self.has_highlights = True
        self.set_layer()

    def define_ctrl_plugin(self):
        self.ctrl_plugin = viz.VizGroup("object", self.scene)

    def set_content(self):
        self.set_value("0085", self.content["logo_line57_6"]["choice"])
        self.set_value("0090", self.content["logo_line57_6"]["logo"])
        self.set_value("0095", self.content["logo_line57_6"]["logo"])
        self.set_value("0100", self.content["txt_line57_2"])
        self.set_value("0102", self.content["txt_line57_3"])
        self.set_value("0002", self.content["cmb_line57_4"])
        self.set_value("", self.content["has_highlights"])
        self.set_value("0500", self.content["txt_line58"])
        self.set_table_col(self.content["tbl_line33"], [1101, 1102, 1104, 1106, 1107, -1, ], (0, 1))
        self.set_value("0004", self.get_line_cnt(self.content["tbl_line33"], (0, 1,), 2, 10))
        self.set_value("9000", self.content["txt_line12_1"])
        self.set_value("9010", self.content["chk_line12_2"])
        pass

    def setup_highlights(self):
        self.set_onair_highlights(self.content["tbl_line33"], 1, 5, (0, 1), "H")

class IBPlayerUI(SimpleBaseUI):
	name = "IBPlayer"
	label = "IB Spieler"
	def __init__(self, parent, wxid, project, *args, **kwargs):
		self.globals_dct.update(globals())
		SimpleBaseUI.__init__(self, parent, wxid, project, *args, **kwargs)
	def setup_control(self):
		self.btn_show = SimpleBaseCtrl(self, -1, "IB Spieler", self.project, get_content_callback=self.get_content, statemachine=self.statemachine)
	def setup_statemachine(self):
		self.statemachine = IBPlayerGfx(self.project)
	def create_ui(self):
		self.ctrl_obj.add_element("line10", "BaseTable", [self.project,
			[[CustomToggleChoice, None], [wx.TextCtrl, None], [wx.TextCtrl, BaseTable.PLAYER], [wx.TextCtrl, BaseTable.PLAYER], [FotoCheckBox, None]],
			["Logo" , "Nr" , "VORNAME" , "NACHNAME" , "Foto"],
			1])
		self.ctrl_obj.add_element("line13", "1|TextPanel,has_highlights!|CheckBox", [["Untertitel","", False],["HL"],])
		self.ctrl_obj.add_element("line12", "BaseTable", [self.project,
			[[wx.TextCtrl, None], [wx.CheckBox, None]],
			["Info", "HL"],
			7])

class IBPlayerGfx(SimpleBaseGfx):
	def evaluate_content(self):
		self.scene_name = "IB_SpielerPortrait"
		self.has_highlights= self.content.get("has_highlights")
	def define_ctrl_plugin(self):
		self.ctrl_plugin = viz.VizGroup("object", self.scene)
	def set_content(self):
		self.set_table_col(self.content["tbl_line10"], [90, -1, 100, 102, 600, ],(0,))
		self.set_value("0085", self.content["tbl_line10"][0][0]["choice"])
		self.set_value("0090", self.content["tbl_line10"][0][0]["logo"])
		self.set_value("0095", self.content["tbl_line10"][0][0]["logo"])
		self.set_value("0500", self.content["txt_line13_1"])
		self.set_table_col(self.content["tbl_line12"], [1102, ], (0, ))
		self.set_value("0004", self.get_line_cnt(self.content["tbl_line12"], (0,), 3, 7))
		pass
	def setup_highlights(self):
		self.set_onair_highlights(self.content["tbl_line12"], 0, 1, (0, ), "H")

class IBWetter2Gfx(SimpleBaseGfx):
    def evaluate_content(self):
        self.scene_name = "CB_Wetter"

    def define_ctrl_plugin(self):
        self.ctrl_plugin = viz.VizGroup("object", self.scene)

    def set_content(self):
        self.set_value("0085", self.content["logo_line57_6"]["choice"])
        self.set_value("0090", self.content["logo_line57_6"]["logo"])
        self.set_value("0095", self.content["logo_line57_6"]["logo"])
        self.set_value("0100", self.content["txt_line57_2"])
        self.set_value("0102", self.content["txt_line57_3"])
        self.set_value("0002", self.content["cmb_line57_4"])
        self.set_value("", self.content["has_highlights"])
        self.set_value("0500", self.content["txt_line58"])
        self.set_table_col(self.content["tbl_line33"], [1101, 1102, 1104, 1106, 1107, -1, ], (0, 1))
        self.set_value("0004", self.get_line_cnt(self.content["tbl_line33"], (0, 1,), 2, 10))
        self.set_value("9000", self.content["txt_line12_1"])
        self.set_value("9010", self.content["chk_line12_2"])
        pass

    def setup_highlights(self):
        self.set_onair_highlights(self.content["tbl_line33"], 1, 6, (), "E", 1, 1)
        self.set_onair_highlights(self.content["tbl_line3"], 3, 6, (1, 3), "T", 2, 2, 18)


class VBErg5Tab10Gfx(MultiBaseGfx):
    def evaluate_content(self):
        self.scene_name = "VB_Erg5_Tab10"
        self.has_highlights = self.content.get("has_highlights")
        self.continues_left = 1

    def define_ctrl_plugin(self):
        self.ctrl_plugin = viz.VizGroup("object", self.scene)

    def set_content(self):
        self.set_value("0085", self.content["logo_line1_1"]["choice"])
        self.set_value("0090", self.content["logo_line1_1"]["logo"])
        self.set_value("0095", self.content["logo_line1_1"]["logo"])
        self.set_value("0100", self.content["txt_line1_2"])
        self.set_value("0102", self.content["txt_line1_3"])
        self.set_value("", self.content["has_highlights"])
        self.set_value("0002", self.content["chk_line1_5"])
        self.set_value("0500", self.content["txt_line2"])
        self.set_value("0008", self.content["chk_line6"])
        self.set_table_col(self.content["tbl_line3"], [1101, 1102, 1104, 1106, 1107, -1, ], 5)
        self.set_value("0003", self.get_line_cnt(self.content["tbl_line3"], (0,), 1, 5))
        self.set_table_col(self.content["tbl_line4"], [2100, 2101, 2102, 2103, 2104, 2105, 2106, 2108, -1, ], 10)
        self.set_value("", self.get_line_cnt(self.content["tbl_line4"], (), 1, 10))
        self.set_value("9000", self.content["txt_line5_1"])
        self.set_value("9010", self.content["chk_line5_2"])
        pass


class ThisTemplateNameUI(SimpleBaseUI):
    name = "ThisTemplateNameUI"
    label = "Label"

    def __init__(self, parent, wxid, project, *args, **kwargs):
        self.globals_dct.update(globals())
        SimpleBaseUI.__init__(self, parent, wxid, project, *args, **kwargs)

    def setup_control(self):
        self.btn_show = SimpleBaseCtrl(self, -1, "Label", self.project, get_content_callback=self.get_content,
                                       statemachine=self.statemachine)

    def setup_statemachine(self):
        self.statemachine = ThisTemplateNameUIGfx(self.project)

    def create_ui(self):
        self.ctrl_obj.add_element("line1", "Label,Text", [["line1"], [""]])


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
        self.btn_show = SimpleBaseCtrl(self, -1, "Label", self.project, get_content_callback=self.get_content,
                                       statemachine=self.statemachine)

    def setup_statemachine(self):
        self.statemachine = CompleteNewUIGfx(self.project)

    def create_ui(self):
        self.ctrl_obj.add_element("line1", "Label,Text", [["line1"], [""]])
        self.ctrl_obj.add_element("line2", "Label,Text", [["line2"], [""]])


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
        self.btn_show = SimpleBaseCtrl(self, -1, "Label", self.project, get_content_callback=self.get_content,
                                       statemachine=self.statemachine)

    def setup_statemachine(self):
        self.statemachine = NameUIGfx(self.project)

    def create_ui(self):
        self.ctrl_obj.add_element("line1", "Label,Text", [["line1"], [""]])


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
        self.btn_show = SimpleBaseCtrl(self, -1, "Label", self.project, get_content_callback=self.get_content,
                                       statemachine=self.statemachine)

    def setup_statemachine(self):
        self.statemachine = NewSceneGfx(self.project)

    def create_ui(self):
        self.ctrl_obj.add_element("line1", "Label,Text", [["line1"], [""]])
        self.ctrl_obj.add_element("line2", "Label,Text", [["line2"], [""]])
        self.ctrl_obj.add_element("line3", "Label,Text", [["line3"], [""]])
        self.ctrl_obj.add_element("line4", "Label,Text", [["line4"], [""]])
        self.ctrl_obj.add_element("line5", "BaseTable",
                                  [self.project, [[wx.TextCtrl, None], [wx.TextCtrl, None], [wx.TextCtrl, None]],
                                   ["Column 1", "Column 2", "Column 3"], 3])


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
