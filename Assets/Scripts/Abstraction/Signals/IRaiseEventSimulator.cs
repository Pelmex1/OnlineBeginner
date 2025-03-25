using ExitGames.Client.Photon;
using UnityEngine;

public class IRaiseEventSimulator
{
    public EventData eventData;
    public object[] CustomData;
    public IRaiseEventSimulator(EventData data, object[] customdata){
        eventData = data;
        CustomData = customdata;
    }
}
