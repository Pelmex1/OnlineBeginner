using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class LaunchSesion : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _ErrorPanel;
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        try
        {
            PhotonNetwork.JoinRandomRoom();
        }
        catch (System.Exception)
        {
            _ErrorPanel.SetActive(true);
        }
    }

}
