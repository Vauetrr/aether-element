using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5f;
    private Vector3 _input;

    void Update() {
        GatherInput();
        Look();
    }

    void FixedUpdate() {
        Move();
    }

    void GatherInput() {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Debug.Log(_input);
    }

    void Look() {
        if(_input != Vector3.zero) {
            var isoBase = Matrix4x4.Rotate(Quaternion.Euler(0, -45, 0));
            var isoInput = isoBase.MultiplyPoint3x4(_input);
            var relative = (transform.position + isoInput) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);
            transform.rotation = rot; // could use lerp (quaternion.rotatetowards)
        }
    }

    void Move() {
        _rb.MovePosition(transform.position + (transform.forward * _input.magnitude) * _speed * Time.deltaTime);
    }
}
