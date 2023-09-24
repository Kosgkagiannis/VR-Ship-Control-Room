using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TWOSOUNDS : MonoBehaviour
{

    public AudioSource alarm;
    public AudioSource hello;
    // Start is called before the first frame update
    void Start()
    {
        alarm = gameObject.AddComponent<AudioSource>();
        hello = gameObject.AddComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
