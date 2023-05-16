import json

def setJson(data):
    string = data.decode()
    j = json.dumps(string)
    return j
    #pass