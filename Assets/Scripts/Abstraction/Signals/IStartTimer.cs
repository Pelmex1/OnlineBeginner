using UnityEngine;
namespace OnlineBeginner.EventBus.Signals
{
    public class IStartTimer
    {
        public int time;
        public IStartTimer(int value)
        {
            time = value;
        }
    }
}
