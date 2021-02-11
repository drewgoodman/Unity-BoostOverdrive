using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    enum CamState { ShowGoal, PanToTarget, AtRest, TrackRocket };
    CamState state = CamState.ShowGoal;
    [SerializeField] GameObject rocketPlayer;
    [SerializeField] GameObject endingObject;

    Rocket rocketPlayerScript;

    Vector3 rocketPlayerOffset;
    [SerializeField] GameObject[] levelPanTargets;
    int numPanTargets = 0;
    int currentPanTargetIndex = 0;

    GameObject currentPanTarget;

    [SerializeField] float startPanDelayOnset = .7f;
    [SerializeField] float startPanDelay = 2f;
    [SerializeField] float panToTargetSpeedMult = .8f;
    float panToTargetDistance;
    float panToTargetSpeed;

    void Start()
    {
        if (rocketPlayer && endingObject)
        {
            numPanTargets = levelPanTargets.Length;
            rocketPlayerScript = rocketPlayer.GetComponent<Rocket>();
            rocketPlayerOffset = transform.position - rocketPlayer.transform.position;
            transform.position = endingObject.transform.position + rocketPlayerOffset;
            Invoke("SetCameraPan", startPanDelayOnset);
        }
        else
        {
            print("Warning! Player and End Goal Objects unassigned!");
        }
    }

    void Update()
    {
        if (state == CamState.PanToTarget)
        {
            PanToTarget();
            CheckForPanCancelInput();
        }
        else if (state == CamState.TrackRocket)
        {
            TrackRocket();
        }
    }

    Vector3 TargetCameraOffset(GameObject target)
    {
        return target.transform.position + rocketPlayerOffset;
    }

    void TrackRocket()
    {
        transform.position = TargetCameraOffset(rocketPlayer);
    }

    void SetCameraPan()
    {
        if(currentPanTargetIndex >= numPanTargets)
        {
            currentPanTarget = rocketPlayer;
        }
        else
        {
            currentPanTarget = levelPanTargets[currentPanTargetIndex++];
        }
        SetCameraPanTarget(currentPanTarget);
    }

    void SetCameraPanTarget(GameObject target)
    {
        panToTargetDistance = Vector3.Distance(transform.position, TargetCameraOffset(target));
        panToTargetSpeed = panToTargetDistance * panToTargetSpeedMult;
        state = CamState.PanToTarget;
    }

    void PanToTarget()
    {
        Vector3 panEndPosition = TargetCameraOffset(currentPanTarget);
        transform.position = Vector3.MoveTowards(transform.position, panEndPosition, Time.deltaTime * panToTargetSpeed);
        if (transform.position == panEndPosition) {
            if (currentPanTarget == rocketPlayer)
            {
                SetCameraToTrack();
            }
            else
            { 
                state = CamState.AtRest;
                Invoke("SetCameraPan", startPanDelay);
            }
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
