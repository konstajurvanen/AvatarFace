using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    public static string ReadFile(string dPath)
    {
        string[] fileEntries = Directory.GetFiles(dPath);
        string coordStr = "";
        try
        {
            if (fileEntries.Length == 0)
            {
                string newestFilePath = fileEntries[0];
                coordStr = System.IO.File.ReadAllText(@newestFilePath);// in format min_x;min_y;max_x;max_y.
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Exception caught.", e);
        }
        return coordStr;
    }

    public static Vector3 GetFacePos(string dPath)
    {
        float targetX = 0f;
        float targetY = 0f;
        
        try
        {
            string boundingBox = ReadFile(dPath);
            string[] boxCoords = boundingBox.Split(';');

            float min_x = float.Parse(boxCoords[0]);
            float min_y = float.Parse(boxCoords[1]);
            float max_x = float.Parse(boxCoords[2]);
            float max_y = float.Parse(boxCoords[4]);

            // count the middle point of the bounding box for target position
            targetX = (max_x - min_x) / 2;
            targetY = (max_y - min_y) / 2;

        }
        catch (Exception e)
        {
            Console.WriteLine("{0} Exception caught.", e);
        }
        return ScaleCoordsToPos(targetX, targetY);
    }

    public static Vector3 ScaleCoordsToPos(float x, float y)
    {
        float sceneWidth = 7.0f;
        float sceneHeight = 3.0f;
        
        var camera = GameObject.Find("Camera");
        Vector3 cameraCenter = new Vector3(0f, 1.2f, 4f);

        if (camera)
        {
            cameraCenter = camera.transform.position;
        }
        else
        {
            Console.WriteLine("No camera found.");
        }

        float coordCenter = 0.5f;

        float xOffset = (x - coordCenter) * sceneWidth;
        float sceneX = cameraCenter.x + xOffset;

        float yOffset = (y - coordCenter) * sceneHeight;
        float sceneY = cameraCenter.y + yOffset;

        float sceneZ = 4.0f;

        return new Vector3(sceneX, sceneY, sceneZ);

    }

    Vector3 tempPos;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        string targetDirectory = "/*/"; // replace with directory path for face detection results.
        Vector3 facePos = GetFacePos(targetDirectory);
        tempPos = transform.position;
        tempPos.x -= 0.01f;
        transform.position = tempPos;
    }

    
}
