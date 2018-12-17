using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSManager : MonoBehaviour {

    Text txtLogger;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTxtLogger(Text txtLogger) {
        this.txtLogger = txtLogger;
        AppendLogger("text logger attached");
    }

    public IEnumerator GPSEnable() {
        if (!Input.location.isEnabledByUser) {
            Debug.Log("cannot enable gps module");
            AppendLogger("cannot enable gps module");
            yield break;
        }
        else {
            Debug.Log("gps module enabled");
            AppendLogger("gps module enabled");
        }
        Input.location.Start();
        AppendLogger("gps input started");

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        switch(Input.location.status) {
            case LocationServiceStatus.Failed:
                AppendLogger("loc service failed");
                break;
            case LocationServiceStatus.Initializing:
                AppendLogger("loc service initializing");
                break;
            case LocationServiceStatus.Running:
                AppendLogger("loc service running");
                break;
            case LocationServiceStatus.Stopped:
                AppendLogger("loc service stopped");
                break;
            default:
                AppendLogger("loc service ???");
                break;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            AppendLogger("gps init timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            AppendLogger("unabled to determine device loc");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            AppendLogger("loc: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            yield return GetLastLocationData();
        }

    }

    public float[] GetLastLocationData() {
        if(Input.location.status == LocationServiceStatus.Running) {
            return new float[] { Input.location.lastData.latitude, Input.location.lastData.longitude, (float)Input.location.status};
        }
        return new float[] { -1, -1, (float)Input.location.status};//37.53153f,127.1229f
    }

    public IEnumerator DisableGPS() {
        Input.location.Stop();
        AppendLogger("input location stopped");
        yield break;
    }

    public void AppendLogger(string logMsg) {
        if(txtLogger == null) {
            Debug.Log("txt logger is null");
            return;
        }
        txtLogger.text = txtLogger.text + "\n" + logMsg;
    }


}
