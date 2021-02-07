using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject cameraTarget;

    Vector3 cameraTargetOffset;
    // Start is called before the first frame update
    void Start()
    {
        if (cameraTarget)
        {
            cameraTargetOffset = transform.position - cameraTarget.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraTarget.transform.position + cameraTargetOffset;
    }
}
