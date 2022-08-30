using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    private ObjectPooler _objectPooler;
    public static ObstacleSpawner Instance;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _objectPooler = ObjectPooler.Instance;
    }
    public void SpawnObstacle(GameObject Parent, int type)
    {
        try
        {
            Transform typeObj = Parent.transform.transform.Find($"Type{type}");
            SetAllChildsFalse(Parent.transform);
            typeObj.gameObject.SetActive(true);
            typeObj = typeObj.Find("Obstacles");
            int obstacleAngleSpecific = 45; // For Type 3 obstacle angle, can be used for other types too
            foreach (Transform child in typeObj)
            {
                GameObject obstacle = _objectPooler.SpawnFromPool(PoolObjects.Obstacle, new Vector3(0, 0, 0), Quaternion.identity);
                obstacle.transform.parent = child;
                obstacle.transform.localPosition = new Vector3(0, 0, 0);
                if(type == 1 || type == 2)
                    obstacle.transform.rotation = Quaternion.Euler(0, 90, 0);
                else if (type == 3)
                {
                    obstacle.transform.rotation = Quaternion.Euler(0, obstacleAngleSpecific, 0);
                    obstacleAngleSpecific += 90;
                }
                    
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }

    }
    public void SetObstaclesFalse(GameObject Parent)
    {
        Transform typeObj;
        GameObject obj;
        try
        {
            typeObj = Parent.transform.Find(GetActiveChild(Parent.transform)).Find("Obstacles");

            foreach (Transform child in typeObj)
            {
                //First 3 roads don't have obstacles. Check for error handling
                Transform obstacle = child.Find("barrier(Clone)");
                if (obstacle != null)
                {
                    obj = obstacle.gameObject;
                    obj.transform.parent = null;
                    obj.SetActive(false);
                }   
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }
    }
    private void SetAllChildsFalse(Transform ParentTransform)
    {
        foreach (Transform child in ParentTransform)
        {
            if(child.CompareTag("PlayArea"))
                child.gameObject.SetActive(false);
        }
    }
    private string GetActiveChild(Transform ParentTransform)
    {
        foreach (Transform child in ParentTransform)
        {
            if (child.gameObject.activeSelf && child.CompareTag("PlayArea"))
                return child.transform.name;
        }
        return string.Empty;
    }
}
