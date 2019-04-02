﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public bool showVelocity;
    public bool showRotation;
    public bool showHeight;

    public Rigidbody rocket;
    public Text heightCurrent;
    public Text heightMax;
    public Text velocity;
    public Text rotation;

    private Rigidbody _rocket;
    private RocketMovement rocketMovement;

    private void Start()
    {
        rocketMovement = rocket.GetComponent<RocketMovement>();
        _rocket = rocket;

        if (showHeight)
            heightCurrent.gameObject.SetActive(true);
        else
            heightCurrent.gameObject.SetActive(false);

        if (showVelocity)
            velocity.gameObject.SetActive(true);
        else
            velocity.gameObject.SetActive(false);

        if (showRotation)
            rotation.gameObject.SetActive(true);
        else
            rotation.gameObject.SetActive(false);

    }
    private void FixedUpdate()
    {
        heightMax.text = "MAX: " + rocketMovement.GetMaxHeight().ToString("0.0") + "m";
        if (showHeight)
            heightCurrent.text = "Altura: " + _rocket.position.y.ToString("0.0") + "m";
        if (showVelocity)
            velocity.text = "Velocidade: " + _rocket.velocity.ToString() + "m/s";
        if (showRotation)
            rotation.text = "Rotação: " + _rocket.rotation.eulerAngles.ToString();
    }
}
