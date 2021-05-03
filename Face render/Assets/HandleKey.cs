using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleKey : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (Input.GetKey("q"))
        {
            Application.Quit();
        }
    }
}
