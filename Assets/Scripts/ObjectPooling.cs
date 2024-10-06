using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    public List<GameObject> pooledObjects;

    // Start is called before the first frame update

    public void CreatePooledObjects(GameObject prefab, int numObjects)
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < numObjects; i++)
        {
            GameObject obj = (GameObject)Instantiate(prefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            obj.transform.SetParent(this.transform);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy) { return pooledObjects[i]; }
        }
        return null;
    }
}
