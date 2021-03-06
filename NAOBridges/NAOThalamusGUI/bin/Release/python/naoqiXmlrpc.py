from SimpleXMLRPCServer import SimpleXMLRPCServer
from SimpleXMLRPCServer import SimpleXMLRPCRequestHandler


import xmlrpclib
import time
import httplib
import sys
import SocketServer



import threading
from threading import Thread
from threading import Lock
from threading import Timer

import naoqi
from naoqi import ALBroker
from naoqi import ALModule
from naoqi import ALProxy
import thread
from time import sleep
from Queue import Queue
import socket
import json
import ast

BROKER_IP = "127.0.0.1"
BROKER_PORT = 9559
BUFFER = 32768

#BROKER_PORT = 55453

import audioModule
import memoryModule
import sensorsModule
import soundLocationModule
import speechModule
import motionModule
import visionModule
import behaviorModule
import eyesModule
import speechRecognitionModule

from collections import namedtuple

global logProxy
logProxy = None
global NAOQIXMLRPCPORT
NAOQIXMLRPCPORT = 10000
TCPSTREAMPORT = 9900
global mutex
mutex = Lock()

global myBroker
myBroker = ALBroker("naoBroker", "0.0.0.0", 0, BROKER_IP, BROKER_PORT)

PointStruct = namedtuple("PointStruct", "s_id d_horizontal d_vertical d_speed s_mode")
WavingStruct = namedtuple("WavingStruct", "s_id d_horizontal d_vertical d_frequency d_amplitude d_duration s_mode")
GazeStruct = namedtuple("GazeStruct", "s_id d_horizontal d_vertical d_speed b_trackfaces")
SpeakStruct = namedtuple("SpeakStruct", "s_id s_text ")
SpeakBookmarksStruct = namedtuple("SpeakBookmarksStruct", "s_id a_s_text, a_s_bookmarks")
PlayAnimationBufferStruct = namedtuple("PlayAnimationBufferStruct", "a_s_joints a_d_angles d_deltaTime")
HeadStruct = namedtuple("HeadStruct", "s_id s_lexeme i_repetitions d_amplitude d_frequency")
HeartbeatStruct = namedtuple("HeartbeatStruct", "d_ticks")



class RequestHandler(SimpleXMLRPCRequestHandler):
    rpc_paths = ()
    def __init__(self, request, client_address, server):
        try:
            SimpleXMLRPCRequestHandler.__init__(self, request, client_address, server)
        except Exception, Argument:
            print "Exception in RequestHandler: " + str(Argument)

class TimeoutTransport(xmlrpclib.Transport):
    timeout = 500.0
    def set_timeout(self, timeout):
        self.timeout = timeout


class AnimationFrame(object):
    def from_JSON(self, j):
        self.__dict__ = json.loads(j, encoding="ASCII")
        self.Joints = map(lambda s: s.encode('ascii'), self.Joints)

class JsonObjectStreamServer(object):
    def start(self, port):
        global count
        count = 0
        s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM);
        s.connect(('8.8.8.8', 80));
        ipAddress = s.getsockname()[0];
        self.tcpStreamServer = SocketServer.TCPServer((ipAddress, port), TCPStreamHandler)
        self.tcpStreamServer.allow_reuse_address = True
        self.tcpStreamServer.socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        thread.start_new_thread(self.tcpStreamServer.serve_forever, ())
        print "Started JsonObjectStreamServer at " + str(ipAddress) + ":" + str(port)
        
    def stop(self):
        self.tcpStreamServer.shutdown()
    

