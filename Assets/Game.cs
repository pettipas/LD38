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
    public Obstacle obstacle;
    public Transform mushroomParent;
    public float width;
    public float height;
    public Dictionary<string, GameObject> obstacles = new Dictionary<string, GameObject>();
    public float centispeed;
    public Collider worldextents;

    public void Awake() {
        instance = this;
        for (int i = 0; i < InitalCoverage; i++) {
            Vector3 pos = RngPosition;
            Obstacle go = obstacle.Duplicate(pos.Round());
            go.transform.SetParent(mushroomParent,false);
            if (!obstacles.ContainsKey(pos.ToString())) {
                obstacles.Add(pos.ToString(), go.gameObject);
            }
        }
    }

    public bool InBounds(Vector3 position) {
        return worldextents.bounds.Contains(position);
    }

    public int InitalCoverage {
        get {
            return Mathf.RoundToInt((width * height) * 0.03f);
        }
    }

    public Vector3 RngPosition {
        get {
            return new Vector3((float)Random.Range(3, width - 3), 0, (float)Random.Range(7, height - 3));
        }
    }

    public Vector3 UserPosition {
       get { return curser.transform.position; }
    }

    public void Update() {
        Vector3 p = player.transform.position/10.0f;
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

    public void OnHitObstacle(Obstacle obst, Projectile projectile) {
        if (projectile != null) {
            projectile.enabled = false;
            projectile.transform.position = gun.transform.position;
        }
        if (obst != null && obst.Destroyed) Destroy(obst.gameObject);
    }

    public void OnDestroySection(Section section, RaycastHit hit, Projectile projectile) {
        obstacle.Duplicate(section.transform.position.Round());
        Destroy(section.gameObject);
        projectile.enabled = false;
        projectile.transform.position = gun.transform.position;
    }


    public void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(worldextents.bounds.center, worldextents.bounds.size);
    }
}
