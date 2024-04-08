using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDoor : Interactable
{
    public GameObject player;
    public GameObject car;

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
        }
    }

    protected override void Interact()
    {
        carController.EnterCar();
        player.active = false;
        entered = true;
    }
}