class TCPStreamHandler(SocketServer.StreamRequestHandler):
    timeout = 5
    
    def JsonObjectReceived(self, objType, objJson):
        global count
        global naoqiXmlRpcServer
        if (objType == 'AnimationFrame'):            
            f = AnimationFrame()
            f.from_JSON(objJson)
            count = count+1
            naoqiXmlRpcServer.Animate(f.Ticks, f.Joints, f.Values, f.Speed, f.Time, f.FrameType)
        else:
            print objType + "." + objJson
        pass
    
    def handle(self):
        content = ""
        cmd = ""
        global naoqiXmlRpcServer
        global cachedContent
        cachedContent = ""
        content = ""
        while (not naoqiXmlRpcServer.quit):
            try:
                self.obj = None
                readData = self.request.recv(BUFFER)
                content = cachedContent + readData.strip()
                lastIndex = content.find("<EOF>")
                while(lastIndex!=-1):
                    contentPartition = content.split("<EOF>", 1)
                    obj = contentPartition[0].split('{', 1)
                    self.obj = obj
                    obj[1] = "{" + obj[1]
                    typeSplit = obj[0].split('.')
                    if (len(typeSplit)>=2):
                        obj[0] = typeSplit[len(typeSplit)-2]
                    else:
                        obj[0] = typeSplit[0]
                    content = contentPartition[1]
                    self.JsonObjectReceived(obj[0], obj[1])
                    if (len(content) == 0):
                        lastIndex = -1;
                    else:
                        lastIndex = content.find("<EOF>")
                    if (lastIndex == -1):
                        if (len(contentPartition[1]) > 0):
                            cachedContent = contentPartition[1]
                        else:
                            cachedContent = ""
            except Exception, Argument:
                print "TCP Failed: " + str(Argument)
                print str(self.obj)
                return

