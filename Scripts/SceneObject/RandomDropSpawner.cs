using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDropSpawner : MonoBehaviour
{
    // Public fields.
    public RandomDropInfo[] randomDropInfos;
    public Vector3 spawnOffset;

    private void Start()
    {
        foreach(var randomDropInfo in randomDropInfos)
        {
            float randomRoll = UnityEngine.Random.Range(0.0F, 1.0F);

            if (randomRoll >= randomDropInfo.dropChance)
                continue;

            Instantiate
                (randomDropInfo.dropPrefab
                , transform.position + spawnOffset
                , transform.rotation);
        }

        GameObject.Destroy(gameObject);
    }

    [Serializable]
    public class RandomDropInfo
    {
        public GameObject dropPrefab;
        public float dropChance;
    }
}
