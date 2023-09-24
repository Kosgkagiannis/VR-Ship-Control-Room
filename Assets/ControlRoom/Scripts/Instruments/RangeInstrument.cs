using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RangeInstrument : MonoBehaviour
{
    public int spawnTime;
    public int min;
    public int max;
    public int danger;
    public int warning;
    public int probability;
    public GameObject parentObject;

    //public int sequenceId;

    public int dangerLevel;
    public int notificationId;

    public bool isOperational = true;
    public string sequenceIdV2 = "";

    public GameObject labelGameObject;
    public GameObject screenGameObject;


    //Message that an Instrument has brocken down.
    public static Action<RangeInstrument> OnInstrumentBrocken;
    public static Action<RangeInstrument> OnInstrumentRepaired;


    public static Action<RangeInstrument> OnInstrumentBrocken_v1;
    public static Action<RangeInstrument> OnInstrumentRepaired_v1;

    protected Guid guid = Guid.NewGuid();

    protected int maxRandomProbability_V1 = 351;
    protected int maxRandomProbability_V2 = 601;


    public void BreakDown()
    {

  
        //If someone is listening for this message.
        if (OnInstrumentBrocken != null)
        {
            isOperational = false;
            OnInstrumentBrocken(this);
        }
    }



    public void RepairInstrument(RangeInstrument rangeInstrument)
    {

        //If someone is listening for this message.
        if (OnInstrumentRepaired != null)
        {
            rangeInstrument.isOperational = true;
            OnInstrumentRepaired(rangeInstrument);
        }
    }


    // Update Lists for broken and fixed instruments
    public void IncreaseBrocken()
    {
        if (OnInstrumentBrocken_v1 != null)
        {
            OnInstrumentBrocken_v1(this);
        }
    }

    public void DecreaseBrocken()
    {
        if (OnInstrumentRepaired_v1 != null)
        {
            OnInstrumentRepaired_v1(this);
        }
    }

}
