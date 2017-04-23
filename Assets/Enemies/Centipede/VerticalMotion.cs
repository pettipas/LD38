using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMotion : MonoState {

    public float speed = 1;
    public Direction startDir = Direction.West;
    public MonoState horizontalMotion;
    public Vector3 startPosition;
    public Section section;

    public void OnEnable() {
        startPosition = transform.position;
    }

    public void OnDisable() {
        transform.position = transform.position.RoundZToInt();
    }

    public void Update() {
        section.Body.forward = Vector3.back;
        Vector3 timeStep = Time.smoothDeltaTime * speed * Vector3.back;
        timeStep = new Vector3(0, 0, timeStep.z);
        if (Vector3.Distance(transform.position, startPosition) >= 1) {
            this.GotoState(this, horizontalMotion);
        }
        transform.position += timeStep;
    }

    public void LateUpdate() {
        transform.position = transform.position.RoundXToInt();
    }

    public Vector3 NextPosition {
        get;
        set;
    }
}
