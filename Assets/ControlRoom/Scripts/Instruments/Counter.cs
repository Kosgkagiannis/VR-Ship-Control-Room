using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : RangeInstrument
{
    public GameObject clock;
    private TextMesh textMesh;
    public int hour;
    public int minutes;
    public int seconds;
    protected System.Random generator;


    // Start is called before the first frame update
    void Start()
    {
        generator = new System.Random();

        if (hour < 0 || hour > 23)
        {
            Debug.LogError("Hour must be between 0 and 23");
        }
        else if (minutes < 0 || hour > 59)
        {
            Debug.LogError("Minutes must be between 0 and 59");
        }
        else if (seconds < 0 || hour > 59)
        {
            Debug.LogError("Seconds must be between 0 and 59");
        }
        else {
            textMesh = clock.GetComponent("TextMesh") as TextMesh;
            CounterStart cs = parentObject.GetComponent<CounterStart>();
            StartCoroutine(cs.CounterStartRoutine(textMesh, hour, minutes, seconds, probability));
        }

    }
}
