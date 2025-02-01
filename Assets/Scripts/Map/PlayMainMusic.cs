using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayMainMusic : MonoBehaviour
{
    [SerializeField]private AudioSource audioSources;

    [PunRPC]
    public void MainMusic() => audioSources.Play();
}
