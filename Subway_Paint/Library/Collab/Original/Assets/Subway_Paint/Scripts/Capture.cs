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
        // debugText.text = ": " + myFilename;
        string myDefaultLocation = Application.persistentDataPath + "/" + myFilename;
        //EXAMPLE OF DIRECTLY ACCESSING THE Camera FOLDER OF THE GALLERY
        //string myFolderLocation = "/storage/emulated/0/DCIM/Camera/";
        //EXAMPLE OF BACKING INTO THE Camera FOLDER OF THE GALLERY
        //string myFolderLocation = Application.persistentDataPath + "/../../../../DCIM/Camera/";
        //EXAMPLE OF DIRECTLY ACCESSING A CUSTOM FOLDER OF THE GALLERY
        string myFolderLocation = "/storage/emulated/0/DCIM/Camera/";
        string myScreenshotLocation = myFolderLocation + myFilename;
        //ENSURE THAT FOLDER LOCATION EXISTS
        if (!System.IO.Directory.Exists(myFolderLocation))
        {
            System.IO.Directory.CreateDirectory(myFolderLocation);
        }


        //  캔버스 제외 카메라에 보이는 부분만 스크린 샷!!
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


        SaveToDB(date, myScreenshotLocation);



        //MOVE THE SCREENSHOT WHERE WE WANT IT TO BE STORED
        //  System.IO.File.Move(myDefaultLocation, myScreenshotLocation);

        //REFRESHING THE ANDROID PHONE PHOTO GALLERY IS BEGUN
        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
        // "android.intent.action.MEDIA_SCANNER_SCAN_FILE" <--- 요거 햇갈림.. 원래 찾은건 "android.intent.action.MEDIA_MOUNTED" 요렇게 하라고 나와있는데 안되서 저렇게 하니 됨.
        AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2] { "android.intent.action.MEDIA_SCANNER_SCAN_FILE", classUri.CallStatic<AndroidJavaObject>("parse", "file://" + myScreenshotLocation) });
        objActivity.Call("sendBroadcast", objIntent);
        // debugText.text = "Complete! - " + myScreenshotLocation;
        //REFRESHING THE ANDROID PHONE PHOTO GALLERY IS COMPLETE

        //AUTO LAUNCH/VIEW THE SCREENSHOT IN THE PHOTO GALLERY!!!
        // Application.OpenURL(myScreenshotLocation);
        //AFTERWARDS IF YOU MANUALLY GO TO YOUR PHOTO GALLERY, 
        //YOU WILL SEE THE FOLDER WE CREATED CALLED "myFolder"
        // count++;

        return TempImage;
    }



    void SaveToDB(string date, string fileName) {
        dBManager.LoadDBJsonData();

        string stationName = PlayerPrefs.GetString("stationName");
        Debug.Log("date: " + date + " file name: " + fileName + " name: " + stationName);
        DBDataDetailModel dBDataDetailModel = new DBDataDetailModel();
        dBDataDetailModel.datetime = date;
        dBDataDetailModel.img = Application.persistentDataPath + "/" + fileName;
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
