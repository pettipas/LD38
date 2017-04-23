using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour {

    public NavMeshAgent agent;
    public Animator animator;
    public float distance = 2.0f;
    public float coolDown = 3.0f;
    public float timer;
    public Anomoly target;

    bool death;
    public void Update() {
        timer += Time.smoothDeltaTime;
        if (Vector3.Distance(transform.position, target.ScreenPosition) < distance && timer >= coolDown) {
            timer = 0;
            if (!death) { 
                death = true;
                StartCoroutine(Death());
            }
        }

        if (timer < coolDown) {
            agent.SetDestination(transform.position);
            return;
        }

        if (agent.enabled) agent.SetDestination(target.ScreenPosition);
        animator.SafePlay("spider_walk");
    }

    public IEnumerator Death() {
        GetComponent<Spin>().SafeEnable();
        animator.SafePlay("spider_attack");
        yield return new WaitForSeconds(1.3f);
        Game.instance.OnSpiderGoesIn(this);
    }
}
