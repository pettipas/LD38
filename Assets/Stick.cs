using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour {

    public void SetAngle(Vector3 orientation) {
        transform.eulerAngles = orientation;
    }
}
