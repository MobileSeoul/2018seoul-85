using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class SceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        QuitOnConnectionErrors();
	}
	
	// Update is called once per frame
	void Update () {
        // The session status must be Tracking in order to access the Frame.
        try {
            if (Session.Status != SessionStatus.Tracking)
            {
                int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
                return;
            }
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        catch(UnityException except) {
            StartCoroutine(CodelabUtils.ToastAndExit(
                "error accured: " + except, 5));
        }
	}

    void QuitOnConnectionErrors() {
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            StartCoroutine(CodelabUtils.ToastAndExit(
                "Camera permission is needed to run this application.", 5));
        }
        else if (Session.Status.IsError())
        {
            // This covers a variety of errors.  See reference for details
            // https://developers.google.com/ar/reference/unity/namespace/GoogleARCore
            StartCoroutine(CodelabUtils.ToastAndExit(
                "ARCore encountered a problem connecting. Please restart the app.", 5));
        }
        else {
            StartCoroutine(CodelabUtils.Toast(
                "Welcome to ARCore", 5));
        }
    }
}
