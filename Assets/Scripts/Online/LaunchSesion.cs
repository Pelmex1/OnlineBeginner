using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
namespace OnlineBeginner.Miltiplayer{
public class Lobby : MonoBehaviourPunCallbacks
    {
        private void Awake() {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        private void Start() {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = StringConstants.GAME_VERSION;
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("Succefully conected to server!");
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("You disconected, reason: " + cause);
            //Когда не удалось подключится к сети
        }
        public void OnClickOnMultiplayer(){
            if(PhotonNetwork.IsConnected){
                //включается панелька сетевой игры
            } else {
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        public override void OnCreatedRoom()
        {
            Debug.Log("Seccesufuly crteate a room");
            //Когда создалась комната
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to create room:" + message);
        }
        public override void OnJoinedLobby()
        {
            
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            //Не удалось найти комнату
        }


        public void ConectToLobby(){
            if(PhotonNetwork.NickName != ""){
                PhotonNetwork.JoinRandomRoom();
            } else {
                //нету имя
            }
        }
        public void CreateLobby(){
            PhotonNetwork.CreateRoom($"{PhotonNetwork.NickName}", new RoomOptions
            { 
                MaxPlayers = 2,
                IsOpen = true,
                IsVisible = true
            });
            //Включается UI лобби 
        }
        public void Test(){
            if(Input.GetKeyDown(KeyCode.T)){
                //панелька теста
            }
        }

    }
}
