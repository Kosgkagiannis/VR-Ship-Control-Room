using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


// 1. This script is attached to a HEAD.
// 2. Update function calls raycast and if gaze for 5 secons to a widget that is not working fix it.


public class SimpleGazeCursor : RangeInstrument
{
    public Camera viewCamera;
    public bool sawIt = false;
    public float maxCursorDistance = 30;
    private readonly float timerMax = 5f;
    private float timer = 0f;
    Ray ray;
    RaycastHit hit;
    public GameObject notificationCanvasList;

    void Update()
    {
        UpdateCursor();
    }


    private void UpdateCursor()
    {

        
        ray = new Ray(viewCamera.transform.position, viewCamera.transform.rotation * Vector3.forward);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {

            GameObject instrument = hit.collider.gameObject;
            string instrumentType = getComponentCoroutineOfInstrument(instrument);


            bool isWorking = true;
            string sequenceId = "";
            if (instrumentType != null)
            {
                if (instrumentType.Equals("NeedleRotation"))
                {
                    isWorking = instrument.GetComponent<NeedleRotation>().isWorking;
                    sequenceId = instrument.GetComponent<NeedleRotation>().sequenceId;
                }
                else if (instrumentType.Equals("NumberGenerator"))
                {
                    isWorking = instrument.GetComponent<NumberGenerator>().isWorking;
                    sequenceId = instrument.GetComponent<NumberGenerator>().sequenceId;
                }
                else if (instrumentType.Equals("BarDrop"))
                {
                    isWorking = instrument.GetComponent<BarDrop>().isWorking;
                    sequenceId = instrument.GetComponent<BarDrop>().sequenceId;
                }
                else if (instrumentType.Equals("CounterStart"))
                {
                    isWorking = instrument.GetComponent<CounterStart>().isWorking;
                    sequenceId = instrument.GetComponent<CounterStart>().sequenceId;
                }
            }

            timer += Time.deltaTime;

            if (!isWorking)
            {
                if (timer >= 1f)
                {
                    notificationCanvasList.transform.Find("Indicator").gameObject.active = true;
                    notificationCanvasList.transform.Find("Indicator").gameObject.GetComponent<TextMesh>().text = "(" + timer.ToString("0") + ")";
                }
                    
            }
            else {
                notificationCanvasList.transform.Find("Indicator").gameObject.active = false;
                timer = 0;
            }

            if (timer >= timerMax)
            {
               
                if (!isWorking) {
                 
                    if (instrumentType != null)
                    {
                        if (instrumentType.Equals("NeedleRotation"))
                        {
                            instrument.GetComponent<NeedleRotation>().isWorking = true;
                            instrument.GetComponent<NeedleRotation>().needNotification = false;
                        }
                        else if (instrumentType.Equals("NumberGenerator"))
                        {
                            instrument.GetComponent<NumberGenerator>().isWorking = true;
                            instrument.GetComponent<NumberGenerator>().needNotification = false;
                        }
                        else if (instrumentType.Equals("BarDrop"))
                        {
                            instrument.GetComponent<BarDrop>().isWorking = true;
                            instrument.GetComponent<BarDrop>().needNotification = false;
                            instrument.GetComponent<BarDrop>().isFixed = true;
                        }
                        else if (instrumentType.Equals("CounterStart"))
                        {
                            instrument.GetComponent<CounterStart>().isWorking = true;
                            instrument.GetComponent<CounterStart>().needNotification = false;
                        }
                    }

                   
                    DecreaseBrocken();
                    
                    long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
                    FileRecord fR = new FileRecord();
                    fR.setTimestamp(unixTime);
                    fR.setNotificationId(sequenceId);
                    fR.setLivingErrors(InstrumentManager_v1.ReturnLivingErrors());
                    FileRecord.AddFileDismissal(fR);

                    notificationCanvasList.transform.Find("Indicator").gameObject.active = false;
              
                }
                timer = 0;
            }
        }
        else
        {
            notificationCanvasList.transform.Find("Indicator").gameObject.active = false;
            timer = 0;
        }
    }


    private string getComponentCoroutineOfInstrument(GameObject instrument)
    {

        if (instrument.GetComponent<NeedleRotation>() != null)
        {
            return "NeedleRotation";
        }
        else if (instrument.GetComponent<NumberGenerator>() != null)
        {
            return "NumberGenerator";
        }
        else if (instrument.GetComponent<BarDrop>() != null)
        {
            return "BarDrop";
        }
        else if (instrument.GetComponent<CounterStart>() != null)
        {
            return "CounterStart";
        }
        else
        {
            return null;
        }
    }

}
 