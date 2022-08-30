using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMove : MonoBehaviour
{

    [SerializeField] private float Speed = 5;
    [SerializeField] private float _objectDistance = -5f;
    [SerializeField] private float _despawnDistance = -10f;

    public bool _canSpawnGround = true;

    // Start is called before the first frame update
    void Start()
    {
        _canSpawnGround = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.State != GameManager.GameState.PlayerTurn)
        {
            return;
        }
        transform.position += -Vector3.forward * Time.deltaTime * Speed;
        try
        {
            if (transform.position.z <= _objectDistance && _canSpawnGround)
            {
                RoadSpawner.Instance.SpawnGround();
                _canSpawnGround = false;
            }

            if (transform.position.z <= _despawnDistance)
            {
                _canSpawnGround = true;
                gameObject.SetActive(false);
                if (transform.CompareTag("RoadWithObstacle"))
                {
                    ObstacleSpawner.Instance.SetObstaclesFalse(transform.gameObject);
                    GemSpawner.Instance.SetGemsFalse(transform.gameObject);
                }

            }
        }
        catch (System.Exception e )
        {
            Debug.Log(e);
            throw;
        }
        
    }
}
