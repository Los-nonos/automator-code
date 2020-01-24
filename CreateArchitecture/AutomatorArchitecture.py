import os

class AutomatorArchitecture():
    def __init__(self, path):
        self.path = path

    def create_architecture_hex(self):
        os.mkdir(self.path +'/src')
        self.path = self.path + '/src'
        os.chdir(self.path)
        os.mkdir(self.path + '/Infraestructure')