using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField] private float _rcsThrust = 100f;
    [SerializeField] private float _mainThrust = 100f;
    
    // Config
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        Thrust();
        Rotate();
    }

    private void Thrust() {
        if (Input.GetKey(KeyCode.Space)) {
            _rigidbody.AddRelativeForce(Vector3.up * _mainThrust * Time.deltaTime);

            if (!_audioSource.isPlaying) {
                _audioSource.Play();
            }
        } else {
            _audioSource.Stop();
        }
    }

    private void Rotate() {
        float rotationSpeed = _rcsThrust * Time.deltaTime;
        
        _rigidbody.freezeRotation = true;
        
        // You can hold either A or D
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * rotationSpeed);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.back * rotationSpeed);
        }

        _rigidbody.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision other) {
        switch (other.transform.tag) {
            case "Friendly":
                Debug.Log("Hit Friendly");
                break;
            case "Finish":
                Debug.Log("Hit Finish");
                SceneManager.LoadScene(1);
                break;
            default:
                Debug.Log("Not friendly");
                SceneManager.LoadScene(0);
                break;
        }
    }
}
