using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : RangeInstrument
{

    public GameObject numberText;
    public int precision;
    public bool isPercentage;
    public bool isDegrees;
    private TextMesh textMesh;
    protected System.Random generator;


    void Start()
    {

        generator = new System.Random();

        if (isPercentage && isDegrees)
        {
            Debug.LogError("Percentage, Degrees cannot be true simultaneously");
        }
        else {
            textMesh = numberText.GetComponent("TextMesh") as TextMesh;
            NumberGenerator nG = parentObject.GetComponent<NumberGenerator>();
            StartCoroutine(nG.NumberGeneratorRoutine(textMesh, min, max, precision, spawnTime, isPercentage, isDegrees, probability, danger));
        }

        
    }


}
