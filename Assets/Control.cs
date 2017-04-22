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
    public Animator animator;

    public Bomb bomb;

    public float timer;
    public float coolDown = 3.0f;
    Vector3 gunLocal;
    public void Awake() {
        timer = coolDown;
        gunLocal = gun.transform.position;
    }

    public void Update() {
        timer += Time.smoothDeltaTime;
        bool moving = false;
        if (Input.GetKey(KeyCode.A)) {
            current = Vector3.left;
            charCtrl.Move(Vector3.left * speed * Time.smoothDeltaTime);
            moving = true;
            gun.transform.localPosition = gunLocal;
        } else if (Input.GetKey(KeyCode.D)) {
            current = Vector3.right;
            charCtrl.Move(Vector3.right * speed * Time.smoothDeltaTime);
            moving = true;
        } else if (Input.GetKey(KeyCode.S)) {
            current = Vector3.back;
            charCtrl.Move(Vector3.back * speed * Time.smoothDeltaTime);
            moving = true;
            gun.transform.localPosition = gunLocal;
        } else if (Input.GetKey(KeyCode.W)) {
            current = Vector3.forward;
            charCtrl.Move(Vector3.forward * speed * Time.smoothDeltaTime);
            moving = true;
            gun.transform.localPosition = gunLocal;
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            gun.transform.position = gun.transform.position - new Vector3(0.2f,0,0);
        }

        if (moving) {
            animator.SafePlay("walk");
        } else {
            animator.SafePlay("rest");
        }

        if(current != Vector3.zero) { 
            gun.forward = current;
            launcher.forward = current;
        }

        if (Input.GetKeyUp(KeyCode.Q ) && timer >= coolDown) {
            timer = 0;
            bomb.Duplicate((transform.position + current/2.0f));
        }
    }
}
