using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    [SerializeField] private float _speed;
    private const float PLUS_TO_SPEED = 0.1F;
    private float _horizontal;
    private Rigidbody _rb;
    private LinkedList<float> positions;
    private LinkedListNode<float> _localPosition;
    private readonly float _index = 5;
    private bool _cooldown = false;
    private void Start() {
        _rb = GetComponent<Rigidbody>();
        positions = new LinkedList<float>(new[] {transform.position.z - _index, transform.position.z, transform.position.z + _index});
        _localPosition = positions.First; _localPosition = _localPosition.Next;
    }
    private void FixedUpdate() {
        _speed += PLUS_TO_SPEED;
        _horizontal = Input.GetAxisRaw("Horizontal");
        ChangePosition(_horizontal);
        if(_localPosition.Value != transform.position.z){
            transform.position = Vector3.Lerp(transform.position, new(transform.position.x, transform.position.y, _localPosition.Value), Time.fixedTime * _speed);
        }
        _rb.MovePosition(_rb.position + _speed * Time.fixedDeltaTime * -transform.right);
    }
    private void ChangePosition(float horizontal){
        if(horizontal == 0 || _cooldown) {return;}
        LinkedListNode<float> pos;
        if(horizontal == 1){
            pos = _localPosition.Next;
            if(pos != null){
                _localPosition = pos;
            }
        } else if(horizontal != 1){
            pos = _localPosition.Previous;
            if(pos != null){
                _localPosition = pos;
            }
        }
        StartCoroutine(Cooldown());
    }
    private IEnumerator Cooldown(){
        _cooldown = true;
        yield return new WaitForSecondsRealtime(Time.fixedDeltaTime * 10);
        _cooldown = false;
    }
}
