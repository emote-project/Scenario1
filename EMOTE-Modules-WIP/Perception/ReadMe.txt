Extract the libs.rar into your Bin folder for opencv files and the missing audio file
Download the latest DIVX codec from www.divx.com (its free)

Perception accepts 2 command line options
the first argument is the thalamus scenario and the second the scenario number as integer (1 or 2) and default is 1

e.g. perception emote 2 
it will load perception on emote and using scenario 2

For troubleshooting please look here and do the steps described especially the last one and the bottom of the page
https://social.msdn.microsoft.com/Forums/en-US/20dbadae-dcee-406a-b66f-a182d76cea3b/troubleshooting-and-common-issues-guide?forum=kinectv2sdk


Logging details:
Kinect:
Time , FaceRotation X(degrees) , FaceRotation Y(degrees) , FaceRotation Z(degrees) , FaceProperty.Happy(True, False, Maybe, Unknown) , FaceProperty.MouthMoved(True, False, Maybe, Unknown) , Head Position.X , Head Position.Y , Head Position.Z , Lean.X(degrees) , Lean.Y(degrees) , LeanTrackingState(True, False, Maybe, Unknown), JawOpen , JawSlideRight , LeftcheekPuff , LefteyebrowLowerer , LefteyeClosed , LipCornerDepressorLeft , LipCornerDepressorRight , LipCornerPullerLeft , LipCornerPullerRight , LipPucker , LipStretcherLeft , LipStretcherRight , LowerlipDepressorLeft , LowerlipDepressorRight , RightcheekPuff , RighteyebrowLowerer , RighteyeClosed
QSensor:
Time , AccelZ , AccelX , AccelY , SkinTemperature, EDA
OKAO:
Time , userID(1-2), ConfidenceVal(0-1000), SmileVal(0-100), Neutral(0-100) , Happiness(0-100) , Surprise(0-100) , Fear(0-100) , Anger(0-100) , Disgust(0-100) , Sadness(0-100), FaceUpDown(degrees), FaceLeftRight(degrees),GazeDirection (labelled)