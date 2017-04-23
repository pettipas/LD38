using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpider : MonoBehaviour {

    public float MoveSpeed = 1f;

    public float frequency = 5f; 
    public float magnitude = 3f;   
    private Vector3 axis;

    private Vector3 pos;

    void Start() {
        pos = transform.position;
        axis = transform.forward;  
    }

    public float timer;
    public float totalTime;

    void Update() {

        timer += Time.smoothDeltaTime;

        if (timer > totalTime) {
            timer = 0;
            MoveSpeed *= -1;
        }

        pos += transform.right * Time.deltaTime * MoveSpeed;
        transform.position = pos + axis * Mathf.Sin(Time.time * frequency) * magnitude;
    }
}
