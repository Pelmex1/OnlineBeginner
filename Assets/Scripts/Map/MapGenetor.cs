using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Transform _startSpawnaning;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Quaternion rotation;
    private float[] _positions;
    private List<float> _three_positions;
    private int _indexX = 0;
    public void Init() {
        _positions = new float[]{_startSpawnaning.position.z - 5,_startSpawnaning.position.z, _startSpawnaning.position.z + 5};
        for (int i = 0; i < 50; i++)
        {
            RandomSpawn();
        }
    }
    private void RandomSpawn(){
        _three_positions = new List<float>(_positions);
        int index = 0;
        for(int i = 0; i < 2; i++){
            float randomZ = _three_positions[Random.Range(0,3-index)];
            _three_positions.Remove(randomZ);
            index++;
            Vector3 randomPosition = new (transform.position.x+_indexX, _startSpawnaning.position.y, randomZ);
            Instantiate(_prefab, randomPosition, rotation);
        }
        _indexX += 6;
    }
}
