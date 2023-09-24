using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogClock : MonoBehaviour
{
    public GameObject MinuteNeedle;
    public GameObject HourNeedle;
    private Transform MinuteNeedleTransform;
    private Transform HourNeedleTransform;
    public bool isLocal;
    public bool isUTC;

    void Start()
    {
        MinuteNeedleTransform = MinuteNeedle.GetComponent<Transform>();
        HourNeedleTransform = HourNeedle.GetComponent<Transform>();

    }
    // Update is called once per frame
    void Update()
    {
        System.DateTime currentTime = System.DateTime.Now; // Get Local time from system
        float minutes = (float)currentTime.Minute;// Get minutes
        float hour = (float)currentTime.Hour; // Get hours

        if (isUTC) {

            // If hour is 0 or 1 we prevent to get hour -2 or -1
            if (hour == 0) {
                hour = 21; 
            }
            else if (hour == 1){
                hour = 22;
            }
            else if (hour == 2)
            {
                hour = 23;
            }
            else {
                hour = hour - 3; //Local time in Greece is UTC+3
            }

            
        }

        //Calculate degrees to rotate needles based on minutes and hours
        float minuteAnlge = ((-1) * 6 * minutes) + 90;
        float hourAnlge = 90-hour*30;

        //Rotate both hour and minutes needles
        MinuteNeedleTransform.localRotation = Quaternion.Euler(0, 0, minuteAnlge);
        HourNeedleTransform.localRotation = Quaternion.Euler(0, 0, hourAnlge);
    }
}
