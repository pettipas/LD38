
using UnityEngine;

public class Gun : MonoBehaviour {

    public Projectile projectile;
    public Transform launcher;
    public Projectile current;
    public Animator gun;

    public void Awake() {
        current = projectile.Duplicate(launcher.transform.position, launcher.transform.rotation);
        current.enabled = false;
    }

    public void Update() {

        if (!current.enabled) {
            current.transform.position = this.launcher.position;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !current.enabled) {
            current.enabled = true;
            current.transform.forward = launcher.forward;
            gun.Play("shoot", 0, 0);
        }
    }
}
