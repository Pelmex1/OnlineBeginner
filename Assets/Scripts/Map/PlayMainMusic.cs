using Photon.Pun;
using UnityEngine;

public class PlayMainMusic : MonoBehaviourPun
{
    [SerializeField] private AudioSource audioSource;
    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView == null)
        {
            Debug.LogError("PhotonView component is missing from this GameObject.");
            return;
        }

        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            Debug.LogError("PhotonView is not owned by this client.");
            return;
        }

        PlayMusic();
    }

    private void PlayMusic()
    {
        if (photonView != null)
        {
            photonView.RPC("MainMusic", RpcTarget.All);
        }
        else
        {
            Debug.LogError("PhotonView is not attached to the GameObject.");
        }
    }
    [PunRPC]
    public void MainMusic() => audioSource.Play();
}
