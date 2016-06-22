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

        if is_found:
            self.export_json()

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
        self.results["parent_gfx"] = node.bases[0].id
        for x in node.body:
            if isinstance(x, ast.FunctionDef) and x.name == "evaluate_content":
                for xx in x.body:
                    if isinstance(xx, ast.Assign) and xx.targets[0].attr == "scene_name":
                        self.results["scene_name"] = xx.value.s

    def get_control(self, node):
        self.results["parent_control"] = node.body[0].value.func.id

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

    def elements_parser(self, astStr):
        return astStr.s.split(',')


def main(f, class_name):
    source = f.read()
    node = ast.parse(source)
    p = TemplateParser(node, class_name)


if __name__ == "__main__":
    f = open(sys.argv[1], 'r')
    class_name = sys.argv[2]
    main(f, class_name)
    f.close()