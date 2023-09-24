using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Counter_V2 : RangeInstrument
{
    public GameObject clock;
    private TextMesh textMesh;
    public int hour;
    public int minutes;
    public int seconds;

    private System.Random generator;

    // Start is called before the first frame update
    void Start()
    {
        generator = new System.Random();
        textMesh = clock.GetComponent("TextMesh") as TextMesh;
        StartCoroutine(CounterStart(probability, textMesh, hour, minutes, seconds));
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    IEnumerator CounterStart(int probability, TextMesh textMesh, int hour, int minutes, int seconds)
    {

        int randomProbability = -1;

        while (true) {
            if (probability >= 0)
                probability = generator.Next(0, 2);
            if (isOperational)
            {             
                textMesh.color = Color.white;

                randomProbability = UnityEngine.Random.Range(1, maxRandomProbability_V2);
                if (randomProbability <= probability && Time.realtimeSinceStartup > 30f && Time.realtimeSinceStartup < 180f && InstrumentFailureManager.ReturnTotalErrors() < 10)
                {
                 
                    dangerLevel = UnityEngine.Random.Range(1, 4);

                    BreakDown();
                    isOperational = false;
                    textMesh.color = Color.red;

                    // ---------------------------------------

                    long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();

                    //sequenceIdV2 = 15000 + InstrumentFailureManager.ReturnTotalErrors();
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
                    fR.setInstrumentValue(textMesh.text);
                    fR.setAngleView(angleDeg.ToString().Replace(',', '.'));

                    FileRecord.AddFileRecord(fR);

                    // ---------------------------------------


                }

                if (seconds <= 59 && seconds > 0)
                {
                    seconds--;
                }
                else if (seconds == 0)
                {
                    seconds = 59;
                    if (minutes <= 59 && minutes > 0)
                    {
                        minutes--;
                    }
                    else if (minutes == 0)
                    {
                        minutes = 59;
                        hour--;
                    }
                }

                textMesh.text = hour.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString();

            }
            
            yield return new WaitForSeconds(1);
        }          
    }




}
