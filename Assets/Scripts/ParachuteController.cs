using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParachuteController : MonoBehaviour
{
    private Animator _anim;
    public void Start()
    {
        _anim = GetComponent<Animator>();
    }
    public void OpenParachute(Rigidbody rocket, float dragVal)
    {
        _anim.SetBool("parachute_open", true);
        StartCoroutine(AdjustDrag(rocket, dragVal));
        StateManager.SetState(StateManager.gameState.LANDING);
        StartCoroutine(StabilizeRocket(rocket));
    }

    public void CloseParachute()
    {
        StateManager.NextState();
        _anim.SetBool("parachute_open", false);
    }

    private IEnumerator AdjustDrag(Rigidbody rocket, float dragVal)
    {
        yield return new WaitForSeconds(2.0f);
        rocket.drag = dragVal;
        yield return null;
    }

    private IEnumerator StabilizeRocket(Rigidbody rocket)
    {
        while (StateManager.GetState() == StateManager.gameState.LANDING)
        {
            Vector3 rotation = rocket.rotation.eulerAngles;
            Vector3 pos = rocket.position;
            rotation = new Vector3(Mathf.LerpAngle(rotation.x, 0, Time.deltaTime / 2), Mathf.LerpAngle(rotation.y, 0, Time.deltaTime / 2), Mathf.LerpAngle(rotation.z, 0, Time.deltaTime / 2));
            rocket.rotation = Quaternion.Euler(rotation);
            rocket.position = pos;
            yield return null;
        }
        rocket.drag = 0;
        yield return null;
    }


}
