using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour {
    private Rocket _rocket;
    
    // Start is called before the first frame update
    void Start() {
        // _rocket = new Rocket();
        _rocket = GameObject.Find("Rocket Ship").GetComponent<Rocket>();

        if (_rocket == null) {
            Debug.LogError("Can't find rocket ship");
        }
    }

    // Update is called once per frame
    void Update() {
        if (Debug.isDebugBuild) {
            RespondToDebugKey();
        }
    }

    private void RespondToDebugKey() {
        if (Input.GetKeyDown(KeyCode.L)) {
            _rocket.LoadNextScene();
        } else if (Input.GetKeyDown(KeyCode.C)) {
            _rocket.ToggleCollisionsDisabled();
        }
    }
}
