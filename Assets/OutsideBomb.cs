using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideBomb : MonoBehaviour {

    public GameObject explosionSystem;
    public Animator animator;
    public Material whiteMat;
    public MeshRenderer theRenderer;
    public AudioSource sound;
    public IEnumerator Start() {
        Game.instance.OnPixelate();
        yield return new WaitForSeconds(0.3f);
        animator.Play("explode");
        yield return new WaitForSeconds(2.0f);
        sound.Play();
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, new Vector3(128, 128, 128),Vector3.up);
        for (int i = 0; i < hits.Length;i++) {
            KnockBack kb = hits[i].transform.GetComponent<KnockBack>();
            if (kb != null) {
                kb.dir = (kb.transform.position-transform.position).normalized;
                kb.dir = new Vector3(kb.dir.x,0, kb.dir.z);
                kb.SafeEnable();
            }
            
        }
        theRenderer.material = whiteMat;
        explosionSystem.Duplicate(transform.position);
        Destroy(this.gameObject);
        yield break;
    }	
}
