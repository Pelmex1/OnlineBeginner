using System.Collections;
using UnityEngine;

public class AnimationText : MonoBehaviour, IStartGame
{
    private const float _startScale = 0.1f;
    private const float _endScale = 1f;
    private const float _duration = 1f; // Duration of the animation in seconds
    private Transform _transform => transform;

    public void StartAnimation()
    {
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        float elapsedTime = 0f;
        Vector3 initialScale = Vector3.one * _startScale;
        Vector3 finalScale = Vector3.one * _endScale;

        while (elapsedTime < _duration)
        {
            _transform.localScale = Vector3.Lerp(initialScale, finalScale, elapsedTime / _duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _transform.localScale = finalScale;
    }
}
