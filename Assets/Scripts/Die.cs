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
    }
}
