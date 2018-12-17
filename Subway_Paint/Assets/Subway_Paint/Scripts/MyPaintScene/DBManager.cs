using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DBManager : MonoBehaviour {

    DBDataModel dbJsonData;
    string DB_FILE_PATH = "/Data/DB.json";
    string DB_FILE_NAME = "DB.json";

	private void Awake()
	{
        string path = Path.Combine(Application.persistentDataPath, DB_FILE_PATH);//Application.persistentDataPath + DB_FILE_PATH;
        path = Application.persistentDataPath + DB_FILE_PATH;
        if (!File.Exists(path))
        {
            string dirName = Path.GetDirectoryName(path);
            Debug.Log("dir not exist so create new one: " + dirName);
            System.IO.Directory.CreateDirectory(dirName);
        }
        else
        {
            Debug.Log("dir already created");
        }
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadDBJsonData()
    {
        //string path = Application.persistentDataPath + "/Resources/Data/DB.json";
        string path = Path.Combine(Application.persistentDataPath, DB_FILE_PATH);//Application.persistentDataPath + DB_FILE_PATH;
        path = Application.persistentDataPath + DB_FILE_PATH;
        string jsonString;
        TextAsset jsonTextAsset = null;
        if (!File.Exists(path)) {
            string dirName = Path.GetDirectoryName(path);
            Debug.Log("test persistent path: " + Application.persistentDataPath);
            Debug.Log("init db file path: " + path + " dir name: " + dirName);
            System.IO.Directory.CreateDirectory(dirName);
            jsonTextAsset = Resources.Load("Data/DB") as TextAsset;

            //jsonTextAsset.
            jsonString = jsonTextAsset.text;

            Debug.Log("load data: " + jsonString);

            ParseDBJsonData(jsonString);

            SaveDBJsonData();
        }
        jsonString = File.ReadAllText(path);

        Debug.Log("load data: " + jsonString);

        ParseDBJsonData(jsonString);
    }

    void ParseDBJsonData(string jsonText)
    {
        dbJsonData = JsonUtility.FromJson<DBDataModel>(jsonText);

        foreach(DBDataDetailModel detail in dbJsonData.DB) {
            Debug.Log("" + detail.img + ", " + detail.location + ", " + detail.datetime);
        }
        //TestShowLoadedData();
    }

    public List<DBDataDetailModel> GetMyPaintsDB() {
        return dbJsonData.DB;
    }

    public void AppendDB(DBDataDetailModel dbDataDetailModel) {
        dbJsonData.DB.Add(dbDataDetailModel);
        SaveDBJsonData();
    }

    void SaveDBJsonData() {
        string str = Json2String();
        Debug.Log("data: " + str);
        //Debug.Log("data: " + Application.dataPath + " " + Application.persistentDataPath + " " + Application.temporaryCachePath + " " + Application.streamingAssetsPath);
        string path = Path.Combine(Application.persistentDataPath, DB_FILE_PATH);
        path = Application.persistentDataPath + DB_FILE_PATH;
        Debug.Log("save path: " + path);

        //if (!System.IO.Directory.Exists(path))
        //{
        //    Debug.Log("dir not exist so create new one");
        //    System.IO.Directory.CreateDirectory(path);
        //}
        using (FileStream fs = new FileStream(path, FileMode.Create)){
         using (StreamWriter writer = new StreamWriter(fs)){
             writer.Write(str);
         }
     }
        //UnityEditor.AssetDatabase.SaveAssets();
        //UnityEditor.AssetDatabase.Refresh();
    }

    string Json2String() {
        return JsonUtility.ToJson(dbJsonData);
    }
}
