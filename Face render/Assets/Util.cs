using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static string ReadFile(string dPath)
    {
        string[] fileEntries = Directory.GetFiles(dPath);
        string readStr = "";
        try
        {
            string coordFilePath = fileEntries[0];
            readStr = System.IO.File.ReadAllText(@coordFilePath);   // in the format min_x;min_y;max_x;max_y.
        }
        catch (Exception e)
        {
            print(e);
        }
        return readStr;
    }
}
