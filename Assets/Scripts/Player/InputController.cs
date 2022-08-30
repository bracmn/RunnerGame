using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Vector3 firstPos, endPos;
    [SerializeField] private float PlayerSpeed = 3.75f;

    void Update()
    {
        if (GameManager.Instance.State != GameManager.GameState.PlayerTurn)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            firstPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            endPos = Input.mousePosition;

            float farkX = endPos.x - firstPos.x;

            farkX = Mathf.Clamp(farkX, -1.5f, 1.5f);

            transform.localPosition = 
                Vector3.MoveTowards(
                    transform.localPosition, 
                    new Vector3(farkX, transform.position.y, transform.position.z),
                    Time.deltaTime * PlayerSpeed
                    );

        }
        if(Input.GetMouseButtonUp(0))
        {
            firstPos = Vector3.zero;
            endPos = Vector3.zero;
        }
    }
}
