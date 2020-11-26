using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    private Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        ProcessInput();
    }

    private void ProcessInput() {
        if (Input.GetKey(KeyCode.Space)) {
            _rigidbody.AddRelativeForce(Vector3.up);
        }

        // You can hold either A or D
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.back);
        }
    }
}
