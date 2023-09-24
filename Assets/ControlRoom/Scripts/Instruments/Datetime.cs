using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datetime : MonoBehaviour
{
    public GameObject textObject;
    public string type;
    private TextMesh textMesh;
    private string t;


    void Update()
    {
        if (type.Equals("time")) {
            t = System.DateTime.Now.ToString("HH:mm");
        }
        else if (type.Equals("date")) {
            t = System.DateTime.Now.ToString("dd/MM/yyyy");
        }   
        textMesh = textObject.GetComponent("TextMesh") as TextMesh;
        textMesh.text = t;
    }
}
