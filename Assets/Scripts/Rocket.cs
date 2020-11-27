using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField] private float _rcsThrust = 100f;
    [SerializeField] private float _mainThrust = 100f;
    [SerializeField] private float _levelLoadDelay = 2f;
    [SerializeField] private AudioClip _mainEngine;
    [SerializeField] private AudioClip _engineExplosion;
    [SerializeField] private AudioClip _success;
    
    [SerializeField] private ParticleSystem _mainEngineParticles;
    [SerializeField] private ParticleSystem _engineExplosionParticles;
    [SerializeField] private ParticleSystem _successParticles;
    
    // Config
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    enum State {
        Alive,
        Dying,
        Trascending
    };

    [SerializeField] private State _state = State.Alive;

    // Start is called before the first frame update
    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (_state == State.Alive) {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void RespondToThrustInput() {
        if (Input.GetKey(KeyCode.Space)) {
            ApplyThrust();
        } else {
            _audioSource.Stop();
            _mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust() {
        _rigidbody.AddRelativeForce(Vector3.up * _mainThrust * Time.deltaTime);

        if (!_audioSource.isPlaying) {
            _audioSource.PlayOneShot(_mainEngine);
        }
        
        _mainEngineParticles.Play();
    }

    private void RespondToRotateInput() {
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
        if (_state != State.Alive) {
            return;
        }
        
        switch (other.transform.tag) {
            case "Friendly":
                Debug.Log("Hit Friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence() {
        _state = State.Dying;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_engineExplosion);
        _engineExplosionParticles.Play();
        Invoke("LoadFirstLevel", _levelLoadDelay);
    }

    private void StartSuccessSequence() {
        _state = State.Trascending;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_success);
        _successParticles.Play();
        Invoke("LoadNextScene", _levelLoadDelay);
    }

    private void LoadNextScene() {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel() {
        SceneManager.LoadScene(0);
    }
}
