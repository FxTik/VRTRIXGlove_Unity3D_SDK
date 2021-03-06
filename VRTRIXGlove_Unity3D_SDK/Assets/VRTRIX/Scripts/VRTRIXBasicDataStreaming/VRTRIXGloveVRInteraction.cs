﻿//============= Copyright (c) VRTRIX INC, All rights reserved. ================
//
// Purpose: Example CSharp script to read data stream under VR environment and
//          perform gesture recognition/ other basic interactions. Other VRTRIX
//          hardware APIs (including shield&sword, extinguisher etc)are also 
//          included in this script so that you will be able to interact with
//          them using our gloves in virtual reality world!
//          To know more about our other products & solutions, please refer to
//          our websit:
//          https://www.vrtrix.com.cn
//
//=============================================================================
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using Valve.VR;
using System;

namespace VRTRIX
{
    public class VRTRIXGloveVRInteraction : MonoBehaviour
    {
        public bool AdvancedMode;
        public static VRTRIXGloveGesture LH_Gesture, RH_Gesture;
        public static VRTRIXDataWrapper RH;
        public static VRTRIXDataWrapper LH;
        private static GameObject LH_tracker, RH_tracker;
        private static bool LH_Mode, RH_Mode;
        private Thread LH_Thread_read, RH_Thread_read, LH_receivedData, RH_receivedData;
        private VRTRIXGloveRunningMode Mode;

        private Quaternion qloffset = Quaternion.identity;
        private Quaternion qroffset = Quaternion.identity;
        private bool qroffset_cal = false;
        private bool qloffset_cal = false;
        private Vector3 troffset = new Vector3(0.01f, 0, -0.035f);
        private Vector3 tloffset = new Vector3(-0.01f, 0, -0.035f);
        private const float degToRad = (float)(Math.PI / 180.0);
        
        void Start()
        {
            RH = new VRTRIXDataWrapper(AdvancedMode);
            LH = new VRTRIXDataWrapper(AdvancedMode);
            try
            {
                RH_tracker = CheckDeviceModelName(HANDTYPE.RIGHT_HAND);
                LH_tracker = CheckDeviceModelName(HANDTYPE.LEFT_HAND);
            }
            catch (Exception e)
            {
                print("Exception caught: " + e);
            }

        }
        void CheckToStart()
        {
            try
            {
                if(RH_tracker != null)
                {
                    RH_Mode = RH.Init(HANDTYPE.RIGHT_HAND);
                    ReceiveRHData();
                }
               
                if(LH_tracker != null)
                {
                    print(LH_tracker);
                    LH_Mode = LH.Init(HANDTYPE.LEFT_HAND);
                    ReceiveLHData();
                }
            }
            catch (Exception e)
            {
                print("Exception caught: " + e);
            }
        }

