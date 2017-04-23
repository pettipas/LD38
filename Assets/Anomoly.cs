using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomoly : MonoBehaviour {

    public Vector3 ScreenPosition {
        get {
            Vector3 p = transform.position / 10.0f;
            return new Vector3(p.x * 128.0f, 5, p.z * 128.0f);
        }
    }
}
