using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    static string ReadFile(string dPath)
    {
        string[] fileEntries = Directory.GetFiles(dPath);
        string coordStr = "0.45;0.45;0.55;0.55";
        try
        {
            string coordFilePath = fileEntries[0];
            coordStr = System.IO.File.ReadAllText(@coordFilePath);   // in format min_x;min_y;max_x;max_y.
        }
        catch (Exception e)
        {
            print(e);
        }
        return coordStr;
    }

    static Vector3 GetFacePos(string dPath)
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
            float max_y = float.Parse(boxCoords[3]);
            
            // count the middle point of the bounding box for target position
            targetX = min_x + (max_x - min_x) / 2;
            targetY = min_y + (max_y - min_y) / 2;

        }
        catch (Exception e)
        {
            print(e);
        }
        return ScaleCoordsToPos(targetX, targetY);
    }

    static Vector3 ScaleCoordsToPos(float x, float y)
    {
        // calibration parameters
        float sceneWidth = 6.0f;
        float sceneHeight = 3.0f;

        // Find the center for the scene coordinate system
        var camera = GameObject.Find("Camera");
        Vector3 cameraCenter = camera.transform.position;

        // the center of the input coordinate system
        float coordCenter = 0.5f;

        // Count the desired distance from the cameracenter for the target
        float xOffset = (x - coordCenter) * sceneWidth;
        float sceneX = cameraCenter.x + xOffset;

        float yOffset = (y - coordCenter) * sceneHeight;
        float sceneY = cameraCenter.y - yOffset;

        float sceneZ = 16.0f;

        return new Vector3(sceneX, sceneY, sceneZ);

    }

    IEnumerator currentMoveCoroutine;
    Vector3 destination;

    // Update is called once per frame
    void Update()
    {
        string targetDirectory = @"E:\Opinnot\Kandi\Avatar\AvatarFace\Face render\Test"; // replace with directory path for face detection results.
        Vector3 facePos = GetFacePos(targetDirectory);
        
        if (facePos != destination) // new face coordinates received
        {
            destination = facePos;
            // stop old coroutine if not yet completed
            if (currentMoveCoroutine != null)
            {
                StopCoroutine(currentMoveCoroutine);
            }
            currentMoveCoroutine = MoveTarget(destination, 4);
            StartCoroutine(currentMoveCoroutine);
        }
    }

    // coroutine to move the target smoothly towards the face position
    IEnumerator MoveTarget(Vector3 destination, float speed)
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;
        }
    }
    
}
