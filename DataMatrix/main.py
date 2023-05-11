import numpy as np
import cv2
import math
from pylibdmtx import pylibdmtx
#import pyrealsense2    #doesn't work
#from 'C:\Users\marte\appdata\local\packages\pythonsoftwarefoundation.python.3.10_qbz5n2kfra8p0\localcache\local-packages\python310\site-packages\pyrealsense2' import __
# Project Python version is 3.11, last supported Python version vor pyrealsense2 is 3.10

def op1 ():

    img = 1#cv2.imread('C:\Users\Public\Pictures\Datamatrix_3.jpg', cv2.IMREAD_UNCHANGED)    #load img    #dont work

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
        #contours, hierarchy = cv2.findContours(thresh, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)

        #if True: cv2.drawContours(frame, contours, -1, (0, 0, 0), 3)   #draws contours for anything the camera sees

        if cv2.waitKey(1) & 0xFF == ord('f'):   #decode GS1-Matrix if found, works with the found thresholds
            # https://docs.opencv.org/4.x/d7/d4d/tutorial_py_thresholding.html

            cv2.imshow('frame', frame)

            msg = pylibdmtx.decode(thresh)

            if msg:
                print(msg)
                for m in msg:
                    print(m[0])
                break
            #continue


        elif cv2.waitKey(1) & 0xFF == ord('g') or True:   #draws contours for anything the program identifies as 'box' (W.I.P.)
            # https://www.tutorialspoint.com/how-to-detect-a-rectangle-and-square-in-an-image-using-opencv-python
            #blur = cv2.GaussianBlur(gray, (7, 7), 0)
            #blur2 = cv2.boxFilter(gray, -1, (9, 9))

            #Masking
            hls = cv2.cvtColor(frame, cv2.COLOR_BGR2HLS)
            #lower = np.array([0, 0, 155])
            #upper = np.array([255, 100, 255])
            Lchannel = hls[:, :, 1]

            mask = cv2.inRange(Lchannel, 180, 255)
            res = cv2.bitwise_and(frame, frame, mask=mask)

            mask_blur = cv2.boxFilter(mask, -1, (5, 5))

            res_gray = cv2.cvtColor(res, cv2.COLOR_BGR2GRAY)
            res_blur = cv2.boxFilter(res_gray, -1, (9, 9))

            #Contour detection
            #contrast = 3 * blur2 + 175

            ret, thresh = cv2.threshold(mask_blur, 180, 255, cv2.THRESH_BINARY | cv2.THRESH_BINARY_INV | cv2.THRESH_OTSU)
            # werkt alleen met grayscaled img
            contours, hierarchy = cv2.findContours(thresh, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)

            cv2.drawContours(frame, contours, -1, (0, 0, 0), 3)  # draws contours for anything the camera sees

            for c in range(len(contours)):

                area = cv2.contourArea(contours[c])
                perimeter = cv2.arcLength(contours[c], True)
                approx = cv2.approxPolyDP(contours[c], 0.01 * perimeter, True)

                #childeren = hierarchy[c][2]

                #print(childeren)

                #if perimeter > 0: vormfactor = (4 * math.pi * area) / perimeter ** 2
                #if area > 500 and area < 1000 and vormfactor > 0.3:
                if (len(approx) >= 4 and len(approx) <= 6 and area > 3000 and area < 300000):# or childeren > 0:
                    cv2.drawContours(frame, [contours[c]], -1, (255, 105, 180), 3)
                    print(area)

            # Edge detection
            edges = cv2.Canny(mask_blur, 100, 200, 90)

            #cv2.imshow('blur', blur2)
            cv2.imshow('edge', edges)
            cv2.imshow('res', res)
            cv2.imshow('mask', mask_blur)
            cv2.imshow('frame', frame)
            #continue

        else:
            cv2.imshow('frame', frame)  # sets type of display and content of display

        if cv2.waitKey(1) & 0xFF == ord('q'):           #waits till 'q' is pressed
            break                                       #breaks out of while loop
    cap.release()

    cv2.destroyAllWindows()

op2()