using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar_V2 : RangeInstrument
{

    public GameObject bar;
    public GameObject barChild;
    private Transform barTransform;

    public bool isFixed = false;

    private System.Random generator;


    // Start is called before the first frame update
    void Start()
    {
        generator = new System.Random();
        barTransform = bar.GetComponent<Transform>();
        StartCoroutine(BarDrop(barTransform, barChild));
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    IEnumerator BarDrop(Transform barTransform, GameObject barChild)
    {
        Renderer renderer;
        float yScale = 0.0f;
        int randomProbability = -1;

        float yMin = min;
        float yMax = max;
        float yYellow = warning;
        float yRed = danger;

        while (true)
        {
            yield return new WaitForSeconds(spawnTime);

            if (probability >= 0)
                probability = generator.Next(0, 2);

            if (isFixed)
            {
                barTransform.localScale = new Vector3(barTransform.localScale.x, yMax, barTransform.localScale.z);
                isFixed = false;
            }


            if (isOperational)
            {

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
                    randomProbability = UnityEngine.Random.Range(1, maxRandomProbability_V2);

                    if (randomProbability <= probability && Time.realtimeSinceStartup > 30f && Time.realtimeSinceStartup < 180f && InstrumentFailureManager.ReturnTotalErrors() < 10)
                    {
                        
                        dangerLevel = UnityEngine.Random.Range(1, 4);

                        renderer = barChild.GetComponent<Renderer>();
                        renderer.material.SetColor("_Color", Color.red);
                        
                        BreakDown();
                        isOperational = false;

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
                        fR.setInstrumentValue(yScale.ToString());
                        fR.setAngleView(angleDeg.ToString().Replace(',', '.'));

                        FileRecord.AddFileRecord(fR);

                        // ---------------------------------------


                    }
                    else
                    {
                        barTransform.localScale = new Vector3(barTransform.localScale.x, yMax, barTransform.localScale.z);
                    }
                }
            }



        }
    }

}
