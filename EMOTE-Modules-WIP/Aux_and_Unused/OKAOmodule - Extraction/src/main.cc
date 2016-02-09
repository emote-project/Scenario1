#include "opencv2/opencv.hpp"
#include <stdio.h>
#include <string.h>
#include "OkaoAPI.h"
#include "CommonDef.h"
#include "OkaoDtAPI.h"
#include "OkaoPtAPI.h"
#include "iostream"
#include "OkaoExAPI.h"
#include "OkaoSmAPI.h"
#include "fstream"
#include "OkaoGbAPI.h"
HANDLE hPipe1;
cv::Rect getCvRect (const FACEINFO & sFaceInfo)
{
    cv::Point center ((sFaceInfo.ptLeftTop.x + sFaceInfo.ptRightBottom.x) / 2,
                      (sFaceInfo.ptLeftTop.y + sFaceInfo.ptRightBottom.y) / 2);
    cv::Point side (sFaceInfo.ptLeftTop.x - sFaceInfo.ptLeftBottom.x,
                    sFaceInfo.ptLeftTop.y - sFaceInfo.ptLeftBottom.y);
    int width = std::sqrt(side.ddot(side));
    return cv::Rect(center.x - width / 2, center.y - width / 2, width, width);
}
bool fexists(const char *fileName)
{
	FILE *fp = fopen(fileName, "r");
	if (fp)
	{
		fclose(fp);
		return true;

	}
	else
		return false;
	return false;

}
using namespace std;
void sendpipe(string messagetxt)
{
	char a[250];
	sprintf(a, "%s", messagetxt.c_str());

	LPTSTR lpszPipename1 = TEXT("\\\\.\\pipe\\serverpipe");
	DWORD cbWritten;
	DWORD dwBytesToWrite = (DWORD)strlen(a);
	hPipe1 = CreateFile(lpszPipename1, GENERIC_WRITE, 0, NULL, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, NULL);
	if ((hPipe1 == NULL || hPipe1 == INVALID_HANDLE_VALUE))
	{
		printf("Could not open the pipe  - (error %d)\n", GetLastError());
	}
	else
	{
		WriteFile(hPipe1, a, dwBytesToWrite, &cbWritten, NULL);
		memset(a, 0xCC, 100);
		CloseHandle(hPipe1);
	}
}
int main(void)
{
	char* LogsLocation = "D:/BACKUP"; // Replace This with the logs location
	////////////////////////////////////////////////////////////////////////////////////////

	std::vector<char*> mLogTracking;
	char message[87][100];
	char videoFile[100];
	for (int i = 1; i < 87; i++)
	{

		int exists = false;

		sprintf(videoFile, "%s/Videos/%i/Front/WEB32%.2d.wmv", LogsLocation, i, i);

		if (fexists(videoFile))
		{
			sprintf(message[i], "%i - Found with WEB3 TAG", i);
			exists = true;
		}
		else
		{
			sprintf(videoFile, "%s/Videos/%i/Front/WEB22%.2d.wmv", LogsLocation, i, i);

			if (fexists(videoFile))
			{
				sprintf(message[i], "%i - Found with WEB2 TAG", i);
				exists = true;
			}
			else
			{
				sprintf(videoFile, "%s/Videos/%i/Front/WEB12%.2d.wmv", LogsLocation, i, i);


				if (fexists(videoFile))
				{
					sprintf(message[i], "%i - Found with WEB1 TAG", i);
					exists = true;
				}
				else
				{
					sprintf(message[i], "%i - NOT Found", i);
					std::cout << "\nA FRONT video does not exist for participant" << i;
					exists = false;
				}
			}
		}
		printf(message[i]);
		mLogTracking.push_back(message[i]);
		if (exists == true)
		{


			//File handling for logs
			FILE *myfile;//smiles
			FILE *myfile2;//expressions
			FILE *myfile3;//gaze information

			char myFileName[100];
			char myFile2Name[100];
			char myFile3Name[100];

			sprintf(myFileName, "%s/%i-smiles.csv", LogsLocation, i, i);
			sprintf(myFile2Name, "%s/%i-expressions.csv", LogsLocation, i, i);
			sprintf(myFile3Name, "%s/%i-gaze.csv", LogsLocation, i, i);

			myfile = fopen(myFileName, "w");
			myfile2 = fopen(myFile2Name, "w");
			myfile3 = fopen(myFile3Name, "w");


			//selects the first webcamera
			//use filename instead of 0 to decode a file
			cv::VideoCapture webcam(videoFile);

					//cv::Size S = cv::Size(640,480);

			cv::Size S = cv::Size((int)webcam.get(CV_CAP_PROP_FRAME_WIDTH), (int)webcam.get(CV_CAP_PROP_FRAME_HEIGHT));


			INT32 nRet = OKAO_ERR_VARIOUS;  /* Return code */
			INT32 nIndex = 0;                   /* Index of face detection */
			INT32 anConf[PT_POINT_KIND_MAX];
			POINT aptPoint[PT_POINT_KIND_MAX];
			INT32 nGazeLeftRight;           /* Right and left estimation result angle for gaze */
			INT32 nGazeUpDown;              /* Upper and lower estimation result angle for gaze */
			INT32 nCloseRatioLeftEye;       /* left eye estimation result for blink */
			INT32 nCloseRatioRightEye;      /* Right eye estimation result for blink */
			INT32 nSmile;
			INT32 nConfidence;
			INT32 nMaxFaceNumber = 1;
			INT32 nMaxSwapNumber = 0;
			double framen;
			char *pExp[EX_EXPRESSION_KIND_MAX] = { "Neutral:", " Happiness:", " Surprise:", " Fear:", " Anger:", " Disgust:", " Sadness:" };
			IMGINFO stImage;
			EX_RESULT stExResult;
			HCOMMON hCO = NULL;
			HDETECTION hDT = NULL;          /* Face Detection Handle */
			HDTRESULT hDtResult = NULL;     /* Face Detection Result Handle */
			HPOINTER hPT = NULL;            /* Facial Parts Detection Handle */
			HPTRESULT hPtResult = NULL;     /* Facial Parts Detection Result Handle */
			HSMILE hSM = NULL;              /* Smile Degree Estimation Handle */
			HSMRESULT hSmResult = NULL;     /* Smile Degree Estimation Result Handle */
			HEXPRESSION hEX = NULL;         /* Expression Estimation Handle */
			HGAZEBLINK hGB = NULL;          /* Gaze and Blink Estimation Handle */
			HGBRESULT hGbResult = NULL;     /* Gaze and Blink Estimation Result Handle */

			std::cout << "To exit the program, press 'q' key" << std::endl;

			hCO = OKAO_CO_CreateHandle(NULL, NULL, malloc, free);
			if (hCO == NULL) {
				std::cout << "OKAO_CO_CreateHandle() Error\n" << nRet << std::endl;
			}
			//face detection serttings
			hDT = OKAO_CreateDetection();
			if (hDT == NULL) {
				std::cout << "OKAO_CreateDetection() Error\n" << nRet << std::endl;
			}
			hDtResult = OKAO_CreateDtResult(35, 0);
			if (hDtResult == NULL) {
				std::cout << "OKAO_CreateDtResult() Error\n" << nRet << std::endl;
			}
			hCO = OKAO_CO_CreateHandle(NULL, NULL, malloc, free);
			if (hCO == NULL) {
				std::cout << "OKAO_CO_CreateHandle() Error\n" << nRet << std::endl;
			}
			/***********************************/
			/* Facial Parts Detection Settings */
			/***********************************/
			/* Creates Facial Parts Detection Handle */
			hPT = OKAO_PT_CreateHandle();
			if (hPT == NULL) {
				std::cout << "OKAO_PT_CreateHandle() Error\n" << nRet << std::endl;
			}
			/* Creates Facial Parts Detection Result Handle */
			hPtResult = OKAO_PT_CreateResultHandle();
			if (hPtResult == NULL) {
				std::cout << "OKAO_PT_CreateResultHandle() Error\n" << nRet << std::endl;
			}
			//smile degree handler
			hSM = OKAO_SM_CreateHandle();
			if (hSM == NULL) {
				std::cout << "OKAO_SM_CreateHandle() Error\n" << nRet << std::endl;
			}
			/* Creates Smile Degree Estimation Result Handle */
			hSmResult = OKAO_SM_CreateResultHandle();
			if (hSmResult == NULL) {
				std::cout << "OKAO_SM_CreateResultHandle() Error\n" << nRet << std::endl;
			}
			hEX = OKAO_EX_CreateHandle(hCO);
			if (hEX == NULL) {
				std::cout << "OKAO_EX_CreateHandle() Error\n" << nRet << std::endl;
			}
			/**************************************/
			/* Gaze and Blink Estimation Settings */
			/**************************************/
			/* Creates Gaze and Blink Estimation Handle */
			hGB = OKAO_GB_CreateHandle();
			if (hGB == NULL) {
				std::cout << "OKAO_GB_CreateHandle() Error\n" << nRet << std::endl;
			}
			/* Creates Gaze and Blink Estimation result Handle */
			hGbResult = OKAO_GB_CreateResultHandle();
			if (hGbResult == NULL) {
				std::cout << "OKAO_GB_CreateResultHandle() Error\n" << nRet << std::endl;
				//break;
			}
			//main while loop
			while ('q' != cv::waitKey(30))
			{
				cv::Mat image;


				webcam >> image;
				if (image.empty() == false)
				{
					cv::Mat grayImage;
					cv::cvtColor(image, grayImage, CV_BGR2GRAY); //converting the image to grey color
					nRet = OKAO_Detection(hDT, grayImage.ptr(), grayImage.cols, grayImage.rows, ACCURACY_NORMAL, hDtResult);

					if (nRet != OKAO_NORMAL) {
						std::cout << "OKAO_Detection() Error : \n" << nRet << std::endl;
						break;
					}
					int nCount = 0;
					nRet = OKAO_GetDtFaceCount(hDtResult, &nCount);
					for (int i = 0; i < nCount; ++i) {
						FACEINFO sFaceInfo = { 0 };
						OKAO_GetDtFaceInfo(hDtResult, i, &sFaceInfo);
						cv::rectangle(image, getCvRect(sFaceInfo), CV_RGB(0, 255, 0), 3);

						nRet = OKAO_PT_SetPositionFromHandle(hPT, hDtResult, nIndex);
						if (nRet != OKAO_NORMAL) {
							std::cout << "OKAO_PT_SetPositionFromHandle() Error :\n" << nRet << std::endl;
							break;
						}
						/* Executes Facial Parts Detection */
						nRet = OKAO_PT_DetectPoint(hPT, grayImage.ptr(), grayImage.cols, grayImage.rows, hPtResult);
						if (nRet != OKAO_NORMAL) {
							std::cout << "OKAO_PT_DetectPoint() Error : %d\n" << nRet << std::endl;
							break;
						}
						/* Gets Facial Parts Position Result */
						nRet = OKAO_PT_GetResult(hPtResult, PT_POINT_KIND_MAX, aptPoint, anConf);
						if (nRet != OKAO_NORMAL) {
							std::cout << "OKAO_PT_GetResult() Error : %d\n" << nRet << std::endl;
							break;
						}
						nRet = OKAO_SM_SetPointFromHandle(hSM, hPtResult);
						if (nRet != OKAO_NORMAL) {
							std::cout << "OKAO_SM_SetPointFromHandle() Error : " << nRet << std::endl;
							break;
						}

						/* Estimate the smile degree */
						nRet = OKAO_SM_Estimate(hSM, grayImage.ptr(), grayImage.cols, grayImage.rows, hSmResult);
						if (nRet != OKAO_NORMAL) {
							std::cout << "OKAO_SM_Estimate() Error : %d\n" << nRet << std::endl;
							break;
						}
						/* Gets the estimated smile degree and its confidence level */
						nRet = OKAO_SM_GetResult(hSmResult, &nSmile, &nConfidence);
						if (nRet != OKAO_NORMAL) {
							std::cout << "OKAO_SM_GetResult() Error : %d\n" << nRet << std::endl;
							break;
						}
						/*****************************/
						/* Gaze and Blink Estimation */
						/*****************************/
						/* Sets facial parts position from PT result handle */
						nRet = OKAO_GB_SetPointFromHandle(hGB, hPtResult);
						if (nRet != OKAO_NORMAL) {
							std::cout << "OKAO_GB_SetPointFromHandle() Error : %d\n" << nRet << std::endl;
							break;
						}
						/* Execute Gaze and Blink Estimation */
						nRet = OKAO_GB_Estimate(hGB, grayImage.ptr(), grayImage.cols, grayImage.rows, hGbResult);
						if (nRet != OKAO_NORMAL) {
							std::cout << "OKAO_GB_Estimate() Error : %d\n" << nRet << std::endl;
							break;
						}
						/* Gets the Gaze estimation result */
						nRet = OKAO_GB_GetGazeDirection(hGbResult, &nGazeLeftRight, &nGazeUpDown);
						if (nRet != OKAO_NORMAL) {
							std::cout << "OKAO_GB_GetGazeDirection() Error : %d\n" << nRet << std::endl;
							break;
						}
						/* Gets the Blink estimation result */
						nRet = OKAO_GB_GetEyeCloseRatio(hGbResult, &nCloseRatioLeftEye, &nCloseRatioRightEye);
						if (nRet != OKAO_NORMAL) {
							std::cout << "OKAO_GB_GetEyeCloseRatio() Error : %d\n" << nRet << std::endl;
							break;
						}
						/***************************/
						/* Expression Estimation   */
						/***************************/
						/* Sets the feature points for Expression Estimation from PT result handle */
						nRet = OKAO_EX_SetPointFromHandle(hEX, hPtResult);
						if (nRet != OKAO_NORMAL) {
							std::cout << "OKAO_EX_SetPointFromHandle() Error : %d\n" << nRet << std::endl;
							break;
						}
						/* Estimate the Expression */
						stImage.apImage[eIMG_PLANAR_FIRST] = grayImage.ptr();
						stImage.apImage[eIMG_PLANAR_SECOND] = NULL;
						stImage.apImage[eIMG_PLANAR_THIRD] = NULL;
						stImage.nWidth = 640;
						stImage.nHeight = 480;
						stImage.unImageFormat = GRAY_Y0Y1Y2Y3;
						nRet = OKAO_EX_Estimate(hEX, stImage, &stExResult);
						if (nRet != OKAO_NORMAL) {
							std::cout << "OKAO_EX_Estimate() Error : %d\n" << nRet << std::endl;
							break;
						}
						framen = webcam.get(CV_CAP_PROP_POS_MSEC);
						framen = framen / 1000;
						//saving the logs
						fprintf(myfile2, "Neutral:%3d, Happiness:%3d, Surprise:%3d, Fear:%3d, Anger:%3d, Disgust:%3d, Sadness:%3d, Time:%f\n", stExResult.anScore[0], stExResult.anScore[1], stExResult.anScore[2], stExResult.anScore[3], stExResult.anScore[4], stExResult.anScore[5], stExResult.anScore[6], framen);
						fprintf(myfile, "User: %2d, Confidence:%3d, Smile:%3d, Time:%f\n", i, nConfidence, nSmile, framen);
						fprintf(myfile3, "User: %2d, GazeLeftRight = %d, GazeUpDown = %d, CloseRatioLeftEye = %d, CloseRatioRightEye = %d, Time:%f\n", i, nGazeLeftRight, nGazeUpDown, nCloseRatioLeftEye, nCloseRatioRightEye, framen);
						//temp buffers
						char text[100] = "";
						char fpsbps[50];

						//					sprintf(fpsbps, "User: %2d Confidence:%3d Smile:%3d", i, nConfidence, nSmile);
											sprintf(fpsbps, "User: %2d Confidence:%3d Smile:%3d Time:%.2f", i, nConfidence, nSmile, framen);

						strcat(text, fpsbps);

						char text1[100] = "";
						char fpsbps1[100];

						//sprintf(fpsbps1, "Neutral:%3d Happiness:%3d Surprise:%3d Fear:%3d Anger:%3d Disgust:%3d Sadness:%3d", stExResult.anScore[0], stExResult.anScore[1], stExResult.anScore[2], stExResult.anScore[3], stExResult.anScore[4], stExResult.anScore[5], stExResult.anScore[6]);
											sprintf(fpsbps1, "Neutral:%3d Happiness:%3d Surprise:%3d Fear:%3d Anger:%3d Disgust:%3d Sadness:%3d Time:%.2f", stExResult.anScore[0], stExResult.anScore[1], stExResult.anScore[2], stExResult.anScore[3], stExResult.anScore[4], stExResult.anScore[5], stExResult.anScore[6], framen);

						strcat(text1, fpsbps1);

						char text2[100] = "";
						char fpsbps2[100];

						sprintf(fpsbps2, "GazeLeftRight = %d, GazeUpDown = %d, CloseRatioLeftEye = %d, CloseRatioRightEye = %d", nGazeLeftRight, nGazeUpDown, nCloseRatioLeftEye, nCloseRatioRightEye);
						strcat(text2, fpsbps2);

						char pipetext[250] = "";
						sprintf(pipetext, "User:%1d,Confidence:%3d,Smile:%3d,Neutral:%3d,Happiness:%3d,Surprise:%3d,Fear:%3d,Anger:%3d,Disgust:%3d,Sadness:%3d,GazeLeftRight=%3d,GazeUpDown=%3d,LeftEye=%3d,RightEye=%3d", i, nConfidence, nSmile, stExResult.anScore[0], stExResult.anScore[1], stExResult.anScore[2], stExResult.anScore[3], stExResult.anScore[4], stExResult.anScore[5], stExResult.anScore[6], nGazeLeftRight, nGazeUpDown, nCloseRatioLeftEye, nCloseRatioRightEye);


						//sendpipe("fotis");
						//printing information on the screen
						cv::putText(image, text2, cv::Point(10, 60), cv::FONT_HERSHEY_SIMPLEX, 0.4, cv::Scalar(155, 0, 200));
						cv::putText(image, text, cv::Point(10, 20), cv::FONT_HERSHEY_SIMPLEX, 0.5, cv::Scalar(255, 0, 200));
						cv::putText(image, text1, cv::Point(10, 40), cv::FONT_HERSHEY_SIMPLEX, 0.4, cv::Scalar(155, 0, 200));


					}
				}
				try{

					cv::imshow("", image);

				}
				catch (exception e){ break; }
			}

			/********************************/
			/* Handle Deletion              */
			/********************************/
			/* Deletes Smile Degree Estimation Handle */
			if (hSM != NULL) {
				OKAO_SM_DeleteHandle(hSM);
			}
			/* Deletes Smile Degree Estimation Result Handle */
			if (hSmResult != NULL) {
				OKAO_SM_DeleteResultHandle(hSmResult);
			}
			/* Deletes Facial Parts Detection Handle */
			if (hPT != NULL) {
				OKAO_PT_DeleteHandle(hPT);
			}
			/* Deletes Facial Parts Detection Result Handle */
			if (hPtResult != NULL) {
				OKAO_PT_DeleteResultHandle(hPtResult);
			}
			/* Deletes Face Detection handle */
			if (hDT != NULL) {
				OKAO_DeleteDetection(hDT);
			}
			/* Deletes Face Detection result handle */
			if (hDtResult != NULL) {
				OKAO_DeleteDtResult(hDtResult);
			}

			if (hCO != NULL) {
				OKAO_CO_DeleteHandle(hCO);
			}

			//closes the files
			fclose(myfile);
			fclose(myfile2);

			fclose(myfile3);


			

		}			
		char logTrackingFile[100];
			sprintf(logTrackingFile, "%s/LogTrackingFile.csv", LogsLocation);
			std::ofstream LogTrackFile(logTrackingFile);
		for (int j = 0; j < mLogTracking.size(); j++)
			{
				if (LogTrackFile.is_open())
				{
					char output[250];
					sprintf(output, "%s",
						mLogTracking[j]
						);

					LogTrackFile << output << "\n";
				}
			}
	}
		return 0;
	
}