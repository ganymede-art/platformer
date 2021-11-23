using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{
    public class PlayerProjectileController : MonoBehaviour
    {
        private GameObject destroyFxObject;
        private AudioSource destroyFxAudioSource;
        public GameObject destroyFxPrefab;
        public AudioClip destroySound;

        private void Start()
        {
            GameObject.Destroy(gameObject, 5);
        }

        private void OnDestroy()
        {
            destroyFxObject = Instantiate(destroyFxPrefab, transform.position, transform.rotation);
            destroyFxAudioSource = destroyFxObject.AddComponent<AudioSource>();
            destroyFxAudioSource.clip = destroySound;
            destroyFxAudioSource.Play();
        }

        private void OnCollisionEnter(Collision collision)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
