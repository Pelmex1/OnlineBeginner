
using System.Collections;
using System.Collections.Generic;
using CustomEventBus;
using ExitGames.Client.Photon;
using OnlineBeginner.Consts;
using OnlineBeginner.Online;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerWalk : MonoBehaviourPun, IPlayerWalk, IPunInstantiateMagicCallback, IOnEventCallback
{
    public float Speed { get; set; } = 0;
    private const float PLUS_TO_SPEED = 0.1F;
    private float _horizontal;
    private Rigidbody _rb;
    private LinkedList<float> positions;
    private LinkedListNode<float> LocalPosition;
    private readonly float _index = 5;
    private bool _cooldown = false;
    private EventBus _eventBus;
    private bool IsEnd = false;
    private AudioListener _audioListener;
    private CameraWork _cameraWork;
    private Camera _playerCamera;
    private GameObject _canvas;
    private IEndGame _endGame;
    private ITimeEnd _timeEnd;
    public void Init()
    {
        Debug.Log("INIT");
        _audioListener = GetComponent<AudioListener>();
        _canvas = GetComponentInChildren<Canvas>().gameObject;
        _playerCamera = GetComponentInChildren<Camera>();
        _cameraWork = GetComponent<CameraWork>();
        _rb = GetComponent<Rigidbody>();
        _endGame = GetComponent<IEndGame>();
        _timeEnd = GetComponent<ITimeEnd>();
        
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Invoke(_endGame);
        Debug.Log(_playerCamera);
        positions = new LinkedList<float>(new[] { transform.position.z - _index, transform.position.z, transform.position.z + _index });
        LocalPosition = PhotonNetwork.IsMasterClient ? positions.First : positions.Last;
        if (photonView.IsMine)
        {
            _audioListener.enabled = true;
            _playerCamera.enabled = true;
            _canvas.SetActive(true);
            _cameraWork.Init(_playerCamera);
            Debug.Log(_playerCamera);
        }
        else
        {
            _audioListener.enabled = false;
            _canvas.SetActive(false);
            _playerCamera.enabled = false;
            Debug.Log(_playerCamera);
        }

    }
    private void FixedUpdate()
    {
        if (PhotonNetwork.IsConnected && photonView.IsMine && IsEnd != true)
        {
            Debug.Log(_playerCamera);
            _playerCamera.enabled = true;
            if(Speed != 0) {Speed += PLUS_TO_SPEED;}
            _horizontal = Input.GetAxisRaw("Horizontal");
            ChangePosition(_horizontal);
            if (LocalPosition.Value != transform.position.z)
            {
                transform.position = Vector3.Lerp(transform.position, new(transform.position.x, transform.position.y, LocalPosition.Value), Time.fixedTime * Speed);
            }
            _rb.MovePosition(_rb.position + Speed * Time.fixedDeltaTime * -transform.right);
        }

    }
    private void ChangePosition(float horizontal)
    {
        if (horizontal == 0 || _cooldown) 
        {
            return; 
        }
        LinkedListNode<float> pos;
        if (horizontal == 1)
        {
            pos = LocalPosition.Next;
            if (pos != null)
            {
                LocalPosition = pos;
            }
        }
        else if (horizontal != 1)
        {
            pos = LocalPosition.Previous;
            if (pos != null)
            {
                LocalPosition = pos;
            }
        }
        StartCoroutine(Cooldown());
    }
    private IEnumerator Cooldown()
    {
        _cooldown = true;
        yield return new WaitForSecondsRealtime(Time.fixedDeltaTime * 10);
        _cooldown = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Speed /= 2;
        }
        if (other.CompareTag("Finish"))
        {
            _endGame.OpenUI();
            _timeEnd.SetTime();
            IsEnd = true;
        }
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] data = new object[]
        {       
              
        };

        RaiseEventOptions raiseEventOptions = new()
        {
            Receivers = ReceiverGroup.Others,
            CachingOption = EventCaching.AddToRoomCache
        };

        SendOptions sendOptions = new()
        {
            Reliability = true
        };
        IPlayerWalk playerWalk = info.photonView.gameObject.GetComponent<IPlayerWalk>(); 
        playerWalk.Init();
        PhotonNetwork.RaiseEvent(StringConstants.OnPhotonPlayerSpawned, data, raiseEventOptions, sendOptions);
    }
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == StringConstants.ON_MATCH_START)
        {
            Speed = 1;
        }
    }
}
