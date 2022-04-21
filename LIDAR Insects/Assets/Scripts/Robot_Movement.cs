using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Movement : MonoBehaviour
{
    private Rigidbody rb;

    //Vector3 cameraRelativeUp;
    public bool isGrounded;

    Vector3 cameraRelativeForward;
    private float moveInput;
    private float turnInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();  
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        transform.Rotate(Vector3.up * turnInput * 3.0f);

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * 5.0f, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        //cameraRelativeUp = transform.TransformDirection (Vector3.up);
        cameraRelativeForward = transform.TransformDirection (Vector3.forward);

        //rb.AddForce(cameraRelativeUp * -9.81f, ForceMode.Acceleration);
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, 0.5f);

        rb.AddForce(cameraRelativeForward * moveInput, ForceMode.VelocityChange);
        if (Vector3.Dot(rb.velocity, transform.forward) > 3.0f || Vector3.Dot(rb.velocity, transform.forward) < -3.0f)
        {
            Vector3 locVel = transform.InverseTransformDirection(rb.velocity);
            if (locVel.z > 3.0f)
                locVel.z = 3.0f;
            else
                locVel.z = -3.0f;
            rb.velocity = transform.TransformDirection(locVel);
        }
        else if (moveInput == 0.0f)
        {
            Vector3 locVel = transform.InverseTransformDirection(rb.velocity);
            locVel.z = 0.0f;
            rb.velocity = transform.TransformDirection(locVel);
        }


    }
}