        void Update()
        {
            if (RH_Mode && LH_Mode)
            {
                Mode = VRTRIXGloveRunningMode.PAIR;
            }
            else if (RH_Mode)
            {
                Mode = VRTRIXGloveRunningMode.RIGHT;
            }
            else if (LH_Mode)
            {
                Mode = VRTRIXGloveRunningMode.LEFT;
            }
            else
            {
                Mode = VRTRIXGloveRunningMode.NONE;
            }
        }
        void FixedUpdate()
        {

            if (RH_Mode && RH.GetReceivedStatus() == VRTRIXGloveStatus.NORMAL)
            {
                if (RH.GetReceivedRotation(VRTRIXBones.R_Hand) != Quaternion.identity && !qroffset_cal)
                {
                    qroffset = GetOffset(RH_tracker, RH, HANDTYPE.RIGHT_HAND);
                    qroffset_cal = true;
                }

                SetPosition(VRTRIXBones.R_Hand, RH_tracker.transform.position, RH_tracker.transform.rotation, troffset);

                SetRotation(VRTRIXBones.R_Forearm, RH.GetReceivedRotation(VRTRIXBones.R_Forearm), RH.DataValidStatus(VRTRIXBones.R_Forearm), HANDTYPE.RIGHT_HAND);
                SetRotation(VRTRIXBones.R_Hand, RH.GetReceivedRotation(VRTRIXBones.R_Hand), RH.DataValidStatus(VRTRIXBones.R_Hand), HANDTYPE.RIGHT_HAND);

                SetRotation(VRTRIXBones.R_Thumb_1, RH.GetReceivedRotation(VRTRIXBones.R_Thumb_1), RH.DataValidStatus(VRTRIXBones.R_Thumb_1), HANDTYPE.RIGHT_HAND);
                SetRotation(VRTRIXBones.R_Thumb_2, RH.GetReceivedRotation(VRTRIXBones.R_Thumb_2), RH.DataValidStatus(VRTRIXBones.R_Thumb_2), HANDTYPE.RIGHT_HAND);
                SetRotation(VRTRIXBones.R_Thumb_3, RH.GetReceivedRotation(VRTRIXBones.R_Thumb_3), RH.DataValidStatus(VRTRIXBones.R_Thumb_3), HANDTYPE.RIGHT_HAND);

                SetRotation(VRTRIXBones.R_Index_1, RH.GetReceivedRotation(VRTRIXBones.R_Index_1), RH.DataValidStatus(VRTRIXBones.R_Index_1), HANDTYPE.RIGHT_HAND);
                SetRotation(VRTRIXBones.R_Index_2, RH.GetReceivedRotation(VRTRIXBones.R_Index_2), RH.DataValidStatus(VRTRIXBones.R_Index_2), HANDTYPE.RIGHT_HAND);
                SetRotation(VRTRIXBones.R_Index_3, RH.GetReceivedRotation(VRTRIXBones.R_Index_3), RH.DataValidStatus(VRTRIXBones.R_Index_3), HANDTYPE.RIGHT_HAND);

                SetRotation(VRTRIXBones.R_Middle_1, RH.GetReceivedRotation(VRTRIXBones.R_Middle_1), RH.DataValidStatus(VRTRIXBones.R_Middle_1), HANDTYPE.RIGHT_HAND);
                SetRotation(VRTRIXBones.R_Middle_2, RH.GetReceivedRotation(VRTRIXBones.R_Middle_2), RH.DataValidStatus(VRTRIXBones.R_Middle_2), HANDTYPE.RIGHT_HAND);
                SetRotation(VRTRIXBones.R_Middle_3, RH.GetReceivedRotation(VRTRIXBones.R_Middle_3), RH.DataValidStatus(VRTRIXBones.R_Middle_3), HANDTYPE.RIGHT_HAND);

                SetRotation(VRTRIXBones.R_Ring_1, RH.GetReceivedRotation(VRTRIXBones.R_Ring_1), RH.DataValidStatus(VRTRIXBones.R_Ring_1), HANDTYPE.RIGHT_HAND);
                SetRotation(VRTRIXBones.R_Ring_2, RH.GetReceivedRotation(VRTRIXBones.R_Ring_2), RH.DataValidStatus(VRTRIXBones.R_Ring_2), HANDTYPE.RIGHT_HAND);
                SetRotation(VRTRIXBones.R_Ring_3, RH.GetReceivedRotation(VRTRIXBones.R_Ring_3), RH.DataValidStatus(VRTRIXBones.R_Ring_3), HANDTYPE.RIGHT_HAND);

                SetRotation(VRTRIXBones.R_Pinky_1, RH.GetReceivedRotation(VRTRIXBones.R_Pinky_1), RH.DataValidStatus(VRTRIXBones.R_Pinky_1), HANDTYPE.RIGHT_HAND);
                SetRotation(VRTRIXBones.R_Pinky_2, RH.GetReceivedRotation(VRTRIXBones.R_Pinky_2), RH.DataValidStatus(VRTRIXBones.R_Pinky_2), HANDTYPE.RIGHT_HAND);
                SetRotation(VRTRIXBones.R_Pinky_3, RH.GetReceivedRotation(VRTRIXBones.R_Pinky_3), RH.DataValidStatus(VRTRIXBones.R_Pinky_3), HANDTYPE.RIGHT_HAND);

                RH_Gesture = VRTRIXGloveGestureRecognition.GestureDetection(RH, HANDTYPE.RIGHT_HAND);

            }



            if (LH_Mode && LH.GetReceivedStatus() == VRTRIXGloveStatus.NORMAL)
            {
                if (LH.GetReceivedRotation(VRTRIXBones.L_Hand) != Quaternion.identity && !qloffset_cal)
                {
                    qloffset = GetOffset(LH_tracker, LH, HANDTYPE.LEFT_HAND);
                    qloffset_cal = true;
                }
                
                SetPosition(VRTRIXBones.L_Hand, LH_tracker.transform.position, LH_tracker.transform.rotation, tloffset);

                SetRotation(VRTRIXBones.L_Forearm, LH.GetReceivedRotation(VRTRIXBones.L_Forearm), LH.DataValidStatus(VRTRIXBones.L_Forearm), HANDTYPE.LEFT_HAND);
                SetRotation(VRTRIXBones.L_Hand, LH.GetReceivedRotation(VRTRIXBones.L_Hand), LH.DataValidStatus(VRTRIXBones.L_Hand), HANDTYPE.LEFT_HAND);

                SetRotation(VRTRIXBones.L_Thumb_1, LH.GetReceivedRotation(VRTRIXBones.L_Thumb_1), LH.DataValidStatus(VRTRIXBones.L_Thumb_1), HANDTYPE.LEFT_HAND);
                SetRotation(VRTRIXBones.L_Thumb_2, LH.GetReceivedRotation(VRTRIXBones.L_Thumb_2), LH.DataValidStatus(VRTRIXBones.L_Thumb_2), HANDTYPE.LEFT_HAND);
                SetRotation(VRTRIXBones.L_Thumb_3, LH.GetReceivedRotation(VRTRIXBones.L_Thumb_3), LH.DataValidStatus(VRTRIXBones.L_Thumb_3), HANDTYPE.LEFT_HAND);

                SetRotation(VRTRIXBones.L_Index_1, LH.GetReceivedRotation(VRTRIXBones.L_Index_1), LH.DataValidStatus(VRTRIXBones.L_Index_1), HANDTYPE.LEFT_HAND);
                SetRotation(VRTRIXBones.L_Index_2, LH.GetReceivedRotation(VRTRIXBones.L_Index_2), LH.DataValidStatus(VRTRIXBones.L_Index_2), HANDTYPE.LEFT_HAND);
                SetRotation(VRTRIXBones.L_Index_3, LH.GetReceivedRotation(VRTRIXBones.L_Index_3), LH.DataValidStatus(VRTRIXBones.L_Index_3), HANDTYPE.LEFT_HAND);

                SetRotation(VRTRIXBones.L_Middle_1, LH.GetReceivedRotation(VRTRIXBones.L_Middle_1), LH.DataValidStatus(VRTRIXBones.L_Middle_1), HANDTYPE.LEFT_HAND);
                SetRotation(VRTRIXBones.L_Middle_2, LH.GetReceivedRotation(VRTRIXBones.L_Middle_2), LH.DataValidStatus(VRTRIXBones.L_Middle_2), HANDTYPE.LEFT_HAND);
                SetRotation(VRTRIXBones.L_Middle_3, LH.GetReceivedRotation(VRTRIXBones.L_Middle_3), LH.DataValidStatus(VRTRIXBones.L_Middle_3), HANDTYPE.LEFT_HAND);

                SetRotation(VRTRIXBones.L_Ring_1, LH.GetReceivedRotation(VRTRIXBones.L_Ring_1), LH.DataValidStatus(VRTRIXBones.L_Ring_1), HANDTYPE.LEFT_HAND);
                SetRotation(VRTRIXBones.L_Ring_2, LH.GetReceivedRotation(VRTRIXBones.L_Ring_2), LH.DataValidStatus(VRTRIXBones.L_Ring_2), HANDTYPE.LEFT_HAND);
                SetRotation(VRTRIXBones.L_Ring_3, LH.GetReceivedRotation(VRTRIXBones.L_Ring_3), LH.DataValidStatus(VRTRIXBones.L_Ring_3), HANDTYPE.LEFT_HAND);

                SetRotation(VRTRIXBones.L_Pinky_1, LH.GetReceivedRotation(VRTRIXBones.L_Pinky_1), LH.DataValidStatus(VRTRIXBones.L_Pinky_1), HANDTYPE.LEFT_HAND);
                SetRotation(VRTRIXBones.L_Pinky_2, LH.GetReceivedRotation(VRTRIXBones.L_Pinky_2), LH.DataValidStatus(VRTRIXBones.L_Pinky_2), HANDTYPE.LEFT_HAND);
                SetRotation(VRTRIXBones.L_Pinky_3, LH.GetReceivedRotation(VRTRIXBones.L_Pinky_3), LH.DataValidStatus(VRTRIXBones.L_Pinky_3), HANDTYPE.LEFT_HAND);

                LH_Gesture = VRTRIXGloveGestureRecognition.GestureDetection(LH, HANDTYPE.LEFT_HAND);
            }
        }
        private void ReceiveLHData()
        {
            //LH_Mode = LH.Init(HANDTYPE.LEFT_HAND);
            print("LH_Mode: " + LH_Mode);
            if (LH_Mode)
            {
                LH_Thread_read = new Thread(LH.streaming_read_begin);
                LH_Thread_read.Start();
                LH_receivedData = new Thread(ReceiveLHDataAsync);
                LH_receivedData.Start();
                //LH.receivedData(HANDTYPE.LEFT_HAND);
            }
        }


