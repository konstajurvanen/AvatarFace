using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JawMove : MonoBehaviour
{
    static bool isSpeaking()
    {
        return true; // modify to apply jaw movement only when 
    }

    float HEAD_JAW_Y_OFFSET = -2.7f;
    float JAW_SPEED = 15;

    //bool jawMovingUp = false;
    GameObject headTarget;
    Vector3 headAimPos;
    IEnumerator currentJawCoroutine = null; // helper variable to avoid conflicting coroutines

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
            transform.position = new Vector3(headAimPos.x, middleY + Mathf.Sin(Time.time*JAW_SPEED), headAimPos.z);
            print(transform.position);
            yield return new WaitForEndOfFrame();
        }
    }
}
