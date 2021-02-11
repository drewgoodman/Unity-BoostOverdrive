using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    GameObject orbitPoint;

    [SerializeField]
    float rotationSpeed; //degrees per second -- use negative for counterclockwise

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotationThisFrame = rotationSpeed * Time.deltaTime;
        transform.RotateAround(orbitPoint.transform.position, Vector3.forward, rotationThisFrame);
    }
}
