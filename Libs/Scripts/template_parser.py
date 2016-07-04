import ast
import sys
import json
import os


class TemplateParser(ast.NodeVisitor):
    def __init__(self, source, class_name):
        self.class_name = class_name
        self.results = {
            "ui_class_name": class_name,
            "template_name": "",
            "gfx_class_name": "",
            "parent_class": "",
            "parent_control": "",
            "parent_gfx": "",
            "template_label": "",
            "control": "",

            "scene_name": "",
            "preframe_default": "",
            "continues_left": "",
            "has_highlights": False,
            "highlight_prefix": "",
            "content": [],
            "ui": [],
        }
        self.visit(source)

    def visit_ClassDef(self, node):
        is_found = False
        if node.name == class_name:
            is_found = True
            self.results["parent_class"] = node.bases[0].id
            for x in node.body:
                if isinstance(x, ast.Assign) and x.targets[0].id == "name":
                    self.results["template_name"] = x.value.s
                if isinstance(x, ast.Assign) and x.targets[0].id == "label":
                    self.results["template_label"] = x.value.s
                if isinstance(x, ast.FunctionDef) and x.name == "create_ui":
                    self.get_ui(x)
                if isinstance(x, ast.FunctionDef) and x.name == "setup_statemachine":
                    self.get_statemachine(x)
                if isinstance(x, ast.FunctionDef) and x.name == "setup_control":
                    self.get_control(x)

        elif node.name == class_name[0:-2] + "Gfx":
            is_found = True
            self.get_gfx(node)

    def export_json(self):
        path = os.path.dirname(__file__) + '/template.json'
        with open(path, 'w') as output:
            json.dump(self.results, output, indent=4, separators=(',', ': '))

    def get_ui(self, node):
        ui_list = self.results['ui']
        for ui in node.body:
            if isinstance(ui, ast.Pass):
                return
            ui_args = {}
            args = ui.value.args
            ui_args['label_id'] = self.args_parser(args[0])
            ui_args['elements'] = self.elements_parser(args[1])
            if len(ui_args['elements']):
                ui_args['parameters'] = [self.args_parser(args[2])]
            else:
                ui_args['parameters'] = self.args_parser(args[2])
            ui_list.append(ui_args)

    def get_statemachine(self, node):
        self.results["gfx_class_name"] = node.body[0].value.func.id

    def get_gfx(self, node):
        content_list = self.results['content']
        self.results["parent_gfx"] = node.bases[0].id
        # attributes for table
        self.node_set_lincnt = None
        self.lin_select = None

        for x in node.body:
            self.evaluate_content_parser(x)
            self.set_content_parser(x, content_list)
            self.setup_highlights_parser(x)

    def get_control(self, node):
        self.results["parent_control"] = node.body[0].value.func.id

    def evaluate_content_parser(self, x):
        if isinstance(x, ast.FunctionDef) and x.name == "evaluate_content":
            for xx in x.body:
                if isinstance(xx, ast.Assign) and xx.targets[0].attr == "scene_name":
                    self.results["scene_name"] = xx.value.s

    def set_content_parser(self, x, content_list):
        if isinstance(x, ast.FunctionDef) and x.name == "set_content":
            for xx in x.body:
                content_arg = {}
                self.set_value_parser(xx, content_arg)
                self.set_table_col_parser(xx, content_arg)
                if isinstance(xx, ast.Pass):
                    break
                else:
                    pass

                if content_arg:
                    content_list.append(content_arg)

            if self.node_set_lincnt:
                args = self.node_set_lincnt.args
                element = args[0].slice.value.s
                label_id = '_'.join(element.split('_')[1:])
                for c in self.results['content']:
                    if c['label_id'] == label_id:
                        lst = args[1].elts
                        c['start_select'] = args[2].n
                        c['end_select'] = args[3].n
                        c['must_filled'] = [x.n for x in lst]
                        c['line_select'] = self.lin_select

    def setup_highlights_parser(self, x):
        if isinstance(x, ast.FunctionDef) and x.name == "setup_highlights":
            self.results["has_highlights"] = True
            for xx in x.body:
                if isinstance(xx, ast.Assign) and isinstance(xx.targets[0], ast.Name) and xx.targets[0].id == "prefix":
                    self.results["highlight_prefix"] = xx.value.s
                if isinstance(xx, ast.Expr):
                    if isinstance(xx.value, ast.Call) and xx.value.func.attr == "set_onair_highlights":
                        args = xx.value.args
                        subscript = args[0]
                        caption_index = args[1].n
                        chk_index = args[2].n
                        name = subscript.slice.value.s
                        ele, label_id = name.split('_')[0:2]
                        for r in self.results["content"]:
                            if r["label_id"] == label_id and r["element"] == "tbl":
                                r["has_highlights"] = True
                                r["caption_index"] = caption_index
                                r["chk_index"] = chk_index

    def get_line_cnt_parser(self, x):
        if isinstance(x[1], ast.Call) and x[1].func.attr == "get_line_cnt":
            self.node_set_lincnt = x[1]
            self.lin_select = x[0].s

    def set_value_parser(self, x, content_arg):
        if isinstance(x, ast.Expr) and x.value.func.attr == "set_value":
            args = x.value.args
            if isinstance(args[1], ast.Call):
                self.get_line_cnt_parser(args)
            else:
                self.set_value_parameters_parser(args, content_arg)

    def set_value_parameters_parser(self, x, content_arg):
        content_arg['control_object'] = x[0].s
        element = x[1].slice.value.s
        content_arg['element'] = element.split('_')[0]
        content_arg['label_id'] = '_'.join(element.split('_')[1:])

    def set_table_col_parser(self, x, content_arg):
        if isinstance(x, ast.Expr) and x.value.func.attr == "set_table_col" :
            args = x.value.args
            element = args[0].slice.value.s
            lst = args[1].elts
            row = args[2].n

            content_arg['element'] = element.split('_')[0]
            content_arg['label_id'] = '_'.join(element.split('_')[1:])
            content_arg['row'] = row
            content_arg['col_fields'] = [x.n for x in lst]
            content_arg['start_select'] = 1
            content_arg['end_select'] = 10
            content_arg['must_filled'] = []
            content_arg['line_select'] = ""
            content_arg['has_highlights'] = False
            content_arg['caption_index'] = 0
            content_arg['chk_index'] = 0

    def args_parser(self, node):
        if isinstance(node, ast.Str):
            return node.s
        elif isinstance(node, ast.Num):
            return node.n
        elif isinstance(node, ast.Name):
            return node.id
        elif isinstance(node, ast.Attribute):
            # for BaseTable's parameters
            return node.value.id + '.' + node.attr
        elif isinstance(node, ast.List):
            res = []
            for x in node.elts:
                res.append(self.args_parser(x))
            return res

    def elements_parser(self, ast_str):
        return ast_str.s.split(',')


def main(f, class_name):
    source = f.read()
    node = ast.parse(source)
    p = TemplateParser(node, class_name)
    p.export_json()

if __name__ == "__main__":
    f = open(sys.argv[1], 'r')
    class_name = sys.argv[2]
    main(f, class_name)
    f.close()