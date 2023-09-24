using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BarDrop : Bar
{

     public AudioSource source;
    public AudioClip clip1;

    private bool stop;
    private float yScale;
    private Renderer renderer;
    public bool isWorking = true;
    public bool isFixed = false;
    public bool needNotification = false;
    private int randomProbability = -1;
    public string sequenceId = "";
    
    public IEnumerator BarDropRoutine(Transform barTransform, float yMin, float yMax, float yYellow, float yRed, float spawnTime, GameObject barChild, int probability)
    {
        while (!stop)
        {
            yield return new WaitForSeconds(spawnTime);

            AudioSource[] audioSources = GetComponents<AudioSource>();
            source = audioSources[0];
            clip1 = audioSources[0].clip;
            
            if (isFixed) {
                barTransform.localScale = new Vector3(barTransform.localScale.x, yMax, barTransform.localScale.z);
                isFixed = false;
            }


            if (isWorking) {
                source.Pause();
                if (probability > 0)
                    probability = generator.Next(0, 2);

                yScale = (float)(barTransform.localScale.y - 0.1);
    
                barTransform.localScale = new Vector3(barTransform.localScale.x, yScale, barTransform.localScale.z);


                // Change Bar color depending on state
               
                if (yScale > yRed)
                {
                    renderer = barChild.GetComponent<Renderer>();
                    renderer.material.SetColor("_Color", Color.green);
                }
                
                else if (yScale < yRed)
                {
                    // Propability implementation 
                    randomProbability = UnityEngine.Random.Range(1, maxRandomProbability_V1);

                    if (randomProbability <= probability && Time.realtimeSinceStartup > 30f && Time.realtimeSinceStartup < 180f && InstrumentManager_v1.ReturnTotalErrors() < 10)
                    {
                        
                        renderer = barChild.GetComponent<Renderer>();
                        renderer.material.SetColor("_Color", Color.red);
                        isWorking = false;
                        needNotification = true;
                        source.PlayOneShot(clip1);


                        // ----------------------------------

                        long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
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
                        fR.setInstrumentValue(yScale.ToString());
                        fR.setAngleView(angleDeg.ToString().Replace(',', '.'));


                        FileRecord.AddFileRecord(fR);

                        IncreaseBrocken();

                        //-----------------------------------
                    }
                    else {
                        barTransform.localScale = new Vector3(barTransform.localScale.x, yMax, barTransform.localScale.z);
                    }
                }
            }


            
        }
    }
    public void FixedWithButton()
{
    isWorking = true ;
}
}
