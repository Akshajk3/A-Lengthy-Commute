using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] KeyCode pickupKey = KeyCode.Mouse0;
    [SerializeField] Transform holdArea;
    [SerializeField] float maxDistance;
    [SerializeField] float minDistance;
    [SerializeField] LayerMask pickupable;
    private GameObject heldObject;
    private Rigidbody heldObjRB;


    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickUpForce = 150.0f;

    private float distance;

    private void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            if (heldObject == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange, pickupable))
                {
                    PickupObject(hit.transform.gameObject);
                }
            }
            else
            {
                DropObject();
            }
        }
        else if (heldObject != null)
        {
            MoveObject();

            distance = Vector3.Distance(gameObject.transform.position, holdArea.position);

            float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

            if (mouseWheel < 0 && distance > minDistance)
                holdArea.position = Vector3.MoveTowards(holdArea.position, gameObject.transform.position, 0.1f);
            if (mouseWheel > 0 && distance < maxDistance)
                holdArea.position = Vector3.MoveTowards(holdArea.position, gameObject.transform.position, -0.1f);
        }

    }

    void PickupObject(GameObject obj)
    {
        if (obj.GetComponent<Rigidbody>())
        {
            heldObjRB = obj.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjRB.transform.parent = holdArea;
            heldObject = obj;
        }
    }

    void DropObject()
    {
        heldObjRB.useGravity = true;
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;

        heldObject.transform.parent = null;
        heldObject = null;
    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObject.transform.position, holdArea.transform.position) > 0.1)
        {
            Vector3 moveDirection = holdArea.position - heldObject.transform.position;
            heldObjRB.AddForce(moveDirection * pickUpForce);
        }
    }
}
