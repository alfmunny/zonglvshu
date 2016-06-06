import ast
import sys


class TemplateParser(ast.NodeVisitor):
    def __init__(self, source):
        self.templates = []
        self.visit(source)

    def visit_ClassDef(self, node):
        self.templates.append(node.name)
        return self.templates


class ImportParser(ast.NodeVisitor):
    def __int__(self):
        pass

    def visit_Import(self, node):
        for x in node.names:
            print x.name

    def visit_ImportFrom(self, node):
        for x in node.names:
            print x.name

'''
def test():
    node = ast.parse(source)
    for n in ast.walk(node):
        if isinstance(n, ast.ClassDef):
            print(n.name)
'''


def main(file):
    source = file.read()
    node = ast.parse(source)
    p = TemplateParser(node)
    print p.templates
    return p.templates


if __name__ == "__main__":
    f = open(sys.argv[1], 'r')
    main(f)



