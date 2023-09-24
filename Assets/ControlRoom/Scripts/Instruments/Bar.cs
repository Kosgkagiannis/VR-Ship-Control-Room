using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : RangeInstrument
{
    public GameObject bar;
    public GameObject barChild;
    private Transform barTransform;
    protected System.Random generator;

    void Start()
    {
        generator = new System.Random();

        barTransform = bar.GetComponent<Transform>();
        BarDrop bd = parentObject.GetComponent<BarDrop>();
        StartCoroutine(bd.BarDropRoutine(barTransform, min, max, warning, danger, spawnTime, barChild, probability)); //Drop from Ymax to Ymin
    }
}
