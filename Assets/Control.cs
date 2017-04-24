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
    public Stick stick;
    public Bomb bomb;
   

    public float timer;
    public float coolDown = 3.0f;
    Vector3 gunLocal;
    public void Awake() {
        timer = coolDown;
    }

    public void Update() {

        RaycastHit[] hits = Physics.BoxCastAll(transform.position, new Vector3(0.3f, 0.3f, 0.3f), Vector3.up);
        for (int i = 0; i < hits.Length; i++) {
            RaycastHit hit = hits[i];
            Section section = hit.transform.GetComponent<Section>();
            if (section != null) {
                Game.instance.OnPlayerHit();
            }

            GameSpider gs = hit.transform.GetComponent<GameSpider>();
            if (gs != null) {
                Game.instance.OnPlayerHit();
            }
        }

        timer += Time.smoothDeltaTime;
        bool moving = false;
        if (Input.GetKey(KeyCode.A)) {
            current = Vector3.left;
            charCtrl.Move(Vector3.left * speed * Time.smoothDeltaTime);
            moving = true;
            stick.SetAngle(new Vector3(0,0, 15));
        } else if (Input.GetKey(KeyCode.D)) {
            current = Vector3.right;
            charCtrl.Move(Vector3.right * speed * Time.smoothDeltaTime);
            moving = true;
            stick.SetAngle(new Vector3(0, 0, -15));
        } else if (Input.GetKey(KeyCode.S)) {
            current = Vector3.back;
            charCtrl.Move(Vector3.back * speed * Time.smoothDeltaTime);
            moving = true;
            stick.SetAngle(new Vector3(-15, 0, 0));
        } else if (Input.GetKey(KeyCode.W)) {
            current = Vector3.forward;
            charCtrl.Move(Vector3.forward * speed * Time.smoothDeltaTime);
            moving = true;
            stick.SetAngle(new Vector3(15, 0, 0));
        }

       

        if (moving) {
            animator.SafePlay("walk");
        } else {
            animator.SafePlay("rest");
            stick.SetAngle(new Vector3(0, 0, 0));
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

    public void LateUpdate() {
        transform.RespectHeight(0);
    }
}
