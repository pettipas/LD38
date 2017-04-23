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

    bool started;
    public Pixelator pixelator;
    public GameObject portalEffect;
    public OutsideBomb outSideBomb;

    public void Awake() {
        instance = this;
    }

    public Vector3 UserPosition {
       get { return curser.transform.position; }
    }

    public void Update() {
        Vector3 p = player.transform.position/5.0f;
        curser.transform.position = new Vector3(p.x * 128.0f, 5, p.z * 128.0f);
    }

    public void OnProjectileAtMaxRange(Projectile projectile) {
        projectile.enabled = false;
        projectile.transform.position = gun.transform.position;
    }

    public void OnPixelate() {
        if (!started) {
            started = true;
            pixelator.StartCoroutine(pixelator.Pixelate(()=> {
                started = false;
            }));
        }
    }

    public void OnBombHit(Bomb bomb) {
        portalEffect.Duplicate(bomb.ScreenPosition);
        outSideBomb.Duplicate(bomb.ScreenPosition);
        outSideBomb.GetComponent<Rigidbody>().AddTorque(transform.up * Random.Range(10,30) * Random.value*1000);
    }
}
