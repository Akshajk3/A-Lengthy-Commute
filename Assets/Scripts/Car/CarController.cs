using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class CarController : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public GameObject steeringWheelModel;

    public Vector3 CenterRot;
    public Vector3 LeftRot;
    public Vector3 RightRot;

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;
    public float topSpeed = 20.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRB;

    public GameObject camera;

    private bool inCar = false;

    private void Start()
    {
        carRB = GetComponent<Rigidbody>();
        carRB.centerOfMass = _centerOfMass;
    }

    private void Update()
    {
        GetInput();
        AnimateWheels();
        AnimateSteeringWheel();
    }

    private void LateUpdate()
    {
        Move();
        Brake();
        Steer();
        LimitSpeed();
    }

    private void GetInput()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    private void Move()
    {
        foreach(var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * 600f * maxAcceleration * Time.deltaTime;
        }
    }

    private void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = brakeAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    private void Steer()
    {
        foreach(var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    [PunRPC]
    private void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    [PunRPC]
    private void AnimateSteeringWheel()
    {
        var currentAngle = steeringWheelModel.transform.localRotation;

        if (steerInput > 0.3)
        {
            steeringWheelModel.transform.localRotation = Quaternion.Lerp(currentAngle, Quaternion.Euler(RightRot), 0.1f);
        }
        else if (steerInput < -0.3)
        {
            steeringWheelModel.transform.localRotation = Quaternion.Lerp(currentAngle, Quaternion.Euler(LeftRot), 0.1f);
        }
        else
        {
            steeringWheelModel.transform.localRotation = Quaternion.Lerp(currentAngle, Quaternion.Euler(CenterRot), 0.1f);
        }
    }

    private void LimitSpeed()
    {
        if (carRB.velocity.magnitude > topSpeed)
        {
            carRB.velocity = carRB.velocity.normalized * topSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
    }

    public void EnterCar()
    {
        inCar = true;
        camera.active = true;
    }

    public void ExitCar()
    {
        inCar = false;
        camera.active = false;
    }
}
