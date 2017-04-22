using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public GameObject target;
    Vector3 velocity;
    public float zOffset;
    public void Update() {
        Vector3 tp = new Vector3(transform.position.x, transform.position.y, target.transform.position.z + zOffset);
        transform.position = Vector3.SmoothDamp(transform.position, tp, ref velocity, 1.0f);
    }
}
