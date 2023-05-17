import json

GS1 = []


def clearGS1():
    GS1.clear()


def setGS1(data):
    string = data.decode()
    GS1.append({"Data": string})


def setJson():
    data = {"GS1": GS1}
    return json.dumps(data)