        private void ReceiveRHData()
        {
            //RH_Mode = RH.Init(HANDTYPE.RIGHT_HAND);
            print("RH_Mode: " + RH_Mode);
            if (RH_Mode)
            {
                RH_Thread_read = new Thread(RH.streaming_read_begin);
                RH_Thread_read.Start();
                RH_receivedData = new Thread(ReceiveRHDataAsync);
                RH_receivedData.Start();
                //RH.receivedData(HANDTYPE.RIGHT_HAND);
            }
        }

        private static void ReceiveLHDataAsync()
        {
            LH.receivedData(HANDTYPE.LEFT_HAND);
        }

        private static void ReceiveRHDataAsync()
        {
            RH.receivedData(HANDTYPE.RIGHT_HAND);
        }


        void OnApplicationQuit()
        {
            if (LH_Mode)
            {
                //LH_Thread.Abort();
                LH.ClosePort();
            }
            if (RH_Mode)
            {
                //RH_Thread.Abort();
                RH.ClosePort();
            }
        }


        void OnGUI()
        {
            if (GUI.Button(new Rect(0, Screen.height / 8, Screen.width / 8, Screen.height / 8), "Reset"))
            {
                if (LH_Mode)
                {
                    LH.alignmentCheck(HANDTYPE.LEFT_HAND);
                }
                if (RH_Mode)
                {
                    RH.alignmentCheck(HANDTYPE.RIGHT_HAND);
                }
            }

            if (LH.GetReceivedStatus() == VRTRIXGloveStatus.CLOSED && RH.GetReceivedStatus() == VRTRIXGloveStatus.CLOSED)
            {
                if (GUI.Button(new Rect(0, 0, Screen.width / 8, Screen.height / 8), "Connect"))
                {
                    //ThreadPool.QueueUserWorkItem(CheckToStart);
                    CheckToStart();
                }
            }

            if (LH.GetReceivedStatus() == VRTRIXGloveStatus.NORMAL || RH.GetReceivedStatus() == VRTRIXGloveStatus.NORMAL)
            {
                if (GUI.Button(new Rect(0, 0, Screen.width / 8, Screen.height / 8), "Pause"))
                {
                    if (LH_Mode)
                    {
                        LH.SetReceivedStatus(VRTRIXGloveStatus.PAUSED);
                    }
                    if (RH_Mode)
                    {
                        RH.SetReceivedStatus(VRTRIXGloveStatus.PAUSED);
                    }
                }
            }

            if (LH.GetReceivedStatus() == VRTRIXGloveStatus.PAUSED || RH.GetReceivedStatus() == VRTRIXGloveStatus.PAUSED)
            {
                if (GUI.Button(new Rect(0, 0, Screen.width / 8, Screen.height / 8), "Resume"))
                {
                    if (LH_Mode)
                    {
                        LH.SetReceivedStatus(VRTRIXGloveStatus.NORMAL);
                    }
                    if (RH_Mode)
                    {
                        RH.SetReceivedStatus(VRTRIXGloveStatus.NORMAL);
                    }
                }
            }

            if (GUI.Button(new Rect(0, Screen.height / 4, Screen.width / 8, Screen.height / 8), "Calibrate"))
            {
                if (LH_Mode)
                {
                    LH.calibration();
                }
                if (RH_Mode)
                {
                    RH.calibration();
                }
            }

            if (GUI.Button(new Rect(0, Screen.height * (3.0f / 8.0f), Screen.width / 8, Screen.height / 8), "Vibrate"))
            {
                if (LH_Mode)
                {
                    LH.vibrate();
                }
                if (RH_Mode)
                {
                    RH.vibrate();
                }
            }

        }
       

