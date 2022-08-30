using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
	public PoolObjects tag;
	public GameObject prefab;
	public int size;
}


public class ObjectPooler : MonoBehaviour
{
    [SerializeField] Transform GemFolder;
    [SerializeField] Transform ObstacleFolder;
    [SerializeField] Transform RoadWithObstacleFolder;
    [SerializeField] Transform EmptyRoadFolder;
    [SerializeField] Transform RoadWithFinishLineFolder;
    [SerializeField] Transform VFXFolder;
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }


    public List<Pool> pools;
	public Dictionary<PoolObjects, Queue<GameObject>> poolDictionary;

    void Start()
    {
		poolDictionary = new Dictionary<PoolObjects, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                AssignFolder(obj, pool.tag);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(PoolObjects tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesnt exists");
            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObjects pooledObject = objectToSpawn.GetComponent<IPooledObjects>();
        if(pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
    private void AssignFolder(GameObject obj, PoolObjects type)
    {
        if (type == PoolObjects.Road)
            obj.transform.parent = RoadWithObstacleFolder;
        else if (type == PoolObjects.RoadEmpty)
            obj.transform.parent = EmptyRoadFolder;
        else if (type == PoolObjects.Gem)
            obj.transform.parent = GemFolder;
        else if (type == PoolObjects.GemRed)
            obj.transform.parent = GemFolder;
        else if (type == PoolObjects.Obstacle)
            obj.transform.parent = ObstacleFolder;
        else if (type == PoolObjects.GemVFX)
            obj.transform.parent = VFXFolder;
        else if (type == PoolObjects.RoadWithFinishLine)
            obj.transform.parent = RoadWithObstacleFolder;

    }
}