using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryStateCameraRotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 20f * Time.deltaTime, 0);
    }
}
