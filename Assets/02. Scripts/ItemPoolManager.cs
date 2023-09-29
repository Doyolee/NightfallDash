using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoolManager : MonoBehaviour
{
    public GameObject[] Prefabs;
    /* Prefab List
     * 0,1 RoadMap
     * 2,3 BridgeMap
     * 4,5 TunnelMap
     */

    public List<GameObject>[] ItemPools { get; private set; }

    public void PoolSetting()
    {
        ItemPools = new List<GameObject>[Prefabs.Length];

        for (int i = 0; i < Prefabs.Length; i++)
        {
            ItemPools[i]=new List<GameObject>();
        }

    }

    public GameObject GetItemPools(int index)
    {
        GameObject selectedObject = null;
        foreach(GameObject pulling in ItemPools[index])
        {
            if (!pulling.activeSelf)
            {
                selectedObject = pulling;
                selectedObject.SetActive(true);

                break;
            }
        }
        if (!selectedObject)
        {
            selectedObject = Instantiate(Prefabs[index], transform);
            ItemPools[index].Add(selectedObject);

        }

        return selectedObject;
    }
 
}
