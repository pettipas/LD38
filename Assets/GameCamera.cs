using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour {

    public Camera gameCamera;
    public Color flashOne;
    public Color flashTwo;
    public float timer;
    public float duration = 1.5f;
    public float totalTime = 2;

    public void Start() {
        timer = 2.1f;
    }

    public void Update() {
        timer += Time.smoothDeltaTime;

        if (timer > totalTime) {
            gameCamera.backgroundColor = Color.black;
            return;
        }

        float t = Mathf.PingPong(Time.time, duration) / duration;
        gameCamera.backgroundColor = Color.Lerp(flashOne, flashTwo, t);
    }

    public void RenewFlash() {
        timer = 0;
    }
}
