using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    enum CamState { ShowGoal, PanToRocket, TrackRocket };
    CamState state = CamState.ShowGoal;

    [SerializeField]
    GameObject rocketPlayer;
    [SerializeField]
    GameObject endingObject;

    Rocket rocketPlayerScript;

    Vector3 rocketPlayerOffset;

    [SerializeField]
    float startPanDelay = .5f;
    [SerializeField]
    float panToRocketSpeed = 25f;

    // Start is called before the first frame update
    void Start()
    {
        if (rocketPlayer && endingObject)
        {
            rocketPlayerScript = rocketPlayer.GetComponent<Rocket>();
            rocketPlayerOffset = transform.position - rocketPlayer.transform.position;
            transform.position = endingObject.transform.position + rocketPlayerOffset;
            Invoke("SetCameraPan", startPanDelay);
        }
        else
        {
            print("Warning! Player and End Goal Objects unassigned!");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (state == CamState.PanToRocket)
        {
            PanToRocket();
        }
        else if (state == CamState.TrackRocket)
        {
            TrackRocket();
        }
    }

    void TrackRocket()
    {
        transform.position = rocketPlayer.transform.position + rocketPlayerOffset;
    }

    void SetCameraPan()
    {
        state = CamState.PanToRocket;
    }

    void PanToRocket()
    {
        Vector3 startingPlayerPos = rocketPlayer.transform.position + rocketPlayerOffset;
        transform.position = Vector3.MoveTowards(transform.position, startingPlayerPos, Time.deltaTime * panToRocketSpeed);
        if (transform.position == startingPlayerPos) {
            SetCameraToTrack();
        }
    }

    void SetCameraToTrack()
    {
        state = CamState.TrackRocket;
        rocketPlayerScript.ReadyToPlay();
        print("Ready to play!");
    }
}
