using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraController : MonoBehaviour
{
    public float moveSmoothness;
    public float rotationSmoothness;

    public Vector3 moveOffset;
    public Vector3 rotationOffset;

    public Transform carTarget;

    void FixedUpdate()
    {
        FollowTarget();    
    }

    void FollowTarget()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 targetPos = new Vector3();
        targetPos = carTarget.TransformPoint(moveOffset);

        transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothness * Time.deltaTime);
    }

    void HandleRotation()
    {
        var direction = carTarget.position - transform.position;
        var rotation = new Quaternion();

        rotation = Quaternion.LookRotation(direction + rotationOffset, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSmoothness * Time.deltaTime);
    }
}
