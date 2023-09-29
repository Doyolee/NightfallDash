using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] Prefabs;
    /* Prefab List
     0,1: RoadMap
     2,3: HalfMap
     4,5: TunnelMap
     6,7,8: zombie
     9,10,11,12: obstacle
     13: itemBox
     */

    public List<GameObject>[] Pools { get; private set; }


    public void PoolSetting()
    {
        Pools = new List<GameObject>[Prefabs.Length];

        for (int i = 0; i < Prefabs.Length; i++)
        {
            Pools[i]=new List<GameObject>();
        }

    }

    public GameObject GetPools(int index)
    {
        GameObject selectedObject = null;
        foreach(GameObject pulling in Pools[index])
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
            Pools[index].Add(selectedObject);

        }

        return selectedObject;
    }
 
}
