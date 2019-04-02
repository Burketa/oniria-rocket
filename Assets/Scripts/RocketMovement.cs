﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    public bool debugText = true;   //Mostrar o texto no console ao lançar

    [Header("Rocket Config")]
    [Tooltip("Speed of the rocket.")]
    public float speed = 1000f;
    [Tooltip("Time in seconds to deplet fuel in each stage.")]
    public float timerMax = 5f;

    [Header("Parachute Config")]
    public ParachuteController parachute;
    [Tooltip("How much air resistance the parachute will provide ?")]
    public float dragCoef = 10f;

    [Header("Modules Config")]
    public GameObject firstStage;
    public GameObject secondStage;

    [HideInInspector]
    public string status;

    private float maxHeight;
    private Rigidbody _rgdbdy;  //Caching nos componentes para melhor performance.
    private ParticleSystem _particle;
    private Animator _parachuteAmim;
    private AudioSource _audio;
    private Cinemachine.CinemachineVirtualCamera cameraFX;
    Cinemachine.CinemachineBasicMultiChannelPerlin perlin;

    [Header("Private Vars")]
    [SerializeField]
    private float timerCurrent = 0f;

    void Awake()
    {
        _rgdbdy = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
        maxHeight = transform.position.y;
        _particle = GetComponentInChildren<ParticleSystem>();

        cameraFX = GameObject.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        perlin = cameraFX.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        ShakeCamera(false);
    }

    void Start()
    {
        StartCoroutine(LauchSequence());
    }

    void FixedUpdate()
    {
        switch (StateManager.GetState())
        {
            case StateManager.gameState.START:
                StartCoroutine(Thrust(StateManager.gameState.STAGE1));
                break;

            case StateManager.gameState.SEPARATING1:
                SeparateModule(firstStage);
                break;

            case StateManager.gameState.SEPARATING2:
                SeparateModule(secondStage);
                break;

            case StateManager.gameState.PARACHUTE:
                if (_rgdbdy.velocity.y < 0)
                {
                    parachute.OpenParachute(_rgdbdy, dragCoef);
                    status = "Parachute deployed.";
                }

                break;
        }
    }
    void Update()
    {
        DetectHeight();
    }

    IEnumerator Thrust(StateManager.gameState state)
    {
        StateManager.SetState(state);

        //Controle das particulas
        switch (state)
        {
            case StateManager.gameState.STAGE1:
                _rgdbdy.useGravity = true;
                ParticleManager.EmmitParticle(_particle, true);
                break;

            case StateManager.gameState.STAGE2:
                ParticleManager.MoveParticle(_particle, secondStage.transform.position);
                ParticleManager.EmmitParticle(_particle, true);
                break;
        }

        //Controle do "combustivel"
        while (timerCurrent < timerMax)
        {
            _rgdbdy.AddForce(transform.up * speed * Time.deltaTime);
            timerCurrent += Time.deltaTime;
            yield return null;
        }

        timerCurrent = 0;

        ParticleManager.EmmitParticle(_particle, false);

        StateManager.NextState(state);
    }

    private void SeparateModule(GameObject module)
    {
        status = "Iniciating separating sequence.";
        module.transform.parent = null;
        Rigidbody module_rgdbdy = module.AddComponent<Rigidbody>();
        module_rgdbdy.velocity = _rgdbdy.velocity * 0.95f + new Vector3(Random.Range(-2, 2), Random.Range(-2, 0), Random.Range(-2, 2));
        module_rgdbdy.drag = _rgdbdy.drag;

        StateManager.NextState();

        if (module == firstStage)
        {
            StartCoroutine(Thrust(StateManager.gameState.STAGE2));
            status = "Separation sequence 1 completed.";
        }
        else
        {
            ShakeCamera(false);
            _audio.Stop();
            status = "Separation sequence 2 completed.";
        }
    }

    IEnumerator LauchSequence()
    {
        if (debugText)
        {
            status = "Systems starting...";
            yield return new WaitForSeconds(1);
            status = "Inializing protocols...";
            yield return new WaitForSeconds(1);
            status = "All modules ready.";
            yield return new WaitForSeconds(1);
            status = "Launching in: 3...";
            yield return new WaitForSeconds(1);
            status = "Launching in: 2...";
            yield return new WaitForSeconds(1);
            status = "Launching in: 1...";
            yield return new WaitForSeconds(1);
            status = "Now.";
            yield return new WaitForSeconds(1);
        }
        status = "Launched.";

        StateManager.SetState(StateManager.gameState.START);

        _audio.Play();

        ShakeCamera(true);

        yield return null;
    }

    private void DetectHeight()
    {
        if (_rgdbdy.position.y >= maxHeight)
            maxHeight = _rgdbdy.position.y;

        if ((_rgdbdy.velocity.y <= 0) && (_rgdbdy.position.y > maxHeight))
        {
            maxHeight = _rgdbdy.position.y;
            status = "Maximum height detected.";
        }
    }

    public float GetMaxHeight()
    {
        return maxHeight;
    }

    void OnCollisionEnter(Collision col)
    {
        if ((col.gameObject.tag == "Terrain") && (StateManager.GetState() == StateManager.gameState.LANDING))
        {
            parachute.CloseParachute();
            status = "Safely landed.";
        }
    }

    private void ShakeCamera(bool state)
    {
        if (state)
            perlin.enabled = true;
        else
            perlin.enabled = false;
    }
}
