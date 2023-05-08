import numpy as np
import cv2
import math
from pylibdmtx import pylibdmtx
#import pyrealsense2    #doesn't work

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
        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        ret, thresh = cv2.threshold(gray, 50, 255, cv2.THRESH_BINARY | cv2.THRESH_BINARY_INV | cv2.THRESH_OTSU)
        contours, hierarchy = cv2.findContours(thresh, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)

        if False: cv2.drawContours(frame, contours, -1, (0, 0, 0), 3)   #draws contours for anything the camera sees

        if cv2.waitKey(1) & 0xFF == ord('f'):   #decode GS1-Matrix if found, works with the found thresholds
            # https://docs.opencv.org/4.x/d7/d4d/tutorial_py_thresholding.html

            cv2.imshow('frame', frame)

            msg = pylibdmtx.decode(thresh)

            if msg:
                print(msg)
                break
                #continue


        elif cv2.waitKey(1) & 0xFF == ord('g'):   #draws contours for anything the program identifies as 'box' (W.I.P.)
            # https://www.tutorialspoint.com/how-to-detect-a-rectangle-and-square-in-an-image-using-opencv-python
            blur = cv2.GaussianBlur(gray, (25, 25), 0)
            contrast = 5 * blur + 200

            ret, thresh = cv2.threshold(contrast, 50, 255, cv2.THRESH_BINARY | cv2.THRESH_BINARY_INV | cv2.THRESH_OTSU)
            contours, hierarchy = cv2.findContours(thresh, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)

            for c in contours:

                area = cv2.contourArea(c)
                perimeter = cv2.arcLength(c, True)
                approx = cv2.approxPolyDP(c, 0.01 * perimeter, True)

                #if perimeter > 0: vormfactor = (4 * math.pi * area) / perimeter ** 2
                #if area > 500 and area < 1000 and vormfactor > 0.3:
                if len(approx) == 4 and area > 100:
                    cv2.drawContours(contrast, [c], -1, (255, 105, 180), 3)

            cv2.imshow('frame', contrast)
            #continue

        else:
            cv2.imshow('frame', frame)  # sets type of display and content of display

        if cv2.waitKey(1) & 0xFF == ord('q'):           #waits till 'q' is pressed
            break                                       #breaks out of while loop
    cap.release()

    cv2.destroyAllWindows()

op2()