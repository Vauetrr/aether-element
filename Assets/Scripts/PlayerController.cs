using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    // player components
    private Rigidbody _rb;
    private Controls _controls;
    private Animator _ani;

    // player inputs sticks
    private InputAction _moveAct;

    // private vars
    [SerializeField] private float _speed = 5f;
    private Vector3 _input;

    // setup functions
    private void Awake() {
        _controls = new Controls();
        _rb = GetComponent<Rigidbody>();
        _ani = GetComponent<Animator>();
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

    // update functions
    void Update() {
        GatherInput();
        Look();
    }

    void FixedUpdate() {
        var move = _speed * Time.deltaTime * _input.ToIso().normalized;
        if (move == Vector3.zero) {
            _ani.SetBool("isWalking", false);
        } else {
            _ani.SetBool("isWalking", true);
            _rb.MovePosition(transform.position + move);
        }
    }

    // input functions
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
