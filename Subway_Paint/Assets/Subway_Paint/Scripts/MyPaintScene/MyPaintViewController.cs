using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPaintViewController : MonoBehaviour {

    public DBManager dbManager;
    public PaintListController paintListController;

	// Use this for initialization
	void Start () {
        dbManager.LoadDBJsonData();
        //DBDataDetailModel test = new DBDataDetailModel();
        //test.img = "test";
        //test.location = "no";
        //test.datetime = "fuck";
        //dbManager.AppendDB(test);

        List<DBDataDetailModel> myPaintDB = dbManager.GetMyPaintsDB();
        foreach(DBDataDetailModel model in myPaintDB) {
            paintListController.AddNewItemToList(model);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BtnGoBackToMain() {
        Application.LoadLevel("Main");
    }
}