class NaoqiXmlrpc(SocketServer.ThreadingMixIn, SimpleXMLRPCServer):
    
    def brokerIp(self):
        return BROKER_IP
    def brokerPort(self):
        return BROKER_PORT

    loadSpeech = True
    loadMotion = True
    loadBehavior = True
    loadAudio = True
    loadSoundLocalization = False
    loadVision = False
    loadSensors = True
    loadEyes = True
    loadSpeechReco = False

    memory = None
    speech = None
    audio = None
    sensors = None
    vision = None
    behavior = None
    motion = None
    soundLocation = None
    eyes = None
    speechReco = None

    actionQueues = dict()

    clientPort = NAOQIXMLRPCPORT+1
    jossPort = clientPort+1
    joscPort = jossPort+1
    eventPublisher = None
    quit = False
    JsonSendCount = 0
    JsonRcvCount = 0;
    thalamusAddress = "127.0.0.1"
    autoThalamusAddress = False

    def setThalamus(self):
        t = TimeoutTransport()
        t.set_timeout(500)
        self.eventPublisher = xmlrpclib.ServerProxy('http://'+self.thalamusAddress+':{0}'.format(self.clientPort), transport=t, verbose=False)
        self.log("XMLRpc Client created.")

    def __init__(self, thalamusClientAddress=""):
        self.log("THALAMUSADDRESS: '" + thalamusClientAddress + "'")
        if (thalamusClientAddress==""): self.autoThalamusAddress = True
        else: self.thalamusAddress=thalamusClientAddress

        t = TimeoutTransport()
        t.set_timeout(500)
        global NAOQIXMLRPCPORT
        SimpleXMLRPCServer.__init__(self, ("", NAOQIXMLRPCPORT), requestHandler=RequestHandler, logRequests=False)
        #SimpleXMLRPCServer.__init__(self, ("127.0.0.1", NAOQIXMLRPCPORT), requestHandler=RequestHandler, logRequests=False)
        self.register_introspection_functions()
        self.register_function(self.Speak)
        self.register_function(self.Gaze)
        self.register_function(self.PointingNao)
        self.register_function(self.WavingNao)
        self.register_function(self.Head)
        self.register_function(self.WalkToTarget)
        self.register_function(self.SpeakBookmarks)
        self.register_function(self.WalkTo)
        self.register_function(self.StopAnimation)
        self.register_function(self.PlayAnimation)
        self.register_function(self.PlayAnimationQueued)
        self.register_function(self.ResetPose)
        self.register_function(self.SetPosture)
        self.register_function(self.PlaySound)
        self.register_function(self.PlaySoundLoop)
        self.register_function(self.StopSound)
        self.register_function(self.Ping)
        self.register_function(self.ConnectToNao)
        self.register_function(self.Animate)
        self.register_function(self.SetIdleBehavior)
        self.register_function(self.SetAddress)
        self.register_function(self.SpeakStop)
        self.register_function(self.EyesIntensity);
        self.register_function(self.SpeakerVolume);
        self.register_function(self.FaceLexeme);
        self.register_function(self.FaceShiftLexeme);
        self.register_function(self.EyeBlink);
        self.register_function(self.SlowEyeBlink);
        self.register_function(self.Heartbeat);

        self.log("XMLRpc Server Running...")
        
        self.setThalamus()

        global usingNutty        
        #self.eventPublisher = xmlrpclib.ServerProxy('http://127.0.0.1:{0}'.format(self.clientPort), transport=t, verbose=False)
        
        thread.start_new_thread(self.pinger, ())
        self.Debug ("Launched thread: ping");
        thread.start_new_thread(self.run, ())
        self.Debug ("Launched thread: run");
        thread.start_new_thread(self.actionDispatcher, ())
        self.Debug ("Launched thread: actionDispatcher");
        self.connectProxies()
        self.Debug ("Connected to Proxies ");
        
        

        if (usingNutty):
            self.joss = JsonObjectStreamServer()
            self.joss.start(self.jossPort)

            thread.start_new_thread(self.JsonObjectStreamerClient, ())
            self.Debug ("Launched thread: JsonObjectStreamerClient");
            self.behavior.switchIdle(False)
        else:
            self.behavior.switchIdle(True)
            self.joss = None

        self.fpsTimer = Timer(1, self.fpsCounter)
        self.fpsTimer.start()

    def fpsCounter(self):
        fps = self.JsonSendCount
        self.JsonSendCount = 0
        if (fps > 0):
            print "JsonSendHz: " + str(fps)
        fps = self.JsonRcvCount
        self.JsonRcvCount = 0
        if (fps > 0):
            print "JsonRcvHz: " + str(fps)
        if (not self.quit):
            self.fpsTimer = Timer(1, self.fpsCounter)
            self.fpsTimer.start()
            

    def JsonObjectStreamerClient(self):
        self.JoscConnected = False
        self.JsonQueue = Queue()
        self.JsonCount = 0
        while(not self.quit):
            if (not self.JoscConnected and self.thalamusAddress != None and self.joscPort != None):
                try:
                    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                    s.connect((self.thalamusAddress, self.joscPort))
                    print "Connected to JsonObjectStreamer at " + str((self.thalamusAddress, self.joscPort))
                    self.JoscConnected = True
                except Exception:
                    print "Unable to connect JsonObjectStreamerClient at " + str((self.thalamusAddress, self.joscPort))
                    time.sleep(3)
            if (self.JoscConnected):
                try:
                    hb = self.motion.MakeHeartbeatEcho()
                    if (hb!=None):
                        self.JsonQueue.put(hb)
                        while (not self.JsonQueue.empty()):
                            obj = self.JsonQueue.get()
                            s.send(str(obj.__class__.__name__) + "." + obj.to_JSON() + "<EOF>")
                            self.JsonCount = self.JsonCount+1
                            self.JsonQueue.task_done()
                    time.sleep(0.02)
                    
                except Exception, Argument:
                    print "JsonObjectStreamerClient Error: " + str(Argument)
                    s.close()
                    self.JoscConnected = False

        print "Terminated JsonObjectStreamerClient"

    def connectProxies(self):
        self.memory = memoryModule.MemoryModule(self)
        self.memory.start()
        self.Debug ("Started MemoryModule");

        if (self.loadSpeech):
            self.speech = speechModule.SpeechModule(self)
            self.speech.start()
            self.actionQueues[self.speech] = Queue()
            self.Debug ("Loaded: Speech");

        if (self.loadSpeechReco):
            self.speechReco = speechRecognitionModule.SpeechRecognitionModule(self)
            self.speechReco.start()
            self.actionQueues[self.speechReco] = Queue()
            self.Debug ("Loaded: SpeechRecognition");

        if (self.loadAudio):
            self.audio = audioModule.AudioModule(self)
            self.audio.start()
            self.actionQueues[self.audio] = Queue()
            self.Debug ("Loaded: Audio");

        if (self.loadBehavior):
            self.behavior = behaviorModule.BehaviorModule(self)
            self.behavior.start()
            self.actionQueues[self.behavior] = Queue()
            self.Debug ("Loaded: Behavior");

        if (self.loadMotion):
            self.motion = motionModule.MotionModule(self)
            self.motion.start()
            self.actionQueues[self.motion] = Queue()
            self.Debug ("Loaded: Motion");

        if (self.loadSoundLocalization):
            self.soundLocation = soundLocationModule.SoundLocationModule(self)
            self.soundLocation.start()
            self.Debug ("Loaded: SoundLocalization");

        if (self.loadSensors):
            self.sensors = sensorsModule.SensorsModule(self)
            self.sensors.start()
            self.Debug ("Loaded: Sensors");

        if (self.loadVision):
            self.vision = visionModule.VisionModule(self)
            self.vision.start()
            self.Debug ("Loaded: Vision");

        if (self.loadEyes):
            self.eyes = eyesModule.EyesModule(self)
            self.eyes.start()
            self.Debug ("Loaded: Eyes");

    def stop(self):
        self.log("Exiting...")
        self.quit = True

        if (self.joss != None):
            self.joss.stop()
        
        self.server_close()
        self.memory.dispose()

        if (self.loadSpeech):
            self.speech.dispose()
        if (self.loadSpeechReco):
            self.speechReco.dispose()
        if (self.loadAudio):
            self.audio.dispose()
        if (self.loadSoundLocalization):
            self.soundLocation.dispose()
        if (self.loadSensors):
            self.sensors.dispose()
        if (self.loadVision):
            self.vision.dispose()
        if (self.loadMotion):
            self.motion.dispose()
        if (self.loadBehavior):
            self.behavior.dispose()
        if (self.loadEyes):
            self.eyes.dispose()

        global myBroker
        myBroker.shutdown()
        

    def run(self):
        global NAOQIXMLRPCPORT
        while not self.quit:
            try:
                self.log("RPC Listening on " + str(self.server_address))
                self.serve_forever()
            except:
                self.log("Unexpected error in RpcServer: " + str(sys.exc_info()))
                self.shutdown()

    def actionDispatcher(self):
        while not self.quit:
            try:
                for proxy in self.actionQueues.keys():
                    while (not self.actionQueues[proxy].empty()):
                        action = self.actionQueues[proxy].get()
                        thread.start_new_thread(proxy.ExecuteAction, (proxy, action))
                        self.actionQueues[proxy].task_done()
            except Exception, Argument:
                print "Failed to dispatch action: " + str(Argument)
            sleep(0.5)

        

    def pinger(self):
        global NAOQIXMLRPCPORT
        while not self.quit:
            try:
                self.eventPublisher.Ping()
            except Exception, Argument:
                print "unable to connect: " + str(Argument)
            sleep(3)

    def log(obj, text):
        print text
        global logProxy
        if (logProxy == None):
            logProxy = ALProxy("ALLogger", BROKER_IP, BROKER_PORT)
        try:
            logProxy.info(str(obj), text)
        except Exception, Argument:
            print("Failed: " + str(Argument))
            try:
                logProxy = ALProxy("ALLogger", BROKER_IP, BROKER_PORT)
                logProxy.info(str(obj), text)
            except Exception:
                return
        return

    def Connect(self, clientName, port):
        self.log("Connected with " + clientName + " on port " + str(port))
        self.CLIENTPORT = port
        return ""

    def ConnectToNao(self, address, port, notify):
        return True

    def SetAddress(self, s_address):
        if (self.autoThalamusAddress):
            self.log("RPC: SetAddress:{0}".format(s_address))
            self.thalamusAddress = s_address
            self.setThalamus()
        self.log("Connected to NAOThalamus on '" + self.thalamusAddress + "'")
        self.NotifyAnimationList(self.behavior._behaviors)
        self.NotifyEyeShapeList(self.eyes.savedStates.keys())
        return ""

    def Ping(self):
        return ""

    def EyeBlink(self, i_count):
        self.log("RPC: EyeBlink count:{0} speed:{1}".format(i_count, 100))
        self.eyes.ExpressState(self.eyes.BlinkExpression, 100)
        return ""

    def SlowEyeBlink(self, i_count):
        self.log("RPC: EyeBlink count:{0} speed:{1}".format(i_count, 500))
        self.eyes.ExpressState(self.eyes.BlinkExpression, 500)
        return ""

    def FaceLexeme(self, s_id, s_eyeShape):
        self.log("RPC: FaceLexeme shape:{0}".format(s_eyeShape))
        self.eyes.ExpressState(s_eyeShape)
        return ""

    def FaceShiftLexeme(self, s_id, s_eyeShape):
        self.log("RPC: FaceShiftLexeme shape:{0}".format(s_eyeShape))
        self.eyes.ShiftState(s_eyeShape)
        return ""

    def SpeakerVolume(self, d_value):
        #self.log("RPC: SpeakerVolume value:{0}".format(d_value))
        self.audio.SetVolume(d_value)
        return ""

    def EyesIntensity(self, d_value):
        self.log("RPC: EyesIntensity value:{0}".format(d_value))
        self.eyes.SetIntensity(d_value)
        return ""

    def SpeakStop(self):
        self.log("RPC: SpeakStop")
        self.speech.SpeakStop()
        return ""

    def Speak(self, s_id, s_text):
        self.log("RPC: Speak id:{0}, text:{1}".format(s_id, s_text  ))
        #self.speech.Speak(s_id, s_text)
        self.actionQueues[self.speech].put(SpeakStruct(s_id, s_text))
        
        return ""

    def SpeakBookmarks(self, s_id, a_s_text, a_s_bookmarks):
        self.log("RPC: SpeakBookmarks id:{0}, text:{1}, bookmarks:{2}".format(s_id, a_s_text, a_s_bookmarks))
        #self.speech.SpeakBookmarks(s_id, a_s_text, a_s_bookmarks)
        self.actionQueues[self.speech].put(SpeakBookmarksStruct(s_id, a_s_text, a_s_bookmarks))
        return ""

    def Gaze(self, s_id, d_horizontal, d_vertical, d_speed, b_trackFaces):
        self.log("RPC: Gaze id:{0}, h:{1}, v:{2}, speed:{3}, trackFaces:{4}".format(s_id, d_horizontal, d_vertical, d_speed, b_trackFaces))
        self.actionQueues[self.motion].put(GazeStruct(s_id, d_horizontal, d_vertical, d_speed, b_trackFaces))
        return ""

    def PointingNao(self, s_id, d_horizontal, d_vertical, d_speed, s_mode):
        self.log("RPC: PointingNao id:{0}, h:{1}, v:{2}, speed:{3}, s_mode:{4}".format(s_id, d_horizontal, d_vertical, d_speed, s_mode))
        self.actionQueues[self.motion].put(PointStruct(s_id, d_horizontal, d_vertical, d_speed, s_mode))
        return ""
    
    def WavingNao(self, s_id,d_horizontal, d_vertical, d_frequency, d_amplitude, d_duration, s_mode):
        self.log("RPC: Waving id:{0}, h:{5}, v:{6}, fr:{1}, amp:{2}, dur:{3}, mode:{4}".format(s_id, d_frequency, d_amplitude, d_duration, s_mode, d_horizontal, d_vertical))
        self.actionQueues[self.motion].put(WavingStruct(s_id, d_horizontal, d_vertical, d_frequency, d_amplitude, d_duration, s_mode))
        return ""

    def Head(self, s_id, s_lexeme, i_repetitions, d_amplitude, d_frequency):
        self.log("RPC: Head id:{0}, lexeme:{1}, repetitions:{2}, amplitude:{3}, frequency:{4}".format(s_id, s_lexeme, i_repetitions, d_amplitude, d_frequency))
        self.actionQueues[self.motion].put(HeadStruct(s_id, s_lexeme, i_repetitions, d_amplitude, d_frequency))
        return ""
        
    def WalkTo(self, s_id, d_x, d_y, d_angle):
        self.log("RPC: WalkTo id:{0}, x:{1}, y:{2}, angle:{3}".format(s_id, d_x, d_y, d_angle))
        return ""
        
    def WalkToTarget(self, s_id, s_target):
        self.log("RPC: WalkToTarget id:{0}, target:{1}".format(s_id, s_target))
        return ""
    
    def StopAnimation(self, s_id):
        self.log("RPC: StopAnimation id:{0}".format(s_id))
        self.behavior.StopBehavior(s_id)
        return ""
        
    def PlayAnimation(self, s_id, s_animation):
        self.log("RPC: PlayAnimation id:{0}, animation:{1}".format(s_id, s_animation))
        self.behavior.PlayBehavior(s_id, s_animation)
        return ""

    def PlayAnimationQueued(self, s_id, s_animation):
        self.log("RPC: PlayAnimationQueued id:{0}, animation:{1}".format(s_id, s_animation))
        self.behavior.PlayBehavior(s_id, s_animation, True)
        return ""
        
    def ResetPose(self):
        self.log("RPC: ResetPose")
        self.behavior.ResetPose()
        return ""
        
    def SetPosture(self, s_id, s_posture, d_percent, d_decay):
        self.log("RPC: SetPosture id:{0}, posture:{1}, percent:{2}, decay:{3}".format(s_id, s_posture, d_percent, d_decay))
        return ""
        
    def PlaySound(self, s_id, s_soundName, d_volume, d_pitch):
        self.log("RPC: PlaySound id:{0}, soundName:{1}, volume:{2} pitch:{3}".format(s_id, s_soundName, d_volume, d_pitch))
        self.audio.PlaySound(s_id, s_soundName, d_volume, d_pitch)
        return ""
        
    def PlaySoundLoop(self, s_id, s_soundName, d_volume, d_pitch):
        self.log("RPC: PlaySoundLoop id:{0}, soundName:{1}, volume:{2} pitch:{3}".format(s_id, s_soundName, d_volume, d_pitch))
        self.audio.PlaySoundLoop(s_id, s_soundName, d_volume, d_pitch)
        return ""
        
    def StopSound(self, s_id):
        self.log("RPC: StopSound id:{0}".format(s_id))
        self.audio.StopSound()
        return ""

    

    def SetIdleBehavior(self, b_idleState):
        self.log("RPC: SetIdleBehavior idleState:{0}".format(b_idleState))
        self.behavior.switchIdle(b_idleState)
        return ""
    
    def Animate(self, ticks, a_s_joints, a_d_angles, a_d_speed, d_deltaTime, s_frameType):
        if (s_frameType == "AngleDeg"):
            self.motion.PlayAnimationBuffer(ticks, a_s_joints, a_d_angles, a_d_speed, d_deltaTime)
        return ""

    def Heartbeat(self, d_ticks):
        #self.log("Heartbeat: " + str(d_ticks))
        self.actionQueues[self.motion].put(HeartbeatStruct(d_ticks))
        return ""

    def NotifyHeartbeatEcho(self, d_ticks, a_s_joints, a_d_angles):
        global mutex
        mutex.acquire()
        try:
            self.eventPublisher.HeartbeatEcho(d_ticks, a_s_joints, a_d_angles)
        except Exception, Argument:
            self.log("NotifyHeartbeatEcho Failed: " + str(Argument))
        finally:
            mutex.release()

    def NotifySpeakStart(self, s_id):
        self.log("NotifySpeakStart: " + str(s_id))
        global mutex
        mutex.acquire()
        try:
            self.eventPublisher.SpeakStarted(s_id)
        except Exception, Argument:
            self.log("NotifySpeakStart Failed: " + str(Argument))
        finally:
            mutex.release()

    def NotifySpeakEnd(self, s_id):
        self.log("NotifySpeakEnd: " + str(s_id))
        global mutex
        mutex.acquire()
        try:
            self.eventPublisher.SpeakFinished(s_id)
        except Exception, Argument:
            self.log("NotifySpeakEnd Failed: " + str(Argument))
        finally:
            mutex.release()
    
    def NotifyWordDetected(self, a_s_words):
        self.log("NotifyWordDetected: " + str(a_s_words))
        global mutex
        mutex.acquire()
        try:
            self.eventPublisher.WordDetected(a_s_words)
        except Exception, Argument:
            self.log("NotifyWordDetected Failed: " + str(Argument))
        finally:
            mutex.release()

    def NotifySpeakBookmark(self, s_id):
        global mutex
        mutex.acquire()
        self.log("NotifySpeakBookmark: " + str(s_id))
        i = 0
        if (s_id==None):
            s_id = ""
        while(i<3):
            try:
                self.eventPublisher.Bookmark(s_id)
                i=3
            except Exception, Argument:
                self.log("NotifySpeakBookmark Failed: " + str(Argument))
                i=i+1
                time.sleep(0.1)
        mutex.release()

    def NotifySensorTouched(self, sensor, state):
        self.log("NotifySensorTouched " + str(sensor) + " " + str(state))
        global mutex
        mutex.acquire()
        try:
            self.eventPublisher.SensorTouched(sensor, state)
        except Exception, Argument:
            self.log("Failed: " + str(Argument))
        finally:
            mutex.release()

    def NotifySoundLocated(self, f_azimuth, f_elevation, t_confidence):
        self.log("NotifySoundLocated a:" + str(f_azimuth) + " e:" + str(f_elevation) + " c:" + str(t_confidence))
        global mutex
        mutex.acquire()
        try:
            self.eventPublisher.SoundSourceLocalized(f_azimuth, f_elevation, t_confidence)
        except Exception, Argument:
            self.log("Failed: " + str(Argument))
        finally:
            mutex.release()

    def NotifyAnimationStarted(self, s_id):
        if (s_id==None):
            s_id = ""
        self.log("NotifyAnimationStarted " + s_id)
        global mutex
        mutex.acquire()
        i = 0
        while(i<3):
            try:
                self.eventPublisher.AnimationStarted(s_id)
                i=3
            except Exception, Argument:
                self.log("NotifyAnimationStarted Failed: " + str(Argument))
                i=i+1
                time.sleep(0.1)
        mutex.release()

    def NotifyAnimationFinished(self, s_id):
        if (s_id==None):
            s_id = ""
        self.log("NotifyAnimationFinished " + s_id)
        global mutex
        mutex.acquire()
        i = 0
        while(i<3):
            try:
                self.eventPublisher.AnimationFinished(s_id)
                i=3
            except Exception, Argument:
                self.log("NotifyAnimationFinished Failed: " + str(Argument))
                i=i+1
                time.sleep(0.1)
        mutex.release()

    def NotifyGazeStart(self, s_id):
        self.log("NotifyGazeStart: " + str(s_id))
        global mutex
        mutex.acquire()
        try:
            self.eventPublisher.GazeStarted(s_id)
        except Exception, Argument:
            self.log("NotifyGazeStart Failed: " + str(Argument))
        finally:
            mutex.release()

    def NotifyGazeEnd(self, s_id):
        self.log("NotifyGazeEnd: " + str(s_id))
        global mutex
        mutex.acquire()
        try:
            self.eventPublisher.GazeFinished(s_id)
        except Exception, Argument:
            self.log("NotifyGazeEnd Failed: " + str(Argument))
        finally:
            mutex.release()

    def NotifyPointingStart(self, s_id):
        global mutex
        mutex.acquire()
        self.log("NotifyPointingStart: " + str(s_id))
        try:
            self.eventPublisher.PointingStarted(s_id)
        except Exception, Argument:
            self.log("NotifyPointingStart Failed: " + str(Argument))
        finally:
            mutex.release()

    def NotifyPointingEnd(self, s_id):
        global mutex
        mutex.acquire()
        self.log("NotifyPointingEnd: " + str(s_id))
        try:
            self.eventPublisher.PointingFinished(s_id)
        except Exception, Argument:
            self.log("NotifyPointingEnd Failed: " + str(Argument))
        finally:
            mutex.release()

    def NotifyWavingStart(self, s_id):
        global mutex
        mutex.acquire()
        self.log("NotifyWavingStart: " + str(s_id))
        try:
            self.eventPublisher.WavingStarted(s_id)
        except Exception, Argument:
            self.log("NotifyWavingStart Failed: " + str(Argument))
        finally:
            mutex.release()

    def NotifyWavingEnd(self, s_id):
        global mutex
        mutex.acquire()
        self.log("NotifyWavingEnd: " + str(s_id))
        try:
            self.eventPublisher.WavingFinished(s_id)
        except Exception, Argument:
            self.log("NotifyWavingEnd Failed: " + str(Argument))
        finally:
            mutex.release()

    def NotifyHeadStart(self, s_id):
        global mutex
        mutex.acquire()
        self.log("NotifyHeadStart: "+str(s_id))
        try: 
            self.eventPublisher.HeadStarted(s_id)
        except Exception, Argument:
            self.log("NotifyHeadStart Failed: "+str(Argument))
        finally: 
            mutex.release()

    def NotifyHeadEnd(self, s_id):
        global mutex
        mutex.acquire()
        self.log("NotifyHeadEnd: "+str(s_id))
        try: 
            self.eventPublisher.HeadFinished(s_id)
        except Exception, Argument:
            self.log("NotifyHeadEnd Failed: "+str(Argument))
        finally: 
            mutex.release()

    def NotifyAnimationList(self, a_s_animations):
        global mutex
        mutex.acquire()
        self.log("NotifyAnimationList")
        try: 
            self.eventPublisher.AnimationsList(a_s_animations)
        except Exception, Argument:
            self.log("NotifyAnimationList Failed: "+str(Argument))
        finally: 
            mutex.release()

    def NotifyEyeShapeList(self, a_s_eyeShapes):
        global mutex
        mutex.acquire()
        self.log("NotifyEyeShapeList")
        try: 
            self.eventPublisher.EyeShapeList(a_s_eyeShapes)
        except Exception, Argument:
            self.log("NotifyEyeShapeList Failed: "+str(Argument))
        finally: 
            mutex.release()

    def Debug(self,string):
        if (True):
            print "DEBUG>> "+string

