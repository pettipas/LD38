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
        if (Physics.SphereCast(r, 0.1f, out hit, speed * Time.smoothDeltaTime)) {
            Bomb bomb = hit.transform.GetComponent<Bomb>();
            if (bomb != null && bomb.name != "hit bomb") {
                bomb.name = "hit bomb";
                Game.instance.OnBombHit(bomb);
                Destroy(bomb.gameObject);
                return;
            }
            Obstacle obs = hit.transform.GetComponent<Obstacle>();
            if (obs != null) {
                obs.TakeHit(1, this);
                return;
            }

            Section section = hit.transform.GetComponent<Section>();
            if (section != null) {
                Game.instance.OnDestroySection(section, hit, this);
            }
        }
      

    }
}
