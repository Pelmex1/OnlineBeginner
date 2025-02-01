using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayMainMusic : MonoBehaviour
{
    [SerializeField] private AudioSource audioSources;

    private void Awake()
    {
        PlayMusic();
    }
    [PunRPC]
    public void MainMusic() => audioSources.Play();
    private void PlayMusic()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("MainMusic", RpcTarget.All);
    }
}
