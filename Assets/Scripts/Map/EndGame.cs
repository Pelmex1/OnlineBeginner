using System.Collections;
using System.Collections.Generic;
using CustomEventBus;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] GameObject _endPanel;
    private EventBus eventBus;
    public void Init() {
        eventBus = ServiceLocator.Current.Get<EventBus>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            eventBus.Invoke<bool>(true);
            _endPanel.SetActive(true);
        }
    }
}