        private void SetRotation(VRTRIXBones bone, Quaternion rotation, bool valid, HANDTYPE type)
        {
            string bone_name = VRTRIXUtilities.GetBoneName((int)bone);
            Quaternion qr = new Quaternion(1f, 0f, 0f, 0f);
   
            GameObject obj = GameObject.Find(bone_name);
            if (obj != null)   
            {
                if (!float.IsNaN(rotation.x) && !float.IsNaN(rotation.y) && !float.IsNaN(rotation.z) && !float.IsNaN(rotation.w))
                {
                    if (valid)
                    {
                        if(type == HANDTYPE.LEFT_HAND)
                        {    
                            obj.transform.rotation = qloffset * (rotation * qr);
                        }
                        else if(type == HANDTYPE.RIGHT_HAND)
                        {
                            obj.transform.rotation = qroffset * (new Quaternion(0f, 1f, 0f, 0f) * rotation * qr);
                            
                        }
                    }
                }
            }
        }

        private void SetPosition(VRTRIXBones bone, Vector3 pos, Quaternion rot, Vector3 offset)
        {
            string bone_name = VRTRIXUtilities.GetBoneName((int)bone);
            GameObject obj = GameObject.Find(bone_name);
            if (obj != null)
            {
                obj.transform.position = pos + rot* offset;
            }
        }

       

