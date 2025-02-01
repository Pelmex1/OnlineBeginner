using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayMainMusic : MonoBehaviour
{
    [SerializeField] private AudioSource audioSources;

    private void Start()
    {
        PlayMusic();
    }
    [PunRPC]
    public void MainMusic() => audioSources.Play();
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
