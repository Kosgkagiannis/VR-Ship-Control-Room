using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// 1. This script is attached to a HEAD.
// 2. Update function calls raycast and if gaze for 5 secons to a widget that is not working fix it.


public class SimpleGazeCursor_V2 : RangeInstrument
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
            
            bool isWorking = true;
            string sequenceIdV2 = "";

            if (instrument.GetComponent<RangeInstrument>() != null) {
                isWorking = instrument.GetComponent<RangeInstrument>().isOperational;
                sequenceIdV2 = instrument.GetComponent<RangeInstrument>().sequenceIdV2;
            }

            
            timer += Time.deltaTime;

            if (!isWorking)
            {
                if (timer >= 1f) {
                    notificationCanvasList.transform.Find("Indicator").gameObject.active = true;
                    notificationCanvasList.transform.Find("Indicator").gameObject.GetComponent<TextMesh>().text = "(" + timer.ToString("0") + ")";
                }
                
            }
            else
            {
                notificationCanvasList.transform.Find("Indicator").gameObject.active = false;
                timer = 0;
            }

            if (timer >= timerMax)
            {

                if (!isWorking)
                {


                    instrument.GetComponent<RangeInstrument>().isOperational = true;

                    if (instrument.GetComponent<Bar_V2>() != null) {
                        instrument.GetComponent<Bar_V2>().isFixed = true; // Bar_V2 to start from the top of bar
                    }

                    RepairInstrument(instrument.GetComponent<RangeInstrument>());
                
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


}
