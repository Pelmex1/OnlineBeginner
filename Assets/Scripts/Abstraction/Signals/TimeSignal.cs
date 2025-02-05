namespace OnlineBeginner.EventBus.Signals{
    public class TimeSignal
    {
        public bool wasEnd;
        public TimeSignal(bool isEnd)
        {
            wasEnd = isEnd;
        } 
    }
}
