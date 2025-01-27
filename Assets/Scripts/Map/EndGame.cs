using CustomEventBus;
using OnlineBeginner.EventBus.Signals;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField]private GameObject _endPanel;
    [SerializeField] TMP_Text _infoText;
    ParticleSystem[] _fireworks = new ParticleSystem[2];
    private EventBus eventBus;
    private int _placeOfPlayer = 0;
    
    public void Init(ParticleSystem[] fireworks)
    {
        eventBus = ServiceLocator.Current.Get<EventBus>();
        _fireworks = fireworks;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Finish"))
        {
            _placeOfPlayer++;
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
