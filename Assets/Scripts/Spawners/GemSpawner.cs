using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    private ObjectPooler _objectPooler;
    public static GemSpawner Instance;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _objectPooler = ObjectPooler.Instance;
    }
    public void SpawnGem(GameObject Parent, int type)
    {
        try
        {
            Transform typeObj = Parent.transform.transform.Find($"Type{type}");
            SetAllChildsFalse(Parent.transform);
            typeObj.gameObject.SetActive(true);
            typeObj = typeObj.Find("Collectables");

            foreach (Transform child in typeObj)
            {
                GameObject gem = null;
                if (type == 1 || type == 2)
                {
                    gem = _objectPooler.SpawnFromPool(PoolObjects.Gem, new Vector3(0, 0, 0), Quaternion.identity);
                }
                else if(type == 3 || type == 4)
                {
                   gem = _objectPooler.SpawnFromPool(PoolObjects.GemRed, new Vector3(0, 0, 0), Quaternion.identity);
                }
                
                if (gem == null) //Better to be cautious
                    return;

                gem.transform.parent = child;
                gem.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }

    }
    public void SetGemsFalse(GameObject Parent)
    {
        Transform typeObj;
        GameObject obj;
        try
        {
            typeObj = Parent.transform.Find(GetActiveChild(Parent.transform)).Find("Collectables");

            foreach (Transform child in typeObj)
            {
                //First 3 roads don't have gems. Check for error handling
                Transform gem = child.Find("Diamond5side(Clone)");
                if (gem != null)
                {
                    obj = gem.gameObject;
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
            if (child.CompareTag("PlayArea"))
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
