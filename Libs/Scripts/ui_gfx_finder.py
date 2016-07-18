import ast
import sys

class UIFinder(ast.NodeVisitor):
    def __init__(self, source, class_name):
        self.class_name = class_name
        self.results = []
        self.gfx = []
        self.visit(source)

    def visit_ClassDef(self, node):
        if node.name == self.class_name:
            for x in node.body:
                if isinstance(x, ast.FunctionDef) and x.name == "setup_statemachine":
                    self.gfx = self.get_statemachine(x)
            self.results.append([node.lineno, node.body[-1].body[-1].lineno])

    def get_statemachine(self, node):
        return [x.value.func.id for x in node.body]

class GfxFinder(ast.NodeVisitor):
    def __init__(self, source, class_name):
        self.class_name = class_name
        self.results = []
        self.gfx = []
        self.visit(source)

    def visit_ClassDef(self, node):
        if node.name == self.class_name:
            self.results.append([node.lineno, node.body[-1].body[-1].lineno])

def main(f, class_name):
    source = f.read()
    node = ast.parse(source)
    p = UIFinder(node, class_name)
    x = GfxFinder(node, p.gfx[-1])

    if p.results:
        print [p.results[0][0], x.results[0][-1]]

if __name__ == "__main__":
    f = open(sys.argv[1], 'r')
    class_name = sys.argv[2]
    main(f, class_name)
    f.close()