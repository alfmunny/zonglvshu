import ast
import sys
import json
import os


class TemplateParser(ast.NodeVisitor):
    def __init__(self, source, class_name):
        self.class_name = class_name
        self.results = {
            "name": "",
            "label": "",
            "statemachine": "",
            "control": "",
            "ui": [],
            "content": [],

        }
        self.visit(source)

    def visit_ClassDef(self, node):
        if node.name == class_name:
            for x in node.body:
                if isinstance(x, ast.FunctionDef) and x.name == "create_ui":
                    self.get_ui(x)
                if isinstance(x, ast.FunctionDef) and x.name == "setup_statemachine":
                    pass
                if isinstance(x, ast.FunctionDef) and x.name == "setup_control":
                    pass

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

    def get_statemachine(self):
        pass

    def get_control(self):
        pass

    def args_parser(self, node):
        if isinstance(node, ast.Str):
            return node.s
        if isinstance(node, ast.Num):
            return node.n
        if isinstance(node, ast.List):
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