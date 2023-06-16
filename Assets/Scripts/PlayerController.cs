using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        Vector2 lsInput = Gamepad.current.leftStick.ReadValue();
        _input = new Vector3(lsInput.x, 0, lsInput.y);
        // _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    void Look() {
        if(_input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = rot; // could use lerp (quaternion.rotatetowards)
    }

    void Move() {
        _rb.MovePosition(transform.position + _input.ToIso() * _input.normalized.magnitude * _speed * Time.deltaTime);
    }
}

public static class Helpers {
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, -45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
