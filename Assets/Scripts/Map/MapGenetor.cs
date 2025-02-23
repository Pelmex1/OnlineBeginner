using System.Collections.Generic;
using CustomEventBus;
using OnlineBeginner.EventBus.Signals;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    private float[] _positions;
    private List<float> _three_positions;
    private float _positionXOfSpawn;
    private readonly int _countAddDistance  = 15;
    private Transform _startSpawning;
    private EventBus _eventBus;
    private Vector3[] playersPositions;
    public void Init() {
        _startSpawning = transform;
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<GetPointsOfSpawn>(GetPoints);
        _positionXOfSpawn = transform.position.x -100;
        _positions = new float[]{_startSpawning.position.z - 5,_startSpawning.position.z , _startSpawning.position.z + 5};
        for (int i = 0; i < 50; i++)
        {
            RandomSpawn(_positions); 
        }
        playersPositions= new Vector3[] { new(transform.position.x,transform.position.y,_positions[0]), new(transform.position.x,transform.position.y,_positions[2])};
        _eventBus.Subscribe<IPlayersPositionsSender>(PlayerPositionSend);
    }
    private void RandomSpawn(float[] positions){
        _three_positions = new List<float>(positions);
        int index = 0;
        for(int i = 0; i < 2; i++){
            float randomZ = _three_positions[Random.Range(0,3-index)];
            _three_positions.Remove(randomZ);
            index++;
            Vector3 randomPosition = new (_positionXOfSpawn, _startSpawning.position.y, randomZ);
            Instantiate(_prefab, randomPosition, _prefab.transform.rotation);
        }
        _positionXOfSpawn -= _countAddDistance;
    }
    private void GetPoints(GetPointsOfSpawn getPointsOfSpawn){
        getPointsOfSpawn.Points = _startSpawning;
    }
    private void PlayerPositionSend(IPlayersPositionsSender playersPositionsSender){
        playersPositionsSender.Positions = playersPositions;
        Debug.Log("Work");
        Debug.Log(playersPositions);
    }
}
