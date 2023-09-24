using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FileRecord 
{
    private long timestamp;
    private string notificationId;
    private string instrumentName;
    private int livingErrors;
    private int dangerLevel;
    private string instrumentValue;
    private string angleView;


    public static Action<FileRecord> onAddFile;
    public static Action<FileRecord> onAddFileDismissal;


    // Setters

    public void setTimestamp(long timestamp)
    {
        this.timestamp = timestamp;
    }

    public void setNotificationId(string notificationId)
    {
        this.notificationId = notificationId;
    }

    public void setInstrumentName(string instrumentName)
    {
        this.instrumentName = instrumentName;
    }

    public void setLivingErrors(int livingErrors)
    {
        this.livingErrors = livingErrors;
    }

    public void setDangerLevel(int dangerLevel)
    {
        this.dangerLevel = dangerLevel;
    }

    public void setInstrumentValue(string instrumentValue)
    {
        this.instrumentValue = instrumentValue;
    }

    public void setAngleView(string angleView)
    {
        this.angleView = angleView;
    }


    // Getters

    public long getTimestamp()
    {
        return this.timestamp;
    }

    public string getNotificationId()
    {
        return this.notificationId;
    }

    public string getInstrumentName()
    {
        return this.instrumentName;
    }

    public int getLivingErrors()
    {
        return this.livingErrors;
    }

    public int getDangerLevel()
    {
        return this.dangerLevel;
    }

    public string getInstrumentValue()
    {
        return this.instrumentValue;
    }

    public string getAngleView()
    {
        return this.angleView;
    }







    public static void AddFileRecord(FileRecord fileRecord)
    {

        //If someone is listening for this message.
        if (onAddFile != null)
        {
            onAddFile(fileRecord);
        }
    }


    public static void AddFileDismissal(FileRecord fileRecord)
    {

        //If someone is listening for this message.
        if (onAddFileDismissal != null)
        {
            onAddFileDismissal(fileRecord);
        }
    }
}
