using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationList : MonoBehaviour
{

    public GameObject instrument;
    public GameObject notificationCanvasList;
    public GameObject screenGameObject;
    public GameObject labelGameObject;
    private string notificationID;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
        string instrumentType = getComponentCoroutineOfInstrument(instrument);
        bool isWorking = true;
        bool needNotification = false;
        int dangerLevel = -1;

        if (instrumentType != null) {
            if (instrumentType.Equals("NeedleRotation"))
            {
                isWorking = this.GetComponent<NeedleRotation>().isWorking;
                needNotification = this.GetComponent<NeedleRotation>().needNotification;
                dangerLevel = this.GetComponent<Needle>().dangerLevel ;
            }
            else if (instrumentType.Equals("NumberGenerator"))
            {
              
                isWorking = this.GetComponent<NumberGenerator>().isWorking;
           
                needNotification = this.GetComponent<NumberGenerator>().needNotification;
                dangerLevel = this.GetComponent<Number>().dangerLevel;
                
            }
            else if (instrumentType.Equals("BarDrop"))
            {
                isWorking = this.GetComponent<BarDrop>().isWorking;
                needNotification = this.GetComponent<BarDrop>().needNotification;
                dangerLevel = this.GetComponent<Bar>().dangerLevel;
            }
            else if (instrumentType.Equals("CounterStart"))
            {
                isWorking = this.GetComponent<CounterStart>().isWorking;
                needNotification = this.GetComponent<CounterStart>().needNotification;
                dangerLevel = this.GetComponent<Counter>().dangerLevel;
            }
        }


        if (!isWorking && needNotification) {
            foreach (Transform notificationElement in notificationCanvasList.transform)
            {
                if (null == notificationElement)
                    continue;
                if (notificationElement.gameObject.active == false && notificationElement.name.Contains("ListElement")) {
                    notificationElement.gameObject.GetComponent<TextMesh>().text = labelGameObject.GetComponent<TextMesh>().text + "\n (" + screenGameObject.GetComponent<TextMesh>().text + ")";
                    notificationElement.gameObject.active = true;

                    SpriteRenderer sprite = notificationElement.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
                    if (dangerLevel == 3)
                        sprite.color = Color.red;
                    if (dangerLevel == 2)
                        sprite.color = new Color(0.8f,0.4f,0,1);
                    else if (dangerLevel <= 1)
                        sprite.color = new Color(0, 0.6f, 0, 1);

                   
                    int notId = -1;
                    if (instrumentType != null)
                    {
                        if (instrumentType.Equals("NeedleRotation"))
                        {
                            this.GetComponent<NeedleRotation>().needNotification = false;
                        }
                        else if (instrumentType.Equals("NumberGenerator"))
                        {
                            this.GetComponent<NumberGenerator>().needNotification = false;
                        }
                        else if (instrumentType.Equals("BarDrop"))
                        {
                            this.GetComponent<BarDrop>().needNotification = false;
                        }
                        else if (instrumentType.Equals("CounterStart"))
                        {
                            this.GetComponent<CounterStart>().needNotification = false;
                        }
                    }
                    notificationID = notificationElement.name;
                    break;
                }
            }

        }
        else if (!isWorking && !needNotification) {
            foreach (Transform notificationElement in notificationCanvasList.transform)
            {
                if (null == notificationElement)
                    continue;
                if (notificationElement.name.Equals(notificationID))
                {
                    notificationElement.gameObject.active = false;
                    notificationID = "";
                    break;
                }
            }


            foreach (Transform notificationElement in notificationCanvasList.transform)
            {
                if (null == notificationElement)
                    continue;
                if (notificationElement.gameObject.active == false && notificationElement.name.Contains("ListElement"))
                {
                    notificationElement.gameObject.GetComponent<TextMesh>().text = labelGameObject.GetComponent<TextMesh>().text + "\n (" + screenGameObject.GetComponent<TextMesh>().text + ")";
                    notificationElement.gameObject.active = true;


                    if (instrumentType != null)
                    {
                        if (instrumentType.Equals("NeedleRotation"))
                        {
                            this.GetComponent<NeedleRotation>().needNotification = false;
                        }
                        else if (instrumentType.Equals("NumberGenerator"))
                        {
                            this.GetComponent<NumberGenerator>().needNotification = false;
                        }
                        else if (instrumentType.Equals("BarDrop"))
                        {
                            this.GetComponent<BarDrop>().needNotification = false;
                        }
                        else if (instrumentType.Equals("CounterStart"))
                        {
                            this.GetComponent<CounterStart>().needNotification = false;
                        }
                    }

                    notificationID = notificationElement.name;
                    break;
                }
            }


        }
        else if (isWorking) {
            foreach (Transform notificationElement in notificationCanvasList.transform)
            {
                if (null == notificationElement)
                    continue;
                if (notificationElement.name.Equals(notificationID))
                {
                    notificationElement.gameObject.active = false;
                    notificationID = "";
                    break;
                }
            }
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
