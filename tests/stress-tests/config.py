import json


class Config:
    def __init__(self):
        with open('config.json') as f:
            self.config = json.load(f)

    def __getitem__(self, key):
        return self.config[key]

    def __setitem__(self, key, value):
        self.config[key] = value

    def __delitem__(self, key):
        del self.config[key]