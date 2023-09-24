using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentManager_v1 : MonoBehaviour
{


    private static int allBrockenInstruments = 0;
    private static int allFixedInstruments = 0;
    private static int allTotalBrockenInstruments = 0;
   
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

    List<FileRecord> fileRecords;
    List<FileRecord> fileDismissals;

    private int sequenceGen = 0;

    //If the app is running at 30 Frames Per Second the check counter will check every 30 seconds.
    static int maxTime = 900;
    int checkCounter = maxTime;


    // Start is called before the first frame update
    void Start()
    {
        fileRecords = new List<FileRecord>();
        fileDismissals = new List<FileRecord>();

        allBrockenInstruments = 0;
        allFixedInstruments = 0;
        allTotalBrockenInstruments = 0;

        //Subscription of OfManager to the message and what is he supposed to do with it. 
        RangeInstrument.OnInstrumentBrocken_v1 += HandleInstrumentBrocken_v1;

        //Subscription of Manager to the message that an instrument is repaired and what to do with it.
        RangeInstrument.OnInstrumentRepaired_v1 += HandleInstrumentRepaired_v1;

        FileRecord.onAddFile += AddFileRecord;
        FileRecord.onAddFileDismissal += AddFileDismissal;


        appLifeCounterTextMesh = appLifeCounter.GetComponent("TextMesh") as TextMesh;
        StartCoroutine(CountAppLife(appLifeCounterTextMesh));

        totalErrorsTextMesh = totalErrors.GetComponent("TextMesh") as TextMesh;
        livingErrorsTextMesh = livingErrors.GetComponent("TextMesh") as TextMesh;
        fixedErrorsTextMesh = fixedErrors.GetComponent("TextMesh") as TextMesh;
        successPercentageTextMesh = successPercentage.GetComponent("TextMesh") as TextMesh;
        fixesPerMinTextMesh = fixesPerMin.GetComponent("TextMesh") as TextMesh;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateLogging(totalErrorsTextMesh, livingErrorsTextMesh, fixedErrorsTextMesh, successPercentageTextMesh, appLifeCounterTextMesh, fixesPerMinTextMesh);


        //Update file every 30 seconds;
        if (checkCounter == 0)
        {
            WriteToFileAsynchronous();
            checkCounter = maxTime;
        }

        checkCounter--;

    }



    private void HandleInstrumentBrocken_v1(RangeInstrument newBrockenInstrument)
    {
        
        allBrockenInstruments++;
        allTotalBrockenInstruments++;

    }

    private void HandleInstrumentRepaired_v1(RangeInstrument repairedInstrument)
    {
        allFixedInstruments++;
        allBrockenInstruments--;
    }


    private void AddFileRecord(FileRecord fileRecord) {
        fileRecords.Add(fileRecord);
    }

  
    private void AddFileDismissal(FileRecord fileRecord)
    {
        fileDismissals.Add(fileRecord);
    }

 
    public void WriteToFileAsynchronous()
    {

      
        File.WriteAllText(Constants.logNotificationPath, "Notification ID, Timestamp, Danger Level, Instrument, Value, Angle view, Active errors\n");

        for (int i = 0; i < fileRecords.Count; i++)
        {

            File.AppendAllText(Constants.logNotificationPath, fileRecords[i].getNotificationId() + ", " + fileRecords[i].getTimestamp() + ", " + fileRecords[i].getDangerLevel() + ", " + fileRecords[i].getInstrumentName() + ", " + fileRecords[i].getInstrumentValue() + ", " + fileRecords[i].getAngleView() + ", " + fileRecords[i].getLivingErrors() + "\n");
        }


        File.WriteAllText(Constants.logDismissalPath, "Notification ID, Timestamp, Active errors\n");

        for (int i = 0; i < fileDismissals.Count; i++)
        {

            File.AppendAllText(Constants.logDismissalPath, fileDismissals[i].getNotificationId() + ", " + fileDismissals[i].getTimestamp() + ", " + fileDismissals[i].getLivingErrors() + "\n");
        }
    }

    private void UpdateLogging(TextMesh total, TextMesh living, TextMesh fix, TextMesh success, TextMesh counter, TextMesh fixesPerMinute)
    {

        
        if (allTotalBrockenInstruments > 0)
        {

            string counterText = counter.text;
            counterText = counterText.Replace("Timer : ", "");
            string[] counterTextParts = counterText.Split(':');

            float minutes = float.Parse(counterTextParts[0]) + float.Parse(counterTextParts[1]) / 60;
            float fixesPerMin = (float)Math.Round(allFixedInstruments / minutes, 2, MidpointRounding.ToEven);

            float successPercentage = 0f;

            successPercentage = (float) Math.Round((double)allFixedInstruments / allTotalBrockenInstruments, 2, MidpointRounding.ToEven) * 100;
            success.text = "Success : " + successPercentage.ToString() + "%";
            fixesPerMinute.text = "Fixes per Minute : " + fixesPerMin.ToString();
        }
        else if (allTotalBrockenInstruments == 0) {
            success.text = "Success : 0%";
            fixesPerMinute.text = "Fixes per Minute : 0";
        }


        total.text = "Total Errors : " + allTotalBrockenInstruments.ToString();
        living.text = "Living Errors : " + allBrockenInstruments.ToString();
        fix.text = "Fixed : " + allFixedInstruments.ToString();

    }

    IEnumerator CountAppLife(TextMesh textMesh)
    {
        int minutes = 0;
        int seconds = 0;
        string minutesText = "";
        string secondsText = "";


        while (true)
        {


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


    public static int ReturnTotalErrors()
    {
        return allTotalBrockenInstruments;
    }

    public static int ReturnLivingErrors()
    {
        return allBrockenInstruments;
    }
}
