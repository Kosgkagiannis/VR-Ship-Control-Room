using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
public class Constants
{
    public static string logNotificationPath = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer ? Application.persistentDataPath : Application.dataPath) + "/Notification_Log_"+((DateTimeOffset) DateTime.UtcNow).ToUnixTimeSeconds()+".csv";
    public static string logDismissalPath = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer ? Application.persistentDataPath : Application.dataPath) + "/Dismissal_Log_" + ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() + ".csv";

    public static string logNotificationPathV2  = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer ? Application.persistentDataPath : Application.dataPath) + "/Notification_Log_v2_" + ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() + ".csv";
    public static string logDismissalPathV2 = (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer ? Application.persistentDataPath : Application.dataPath) + "/Dismissal_Log_v2_" + ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds() + ".csv";

}
