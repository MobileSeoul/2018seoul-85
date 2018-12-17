using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class DeviceCheckManager : MonoBehaviour {

    public Camera firstPersonCamera;
    public ScoreboardController scoreboard;
    public SnakeController snakeController;

	// Use this for initialization
    void Start()
    {
        QuitOnConnectionErrors();
    }
	
	// Update is called once per frame
	void Update () {
        // The session status must be Tracking in order to access the Frame.
        if (Session.Status != SessionStatus.Tracking)
        {
            int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
            return;
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // Add to the end of Update()
        ProcessTouches();
	}

    void QuitOnConnectionErrors()
    {
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
                "This device is rdy for run ARCore", 5));
        }
    }

    void ProcessTouches()
    {
        Touch touch;
        if (Input.touchCount != 1 ||
            (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        TrackableHit hit;
        TrackableHitFlags raycastFilter =
            TrackableHitFlags.PlaneWithinBounds |
            TrackableHitFlags.PlaneWithinPolygon;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            SetSelectedPlane(hit.Trackable as DetectedPlane);
        }
    }

    void SetSelectedPlane(DetectedPlane selectedPlane)
    {
        //Debug.Log("Selected plane centered at " + selectedPlane.CenterPose.position);
        StartCoroutine(CodelabUtils.Toast(
            "Selected plane centered at " + selectedPlane.CenterPose.position, 5));
        scoreboard.SetSelectedPlane(selectedPlane);
        snakeController.SetPlane(selectedPlane);
    }
}
