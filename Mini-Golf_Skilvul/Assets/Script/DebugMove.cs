using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMove : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float force;

    Vector3 MoveDir;

    void Update()
    {
        MoveDir = new Vector3
        (
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );
    }

    private void FixedUpdate()
    {
        if (MoveDir != Vector3.zero)
        {
            rb.AddForce(MoveDir * force, ForceMode.Force);
        }
    }

}
