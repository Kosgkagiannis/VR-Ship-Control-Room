using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberGenerator : Number


{
    //[SerializeField] public Animator myAnimationController;
    //public GameObject light;
    public AudioSource source;
    public AudioClip clip1;
    
    private static bool stop;
    public bool isWorking = true;
    public bool needNotification = false;
    private int randomProbability = -1;
    private static float number;
    public string sequenceId = "";
    public float timer2 = 0;
    public float timerStart2 ;


     public void Update() {

    timer2+=Time.deltaTime;
    }

    public IEnumerator NumberGeneratorRoutine(TextMesh textMesh, float min, float max, int precesion, float spawnTime, bool isPercentage, bool isDegrees, int probability, float danger)
    {
        while (!stop)
        {
            yield return new WaitForSeconds(spawnTime);

            AudioSource[] audioSources = GetComponents<AudioSource>();
            source = audioSources[0];
            clip1 = audioSources[0].clip;

            if (isWorking) {
                
                source.Pause();
               // light.SetActive(false);
             

                textMesh.color = Color.white;

                if (probability >= 0)
                    probability = generator.Next(0, 2);

                // Propability implementation 
                randomProbability = UnityEngine.Random.Range(1, maxRandomProbability_V1);

                if (randomProbability <= probability && Time.realtimeSinceStartup > 20f && Time.realtimeSinceStartup < 180f && InstrumentManager_v1.ReturnTotalErrors() < 10)
                {
                    
                    isWorking = false;
                     timerStart2=timer2;
                    Debug.Log(" 3D AUDIO NOTIFICATION - Engine name: " + this.GetComponent<NotificationList>().labelGameObject.GetComponent<TextMesh>().text + " - Time error appeared: " + timerStart2 +  " - Danger Level: " + this.GetComponent<RangeInstrument>().dangerLevel);

                    needNotification = true;

                    source.PlayOneShot(clip1);
                   // light.SetActive(false);

                  //  myAnimationController.SetBool("PlayLightRotation", false);





                    // ----------------------------------

                    long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
                    sequenceId = guid.ToString();

                    GameObject head = GameObject.Find("InteractionRigOVR-Basic/OVRCameraRig/TrackingSpace/CenterEyeAnchor");
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
                    fR.setInstrumentValue(danger.ToString());
                    fR.setAngleView(angleDeg.ToString().Replace(',', '.'));


                    FileRecord.AddFileRecord(fR);

                    IncreaseBrocken();

                    //-----------------------------------

                }

                number = UnityEngine.Random.Range(min, max);

                if (!isWorking)
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
  public void FixedWithButton()
{
    isWorking = true ;
     Debug.Log(" Engine name: " + this.GetComponent<NotificationList>().labelGameObject.GetComponent<TextMesh>().text + " -  Time error fixed: " + timerStart2 +  " -  Danger Level: " + this.GetComponent<RangeInstrument>().dangerLevel);

}

}

