using UnityEngine;
namespace OnlineBeginner.EventBus.Signals{
    public class GetPointsOfSpawn : MonoBehaviour
    {
        public Transform Points;
        public GetPointsOfSpawn(ref Transform outPoints)
        {
            outPoints = ref Points;
        }
    }
}
