using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle_V2 : RangeInstrument
{

    public GameObject needle;
    private Transform needleTransform;


    //If the app is running at 30 Frames Per Second the check counter will check every 1 seconds.
    static int maxTime = 30;
    int checkCounter = maxTime;

    private System.Random generator;

    // Start is called before the first frame update
    void Start()
    {
        generator = new System.Random();
        needleTransform = needle.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (checkCounter == 0)
        {
            if (probability >= 0)
                probability = generator.Next(0, 2);
            RotateNeedle(isOperational, probability, min, max);
            checkCounter = maxTime;
        }

        checkCounter--;
    }




    void RotateNeedle(bool isOperational, int probability, int zMin, int zMax) {

        int randomProbability = -1;
        int zRotation = -1;

        if (isOperational)
        {

            // Propability implementation 
            randomProbability = UnityEngine.Random.Range(1, maxRandomProbability_V2);

            if (randomProbability <= probability && Time.realtimeSinceStartup > 30f && Time.realtimeSinceStartup < 180f && InstrumentFailureManager.ReturnTotalErrors() < 10)
            {
                
                dangerLevel = UnityEngine.Random.Range(1, 4);

                BreakDown(); //OnInstrumentBrocken Action
                isOperational = false;


                // ---------------------------------------

                long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();

                sequenceIdV2 = guid.ToString();

                GameObject head = GameObject.Find("/Head/Main Camera");
                Camera mainCamera = head.GetComponent<Camera>();
                float sideA = Vector3.Distance(mainCamera.transform.position, new Vector3(this.transform.position.x, mainCamera.transform.position.y, this.transform.position.z));
                float sideB = this.transform.position.y - mainCamera.transform.position.y;
                //Vertical difference
                float angleRad = Mathf.Atan(sideB / sideA);
                //Or if degrees are needed
                float angleDeg = angleRad * Mathf.Rad2Deg;

                FileRecord fR = new FileRecord();
                fR.setTimestamp(unixTime);
                fR.setNotificationId(sequenceIdV2);
                fR.setInstrumentName(labelGameObject.GetComponent<TextMesh>().text);
                fR.setLivingErrors(InstrumentFailureManager.ReturnLivingErrors());
                fR.setDangerLevel(dangerLevel);
                fR.setInstrumentValue(danger.ToString());
                fR.setAngleView(angleDeg.ToString().Replace(',', '.'));

                FileRecord.AddFileRecord(fR);

                // ---------------------------------------



            }

            zRotation = UnityEngine.Random.Range(zMin, zMax);

            if (!isOperational)
            {
                zRotation = danger;
            }

            needleTransform.eulerAngles = new Vector3(needleTransform.rotation.eulerAngles.x, needleTransform.rotation.eulerAngles.y, zRotation);
        }

    }

}
