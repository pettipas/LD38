using UnityEngine;
using System.Collections.Generic;

public class Obstacle : MonoBehaviour {

    public int hits;
    public List<GameObject> renderers = new List<GameObject>();

    public void Dye(Color c) {
        if (gameObject == null) {
            Debug.Log("hmmm.. oh well");
            return;
        }
        renderers.ForEach(x => {
            x.GetComponent<MeshRenderer>().material.color = c;
        });
    }

    public bool Destroyed {
        get {
            return hits >= renderers.Count;
        }
    }

    public void Start() {
        TakeHit(0, null);
    }

    public void TakeHit(int hits, Projectile proj) {
        GetComponent<Animator>().SafePlay("popin");
        this.hits += hits;
        Game.instance.OnHitObstacle(this, proj);
        if (this.hits >= renderers.Count) {
            return;
        }

        GameObject g = renderers[this.hits].gameObject;
        g.SetActive(true);
        for (int i = 0; i < renderers.Count; i++) {
            if (renderers[i] != g) {
                renderers[i].SetActive(false);
            }
        }
    }
}
