using UnityEngine;
using System.Collections;

public class ReverseDirection : MonoBehaviour {
	// Use this for initialization
	void Start () {
		Animator anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
			
		
	}
    public void ReverseBool (){
            Animator anim = gameObject.GetComponent<Animator> ();
			// Reverse animation play
			anim.SetFloat ("Direction", -1);
			anim.Play ("FlashingLight", -1, float.NegativeInfinity);

    }

  public void NormalBool (){
            Animator anim = gameObject.GetComponent<Animator> ();
			// Reverse animation play
			anim.SetFloat ("Direction", 1);

    }
}