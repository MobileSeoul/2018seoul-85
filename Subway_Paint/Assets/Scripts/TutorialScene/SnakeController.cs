using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour {

    public GameObject unityChanSDPrefab;
    public Text txtSnakeSpeedDebugger;
    public DetectedPlane detectedPlane;
    public GameObject snakeHeadPrefab;
    public GameObject pointer;
    public Camera firstPersonCamera;
    // Speed to move.
    public float speed = 20f;

    private GameObject snakeInstance;
    private GameObject unityChanSDInstance;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (snakeInstance == null || snakeInstance.activeSelf == false)
        //{
        //    pointer.SetActive(false);
        //    return;
        //}
        //else
        //{
        //    pointer.SetActive(true);
        //}
        if(unityChanSDPrefab == null || unityChanSDPrefab.activeSelf == false) {
            pointer.SetActive(false);
        }
        else {
            pointer.SetActive(true);
        }

        MakeFollowerTrackPoint(unityChanSDInstance);
	}

    public void MakeFollowerTrackPoint(GameObject follower) {
        TrackableHit hit;
        Vector3 followerVelocity = new Vector3();
        Animator followerAnimator = follower.GetComponent<Animator>();
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds;

        if (Frame.Raycast(Screen.width / 2, Screen.height / 2, raycastFilter, out hit))
        {
            Vector3 pt = hit.Pose.position;
            //Set the Y to the Y of the snakeInstance
            pt.y = follower.transform.position.y;
            // Set the y position relative to the plane and attach the pointer to the plane
            Vector3 pos = pointer.transform.position;
            pos.y = pt.y;
            pointer.transform.position = pos;

            // Now lerp to the position                                         
            pointer.transform.position = Vector3.Lerp(pointer.transform.position, pt,
              Time.smoothDeltaTime * speed);
        }

        // Move towards the pointer, slow down if very close.                                                                                     
        float dist = Vector3.Distance(pointer.transform.position,
                                      follower.transform.position) - 0.05f;
        if (dist < 0)
        {
            dist = 0;
        }

        Rigidbody rb = follower.GetComponent<Rigidbody>();
        rb.transform.LookAt(pointer.transform.position);
        followerVelocity = follower.transform.localScale.x *
                                follower.transform.forward * dist / .01f;
        rb.velocity = followerVelocity;
        rb.AddForce(followerVelocity, ForceMode.VelocityChange);
        followerAnimator.SetFloat("speed", dist);
        txtSnakeSpeedDebugger.text = "rigidbody velocity: " + rb.velocity + " dist: " + dist;
    }

    public void SetPlane(DetectedPlane plane)
    {
        detectedPlane = plane;
        // Spawn a new snake.
        //SpawnSnake();
        SpawnUnityChanSD();
    }

    void SpawnUnityChanSD() {
        if(unityChanSDInstance != null) {
            DestroyImmediate(unityChanSDInstance);
        }

        Vector3 pos = detectedPlane.CenterPose.position;

        unityChanSDInstance = Instantiate(unityChanSDPrefab, pos, Quaternion.identity, transform);

    }

    void SpawnSnake ()
    {
        if (snakeInstance != null)
        {
            DestroyImmediate (snakeInstance);
        }

        Vector3 pos = detectedPlane.CenterPose.position;

        // Not anchored, it is rigidbody that is influenced by the physics engine.
        snakeInstance = Instantiate (snakeHeadPrefab, pos,
                Quaternion.identity, transform);

        // Pass the head to the slithering component to make movement work.
        GetComponent<Slithering> ().Head = snakeInstance.transform;
    }
}
