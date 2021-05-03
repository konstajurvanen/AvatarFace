using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using static Util;

public class JawMove : MonoBehaviour
{
    
    static string TARGET_DIRECTORY = Path.Combine("LipSync");
    // Calibration parameters:
    static float HEAD_JAW_Y_OFFSET = -3.0f; 
    static float JAW_SPEED = 15;

    // helpers to adapt to head rotations
    GameObject headTarget;
    Vector3 headAimPos;
    IEnumerator currentJawCoroutine = null;

    void Start()
    {
        headTarget = GameObject.Find("Target");
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpeaking())
        {
            if (currentJawCoroutine == null)
            {
                currentJawCoroutine = Speak();
                StartCoroutine(currentJawCoroutine);
            }
        }
        else if (currentJawCoroutine != null)
        {
            StopCoroutine(currentJawCoroutine);
            currentJawCoroutine = null;
        }
    }

    // oscillates the target according to a sine function on the y-axis
    IEnumerator Speak()
    {
        while(true)
        {
            headAimPos = headTarget.transform.position;
            float middleY = headAimPos.y + HEAD_JAW_Y_OFFSET;
            transform.position = new Vector3(0f, middleY + Mathf.Sin(Time.time*JAW_SPEED), headAimPos.z);
            yield return new WaitForEndOfFrame();
        }
    }

    static bool isSpeaking()
    {
        string inputStr = Util.ReadFile(TARGET_DIRECTORY);
        bool speak = (inputStr == "1" || inputStr == "true");
        return speak;
    }
}
