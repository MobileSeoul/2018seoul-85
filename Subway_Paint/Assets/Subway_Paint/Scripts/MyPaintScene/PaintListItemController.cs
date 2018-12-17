using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PaintListItemController : MonoBehaviour {

    public RawImage rawImageView;
    public Text txtStationName;
    public Text txtDateTime;

    public DBDataDetailModel dBDataDetailModel { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPaintItem(DBDataDetailModel dbDataDetailModel) {
        if(dbDataDetailModel.img != null && dbDataDetailModel.img != "") {
            //Texture savedImage = Resources.Load(dbDataDetailModel.img) as Texture;
            //rawImageView.texture = savedImage;

            // Read the data from the file
            Debug.Log("load file from path: " + dbDataDetailModel.img);
            if(File.Exists(dbDataDetailModel.img)) {
                byte[] data = File.ReadAllBytes(dbDataDetailModel.img);

                // Create the texture
                Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height);

                // Load the image
                screenshotTexture.LoadImage(data);

                rawImageView.texture = screenshotTexture;
            }
            else {
                Debug.Log("cannot load img, wrong path");
            }
        }
        Debug.Log("station: " + dbDataDetailModel.location + " date: " + dbDataDetailModel.datetime);
        txtStationName.text = dbDataDetailModel.location + "역에서 그린 그림";
        txtDateTime.text = "그린 날짜 : " + dbDataDetailModel.datetime;
    }

}
