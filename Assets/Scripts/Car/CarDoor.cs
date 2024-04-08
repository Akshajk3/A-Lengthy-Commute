using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDoor : Interactable
{
    public GameObject player;
    public GameObject car;
    public Transform exitPos;

    private CarController carController;

    bool entered = false;

    // Start is called before the first frame update
    void Start()
    {
        carController = car.GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && entered)
        {
            carController.ExitCar();
            player.active = true;
            entered = false;
        }

        if (entered)
        {
            player.transform.position = exitPos.position;
        }
    }

    public void SetPlayer(GameObject _player)
    {
        player = _player;
    }

    protected override void Interact()
    {
        if (!entered) 
        {
            carController.EnterCar();
            player.active = false;
            entered = true;
        }
    }
}
