using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Camera camera;
    [SerializeField]
    private float distance = 3f;

    [SerializeField]
    private KeyCode interactKey = KeyCode.E;

    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;

    void Start()
    {
        playerUI = GetComponent<PlayerUI>();
    }
    
    void Update()
    {
        playerUI.UpdateText(string.Empty);

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(hitInfo.collider.GetComponent<Interactable>().promptMessage);
                if (Input.GetKeyDown(interactKey))
                {
                    interactable.BaseInteract();
                }
            }
        }
    }
}
