﻿using UnityEngine;

namespace VRTRIX
{
    public class VRTRIXGloveGestureDetection : MonoBehaviour
    {

        [Header("GestureComponent")]
        public GameObject m_Scissors;
        public GameObject m_Rock;
        public GameObject m_Paper;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (GetScissorsButtonDown(HANDTYPE.LEFT_HAND) || GetScissorsButtonDown(HANDTYPE.RIGHT_HAND))
            {
                print("Scissors!");
                m_Scissors.GetComponent<Renderer>().materials[0].color = new Color(99f/255f, 1f, 1f, 1f);
            }
            else
            {
                m_Scissors.GetComponent<Renderer>().materials[0].color = Color.white;
            }

            if (GetRockButtonDown(HANDTYPE.LEFT_HAND) || GetRockButtonDown(HANDTYPE.RIGHT_HAND))
            {
                print("Rock!");
                m_Rock.GetComponent<Renderer>().materials[0].color = new Color(0f, 1f, 146f / 255f, 1f);
            }
            else
            {
                m_Rock.GetComponent<Renderer>().materials[0].color = Color.white;
            }

            if (GetPaperButtonDown(HANDTYPE.LEFT_HAND) || GetPaperButtonDown(HANDTYPE.RIGHT_HAND))
            //if (GetPaperButtonDown(HANDTYPE.LEFT_HAND))
            {
                print("Paper!");
                m_Paper.GetComponent<Renderer>().materials[0].color = new Color(1f, 1f, 157f / 255f, 1f);
            }
            else
            {
                m_Paper.GetComponent<Renderer>().materials[0].color = Color.white;
            }
        }


        private bool GetScissorsButtonDown(HANDTYPE tpye)
        {
            return VRTRIXGloveSimpleDataRead.GetGesture(tpye) == VRTRIXGloveGesture.BUTTONTELEPORT;
        }

        private bool GetRockButtonDown(HANDTYPE tpye)
        {
            return VRTRIXGloveSimpleDataRead.GetGesture(tpye) == VRTRIXGloveGesture.BUTTONGRAB;
        }

        private bool GetPaperButtonDown(HANDTYPE tpye)
        {
            return VRTRIXGloveSimpleDataRead.GetGesture(tpye) == VRTRIXGloveGesture.BUTTONPAPER;
        }
    }
}
