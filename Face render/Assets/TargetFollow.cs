using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using static Util;

/*
 * The program expects the input coordinates to be in format min_x;min_y;max_x;max_y,
 * where the values are decimal numbers between 0.0 and 1.0. 
 * 
 * The directory for the input coordinate file is expected to have only the single text file for coordinates.
 */

public class TargetFollow : MonoBehaviour
{
    static string TARGET_DIRECTORY = Path.Combine("FaceCoordinates");

    // calibration parameters
    static float SCENE_WIDTH = 6.0f;
    static float SCENE_HEIGHT = 3.0f;
    static float DEFAULT_Z_VALUE = 16.0f;
    static float TARGET_MOVE_SPEED = 3;

    // the center of the input coordinate system
    static float COORDINATE_CENTER = 0.5f;

    IEnumerator currentMoveCoroutine;
    Vector3 destination;


    // Update is called once per frame
    void Update()
    {
        Vector3 facePos = GetFacePos();
        
        if (facePos != destination) // new face coordinates received
        {
            destination = facePos;
            // stop old coroutine if not yet completed
            if (currentMoveCoroutine != null)
            {
                StopCoroutine(currentMoveCoroutine);
            }
            currentMoveCoroutine = MoveTarget(destination, TARGET_MOVE_SPEED);
            StartCoroutine(currentMoveCoroutine);
        }
    }

    // coroutine to move the target smoothly towards the face position
    private IEnumerator MoveTarget(Vector3 destination, float speed)
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;
        }
    }

    private Vector3 GetFacePos()
    {
        float targetX = 0f;
        float targetY = 0f;

        try
        {
            string boundingBox = Util.ReadFile(TARGET_DIRECTORY);
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

    private Vector3 ScaleCoordsToPos(float x, float y)
    {
        // Find the center for the scene coordinate system
        var camera = GameObject.Find("Camera");
        Vector3 cameraCenter = camera.transform.position;

        // Count the desired distance from the cameracenter for the target
        float xOffset = (x - COORDINATE_CENTER) * SCENE_WIDTH;
        float sceneX = cameraCenter.x + xOffset;

        float yOffset = (y - COORDINATE_CENTER) * SCENE_HEIGHT;
        float sceneY = cameraCenter.y - yOffset;

        float sceneZ = DEFAULT_Z_VALUE;

        return new Vector3(sceneX, sceneY, sceneZ);
    }

}
