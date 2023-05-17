import numpy as np
import cv2
import math
from pylibdmtx import pylibdmtx
import pyrealsense2 as rs

import GS1


def op1():

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


def op2():
    realsense = False
    Windows = True
    firstFrame = True
    dim = ()

    if realsense:
        cap = cv2.VideoCapture(2)
        pipe = rs.pipeline()
        config = rs.config()

        config.enable_stream(rs.stream.depth, 640, 480, rs.format.z16, 30)

        pipe.start(config)

    else: cap = cv2.VideoCapture(0)

    while(True):
        ret, frame = cap.read()

        if firstFrame:
            dim = frame.shape
            print("Width:", dim[1])
            firstFrame = False

        if realsense:
            frame2 = pipe.wait_for_frames()
            depth = frame2.get_depth_frame()
            #color = frame2.get_color_frame()

            depth2 = np.asanyarray(depth.get_data())
            #if color: color2 = np.asanyarray(color.get_data())

            depth_colormap = cv2.applyColorMap(cv2.convertScaleAbs(depth2, alpha=0.03), cv2.COLORMAP_JET)
            #color_colormap_dim = color.shape

        gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
        #gray_blur = cv2.boxFilter(gray, -1, (3, 3))
        #gray_blur = cv2.GaussianBlur(gray, (3, 3), 0)
        ret, thresh = cv2.threshold(gray, 50, 255, cv2.THRESH_BINARY | cv2.THRESH_BINARY_INV | cv2.THRESH_OTSU)

        if False:
            contours, hierarchy = cv2.findContours(thresh, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
            cv2.drawContours(frame, contours, -1, (0, 0, 0), 3)   #draws contours for anything the camera sees

        if cv2.waitKey(1) & 0xFF == ord('f'):   #decode GS1-Matrix if found, works with the found thresholds
            # https://docs.opencv.org/4.x/d7/d4d/tutorial_py_thresholding.html

            msg = pylibdmtx.decode(thresh)  # kost veel computer power

            if msg:
                GS1.clearGS1()
                for m in msg:
                    print(m[0])     #data in bytes
                    GS1.setGS1(m[0])

                if Windows:
                    file = open(r"C:\Users\Public\file.txt", "w")
                    file.write(GS1.setJson())
                    file.close()
                else:
                    #path = Path(__file__).parent / ""
                    #file = open(r"$HOME/", "w")
                    #file = open(r"~/home", "w")
                    file = open(r"/home/jetson/log/GS1/current_frame.log", "w")
                    file.write(GS1.setJson())
                    file.close()
            else: print('Nothing detected')


        elif cv2.waitKey(1) & 0xFF == ord('g') or True:   #draws contours for anything the program identifies as 'box' (W.I.P.)
            # https://www.tutorialspoint.com/how-to-detect-a-rectangle-and-square-in-an-image-using-opencv-python
            #blur = cv2.GaussianBlur(gray, (7, 7), 0)
            #blur2 = cv2.boxFilter(gray, -1, (9, 9))

            #region Masking
            hls = cv2.cvtColor(frame, cv2.COLOR_BGR2HLS)
            Lchannel = hls[:, :, 1]

            mask = cv2.inRange(Lchannel, 180, 255)
            res = cv2.bitwise_and(frame, frame, mask=mask)

            mask_blur = cv2.boxFilter(mask, -1, (5, 5))

            res_gray = cv2.cvtColor(res, cv2.COLOR_BGR2GRAY)
            res_blur = cv2.boxFilter(res_gray, -1, (9, 9))
            #endregion

            #region Contour detection
            #contrast = 3 * blur2 + 175

            ret, thresh = cv2.threshold(mask_blur, 180, 255, cv2.THRESH_BINARY | cv2.THRESH_BINARY_INV | cv2.THRESH_OTSU)
            # werkt alleen met grayscaled img
            contours, hierarchy = cv2.findContours(thresh, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)

            #endregion

            boxes = []
            boxes_count = 0

            for c in range(len(contours)):
                # childeren = hierarchy[c][2]

                # print(childeren)

                # if perimeter > 0: vormfactor = (4 * math.pi * area) / perimeter ** 2
                # if area > 500 and area < 1000 and vormfactor > 0.3:
                # con = contours[c]

                area = cv2.contourArea(contours[c])
                perimeter = cv2.arcLength(contours[c], True)
                approx = cv2.approxPolyDP(contours[c], 0.01 * perimeter, True)

                #cv2.drawContours(frame, contours, -1, (0, 0, 0), 3)  # draws contours for anything the camera sees
                if (len(approx) >= 4 and len(approx) <= 6 and area > 3000 and area < 200000):# or childeren > 0:
                    #cv2.drawContours(frame, [contours[c]], -1, (255, 105, 180), 3)
                    boxes.append(contours[c])
                    boxes_count = boxes_count + 1

                    #x, y, w, h = cv2.boundingRect(contours[c])

                    #print('X: ', x + w / 2, dim[1] / 2)
                    #print('Y: ', y + h / 2, dim[0] / 2)

                    #cropped = frame[y:h+y, x:w+x]
                    #print(x, w, y, h)
                    #print(x:w, y:h)
                    #cv2.imshow('cropped', cropped)

                    #cv2.putText(frame, "Medicijndoosje", (x, y), cv2.FONT_HERSHEY_COMPLEX, 1, (255, 105, 180))

            for b in boxes:
                cv2.drawContours(frame, [b], -1, (255, 105, 180), 3)
                x, y, w, h = cv2.boundingRect(b)

                X = x + w / 2
                # Y = y + h / 2

                if X < dim[1] / 2:
                    #dif = (dim[1] / 2) - X  # 0 - 319
                    dif = X / 320
                    print("Left", "dif:", dif)
                    pass
                elif X > dim[1] / 2:
                    #dif = X - (dim[1] / 2)  # 321 - 640
                    dif = (dim[1] - X) / 320
                    print("Right", "dif:", dif)
                    pass

                #print(len(range(boxes_count)))

                cv2.putText(frame, "Medicijndoosje", (x, y), cv2.FONT_HERSHEY_COMPLEX, 1, (255, 105, 180))

                # string = 'cropped' + str(boxes_count)
                # cropped = frame[y:h + y, x:w + x]
                # cv2.imshow('cropped' + str(boxes_count), cropped)

            # Edge detection
            #edges = cv2.Canny(mask_blur, 100, 200, 90)

            #cv2.imshow('blur', blur2)
            #cv2.imshow('edge', edges)
            #cv2.imshow('res', res)
            cv2.imshow('mask', mask_blur)
            cv2.imshow('frame', frame)
            if realsense: cv2.imshow('realsense', depth_colormap)
            #if color: cv2.imshow('realsense2', color2)
            #continue

        else:
            cv2.imshow('frame', frame)  # sets type of display and content of display

        if cv2.waitKey(1) & 0xFF == ord('q'):           #waits till 'q' is pressed
            break                                       #breaks out of while loop

    #region Shutdown
    cap.release()
    cv2.destroyAllWindows()

    #d = open('file.txt', 'w')
    #d.write('')
    #d.close()
    #endregion

op2()