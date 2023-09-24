using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class FileManager {

    public static void WriteToFile(int dangerLevel, int notificationId)
    {
        //Debug.Log("Mpikeeee : " + Constants.logNotificationPath);
        String path = Constants.logNotificationPath;

        long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();

        //Create File if it doesn't exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Timestamp : " + unixTime + "\nDanger level : " + dangerLevel + "\nNotification ID : "+ notificationId +"\n\n\n");
        }
        else
        {
            File.AppendAllText(path, "Timestamp : " + unixTime + "\nDanger level : " + dangerLevel + "\nNotification ID : " + notificationId + "\n\n\n");
        }


        //File.AppendAllText(logNotificationPath, "Instrument : " + newNotification.labelGameObject.GetComponent<TextMesh>().text + "\n");
        //File.AppendAllText(logNotificationPath, "\n\n");

    }

}

