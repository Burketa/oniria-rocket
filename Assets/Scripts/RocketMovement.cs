using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    public bool debugText = true;
    public float speed = 10f;
    public float maxTimer = 5f;

    public Vector3 cameraOffset;

    public GameObject firstStage;

    private float maxHeight;
    private Rigidbody _rgdbdy;  //Caching no componente para melhor performance.

    [SerializeField]
    private float currentTimer = 0f;

    void Awake()
    {
        _rgdbdy = GetComponent<Rigidbody>();
        maxHeight = transform.position.y + 1;  //Uma folga para não acusar a altura maxima no inicio do lançamento.
    }

    void Start()
    {
        StartCoroutine("LauchingSequence");
    }

    void Update()
    {
        Camera.main.transform.position = transform.position + cameraOffset;
        DetectHeight();
        switch (StateManager.currentState)
        {
            case StateManager.gameState.START:
                StartCoroutine("Thrust");
                break;

            case StateManager.gameState.STAGE1:
                break;

            case StateManager.gameState.SEPARATING:
                StartCoroutine("SeparateModules");
                break;

            case StateManager.gameState.STAGE2:
                break;
        }
    }

    IEnumerator Thrust()
    {
        StateManager.currentState = StateManager.gameState.STAGE1;
        _rgdbdy.useGravity = true;

        while (currentTimer < maxTimer)
        {
            _rgdbdy.AddForce(new Vector3(0, speed, 0));
            speed += speed / 1000;
            currentTimer += Time.deltaTime;
            yield return null;
        }

        StateManager.currentState = StateManager.gameState.SEPARATING;
    }

    IEnumerator SeparateModules()
    {
        StateManager.currentState = StateManager.gameState.STAGE2;

        Debug.Log("Iniciating separating sequence.");
        firstStage.transform.parent = null;
        firstStage.AddComponent<Rigidbody>();
        Rigidbody firstStage_rgdbdy = firstStage.GetComponent<Rigidbody>();
        firstStage_rgdbdy.velocity = _rgdbdy.velocity * 0.99f;
        yield return new WaitForSeconds(0.3f);
        Debug.Log("Separation sequence completed.");
        yield return null;
    }

    IEnumerator LauchingSequence()
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
        StateManager.currentState = StateManager.gameState.START;
        yield return null;
    }

    private void DetectHeight()
    {
        if ((_rgdbdy.velocity.y <= 0) && (_rgdbdy.position.y > maxHeight))
        {
            maxHeight = _rgdbdy.position.y;
            Debug.Log("Maximum height detected: " + maxHeight);
        }
    }
}
