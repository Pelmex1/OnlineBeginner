namespace OnlineBeginner.Abstraction.Signals
{
    public class EndingPlayerSignal
    {
        public int PlaceOfPlayer { get; set; }
        public EndingPlayerSignal(int placeOfPlayer)
        {
            PlaceOfPlayer = placeOfPlayer;
        }
    }
}