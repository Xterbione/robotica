import numpy as np
import cv2
import math
from pylibdmtx import pylibdmtx
import datetime
import pyrealsense2 as rs

import main

frameCount = 0

currentDate = str(datetime.date.today())
path = ""

def innit():
    main.path = "C:/Users/Public/" + currentDate + ".txt"

    file = open(main.path, "w")     # Makes sure file exists
    file.close()

    #videoCap()


def GS1decode(frame):
    #print(frame.shape)
    if frame.shape[0] < 200 or frame.shape[1] < 200:
        frame = cv2.resize(frame, (0,0), fx=5, fy=5)

    #print(frame.shape)

    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    ret, threshInnit = cv2.threshold(gray, 50, 255, cv2.THRESH_BINARY | cv2.THRESH_BINARY_INV | cv2.THRESH_OTSU)

    msg = pylibdmtx.decode(threshInnit)

    if msg:
        file = open(main.path, "a")
        for m in msg:
            print(m[0])  # data in bytes
            file.write('{"timestamp":"' + datetime.datetime.now().strftime("%X") + '","data":"' + m[0].decode() + '"}' + "\n")
        file.close()
        #    file = open(r"/home/jetson/log/GS1/current_frame.log", "w")
    else:
        print('Nothing detected')

    #cv2.imshow('GS1', frame)


def videoCap():
    realsense = False
    Windows = True

    if realsense:
        cap = cv2.VideoCapture(1)
        pipe = rs.pipeline()
        config = rs.config()

        config.enable_stream(rs.stream.depth, 640, 480, rs.format.z16, 30)

        pipe.start(config)

    else: cap = cv2.VideoCapture(0)

    while(True):
        ret, frame = cap.read()

        if realsense:
            frame2 = pipe.wait_for_frames()
            depth = frame2.get_depth_frame()

            depth2 = np.asanyarray(depth.get_data())

            depth_colormap = cv2.applyColorMap(cv2.convertScaleAbs(depth2, alpha=0.03), cv2.COLORMAP_JET)


        if cv2.waitKey(1) & 0xFF == ord('f'):   #decode GS1-Matrix if found, works with the found thresholds
            # https://docs.opencv.org/4.x/d7/d4d/tutorial_py_thresholding.html
            GS1decode(frame)


        elif cv2.waitKey(1) & 0xFF == ord('g') or True:   #draws contours for anything the program identifies as 'box' (W.I.P.)
            # https://www.tutorialspoint.com/how-to-detect-a-rectangle-and-square-in-an-image-using-opencv-python
            #blur = cv2.GaussianBlur(gray, (7, 7), 0)
            #blur2 = cv2.boxFilter(gray, -1, (9, 9))


            #region Masking
            hls = cv2.cvtColor(frame, cv2.COLOR_BGR2HLS)
            #hls2 = hls
            if False:       # Code for messing with the contrast of individual pixels
                for y in range(len(hls)):
                    for x in range(len(hls[y])):
                        if hls[y][x][1] < 150:
                            hls[y][x] = 0.3 * hls[y][x]
                        else: hls[y][x] = 5 * hls[y][x]
                    #print(hls[y][x])

            Lchannel = hls[:, :, 1]

            mask = cv2.inRange(Lchannel, 200, 255)
            #res = cv2.bitwise_and(frame, frame, mask=mask)

            mask_blur = cv2.boxFilter(mask, -1, (5, 5))

            #res_gray = cv2.cvtColor(res, cv2.COLOR_BGR2GRAY)
            #res_blur = cv2.boxFilter(res_gray, -1, (9, 9))
            #endregion


            #region Contour detection
            #contrast = 3 * blur2 + 175

            ret, thresh = cv2.threshold(mask_blur, 180, 255, cv2.THRESH_BINARY | cv2.THRESH_BINARY_INV | cv2.THRESH_OTSU)
            contours, hierarchy = cv2.findContours(thresh, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
            #endregion

            boxes = []

            for c in range(len(contours)):
                area = cv2.contourArea(contours[c])
                perimeter = cv2.arcLength(contours[c], True)
                approx = cv2.approxPolyDP(contours[c], 0.01 * perimeter, True)

                isBox = len(approx) >= 4 and len(approx) <= 6 and area > 3000 and area < 200000

                #cv2.drawContours(frame, contours, -1, (0, 0, 0), 3)  # draws contours for anything the camera sees
                if isBox:# or childeren > 0:
                    boxes.append(contours[c])

            for b in boxes:
                x, y, w, h = cv2.boundingRect(b)
                cropped = frame[y:h + y, x:w + x]
                #GS1decode(cropped)
                #cv2.imshow('GS1', cropped)

                cv2.drawContours(frame, [b], -1, (255, 105, 180), 3)
                # cv2.imshow('cropped' + str(boxes_count), cropped)

                X = x + w / 2
                # Y = y + h / 2

                if X < frame.shape[1] / 2:      # Left of screen, dif indicates how far away from center screen on a scale of 0 to 1
                    dif = 1 - (X / 320)
                    print("Left", "dif:",  dif)
                elif X > frame.shape[1] / 2:    # Right of screen, dif indicates how far away from center screen on a scale of 0 to 1
                    dif = 1 - (frame.shape[1] - X) / 320
                    print("Right", "dif:", dif)

                cv2.putText(frame, "Medicijndoosje", (x, y), cv2.FONT_HERSHEY_COMPLEX, 1, (255, 105, 180))



            # Edge detection
            edges = cv2.Canny(mask_blur, 100, 200, 90)

            #cv2.imshow('blur', blur2)
            #cv2.imshow('edge', edges)
            cv2.imshow('canny', edges)
            cv2.imshow('mask', mask_blur)
            cv2.imshow('frame', frame)
            cv2.imshow('hls', hls)
            #cv2.imshow('hls2', hls2)
            if realsense: cv2.imshow('realsense', depth_colormap)
            #if color: cv2.imshow('realsense2', color2)
            #continue

        else:
            cv2.imshow('frame', frame)  # sets type of display and content of display
            if realsense: cv2.imshow('realsense', depth_colormap)

        # region Shutdown
        if cv2.waitKey(1) & 0xFF == ord('q'):           #waits till 'q' is pressed
            #break
            cap.release()
            cv2.destroyAllWindows()
            exit()
        # endregion
        #print(main.frameCount)
        main.frameCount = main.frameCount + 1


innit()
videoCap()