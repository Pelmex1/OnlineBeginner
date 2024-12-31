using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using CustomEventBus;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] GameObject _endPanel;
    [SerializeField] TMP_Text _infoText;
    private EventBus eventBus;
    private int _placeOfPlayer = 0;
    public void Init()
    {
        eventBus = ServiceLocator.Current.Get<EventBus>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _placeOfPlayer++;
            eventBus.Invoke(new TimeSignal(true));
            eventBus.Invoke<bool>(true);
            _endPanel.SetActive(true);
            switch(_placeOfPlayer)
            {
                case 1:
                _infoText.text = "You won";
                break;
                case 2:
                _infoText.text = "You lost";
                break;
            }
        }
    }
}
