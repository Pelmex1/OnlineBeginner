using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Transform _startSpawnaning;
    [SerializeField] private GameObject _prefab;
    private float[] _positions;
    private List<float> _three_positions;
    private void Start() {
        _positions = new float[]{_startSpawnaning.position.z - 5,_startSpawnaning.position.z, _startSpawnaning.position.z + 5};
    }
    private void RandomSpawn(){
        _three_positions = new List<float>(_positions);
        for(int i = 0; i < 2; i++){
            float randomZ = _three_positions[Random.Range(1,3)];
            _three_positions.Remove(randomZ);
            Vector3 randomPosition = new (transform.position.x, transform.position.y, randomZ);
            Instantiate(_prefab, randomPosition, Quaternion.identity);
        }
    }
}
