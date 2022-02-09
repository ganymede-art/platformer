using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script.utility
{
    public static class UtilityStaticMethods
    {
        public static GameObject[] FindObjectsOfLayer(int layer)
        {
            GameObject[] gameObjects = GameObject
                .FindObjectsOfType<GameObject>()
                .Where(go => go.layer == layer)
                .ToArray();

            if (gameObjects.Length > 0)
                return gameObjects;

            return null;
        }
    }
}
