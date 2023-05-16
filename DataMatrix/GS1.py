import json
temp = []
def setJson(data):
    string = data.decode()
    j = json.dumps({'GS1Model': string})
    return j
    #pass