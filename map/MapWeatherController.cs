using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script
{
    public class MapWeatherController : MonoBehaviour
    {
        private AudioSource weatherAudioSource;
        private GameObject weatherObject;
        private GameObject trackingObject;

        public bool isWeatherEnabled;
        public GameObject weatherFxPrefab;
        public AudioClip weatherSound;

        public Vector3 trackingOffset;

        private void Start()
        {
            weatherObject = Instantiate(weatherFxPrefab,transform);

            weatherAudioSource = GetComponent<AudioSource>();

            if(weatherAudioSource == null)
            {
                weatherAudioSource = gameObject.AddComponent<AudioSource>();
                weatherAudioSource.loop = true;
            }

            weatherAudioSource.clip = weatherSound;
            weatherAudioSource.Play();

            trackingObject = GameMasterController.GlobalPlayerObject;

            weatherObject.SetActive(isWeatherEnabled);
            weatherAudioSource.enabled = isWeatherEnabled;

            
        }

        private void Update()
        {
            transform.position = trackingObject.transform.position + trackingOffset;
        }
    }
}
