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

    private RocketMovement rocketMovement;

    private void Start()
    {
        rocketMovement = rocket.GetComponent<RocketMovement>();
    }
    private void FixedUpdate()
    {
        heightCurrent.text = "Altura: " + rocket.position.y.ToString();
        heightMax.text = "Altura Max: " + rocketMovement.GetMaxHeight().ToString();
        velocity.text = "Velocidade: " + rocket.velocity.ToString();
    }
}
