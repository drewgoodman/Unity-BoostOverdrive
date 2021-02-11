using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    [SerializeField]
    float spin = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotationThisFrame = spin * Time.deltaTime;
        transform.Rotate(Vector3.forward * rotationThisFrame);
    }
}
