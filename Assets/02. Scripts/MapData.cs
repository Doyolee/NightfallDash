using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Map",menuName ="Scripts Object/MapData")]
public class MapData : ScriptableObject
{
    [Header("SpawnTime")]
    public float spawnTimer;

    [Header("SpawnProbability")]
    public float spawnPZombie;
    public float spawnPObstacle;
    public float spawnPItemBox;

    [Header("Fog")]
    public float fogDistance;
}
