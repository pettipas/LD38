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

    public void Update() {
        timer += Time.smoothDeltaTime;
        if (Vector3.Distance(transform.position, Game.instance.UserPosition) < distance && timer >= coolDown) {
            timer = 0;
            animator.SafePlay("spider_attack");
        }

        if (timer < coolDown) {
            agent.SetDestination(transform.position);
            return;
        }

        if(agent.enabled) agent.SetDestination(Game.instance.UserPosition);
        animator.SafePlay("spider_walk");
    }
}
