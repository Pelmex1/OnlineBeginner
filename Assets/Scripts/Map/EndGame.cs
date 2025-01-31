using CustomEventBus;
using OnlineBeginner.EventBus.Signals;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour, IEndGame
{
    [SerializeField] private GameObject _endPanel;
    [SerializeField] TMP_Text _infoText;
    ParticleSystem[] _fireworks = new ParticleSystem[2];
    private int _placeOfPlayer = 0;

    public void Init(ParticleSystem[] fireworks)
    {
        _fireworks = fireworks;
    }
    public void OpenUI()
    {
        _placeOfPlayer++;
        _endPanel.SetActive(true);
        _fireworks[0].Play();
        _fireworks[1].Play();
        switch (_placeOfPlayer)
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
