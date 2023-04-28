import numpy as np
import cv2
import math
from pylibdmtx import pylibdmtx

def op1 ():
    img = cv2.imread('Datamatrix_3.jpg', cv2.IMREAD_UNCHANGED)    #load img

    #h, w, c = img.shape

    resize = cv2.resize(img, (500, 500), interpolation=cv2.INTER_LINEAR)
    gray = cv2.cvtColor(resize, cv2.COLOR_BGR2GRAY)

    ret, thresh = cv2.threshold(gray, 0, 255, cv2.THRESH_BINARY | cv2.THRESH_OTSU)

    msg = pylibdmtx.decode(thresh)
    print(msg)

    cv2.imshow('matrix', resize)         #sets type of display and content of display
    k = cv2.waitKey(0)              #waits for key press
    #print(k)                        #prints hexadecimal id of pressed key
    cv2.destroyAllWindows()         #closes window

def op2 ():
    cap = cv2.VideoCapture(0)

    while(True):
        ret, frame = cap.read()

        if cv2.waitKey(1) & 0xFF == ord('f'):

            gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)  #colour filter?

            #https://docs.opencv.org/4.x/d7/d4d/tutorial_py_thresholding.html
            ret, thresh = cv2.threshold(gray, 0, 255, cv2.THRESH_BINARY | cv2.THRESH_BINARY_INV | cv2.THRESH_OTSU)
            msg = pylibdmtx.decode(thresh)

            if msg:
                print(msg)
                break

        if cv2.waitKey(1) & 0xFF == ord('g'):

            gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)  #colour filter?

            ret, thresh = cv2.threshold(gray, 0, 255, cv2.THRESH_BINARY | cv2.THRESH_BINARY_INV | cv2.THRESH_OTSU)
            contours, hierarchy = cv2.findContours(thresh, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)

            cv2.imshow('frame', gray)

            for c in contours:
                cv2.drawContours(gray, c, -1, (255, 105, 180), 3)

        cv2.imshow('frame', frame)  # sets type of display and content of display
        if cv2.waitKey(1) & 0xFF == ord('q'):           #waits till 'q' is pressed
            break                                       #breaks out of while loop
    cap.release()

    cv2.destroyAllWindows()

op2()