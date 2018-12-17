using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintListController : MonoBehaviour {

    public GameObject paintListItemPrefab;
    public GameObject elementControllerObj;

	// Use this for initialization
	void Start () {
        
        //AddNewItemToList();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddNewItemToList(DBDataDetailModel dBDataDetailModel) {
        var createdItemObj = Instantiate(paintListItemPrefab);
        createdItemObj.transform.parent = elementControllerObj.transform;
        createdItemObj.transform.localScale = new Vector3(1, 1, 1);
        var position = createdItemObj.transform.position;
        position.z = 0;
        createdItemObj.transform.position = position;
        position = createdItemObj.transform.localPosition;
        position.z = 0;
        createdItemObj.transform.localPosition = position;

        PaintListItemController paintListItemController = createdItemObj.GetComponent<PaintListItemController>();
        paintListItemController.SetPaintItem(dBDataDetailModel);

        Debug.Log("tran: " + createdItemObj.transform.position + ", " + createdItemObj.transform.localPosition);
    }
}
