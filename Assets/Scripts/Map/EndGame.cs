using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using CustomEventBus;
using OnlineBeginner.EventBus.Signals;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    
    [SerializeField] TMP_Text _infoText;
    [SerializeField] ParticleSystem[] _fireworks = new ParticleSystem[2];
    private EventBus eventBus;
    private int _placeOfPlayer = 0;
    private GameObject _endPanel;
    public void Init()
    {
        eventBus = ServiceLocator.Current.Get<EventBus>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _placeOfPlayer++;
            _endPanel = other.GetComponentInChildren<Canvas>().gameObject;
            eventBus.Invoke(new TimeSignal(true));
            eventBus.Invoke<bool>(true);
            _endPanel.SetActive(true);
            _fireworks[0].Play();
            _fireworks[1].Play();
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
