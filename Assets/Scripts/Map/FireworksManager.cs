using UnityEngine;

public class FireworksManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _fireworks = new ParticleSystem[2];
    public void PlayFireworks()
    {
        _fireworks[0].Play();
        _fireworks[1].Play();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayFireworks();
        }
    }
}
