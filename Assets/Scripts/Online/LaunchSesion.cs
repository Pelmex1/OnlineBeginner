using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using System.Collections;
namespace OnlineBeginner.Multiplayer
{
    public class Lobby : MonoBehaviourPunCallbacks
    {
        [SerializeField] GameObject _createRoomPanel;
        [SerializeField] GameObject _loadingScene;
        [SerializeField] TMP_InputField _createRoom;
        [SerializeField] TMP_Text _infoText;
        private bool _isTwoPlayersInRoom;
        private bool _isCreatedRoom = false;
        private bool _isConnectedToMasterClient = false;
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = StringConstants.GAME_VERSION;
        }
        public void JoinRandomRoom()
        {
            if(!_isCreatedRoom){
                if(_isConnectedToMasterClient){
                    PhotonNetwork.JoinRandomRoom();
                } else {
                    PhotonNetwork.ConnectUsingSettings();
                    PhotonNetwork.GameVersion = StringConstants.GAME_VERSION;
                }
            }
        }
        public override void OnJoinedRoom()
        {
            if(PhotonNetwork.PlayerList.Length > 1){
                Wait();
            }
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to load any room");
            _createRoomPanel.SetActive(true);
            _infoText.text = "Faild To Join Game. Try to crate new";
            CreateRoom();
        }
        private void CreateRoom()
        {
            RoomOptions roomOptions = new()
            {
                MaxPlayers = 2,
                IsOpen = true,
                IsVisible = true
            };
            PhotonNetwork.CreateRoom(_createRoom.text,roomOptions);
            _isCreatedRoom = true;
        }
        public override void OnCreatedRoom()
        {
            Debug.Log("Seccesufuly create a room");
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to create room:" + message);
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("Succefully conected to server!");
            _isConnectedToMasterClient = true;
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("Player entered on room");
            _isTwoPlayersInRoom = true;
            Wait();
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            _isTwoPlayersInRoom = false;
        }
        private void Wait()
        {
            _loadingScene.SetActive(true);
            for(int i = 5; i > 0; i--){
                if(_isTwoPlayersInRoom){
                    Debug.Log("Старт через: " + i);
                } else {
                    break;
                }
            }
            if(!_isTwoPlayersInRoom)
            {
                return;
            }
            PhotonNetwork.LoadLevel("GameScene");
        }
    }
}
