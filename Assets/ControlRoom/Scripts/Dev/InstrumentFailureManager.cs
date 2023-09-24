using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentFailureManager : MonoBehaviour
{

    List<RangeInstrument> allBrockenInstruments;
    List<RangeInstrument> allFixedInstruments;
    List<RangeInstrument> allTotalBrockenInstruments;
    public GameObject arrow;
    public GameObject mostCriticalObjet;


    public GameObject appLifeCounter;
    private TextMesh appLifeCounterTextMesh;

    public GameObject totalErrors;
    private TextMesh totalErrorsTextMesh;

    public GameObject livingErrors;
    private TextMesh livingErrorsTextMesh;

    public GameObject fixedErrors;
    private TextMesh fixedErrorsTextMesh;

    public GameObject successPercentage;
    private TextMesh successPercentageTextMesh;

    public GameObject fixesPerMin;
    private TextMesh fixesPerMinTextMesh;

    public GameObject mostCriticalNotification;
    private TextMesh mostCriticalNotificationTextMesh;
    private string mostCriticalNotificationLabel;
    private int mostCriticalNotificationDangerLevel;

    public GameObject notificationCanvas;

    private static int totalErrorsCounter;
    private static int livingErrorsCounter;

    List<FileRecord> fileRecords;
    List<FileRecord> fileDismissals;

    //If the app is running at 30 Frames Per Second the check counter will check every 30 seconds.
    static int maxTime = 900;
    int checkCounter = maxTime;

    // Start is called before the first frame update
    void Start()
    {
        appLifeCounterTextMesh = appLifeCounter.GetComponent("TextMesh") as TextMesh;
        StartCoroutine(CountAppLife(appLifeCounterTextMesh));

        totalErrorsTextMesh = totalErrors.GetComponent("TextMesh") as TextMesh;
        livingErrorsTextMesh = livingErrors.GetComponent("TextMesh") as TextMesh;
        fixedErrorsTextMesh = fixedErrors.GetComponent("TextMesh") as TextMesh;
        mostCriticalNotificationTextMesh = mostCriticalNotification.GetComponent("TextMesh") as TextMesh;
        successPercentageTextMesh = successPercentage.GetComponent("TextMesh") as TextMesh;
        fixesPerMinTextMesh = fixesPerMin.GetComponent("TextMesh") as TextMesh;

        allBrockenInstruments = new List<RangeInstrument>();
        allFixedInstruments = new List<RangeInstrument>();
        allTotalBrockenInstruments = new List<RangeInstrument>();

        fileRecords = new List<FileRecord>();
        fileDismissals = new List<FileRecord>();

        //Subscription of OfManager to the message and what is he supposed to do with it. 
        RangeInstrument.OnInstrumentBrocken += HandleInstrumentBrocken;

        //Subscription of Manager to the message that an instrument is repaired and what to do with it.
        RangeInstrument.OnInstrumentRepaired += HandleInstrumentRepaired;

        FileRecord.onAddFile += AddFileRecord;
        FileRecord.onAddFileDismissal += AddFileDismissal;
    }

    // Update is called once per frame
    void Update()
    {

        totalErrorsCounter = allTotalBrockenInstruments.Count;
        livingErrorsCounter = allBrockenInstruments.Count;

        UpdateLogging(totalErrorsTextMesh, livingErrorsTextMesh, fixedErrorsTextMesh, mostCriticalNotificationTextMesh, successPercentageTextMesh, fixesPerMinTextMesh, appLifeCounterTextMesh);
        UpdateLatestNotifications(notificationCanvas);
        ShowcaseMostUrgent();

        //Update file every 30 seconds;
        if (checkCounter == 0)
        {
            WriteToFileAsynchronous();
            checkCounter = maxTime;
        }

        checkCounter--;
    }



    private void HandleInstrumentBrocken(RangeInstrument newBrockenInstrument)
    {        
        if (newBrockenInstrument != null) {
            allBrockenInstruments.Add(newBrockenInstrument);
            allTotalBrockenInstruments.Add(newBrockenInstrument);
        }
    }

    private void HandleInstrumentRepaired(RangeInstrument repairedInstrument)
    {        
        if (repairedInstrument != null)
        {
            allBrockenInstruments.Remove(repairedInstrument);
            allFixedInstruments.Add(repairedInstrument);
        }

    }



    private void ShowcaseMostUrgent()
    {
             
        RangeInstrument mostCriticalInstrument = mostCriticalObjet.AddComponent<RangeInstrument>();


        if (allBrockenInstruments != null && allBrockenInstruments.Count > 0)
        {

            for (int i = 0; i < allBrockenInstruments.Count; i++)
            {

                if (allBrockenInstruments[i].dangerLevel > mostCriticalInstrument.dangerLevel)
                {
                    mostCriticalInstrument = allBrockenInstruments[i];
                }
            }

            mostCriticalNotificationLabel = mostCriticalInstrument.labelGameObject.GetComponent<TextMesh>().text + "\n (" + mostCriticalInstrument.screenGameObject.GetComponent<TextMesh>().text + ")";
            mostCriticalNotificationDangerLevel = mostCriticalInstrument.dangerLevel;



            arrow.SetActive(true);
            arrow.transform.LookAt(mostCriticalInstrument.transform);


        }
        else {
            arrow.SetActive(false);
            mostCriticalNotification.SetActive(false);
        }
        
            
    }



    private void UpdateLogging(TextMesh total, TextMesh living, TextMesh fix, TextMesh mostCritical, TextMesh success, TextMesh fixesPerMinute, TextMesh counter) {

        if (allTotalBrockenInstruments.Count > 0)
        {

            string counterText = counter.text;
            counterText = counterText.Replace("Timer : ", "");
            string[] counterTextParts = counterText.Split(':');

            float minutes = float.Parse(counterTextParts[0]) + float.Parse(counterTextParts[1]) / 60;
            float fixesPerMin = (float)Math.Round(allFixedInstruments.Count / minutes, 2, MidpointRounding.ToEven);

            float successPercentage = 0f;

            successPercentage = (float)Math.Round((double)allFixedInstruments.Count / allTotalBrockenInstruments.Count, 2, MidpointRounding.ToEven) * 100;
            success.text = "Success : " + successPercentage.ToString() + "%";
            fixesPerMinute.text = "Fixes per Minute : " + fixesPerMin.ToString();

            mostCriticalNotification.SetActive(true);
            //SpriteRenderer sprite = mostCriticalNotification.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

            /*
            if (mostCriticalNotificationDangerLevel >= 3)
                sprite.color = Color.red;
            if (mostCriticalNotificationDangerLevel == 2)
                sprite.color = new Color(0.8f, 0.4f, 0, 1);
            else if (mostCriticalNotificationDangerLevel <= 1)
                sprite.color = new Color(0, 0.6f, 0, 1);
                */


            mostCritical.text = mostCriticalNotificationLabel;


        }
        else if (allTotalBrockenInstruments.Count == 0)
        {
            success.text = "Success : 0%";
            fixesPerMinute.text = "Fixes per Minute : 0";
        }

        total.text = "Total Errors : " + allTotalBrockenInstruments.Count;
        living.text = "Living Errors : " + allBrockenInstruments.Count;
        fix.text = "Fixed : " + allFixedInstruments.Count;

       
    }

    IEnumerator CountAppLife(TextMesh textMesh)
    {
        int minutes = 0;
        int seconds = 0;
        string minutesText = "";
        string secondsText = "";

       
        while (true) {

            
            if (seconds < 59 && seconds >= 0)
            {
                seconds++;
            }
            else if (seconds == 59)
            {
                seconds = 0;
                if (minutes < 59 && minutes >= 0)
                {
                    minutes++;
                }
               
            }

            
            secondsText = seconds.ToString();
            minutesText = minutes.ToString();
            if (seconds >= 0 && seconds < 10)
            {
                secondsText = "0" + seconds.ToString();
            }
            if (minutes >= 0 && minutes < 10)
            {
                minutesText = "0" + minutes.ToString();
            }

           
            textMesh.text = "Timer : " + minutesText + ":" + secondsText;

            yield return new WaitForSeconds(1);
        }
    }




    private void UpdateLatestNotifications(GameObject latestNotificationsList) {


        if (allBrockenInstruments != null && allBrockenInstruments.Count > 0 && allBrockenInstruments.Count <= 10)
        {


            foreach (Transform listElement in latestNotificationsList.transform)
            {
                if (null == listElement)
                    continue;
                else if (listElement.name.Contains("ListElement_"))
                {
                    listElement.gameObject.SetActive(false);
                }
            }

            int labelCounter = 1;
            for (int i = 0; i < allBrockenInstruments.Count; i++)
            {
                RangeInstrument brockenInstrument = allBrockenInstruments[i];

                foreach (Transform listElement in latestNotificationsList.transform)
                {
                    if (null == listElement)
                        continue;
                    else if (listElement.name.Contains("ListElement_" + labelCounter) && listElement.gameObject.active == false)
                    {
                        listElement.gameObject.SetActive(true);
                        listElement.gameObject.GetComponent<TextMesh>().text = brockenInstrument.labelGameObject.GetComponent<TextMesh>().text + "\n (" + brockenInstrument.screenGameObject.GetComponent<TextMesh>().text + ")";
                        SpriteRenderer sprite = listElement.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();


                        if (brockenInstrument.dangerLevel >= 3)
                            sprite.color = Color.red;
                        if (brockenInstrument.dangerLevel == 2)
                            sprite.color = new Color(0.8f, 0.4f, 0, 1);
                        else if (brockenInstrument.dangerLevel <= 1)
                            sprite.color = new Color(0, 0.6f, 0, 1);

                        break;
                    }
                }
                labelCounter++;

            }

        }
        else if (allBrockenInstruments.Count == 0) {
            foreach (Transform listElement in latestNotificationsList.transform)
            {
                if (null == listElement)
                    continue;
                else if (listElement.name.Contains("ListElement_"))
                {
                    listElement.gameObject.SetActive(false);
                }
            }
        }

    }



    public static void WriteToFileSynchronous(FileRecord fileRecord)
    {

        String path = Constants.logNotificationPathV2;
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Notification ID, Timestamp, Danger Level, Instrument, Value, Angle view, Active errors\n");
            File.AppendAllText(path, fileRecord.getNotificationId() + ", " + fileRecord.getTimestamp() + ", " + fileRecord.getDangerLevel() + ", " + fileRecord.getInstrumentName() + ", " + fileRecord.getInstrumentValue() + ", " + fileRecord.getAngleView() + ", " + fileRecord.getLivingErrors() + "\n");
        }
        else
        {
            File.AppendAllText(path, fileRecord.getNotificationId() + ", " + fileRecord.getTimestamp() + ", " + fileRecord.getDangerLevel() + ", " + fileRecord.getInstrumentName() + ", " + fileRecord.getInstrumentValue() + ", " + fileRecord.getAngleView() + ", " + fileRecord.getLivingErrors() + "\n");
        }

    }


    public void WriteToFileAsynchronous()
    {

        File.WriteAllText(Constants.logNotificationPathV2, "Notification ID, Timestamp, Danger Level, Instrument, Value, Angle view, Active errors\n");

        for (int i = 0; i < fileRecords.Count; i++)
        {

            File.AppendAllText(Constants.logNotificationPathV2, fileRecords[i].getNotificationId() + ", " + fileRecords[i].getTimestamp() + ", " + fileRecords[i].getDangerLevel() + ", " + fileRecords[i].getInstrumentName() + ", " + fileRecords[i].getInstrumentValue() + ", " + fileRecords[i].getAngleView() + ", " + fileRecords[i].getLivingErrors() + "\n");
        }


        File.WriteAllText(Constants.logDismissalPathV2, "Notification ID, Timestamp, Active errors\n");

        for (int i = 0; i < fileDismissals.Count; i++)
        {

            File.AppendAllText(Constants.logDismissalPathV2, fileDismissals[i].getNotificationId() + ", " + fileDismissals[i].getTimestamp() + ", " + fileDismissals[i].getLivingErrors() + "\n");
        }
    }


    public static int ReturnTotalErrors() {
        return totalErrorsCounter;
    }

    public static int ReturnLivingErrors()
    {
        return livingErrorsCounter;
    }


    private void AddFileRecord(FileRecord fileRecord)
    {
        fileRecords.Add(fileRecord);
    }

    private void AddFileDismissal(FileRecord fileRecord)
    {
        fileDismissals.Add(fileRecord);
    }



}
