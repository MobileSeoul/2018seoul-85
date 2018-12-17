using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MonoBehaviour {
    public GameObject GO;
    GameObject thisTr;
    Vector3 startpos;
    Plane obj;


    // Use this for initialization
    void Start () {
        obj = new Plane(Camera.main.transform.forward * -1, this.transform.position);  
	}
	
	// Update is called once per frame
	void Update () {
      if((Input.touchCount>0 && Input.GetTouch(0).phase==TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            thisTr = (GameObject)Instantiate(GO, this.transform.position, Quaternion.identity);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayD;
            if (obj.Raycast(ray, out rayD))
                thisTr.transform.position = ray.GetPoint(rayD);
          
        }
      else if((Input.touchCount>0 && Input.GetTouch(0).phase==TouchPhase.Moved) || Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayD;
            if (obj.Raycast(ray, out rayD))
                thisTr.transform.position = ray.GetPoint(rayD);
          
        }
      else if((Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0))
        {
            
            if (Vector3.Distance(thisTr.transform.position, startpos) < 0.1)
                Destroy(thisTr);
            
        }

        obj = new Plane(Camera.main.transform.forward * -1, thisTr.transform.position);

    }

}
