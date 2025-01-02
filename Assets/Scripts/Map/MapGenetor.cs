using System.Collections.Generic;
using CustomEventBus;
using OnlineBeginner.EventBus.Signals;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Quaternion rotation;
    private float[][] _positions;
    private List<float> _three_positions;
    private float _positionXOfSpawn;
    private readonly int _countAddDistance  = 15;
    private Transform[] _startSpawning;
    private EventBus _eventBus;
    public void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<GetPointsOfSpawn>(GetPoints);
        _startSpawning = new Transform[transform.childCount];
        for (int i = 0; i < _startSpawning.Length; i++)
        {
            _startSpawning[i] = transform.GetChild(i);
        }
        _positionXOfSpawn = transform.position.x;
        _positions = new float[_startSpawning.Length][];
        for (int i = 0; i < _startSpawning.Length; i++)
        {
            _positions[i] = new float[]{_startSpawning[i].position.z - 5,_startSpawning[i].position.z, _startSpawning[i].position.z + 5};
        }
        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < _startSpawning.Length; j++)
            {
               RandomSpawn(_positions[j]); 
            }
            
        }
    }
    private void RandomSpawn(float[] positions){
        _three_positions = new List<float>(positions);
        int index = 0;
        for(int i = 0; i < 2; i++){
            float randomZ = _three_positions[Random.Range(0,3-index)];
            _three_positions.Remove(randomZ);
            index++;
            Vector3 randomPosition = new (_positionXOfSpawn, _startSpawning[i].position.y, randomZ);
            Instantiate(_prefab, randomPosition, rotation);
        }
        _positionXOfSpawn += _countAddDistance;
    }
    private void GetPoints(GetPointsOfSpawn getPointsOfSpawn){
        getPointsOfSpawn.Points = _startSpawning;
    }
}
