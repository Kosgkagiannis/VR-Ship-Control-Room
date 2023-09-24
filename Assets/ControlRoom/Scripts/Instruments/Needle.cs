using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : RangeInstrument
{
    public GameObject needle;
    private Transform needleTransform;
    private IEnumerator coroutine;
    protected System.Random generator;

    private void Start()
    {
        generator = new System.Random();

        needleTransform = needle.GetComponent<Transform>();
        NeedleRotation nR = parentObject.GetComponent<NeedleRotation>();
        StartCoroutine(nR.NeedleRotationRoutine(needleTransform, min, max, spawnTime, probability, danger));
    }
  
}