global BehaviorWatcher
BehaviorWatcher = behaviorModule.BehaviorWatcherModule("BehaviorWatcher")
global SpeechWatcher
SpeechWatcher = speechModule.SpeechWatcherModule("SpeechWatcher")
global SpeechRecoWatcher
SpeechRecoWatcher = speechRecognitionModule.SpeechRecoWatcherModule("SpeechRecoWatcher")
global VisionRecognizer
VisionRecognizer = visionModule.VisionRecognizerModule("VisionRecognizer")
global SensorsWatcher
SensorsWatcher = sensorsModule.SensorsWatcherModule("SensorsWatcher")
global SoundLocator
SoundLocator = soundLocationModule.SoundLocatorModule("SoundLocator")

ipArg = 1
global usingNutty
usingNutty = False
address = ""
if (len(sys.argv) > 1):
    if (sys.argv[1] == "nutty"):
        usingNutty = True
        if (len(sys.argv) > 2):
            address = sys.argv[2]
    else:
        address = sys.argv[1]
print "Will connect with Thalamus Client at: "+address
global naoqiXmlRpcServer
naoqiXmlRpcServer = NaoqiXmlrpc(address)
#naoqiXmlRpcServer = NaoqiXmlrpc()


print "Press any key to quit"
raw_input()
naoqiXmlRpcServer.stop()

