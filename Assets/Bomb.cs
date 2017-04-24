using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public GameObject poof;
    
    public IEnumerator Start() {

        yield return new WaitForSeconds(2.5f);
        poof.Duplicate(transform.position,poof.transform.rotation);

        RaycastHit[] hits = Physics.BoxCastAll(transform.position, new Vector3(1, 1, 1), Vector3.up);
        for (int i = 0; i < hits.Length; i++) {
            RaycastHit hit = hits[i];
            Obstacle obs = hit.transform.GetComponent<Obstacle>();
            if (obs != null) {
                obs.TakeHit(2,null);
            }

            Section section = hit.transform.GetComponent<Section>();
            if (section != null) {
                Game.instance.OnDestroySection(section, hit, null);
            }

            GameSpider gs = hit.transform.GetComponent<GameSpider>();
            if (gs != null) {
                Game.instance.OnDestroyGameSpider(gs);
            }
        }
        Destroy(this.gameObject);
    }

    public void PlaySound() {
    }
    
    public Vector3 ScreenPosition {
        get {
            Vector3 p = transform.position/10.0f;
            return new Vector3(p.x* 128.0f, 5, p.z* 128.0f);
        }
    }
}
