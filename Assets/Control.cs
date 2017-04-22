using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

    public CharacterController charCtrl;
    public float speed;
    bool moving;
    public Vector3 next;
    public Vector3 current;
    public Transform body;
    public Transform gun;
    public Transform launcher;

    public void Update() {

        if (Input.GetKey(KeyCode.A)) {
            current = Vector3.left;
            charCtrl.Move(Vector3.left * speed * Time.smoothDeltaTime);
        } else if (Input.GetKey(KeyCode.D)) {
            current = Vector3.right;
            charCtrl.Move(Vector3.right * speed * Time.smoothDeltaTime);
        } else if (Input.GetKey(KeyCode.S)) {
            current = Vector3.back;
            charCtrl.Move(Vector3.back * speed * Time.smoothDeltaTime);
        } else if (Input.GetKey(KeyCode.W)) {
            current = Vector3.forward;
            charCtrl.Move(Vector3.forward * speed * Time.smoothDeltaTime);
        }

        if(current != Vector3.zero) { 
            body.forward = current;
            gun.forward = current;
            launcher.forward = current;
        }
    }
}
