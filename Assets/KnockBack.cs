using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockBack : MonoBehaviour {

    public Vector3 dir;
    public float speed;
    public CharacterController ctrl;
    public float distance = 0.01f;
    public float expectedDistance = 35;
    public NavMeshAgent agent;
    public AnimationCurve curve;
    public Enemy enemy;
    public MaterialFlasher flasher;
    public Spin spin;

    public AudioSource source;

    public void OnEnable() {
        Game.instance.score += 1000;
        enemy.SafeDisable();
        distance = 0.01f;
        agent.enabled = false;
        ctrl.enabled = true;
        flasher.FlashForTime(5.0f);
        source.Play();
    }

    public void OnDisable() {
        agent.enabled = true;
        ctrl.enabled = false;
        enemy.SafeEnable();
       
    }

    public void Update() {

        if (gameObject == null || spin.enabled ) {
            return;
        }

        distance += (speed * Time.smoothDeltaTime);
        ctrl.Move(dir * speed * Time.smoothDeltaTime * curve.Evaluate(distance/expectedDistance));
        if (distance >= expectedDistance) {
            this.SafeDisable();
        }
    }
}
