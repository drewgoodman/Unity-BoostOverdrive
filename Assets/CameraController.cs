using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    enum CamState { ShowGoal, PanToRocket, TrackRocket };
    CamState state = CamState.ShowGoal;
    [SerializeField] GameObject rocketPlayer;
    [SerializeField] GameObject endingObject;

    Rocket rocketPlayerScript;

    Vector3 rocketPlayerOffset;

    [SerializeField] float startPanDelay = .5f;
    [SerializeField] float panToRocketSpeedMult = 1f;
    float panToRocketDistance;
    float panToRocketSpeed;

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
            CheckForPanCancelInput();
        }
        else if (state == CamState.TrackRocket)
        {
            TrackRocket();
        }
    }

    Vector3 PlayerCameraOffset()
    {
        return rocketPlayer.transform.position + rocketPlayerOffset;
    }

    void TrackRocket()
    {
        transform.position = PlayerCameraOffset();
    }

    void SetCameraPan()
    {
        panToRocketDistance = Vector3.Distance(transform.position, PlayerCameraOffset());
        panToRocketSpeed = panToRocketDistance * panToRocketSpeedMult;
        state = CamState.PanToRocket;
    }

    void PanToRocket()
    {
        Vector3 startingPlayerPos = PlayerCameraOffset();
        transform.position = Vector3.MoveTowards(transform.position, startingPlayerPos, Time.deltaTime * panToRocketSpeed);
        if (transform.position == startingPlayerPos) {
            SetCameraToTrack();
        }
    }

    void CheckForPanCancelInput()
    {
        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))
        {
            SetCameraToTrack();
        }
    }

    void SetCameraToTrack()
    {
        state = CamState.TrackRocket;
        rocketPlayerScript.ReadyToPlay();
    }
}
