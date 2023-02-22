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
        if (Input.GetKeyDown(KeyCode.W))
        {
            //roll();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //Debug.Log(getResult());
        }
    }

    public void roll(Vector3 position, int direction)
    {
        transform.position = position + throwPos;
        transform.rotation = Random.rotation;
        rb.velocity = Vector3.zero;
        float directionDegrees = Random.Range(-throwRadius, throwRadius);
        float directionRadians = directionDegrees * Mathf.PI / 180f;
        float forceZ = throwForce * Mathf.Cos(directionRadians);
        float forceX = throwForce * Mathf.Sin(directionRadians);
        Vector3 force = new Vector3(forceX, 0f, direction * forceZ);
        rb.AddForce(force, ForceMode.VelocityChange);
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
     1 = 0 y 0
     2 = 0 y 270
     3 = 90 y 0
     4 = 270 y 0
     5 = 0 y 90
     6 = 0 y 180
     */

    public int getResult()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        Debug.Log(transform.rotation.eulerAngles);
        if (closeEnough(rotation.x, 0f) || closeEnough(rotation.x, 360f))
        {
            if (closeEnough(rotation.z, 0f) || closeEnough(rotation.z, 360f))
            {
                return 1;
            }
            if (closeEnough(rotation.z, 270f))
            {
                return 2;
            }
            if (closeEnough(rotation.z, 90f))
            {
                return 5;
            }
            if (closeEnough(rotation.z, 180f))
            {
                return 6;
            }
        }
        if (closeEnough(rotation.z, 0f) || closeEnough(rotation.z, 360f))
        {
            if (closeEnough(rotation.x, 90f))
            {
                return 3;
            }
            if (closeEnough(rotation.x, 270f))
            {
                return 4;
            }
        }
        return -1;
    }

    public bool closeEnough(float f1, float f2)
    {
        return Mathf.Abs(f1 - f2) < 5f;
    }
}
