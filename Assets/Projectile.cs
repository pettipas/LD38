using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float speed;
    float distance = 10;
    [System.NonSerialized]
    public float _distance;
    public MeshRenderer skin;

    public void OnDisable() {
        skin.enabled = false;
        _distance = 0;
    }

    public void Update() {
        skin.enabled = true;
        transform.Translate(Vector3.forward * speed * Time.smoothDeltaTime);
        _distance += speed * Time.smoothDeltaTime;
        if (_distance >= distance) {
            _distance = 0;
            Game.instance.OnProjectileAtMaxRange(this);
        }
        RaycastHit hit;
        Ray r = new Ray(transform.position, transform.forward);
        if (Physics.SphereCast(r, 0.4f, out hit, 0.5f)) {

           
        }
    }
}