        private static Quaternion GetOffset(GameObject tracker, VRTRIXDataWrapper glove, HANDTYPE type)
        {
            //print("IMU: " + glove.GetReceivedRotation(VRTRIXBones.L_Hand).eulerAngles.y);
            //print("Tracker: " + tracker.transform.rotation.eulerAngles.y);
            float offset = glove.GetReceivedRotation(VRTRIXBones.L_Hand).eulerAngles.y - tracker.transform.rotation.eulerAngles.y -180f;

            if(offset > 180f)
            {
                offset = 360f - offset;
            }else if(offset < -180f)
            {
                offset = 360f + offset;
            }
            //print(offset);
            return new Quaternion(0, Mathf.Sin(-offset * degToRad/2), 0, Mathf.Cos(-offset * degToRad / 2));
        }

        public static VRTRIXGloveGesture GetGesture (HANDTYPE type)
        {
            if (type == HANDTYPE.LEFT_HAND)
            {
                return LH_Gesture;
            }else
            {
                return RH_Gesture;
            }
        }

        public static GameObject CheckDeviceModelName(HANDTYPE type = HANDTYPE.NONE, InteractiveDevice device = InteractiveDevice.NONE)
        {
            var system = OpenVR.System;
            if (system == null)
                return null;
            for (int i = 0; i < 16; i++)
            {
                var error = ETrackedPropertyError.TrackedProp_Success;
                var capacity = system.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_RenderModelName_String, null, 0, ref error);
                if (capacity <= 1)
                {
                    continue;
                }

                var buffer = new System.Text.StringBuilder((int)capacity);
                system.GetStringTrackedDeviceProperty((uint)i, ETrackedDeviceProperty.Prop_RenderModelName_String, buffer, capacity, ref error);
                var s = buffer.ToString();
                if (type == HANDTYPE.LEFT_HAND)
                {
                    if (s.Contains("LH"))
                    {
                        return GameObject.Find("Device" + i);
                    }
                }
                else if (type == HANDTYPE.RIGHT_HAND)
                {
                    if (s.Contains("RH"))
                    {
                        return GameObject.Find("Device" + i);
                    }
                }

                else if(device == InteractiveDevice.SWORD)
                {
                    if (s.Contains("sword"))
                    {
                        GameObject sword_ref = GameObject.Find("Device" + i);
                        sword_ref.GetComponent<SteamVR_RenderModel>().enabled = false;
                        return sword_ref;
                    }
                }
                else if (device == InteractiveDevice.SHIELD)
                {
                    if (s.Contains("shield"))
                    {
                        GameObject shield_ref = GameObject.Find("Device" + i);
                        shield_ref.GetComponent<SteamVR_RenderModel>().enabled = false;
                        return shield_ref;
                    }
                }
                else if (device == InteractiveDevice.EXTINGUISHER)
                {
                    if (s.Contains("fire"))
                    {
                        GameObject ex_ref = GameObject.Find("Device" + i);
                        //ex_ref.GetComponent<SteamVR_RenderModel>().enabled = false;
                        return ex_ref;
                    }
                }
            }
            return null;
        }
    }
}


