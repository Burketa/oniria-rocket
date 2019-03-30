using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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
    }
    private void FixedUpdate()
    {
        heightCurrent.text = "Altura: " + _rocket.position.y.ToString("0.0");
        heightMax.text = "MAX: " + rocketMovement.GetMaxHeight().ToString("0.0");
        velocity.text = "Velocidade: " + _rocket.velocity.ToString();
        rotation.text = "Rotação: " + _rocket.rotation.eulerAngles.ToString();
    }
}
