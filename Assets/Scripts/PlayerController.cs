using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    // player movement vars
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5f;

    // player inputs vars
    private Vector3 _input;
    private Controls _controls;
    private InputAction _moveAct;

    private void Awake() {
        _controls = new Controls();
    }

    private void OnEnable() {
        _moveAct = _controls.Player.Move;
        _moveAct.Enable();

        _controls.Player.Fire.performed += OnFire;
        _controls.Player.Fire.Enable();
    }

    private void OnDisable() {
        _moveAct.Disable();
        _controls.Player.Fire.Disable();
    }

    void Update() {
        GatherInput();
        Look();
    }

    void FixedUpdate() {
        _rb.MovePosition(transform.position + _input.ToIso().normalized * _speed * Time.deltaTime);
    }

    private void OnFire(InputAction.CallbackContext ctx) {
        Debug.Log("Fire!");
    }

    private void GatherInput() { // consider using callbacks too (InputAction.CallbackContext)
        Vector2 moveInput = _moveAct.ReadValue<Vector2>();
        _input = new Vector3(moveInput.x, 0, moveInput.y);
    }

    private void Look() {
        if(_input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = rot; // could use lerp (quaternion.rotatetowards), or also rotate a separate model ontop
    }
}

public static class Helpers {
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, -45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
