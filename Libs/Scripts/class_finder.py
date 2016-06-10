import ast
import sys


class ClassFinder(ast.NodeVisitor):
    def __init__(self, source, class_name):
        self.class_name = class_name
        self.results = []
        self.visit(source)

    def visit_ClassDef(self, node):
        if node.name == class_name:
            self.results.append([node.lineno, node.body[-1].body[-1].lineno])


def main(f, class_name):
    source = f.read()
    node = ast.parse(source)
    p = ClassFinder(node, class_name)
    if p.results:
        print p.results[0]


if __name__ == "__main__":
    f = open(sys.argv[1], 'r')
    class_name = sys.argv[2]
    main(f, class_name)
    f.close()
