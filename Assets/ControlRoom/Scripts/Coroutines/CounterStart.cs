using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CounterStart : Counter
{
    private static bool stop;
    public bool isWorking = true;
    public bool needNotification = false;
    private int randomProbability = -1;
    public string sequenceId = "";

    public IEnumerator CounterStartRoutine(TextMesh textMesh, int hour, int minutes, int seconds, int probability)
    {

        generator = new System.Random();

        while (!stop)
        {
            if (isWorking) {

                if (probability >= 0)
                    probability = generator.Next(0, 2);

                textMesh.color = Color.white;

                randomProbability = UnityEngine.Random.Range(1, maxRandomProbability_V1);

                if (randomProbability <= probability && Time.realtimeSinceStartup > 30f && Time.realtimeSinceStartup < 180f && InstrumentManager_v1.ReturnTotalErrors() < 10)
                {
                    
                    isWorking = false;
                    needNotification = true;
                    textMesh.color = Color.red;

                    // ----------------------------------

                    long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();

                    //sequenceId = (15000 + InstrumentManager_v1.ReturnTotalErrors()).ToString();
                    sequenceId = guid.ToString();

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
                    fR.setNotificationId(sequenceId);
                    fR.setInstrumentName(this.GetComponent<NotificationList>().labelGameObject.GetComponent<TextMesh>().text);
                    fR.setLivingErrors(InstrumentManager_v1.ReturnLivingErrors());
                    fR.setDangerLevel(this.GetComponent<RangeInstrument>().dangerLevel);
                    fR.setInstrumentValue(textMesh.text);
                    fR.setAngleView(angleDeg.ToString().Replace(',', '.'));


                    FileRecord.AddFileRecord(fR);

                    IncreaseBrocken();

                    //-----------------------------------
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
            yield return new WaitForSeconds(1f);
        }

    }
}
