using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform _player;

    private void Update() {
        if (_player != null) {
            transform.position = _player.position;
        }
    }
}
