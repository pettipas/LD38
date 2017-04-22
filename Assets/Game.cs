using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public static Game instance;
    public Gun gun;
    public Transform player;
    public Transform boundary;
    public Transform screen;


    public void Awake() {
        instance = this;
    }

    public void OnProjectileAtMaxRange(Projectile projectile) {
        projectile.enabled = false;
        projectile.transform.position = gun.transform.position;
    }

    public void OnGUI() {
        GUILayout.Label(player.transform.localPosition.ToString());
    }
}
