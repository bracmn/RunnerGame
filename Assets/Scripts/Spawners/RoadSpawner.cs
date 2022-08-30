using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    private ObjectPooler _objectPooler;
    [SerializeField] private float groundSpawnDistance = 10f;

    private int _emptyRoadCount = 0;
    private int _roadCount = 0;
    private bool _hasFinishLineGenerated = false;

    public static RoadSpawner Instance;
    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }


    // Start is called before the first frame update
    void Start()
    {
        _objectPooler = ObjectPooler.Instance;
        
    }

    public void SpawnGround()
    {
        GameObject Road;
        try
        {
            if (_roadCount >= LevelManager.Instance.RoadLenght)
            {
                if (_emptyRoadCount > LevelManager.Instance.EmptyRoadLength && !_hasFinishLineGenerated)
                {
                    Road = _objectPooler.SpawnFromPool(PoolObjects.RoadWithFinishLine, new Vector3(0, 0, groundSpawnDistance), Quaternion.identity);
                    MakeSureCanSpawnRoad(Road);
                    Road.transform.Find("FinishLine").gameObject.SetActive(true);
                    _hasFinishLineGenerated = true;
                    return;
                }
                _emptyRoadCount++;
                Road = _objectPooler.SpawnFromPool(PoolObjects.RoadEmpty, new Vector3(0, 0, groundSpawnDistance), Quaternion.identity);
                MakeSureCanSpawnRoad(Road);
                return;
            }
            _roadCount++;
            Road = _objectPooler.SpawnFromPool(PoolObjects.Road, new Vector3(0, 0, groundSpawnDistance), Quaternion.identity);
            MakeSureCanSpawnRoad(Road);
            int type = Random.Range(1, 4);
            ObstacleSpawner.Instance.SpawnObstacle(Road, type);
            GemSpawner.Instance.SpawnGem(Road, type);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            throw;
        }
        
    }
    private void MakeSureCanSpawnRoad(GameObject obj)
    {
        obj.GetComponent<RoadMove>()._canSpawnGround = true;
    }
    private void GameManager_OnGameStateChanged(GameManager.GameState obj)
    {
        if(obj == GameManager.GameState.PlayerTurn && GameManager.Instance.NewGame)
        {
            _objectPooler.SpawnFromPool(PoolObjects.RoadEmpty, new Vector3(0, 0, 0), Quaternion.identity);
            _objectPooler.SpawnFromPool(PoolObjects.RoadEmpty, new Vector3(0, 0, 7.62f), Quaternion.identity);
            _objectPooler.SpawnFromPool(PoolObjects.RoadEmpty, new Vector3(0, 0, 15.24f), Quaternion.identity);
        }
        _emptyRoadCount = 0;
        _roadCount = 0;
        _hasFinishLineGenerated = false;
    }
}
