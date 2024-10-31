using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    [SerializeField] private float _speed;
    private const float PLUS_TO_SPEED = 0.1F;
    private float _vertical;
    private float _horizontal;
    private Rigidbody _rb;
    private void Start() {
        _rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate() {
        _speed += PLUS_TO_SPEED;
        _horizontal = Input.GetAxisRaw("Horizontal");
        _rb.MovePosition(_rb.position + _speed * Time.fixedDeltaTime * transform.forward);
        _rb.MovePosition(_rb.position + _horizontal * _speed * Time.fixedDeltaTime * transform.right);
    }
}
