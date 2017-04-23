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
    public GameCamera gameCam;
    bool started;
    public Pixelator pixelator;
    public GameObject portalEffect;
    public OutsideBomb outSideBomb;
    public Obstacle obstacle;
    public Transform mushroomParent;
    public float width;
    public float height;
    public Dictionary<string, Obstacle> obstacles = new Dictionary<string, Obstacle>();
    public float centispeed;
    public Collider worldextents;
    public Collider centipedeDeathZone;
    public Centipede cent;
    public Transform centLaunchPoint;
    public Anomoly anomolyPrefab;
    public List<Transform> spiderSpawns = new List<Transform>();
    public Enemy spiderPrefab;
    public List<Anomoly> anomoly = new List<Anomoly>();
    public List<Level> levels = new List<Level>();
    public CameraShake shakeCamera;
    public ParticleSystem energyWave;
    public GameSpider gameSpider;
    public List<Obstacle> instances = new List<Obstacle>();

    public void Awake() {
        instance = this;
        Anomoly a = anomolyPrefab.Duplicate(RngPosition);
        a.transform.SetParent(mushroomParent, false);
        anomoly.Add(a);
    }

    public bool OutOfBounds(Vector3 v) {
        return centipedeDeathZone.bounds.Contains(v);
    }

    public int centipedeSections;
    public int index;

    bool firstRunComplete;
    int currentLevel;
    public IEnumerator NextLevel() {

        centipedeSections = 8;
        for (int i = 0; i < anomoly.Count; i++) {
            Enemy e = spiderPrefab.Duplicate(spiderSpawns.GetRandomElement().position);
            e.target = anomoly[i];
        }

        anomoly.Clear();

        Level l = levels[index];
        currentLevel = index;
        Centipede c = cent.Duplicate(centLaunchPoint.position);
        c.Dye(l.centipede);

        if (!firstRunComplete) {
            //make new ones
            firstRunComplete = true;
            for (int i = 0; i < InitalCoverage; i++) {
                Vector3 pos = RngPosition;
                Obstacle go = obstacle.Duplicate(pos.Round());

                go.Dye(l.mushrooms);
                go.transform.SetParent(mushroomParent, false);
                if (!obstacles.ContainsKey(pos.ToString())) {
                    obstacles.Add(pos.ToString(), go.GetComponent<Obstacle>());
                }
            }
        }
        else {
            //dye current ones
            foreach (var o in obstacles.Values) {
                o.Dye(l.mushrooms);
            }
        }

        index++;
        if (index >= levels.Count) {
            index = 0;
        }
        yield break;
    }

    public void Start() {
        StartCoroutine(NextLevel());
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

    public void OnSpiderGoesIn(Enemy e) {
        gameSpider.Duplicate(e.target.transform.position);
        Destroy(e.target.gameObject);
        Destroy(e.gameObject);
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
        obstacles.Remove(obst.transform.position.ToString());
        if (obst != null && obst.Destroyed) Destroy(obst.gameObject);
    }

    bool nextLevel;
    public void OnDestroySection(Section section, RaycastHit hit, Projectile projectile) {

        Obstacle ob = obstacle.Duplicate(section.transform.position.Round());

        if (obstacles.ContainsKey(ob.transform.position.ToString())) {
            Destroy(obstacles[ob.transform.position.ToString()].gameObject);
            obstacles.Remove(ob.transform.position.ToString());
            obstacles.Add(ob.transform.position.ToString(), ob);
        }
        else {
            obstacles.Add(ob.transform.position.ToString(), ob);
        }
        
        Level l = levels[currentLevel];
        ob.Dye(l.mushrooms);
        Destroy(section.gameObject);
        projectile.enabled = false;
        projectile.transform.position = gun.transform.position;

        if (Random.value > 0.9f) {
            shakeCamera.Shake();
            gameCam.RenewFlash();
            Anomoly a = anomolyPrefab.Duplicate(RngPosition);
            a.transform.SetParent(mushroomParent, false);
            anomoly.Add(a);
        }

        centipedeSections--;
        if (centipedeSections <= 0 && !nextLevel) {
            nextLevel = true;
            centipedeSections = 8;
            StartCoroutine(NextLevel());
        }
    }

    public void OnCentipedeEscape(Section cent) {
        shakeCamera.Shake();
        gameCam.RenewFlash();
        Destroy(cent.gameObject);
        if(Random.value > 0.8f) { 
            Anomoly a = anomolyPrefab.Duplicate(RngPosition);
            a.transform.SetParent(mushroomParent, false);
            anomoly.Add(a);
        }
        centipedeSections--;
        if (centipedeSections <= 0 && !nextLevel) {
            nextLevel = true;
            centipedeSections = 8;
            StartCoroutine(NextLevel());
        }
    }


    public void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(worldextents.bounds.center, worldextents.bounds.size);

        spiderSpawns.ForEach(x => {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(x.position,10);
        });
    }

    public void LateUpdate() {
        anomoly.RemoveAll(x => x == null);
    }
}


