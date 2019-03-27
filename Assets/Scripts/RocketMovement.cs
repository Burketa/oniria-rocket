using System.Collections;
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
    public GameObject parachute;
    [Tooltip("How much air resistance the parachute will provide ?")]
    public float dragCoef = 10f;

    [Header("Modules Config")]
    public GameObject firstStage;
    public GameObject secondStage;

    private float maxHeight;
    private Rigidbody _rgdbdy;  //Caching no componente para melhor performance.

    [Header("Private Vars")]
    [SerializeField]
    private float timerCurrent = 0f;

    void Awake()
    {
        _rgdbdy = GetComponent<Rigidbody>();
        maxHeight = transform.position.y + 1;  //Uma folga para não acusar a altura maxima no inicio do lançamento.
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
                    OpenParachute();
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

        _rgdbdy.useGravity = true;

        while (timerCurrent < timerMax)
        {
            _rgdbdy.AddForce(new Vector3(0, speed * Time.deltaTime, 0));
            //speed += speed / 1000;
            timerCurrent += Time.deltaTime;
            yield return null;
        }
        timerCurrent = 0;

        StateManager.NextState(state);
    }

    private void SeparateModule(GameObject module)
    {
        Debug.Log("Iniciating separating sequence.");
        module.transform.parent = null;
        Rigidbody module_rgdbdy = module.AddComponent<Rigidbody>();
        //Rigidbody module_rgdbdy = module.GetComponent<Rigidbody>();
        module_rgdbdy.velocity = _rgdbdy.velocity * 0.95f + new Vector3(Random.Range(-2, 2), Random.Range(-2, 0), Random.Range(-2, 2));
        module_rgdbdy.drag = _rgdbdy.drag;
        //module_rgdbdy.mass = _rgdbdy.mass / 3;
        //_rgdbdy.mass /= 3;

        StateManager.NextState();

        if (module == firstStage)
        {
            StartCoroutine(Thrust(StateManager.gameState.STAGE2));
            Debug.Log("Separation sequence 1 completed.");
        }
        else
            Debug.Log("Separation sequence 2 completed.");
    }

    IEnumerator LauchSequence()
    {
        if (debugText)
        {
            Debug.Log("Systems starting...");
            yield return new WaitForSeconds(1);
            Debug.Log("Inializing protocols...");
            yield return new WaitForSeconds(1);
            Debug.Log("All modules ready.");
            yield return new WaitForSeconds(1);
            Debug.Log("Launching in: 3");
            yield return new WaitForSeconds(1);
            Debug.Log("2");
            yield return new WaitForSeconds(1);
            Debug.Log("1");
            yield return new WaitForSeconds(1);
            Debug.Log("Now.");
        }

        StateManager.SetState(StateManager.gameState.START);

        yield return null;
    }

    private void DetectHeight()
    {
        if (_rgdbdy.position.y > maxHeight)
            maxHeight = _rgdbdy.position.y;

        if ((_rgdbdy.velocity.y <= 0) && (_rgdbdy.position.y > maxHeight))
        {
            maxHeight = _rgdbdy.position.y;
            Debug.Log("Maximum height detected: " + maxHeight);
        }
    }

    //TODO: Abrir o paraquedas apenas quando estiver caindo
    private void OpenParachute()
    {
        parachute.gameObject.SetActive(true);
        Invoke("AdjustDrag", 2.0f);

        StateManager.SetState(StateManager.gameState.LANDING);
    }

    private void AdjustDrag()
    {
        _rgdbdy.drag = dragCoef;
    }

    public float GetMaxHeight()
    {
        return maxHeight;
    }
}
