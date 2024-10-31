using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
namespace OnlineBeginner.Miltiplayer{
public class LaunchSesion : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _ErrorPanel;
        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("Succefully conected to server!");
        }
        public void OnClickOnMultiplayer(){
            PhotonNetwork.AutomaticallySyncScene = true;
            if(PhotonNetwork.IsConnected){
                //включается панелька сетевой игры
            } else {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = StringConstants.GAME_VERSION;
            }
        }
        public override void OnJoinedLobby()
        {
            
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            //Не удалось найти комнату
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            //Когда не удалось подключится к сети
        }

        public void ConectToLobby(){
            PhotonNetwork.JoinRandomRoom();
        }
        public void CreateLobby(){
            PhotonNetwork.CreateRoom($"{PhotonNetwork.NickName}",new RoomOptions{ MaxPlayers = 2});
            //Включается UI лобби (без игроков)
        }

    }
}
