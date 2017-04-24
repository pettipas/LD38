using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Text scoreText;
    public Text highScorePhrase;
    public Text newhighScorePhrase;
    public Text actualHighScore;
    public Text gameOver;
    public int score;
    public GameObject enemyDeath;
    public GameObject enemySlime;
    public GameObject playerDeath;
    public void Awake() {
        Time.timeScale = 1;
        score = 0;
        scoreText.text = score.ToString("D8");
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

        nextLevel = false;
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
        centispeed = l.centipspeed;

        if (!firstRunComplete) {
            //make new ones
            firstRunComplete = true;
            for (int i = 0; i < InitalCoverage; i++) {
                Vector3 pos = RngPosition;
                pos = pos.Round();
                Obstacle go = obstacle.Duplicate(pos.Round());

                go.Dye(l.mushrooms);
                go.transform.SetParent(mushroomParent, false);
                if (!obstacles.ContainsKey(pos.ToString())) {
                    obstacles.Add(pos.ToString(), go.GetComponent<Obstacle>());
                }
                go.GetComponent<Animator>().SafePlay("popin");
                yield return new WaitForSeconds(0.2f);
            }
        }
        else {
            //uhhhh
            List<string> keysToCleanUp = new List<string>();

            //dye current ones
            foreach (KeyValuePair<string, Obstacle> keyval in obstacles) {
                if (keyval.Value == null) {
                    keysToCleanUp.Add(keyval.Key);
                }
                else {
                    keyval.Value.Dye(l.mushrooms);
                    keyval.Value.GetComponent<Animator>().SafePlay("popin");
                    yield return new WaitForSeconds(0.2f);
                }
            }

            keysToCleanUp.ForEach(k => {
                obstacles.Remove(k);
            });
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

    Color InvertColor(Color color) {
        return new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b);
    }

    public void InvertAll() {
        Renderer[] renders = GameObject.FindObjectsOfType<Renderer>();
        foreach (var render in renders) {
            if (render.material.HasProperty("_Color")) {
                render.material.color = InvertColor(render.material.color);
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

    public Animator scoreAnmimator;

    string textLastFrame;
    public void Update() {
        if (player == null || gameend) {
            return;
        }

        if (textLastFrame != scoreText.text) {
            scoreAnmimator.SafePlay("scoreget");
        }
        scoreText.text = score.ToString("D8");
        textLastFrame = scoreText.text;
       

        Vector3 p = player.transform.position / 10.0f;
        curser.transform.position = new Vector3(p.x * 128.0f, 5, p.z * 128.0f);
    }

    public void OnProjectileAtMaxRange(Projectile projectile) {
        if (gun == null) {
            //you prolly dead
            return;
        }
        projectile.enabled = false;
        projectile.transform.position = gun.transform.position;
    }

    public void OnPixelate() {
        if (!started) {
            started = true;
            pixelator.StartCoroutine(pixelator.Pixelate(() => {
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
        outSideBomb.GetComponent<Rigidbody>().AddTorque(transform.up * Random.Range(10, 30) * Random.value * 1000);
    }

    public void OnHitObstacle(Obstacle obst, Projectile projectile) {

        if (player == null || gameend) {
            return;
        }

        if (projectile != null) {
            projectile.enabled = false;
            projectile.transform.position = gun.transform.position;
        }

        if (obst != null && obst.Destroyed) {
            obstacles.Remove(obst.transform.position.ToString());
            Destroy(obst.gameObject);
        }
    }

    bool nextLevel;
    public void OnDestroySection(Section section, RaycastHit hit, Projectile projectile) {
        enemyDeath.Duplicate(section.transform.position);
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

        if (projectile != null) {
            projectile.enabled = false;
            projectile.transform.position = gun.transform.position;
        }

        if (Random.value > 0.7f) {
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

        score += 500;
    }

    bool gameend;
    public void OnPlayerHit() {
        if (!gameend) {
            gameend = true;
            StartCoroutine(EndGame());
        }
    }

    public void OnDestroyGameSpider(GameSpider gs) {
        enemyDeath.Duplicate(gs.gameObject.transform.position);
        GameObject slime = enemySlime.Duplicate(gs.gameObject.transform.position);
        slime.transform.position = new Vector3(slime.transform.position.x, -5, slime.transform.position.z);

        if (Random.value > 0.4f) {
            shakeCamera.Shake();
            gameCam.RenewFlash();
            Anomoly a = anomolyPrefab.Duplicate(RngPosition);
            a.transform.SetParent(mushroomParent, false);
            anomoly.Add(a);
        }
        score += 100;
        Destroy(gs.gameObject);
    }

    public void OnCentipedeEscape(Section cent) {
        shakeCamera.Shake();
        gameCam.RenewFlash();
        Destroy(cent.gameObject);
        if(Random.value > 0.6f) { 
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

    public IEnumerator EndGame() {
        Time.timeScale = 0.3f;
        Destroy(player.gameObject);
        playerDeath.Duplicate(player.gameObject.transform.position);
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = 0.6f;

        List<GameObject> allofem = new List<GameObject>();
        GameSpider[] spiders = GameObject.FindObjectsOfType<GameSpider>();
        Obstacle[] obstacles = GameObject.FindObjectsOfType<Obstacle>();
        Section[] sections = GameObject.FindObjectsOfType<Section>();

        for (int i = 0; i< spiders.Length; i++) {
            enemyDeath.Duplicate(spiders[i].transform.position);
            Destroy(spiders[i].gameObject);
            yield return null;
        }
        for (int i = 0; i < obstacles.Length; i++) {
            enemyDeath.Duplicate(obstacles[i].transform.position);
            Destroy(obstacles[i].gameObject);
            yield return null;
        }
        for (int i = 0; i < sections.Length; i++) {
            enemyDeath.Duplicate(sections[i].transform.position);
            Destroy(sections[i].gameObject);
            yield return null;
        }
        gameOver.enabled = true;
        //   public Text highScorePhrase;
        //  public Text actualHighScore;newhighScorePhrase
        //  public Text gameOver;
        int highscore = PlayerPrefs.GetInt("highscore", 0);

        if (highscore <= score) {
            highScorePhrase.enabled = true;
        }else {
            newhighScorePhrase.enabled = true;
            PlayerPrefs.SetInt("highscore", score);
        }
        actualHighScore.text = highscore.ToString("D8");

        while (!Input.anyKey) {
            yield return null;
        }
        
        SceneManager.LoadScene("start");
        yield break;
    }

    public void LateUpdate() {
        anomoly.RemoveAll(x => x == null);
    }
}


