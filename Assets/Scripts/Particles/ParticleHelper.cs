using UnityEngine;

namespace AF
{
    public class ParticleHelper : MonoBehaviour
    {
        ParticleSystem _particleSystem => GetComponent<ParticleSystem>();

        public void SafeStop()
        {
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _particleSystem.gameObject.SetActive(false);
            _particleSystem.gameObject.SetActive(true);
        }
        public void SafePlay()
        {
            _particleSystem.Play();
        }
    }
}
