using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number_V2 : RangeInstrument
{
    public GameObject numberText;
    public int precision;
    private TextMesh textMesh;
    public bool isPercentage;
    public bool isDegrees;

    //If the app is running at 30 Frames Per Second the check counter will check every 1 seconds.
    static int maxTime = 30;
    int checkCounter = maxTime;

    private System.Random generator;


    // Start is called before the first frame update
    void Start()
    {
        generator = new System.Random();
        textMesh = numberText.GetComponent("TextMesh") as TextMesh;
    }

    // Update is called once per frame
    void Update()
    {

        if (checkCounter == 0)
        {
            if (probability >= 0)
                probability = generator.Next(0, 2);
            GenerateNumber(isOperational, probability, textMesh, precision, isPercentage, isDegrees);
            checkCounter = maxTime;
        }

        checkCounter--;

    }



    void GenerateNumber(bool isOperational, int probability, TextMesh textMesh, int precesion, bool isPercentage, bool isDegrees) {

        int randomProbability = -1;
        double number = -1;

        if (isOperational)
        {

            textMesh.color = Color.white;


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

            number = UnityEngine.Random.Range(min, max);

            if (!isOperational)
            {
                number = danger;
                textMesh.color = Color.red;
            }

            if (precesion == 1)
            {
                number = (float)Math.Round(number, 2, MidpointRounding.ToEven);
                textMesh.text = number.ToString();

            }
            else if (precesion == 0)
            {
                int intNumber = (int)number;
                textMesh.text = intNumber.ToString();
            }
            else
            {
                number = (float)Math.Round(number, precesion, MidpointRounding.ToEven);
                textMesh.text = number.ToString();
            }


            if (isPercentage)
            {
                textMesh.text = textMesh.text + "%";
            }
            if (isDegrees)
            {
                textMesh.text = textMesh.text + "\u00B0";
            }

        }
    }
}
