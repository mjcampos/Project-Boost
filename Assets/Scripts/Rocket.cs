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
    private bool _isTransitioning = false;

    [SerializeField] private bool _collisionsDisabled = false;

    // Start is called before the first frame update
    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update() {
        if (!_isTransitioning) {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void RespondToThrustInput() {
        if (Input.GetKey(KeyCode.Space)) {
            ApplyThrust();
        } else {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust() {
        _audioSource.Stop();
        _mainEngineParticles.Stop();
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

        _rigidbody.angularVelocity = Vector3.zero;
        
        // You can hold either A or D    
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * rotationSpeed);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.back * rotationSpeed);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (_isTransitioning || _collisionsDisabled) {
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
        _isTransitioning = true;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_engineExplosion);
        _engineExplosionParticles.Play();
        Invoke("LoadFirstLevel", _levelLoadDelay);
    }

    private void StartSuccessSequence() {
        _isTransitioning = true;
        _audioSource.Stop();
        _audioSource.PlayOneShot(_success);
        Instantiate(_successParticles, transform.position, Quaternion.identity);
        Invoke("LoadNextScene", _levelLoadDelay);
    }

    public void LoadNextScene() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex + 1 >= sceneCount) {
            SceneManager.LoadScene(0);
        } else {
            currentSceneIndex++;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }

    private void LoadFirstLevel() {
        SceneManager.LoadScene(0);
    }

    public void ToggleCollisionsDisabled() {
        _collisionsDisabled = !_collisionsDisabled;
    }
}
