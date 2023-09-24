using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

  

public class NeedleRotation : Needle
{
    [SerializeField] public Animator myAnimationController;
    public GameObject light;
    private bool stop;
    public bool isWorking = true;
    public bool needNotification = false;
    private int randomProbability = -1;
    private float zRotation;
    public string sequenceId = "";
    public float timer = 0;
    public float timerStart ;
  


      public void Update() {

    timer+=Time.deltaTime;
    }

    public IEnumerator NeedleRotationRoutine(Transform needleTransform, float zMin, float zMax, float spawnTime, int probability, float danger)
    {
        light.SetActive(false);
        while (!stop)
        {
            

            yield return new WaitForSeconds(spawnTime);
            light.SetActive(true);
            if (isWorking) {

                 light.SetActive(false);
                if (probability >= 0)
                    probability = generator.Next(0, 2);

                // Propability implementation
                randomProbability = UnityEngine.Random.Range(1, maxRandomProbability_V1);

                if (randomProbability <= probability && Time.realtimeSinceStartup > 20f && Time.realtimeSinceStartup < 180f && InstrumentManager_v1.ReturnTotalErrors() < 10)
                {    
                    isWorking = false;
                    timerStart=timer;
                    Debug.Log("ROTATING LIGHT NOTIFICATION  - Engine name: " + this.GetComponent<NotificationList>().labelGameObject.GetComponent<TextMesh>().text + " -  Time error appeared: " + timerStart +  " - Danger Level: " + this.GetComponent<RangeInstrument>().dangerLevel);
                    needNotification = true;
                    light.SetActive(false);
                    myAnimationController.SetBool("PlayLightRotation", false);



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

                zRotation = UnityEngine.Random.Range(zMin, zMax);

                if (!isWorking)
                {
                    zRotation = danger;
                  
                   
                }

                needleTransform.eulerAngles = new Vector3(needleTransform.rotation.eulerAngles.x, needleTransform.rotation.eulerAngles.y, zRotation);
            }
            
        }
    }
    public void FixedWithButton()
{
    isWorking = true ;
     Debug.Log(" Engine name: " + this.GetComponent<NotificationList>().labelGameObject.GetComponent<TextMesh>().text + " - Time error fixed: " + timerStart +  " - Danger Level: " + this.GetComponent<RangeInstrument>().dangerLevel);

}

}

