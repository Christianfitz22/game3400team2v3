using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    [SerializeField]
    private Vector3 throwPos;
    [SerializeField]
    private float throwForce;
    [SerializeField]
    private float throwRadius;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void roll()
    {
        transform.position = throwPos;
        transform.rotation = Random.rotation;
        rb.velocity = Vector3.zero;
        float directionDegrees = Random.Range(-throwRadius, throwRadius);
        float directionRadians = directionDegrees * Mathf.PI / 180f;
        float forceZ = throwForce * Mathf.Cos(directionRadians);
        float forceX = throwForce * Mathf.Sin(directionRadians);
        Vector3 force = new Vector3(forceX, 0f, -forceZ);
        rb.AddForce(force, ForceMode.VelocityChange);
    }

    /*
     1 = 0 Y 0
     2 = 0 Y -90
     3 = 90 0 Z
     4 = -90 0 Z
     5 =  0 Y 90
     6 = -180 Y 0
     */

    public int getResult()
    {
        Vector3 rotation = transform.localRotation.eulerAngles;
        if (Mathf.Aprroximately(rotation.x, 0f))
        {
            if (Mathf.Approximately(rotation.z, 0f))
            {
                return 1;
            }
            if (Mathf.Approximately(rotation.z, -90f))
            {
                return 2;
            }
            if (Mathf.Approximately(rotation.z, 90f))
            {
                return 5;
            }
        }
        if (Mathf.Approximately(rotation.y, 0f))
        {
            if (Mathf.Approximately(rotation.x, 90f))
            {
                return 3;
            }
            if (Mathf.Approximately(rotation.x, -90f))
            {
                return 4;
            }
        }
        if (Mathf.Approximately(rotation.z, 0f) && Mathf.Approximately(rotation.x, -180f))
        {
            return 6;
        }
        return -1;
    }
}
