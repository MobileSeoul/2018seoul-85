using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint_Eraser : MonoBehaviour {

    GameObject go;

    public void ER()
    {
        go = GameObject.FindGameObjectWithTag("Draw");
        Destroy(go);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
