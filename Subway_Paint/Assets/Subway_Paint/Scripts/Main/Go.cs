using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Go : MonoBehaviour {

    public void Pgo()
    {
        SceneManager.LoadScene("paint");
    }
    
    public void Vgo()
    {
        SceneManager.LoadScene("MyPaintScene");
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
