using Photon.Pun;
using UnityEngine;

public class PlayMainMusic : MonoBehaviourPun
{
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        PlayMusic();
    }
    [PunRPC]
    public void MainMusic() => audioSource.Play();
    private void PlayMusic()
    {
        PhotonView photonView = PhotonView.Get(this);
        if (photonView != null)
        {
            photonView.RPC("MainMusic", RpcTarget.All);
        }
        else
        {
            Debug.LogError("PhotonView is not attached to the GameObject.");
        }
    }
}
