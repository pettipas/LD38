using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public static Game instance;
    public Gun gun;

    public void Awake() {
        instance = this;
    }

    public void OnProjectileAtMaxRange(Projectile projectile) {
        projectile.enabled = false;
        projectile.transform.position = gun.transform.position;
    }
}
