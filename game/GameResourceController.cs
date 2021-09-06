using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResourceController : MonoBehaviour
{
    [System.NonSerialized] public GameObject default_particle_prefab;
    [System.NonSerialized] public Dictionary<string, GameObject> particle_prefab_dictionary;

    void Start()
    {
        LoadParticlePrefabDictionary();
    }

    private void LoadParticlePrefabDictionary()
    {
        default_particle_prefab = Resources.Load<GameObject>("prefab/particle/ps_air_bubble_small");

        var prefabs = Resources.LoadAll<GameObject>("prefab/particle");

        particle_prefab_dictionary = new Dictionary<string, GameObject>();

        foreach (var prefab in prefabs)
        {
            particle_prefab_dictionary.Add(prefab.name, prefab);
        }
    }

    public GameObject GetParticlePrefab(string name)
    {
        if (particle_prefab_dictionary.ContainsKey(name))
            return particle_prefab_dictionary[name];

        return default_particle_prefab;
    }
}
