using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {
    [SerializeField] private Vector3 _movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] private float _period = 2f;
    
    private float _movementFactor;
    private Vector3 _startingPos;
    
    // Start is called before the first frame update
    void Start() {
        _startingPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        _period = (_period <= Mathf.Epsilon) ? 2f : _period;
        
        float cycles = Time.time / _period;
        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);
        _movementFactor = rawSinWave / 2f + 0.5f;
        
        Vector3 offset = _movementVector * _movementFactor;

        transform.position = _startingPos + offset;
    }
}
