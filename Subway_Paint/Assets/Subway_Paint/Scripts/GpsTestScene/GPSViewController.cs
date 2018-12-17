using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GPSViewController : MonoBehaviour {

    public GPSManager gpsManager;
    public SubwayManager subwayManager;
    public Text txtLogger, txtMyLocation;
    float[] gpsLastLoc;
    float systemMilSec;
    float UPDATE_TIME_GAP = 5;

	// Use this for initialization
	void Start () {
        gpsManager.SetTxtLogger(txtLogger);
        subwayManager.SetTxtLogger(txtLogger);
        gpsManager.GPSEnable();
        subwayManager.LoadSubwayJsonData();
        systemMilSec = 0;
	}
	
	// Update is called once per frame
	void Update () {
        systemMilSec = systemMilSec + Time.deltaTime;
        if(UPDATE_TIME_GAP < systemMilSec) {
            systemMilSec = 0;
            gpsLastLoc = gpsManager.GetLastLocationData();
            AppendLogger("lat: " + gpsLastLoc[0] + " long: " + gpsLastLoc[1] + " state: " + gpsLastLoc[2]);
            SubwayDataDetailModel subwayDataDetailModel = subwayManager.FindNearestSubwayStation(gpsLastLoc[0], gpsLastLoc[1]);
            if(subwayDataDetailModel == null) {
                AppendLogger("cannot find nearest station");
                SetNearestSubwayLocation(null);
            }
            else {
                AppendLogger("station name: " + subwayDataDetailModel.station_nm);
                Debug.Log("nearest station: " + subwayDataDetailModel.station_nm);
                SaveNearestStation(subwayDataDetailModel);
                           
                SetNearestSubwayLocation(subwayDataDetailModel);
            }
        }
	}

    void SaveNearestStation(SubwayDataDetailModel subwayDataDetailModel) {
        PlayerPrefs.SetString("stationName", subwayDataDetailModel.station_nm);
        PlayerPrefs.SetString("stationCode", subwayDataDetailModel.fr_code);
        PlayerPrefs.Save();
    }

    void SetNearestSubwayLocation(SubwayDataDetailModel subwayDataDetailModel) {
        if(txtMyLocation != null) {
            if(subwayDataDetailModel != null) {
                txtMyLocation.text = "내 위치 " + subwayDataDetailModel.station_nm + "역";
            }
            else {
                txtMyLocation.text = "가장 가까운 역을 검색하는 중입니다...";
            }
        }
    }

    public void BtnEnableGPS() {
        Debug.Log("enable gps");

        var returnVal = StartCoroutine(gpsManager.GPSEnable());
        if(returnVal != null) {
            Debug.Log("returned val: " + returnVal.ToString());
            AppendLogger("returned val: " + returnVal.ToString());
        }
    }

    public void BtnLoadJson() {
        subwayManager.LoadSubwayJsonData();
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
}
