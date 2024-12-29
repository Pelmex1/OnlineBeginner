using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using CustomEventBus;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] GameObject _winPanel;
    [SerializeField] GameObject _losePanel;
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
            eventBus.Invoke<bool>(true);
            _winPanel.SetActive(true);
            switch(_placeOfPlayer)
            {
                case 1:
                _winPanel.SetActive(true);
                break;
                case 2:
                _losePanel.SetActive(true);
                break;
            }
        }
    }
}
