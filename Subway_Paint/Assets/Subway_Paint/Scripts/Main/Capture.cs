using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Capture : MonoBehaviour {

    public DBManager dBManager;
    private int resWidth=Screen.width;
    private int resHeight = Screen.height;

    public Sprite TempImage { get; private set; }

    public void cap()
    {
        sc();
    }
    public Sprite sc()
    {
        string date = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string myFilename = "myScreenshot_" + date + ".png";
        
        string myDefaultLocation = Application.persistentDataPath + "/" + myFilename;
        
        string myFolderLocation = "/storage/emulated/0/DCIM/Camera/";
        string myScreenshotLocation = myFolderLocation + myFilename;
       
        if (!System.IO.Directory.Exists(myFolderLocation))
        {
            System.IO.Directory.CreateDirectory(myFolderLocation);
        }


       
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        Camera.main.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        Camera.main.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        TempImage = Sprite.Create(screenShot, rec, new Vector2(0, 0), .01f);

        Camera.main.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(myScreenshotLocation, bytes);


        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");

        AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2] { "android.intent.action.MEDIA_SCANNER_SCAN_FILE", classUri.CallStatic<AndroidJavaObject>("parse", "file://" + myScreenshotLocation) });
        objActivity.Call("sendBroadcast", objIntent);
       

        SaveToDB(date, myScreenshotLocation);

        return TempImage;
    }



    void SaveToDB(string date, string filePath) {

        dBManager.LoadDBJsonData();

        string stationName = PlayerPrefs.GetString("stationName");
        Debug.Log("date: " + date + " file path: " + filePath + " name: " + stationName);
        DBDataDetailModel dBDataDetailModel = new DBDataDetailModel();
        dBDataDetailModel.datetime = date;
        dBDataDetailModel.img = filePath;
        dBDataDetailModel.location = stationName;

        dBManager.AppendDB(dBDataDetailModel);
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
