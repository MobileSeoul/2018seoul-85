using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubwayManager : MonoBehaviour {

    Text txtLogger;
    SubwayDataModel subwayJsonData;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTxtLogger(Text txtLogger)
    {
        this.txtLogger = txtLogger;
        AppendLogger("text logger attached");
    }

    public void AppendLogger(string logMsg)
    {
        if (txtLogger == null)
        {
            Debug.Log("txt logger is null");
            return;
        }
        txtLogger.text = txtLogger.text + "\n" + logMsg;
    }

    public void LoadSubwayJsonData() {
        //string path = Application.persistentDataPath + "/Resources/Data/Subway.json";
        TextAsset jsonTextAsset = Resources.Load("Data/Subway") as TextAsset;
        //jsonTextAsset.
        string jsonString = jsonTextAsset.text;

        Debug.Log("load data: " + jsonString);
        AppendLogger("subway data loaded len: " + jsonString.Length);

        ParseSubwayJsonData(jsonString);
    }

    public void ParseSubwayJsonData(string jsonText) {
        subwayJsonData = JsonUtility.FromJson<SubwayDataModel>(jsonText);
        Debug.Log("parsed data len: " + subwayJsonData.DATA.Count);

        //TestShowLoadedData();
    }

    public SubwayDataDetailModel FindNearestSubwayStation(double lat, double lon) {
        double min = 987654321, distance;
        SubwayDataDetailModel nearestStation = null;

        foreach(SubwayDataDetailModel subwayDataDetailModel in subwayJsonData.DATA) {
            if(subwayDataDetailModel.xpoint_wgs != null && subwayDataDetailModel.ypoint_wgs != null && subwayDataDetailModel.xpoint_wgs != "" && subwayDataDetailModel.ypoint_wgs != "") {
                try {
                    //Debug.Log("" + subwayDataDetailModel.xpoint_wgs + ", " + subwayDataDetailModel.ypoint_wgs);
                    double lat2 = double.Parse(subwayDataDetailModel.xpoint_wgs), lon2 = double.Parse(subwayDataDetailModel.ypoint_wgs);

                    distance = GpsDistance(lat, lon, lat2, lon2);
                    if (distance < min)
                    {
                        Debug.Log("change min: " + subwayDataDetailModel.station_nm + " dist: " + distance + " min: " + min);
                        min = distance;
                        nearestStation = subwayDataDetailModel;
                    }
                }
                catch(UnityException except) {
                    Debug.Log("cannot parse data: " + except.Message);
                }


            }
        }
        if(nearestStation != null) {
            Debug.Log("nearest station: " + nearestStation.station_nm + " dist: " + min.ToString());
        }
        return nearestStation;
    }

    double Deg2Rad(double deg)
    {
        return (double)(deg * Mathf.PI / (double)180d);
    }

    double Rad2Deg(double rad)
    {
        return (double)(rad * (double)180d / Mathf.PI);
    }

    double GpsDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double theta, dist;
        theta = lon1 - lon2;


        dist = Mathf.Sin((float)Deg2Rad(lat1)) * Mathf.Sin((float)Deg2Rad(lat2)) + Mathf.Cos((float)Deg2Rad(lat1)) * Mathf.Cos((float)Deg2Rad(lat2)) * Mathf.Cos((float)Deg2Rad(theta));
        dist = Mathf.Acos((float)dist);
        dist = Rad2Deg(dist);
        dist = dist * 60 * 1.1515;
        dist = dist * 1.609344;
        return dist;
    }

    void TestShowLoadedData() {
        foreach(SubwayDataDetailModel subwayDataDetailModel in subwayJsonData.DATA) {
            Debug.Log("name: " + subwayDataDetailModel.station_nm + " lat: " + subwayDataDetailModel.xpoint_wgs + " lon: " + subwayDataDetailModel.ypoint_wgs);
        }
    }
}
