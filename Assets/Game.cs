using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public static Game instance;
    public Gun gun;
    public Transform player;
    public Transform boundary;
    public Transform screen;
    public Transform curser;

    public void Awake() {
        instance = this;
    }

    public void Update() {
        Vector3 p = player.transform.position/5.0f;
    
        curser.transform.position = new Vector3(p.x * 128.0f, 5, p.z * 128.0f);
    }

    public void OnProjectileAtMaxRange(Projectile projectile) {
        projectile.enabled = false;
        projectile.transform.position = gun.transform.position;
    }

    public void OnGUI() {
        GUILayout.Label(player.transform.localPosition.ToString());
    }
}
