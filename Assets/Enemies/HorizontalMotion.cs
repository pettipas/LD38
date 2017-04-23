using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMotion : MonoState {

    public float speed {
        get {
            return Game.instance.centispeed;
        }
    }
    public Direction startDir = Direction.West;
    public Direction current;
    public MonoState verticalMotion;
    public MonoState transition;
    public float currentDistance;
    public HorizontalMotion leader;
    public bool Evading;
    Vector3 velocity;
    bool hadLeaderLastFrame = false;
    public Section section;

    public bool IsLeader {
        get {
            return leader == null;
        }
    }

    public void OnEnable() {
        Evading = false;
        currentDistance = 0;
        if (current == Direction.West) {
            current = Direction.East;
        } else if(current == Direction.East) {
            current = Direction.West;
        }
        transform.position = transform.position.RoundZToInt();
    }

    public void OnDisable() {
        transform.position = transform.position.Round();
    }

    public Vector3 Dir {
        get {
            if (current == Direction.West) {
                return Vector3.left;
            } else if (current == Direction.East) {
                return Vector3.right;
            } else if (current == Direction.South) {
                return Vector3.back;
            } else if (current == Direction.North) {
                return Vector3.forward;
            }
            return Vector3.zero;
        }
    }

    public void Update() {

        if (leader != null && Vector3.Distance(transform.position, leader.transform.position) > 4.0f) {
            leader = null;
        }

        section.Body.forward = Dir;

        if (leader != null && leader.Evading) {
            this.GotoState(this, transition);
        }

        //if your leader was shot then go down
        if (hadLeaderLastFrame && leader == null) {
            hadLeaderLastFrame = false;
            this.GotoState(this, verticalMotion);
        }

        if (leader != null) {
            hadLeaderLastFrame = true;
        }

        //can only follow if its going the same direction as the leader
        if (leader != null && leader.enabled && leader.Dir == Dir) {
            transform.position = Vector3.SmoothDamp(transform.position, leader.transform.position - Dir, ref velocity, 0.01f);
            return;
        }

        //you are the leader
        Vector3 nextPosition = Time.smoothDeltaTime * speed * Dir;
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + nextPosition, Vector3.one / 2.0f, Dir, transform.rotation, Time.smoothDeltaTime * speed);

        nextPosition = new Vector3(nextPosition.x, 0, 0);
      
        if (currentDistance <= 0.8f) {//magic
            currentDistance += Time.smoothDeltaTime * speed;
            transform.position += nextPosition;
            return;
        }
       
        for (int i = 0; i < hits.Length;i++) {
            Obstacle o = hits[i].transform.GetComponent<Obstacle>();
            if (o != null) {
                this.GotoState(this, verticalMotion);
            }
        }

        Section s = this.Detect<Section>(Vector3.one / 3.0f);
        if (s != null && leader == null) {
            Evading = true;
            this.GotoState(this, verticalMotion);
        }

        if (!Game.instance.InBounds(transform.position + nextPosition)) {
            this.GotoState(this, verticalMotion);
        }

        transform.position += nextPosition;
    }

    public void LateUpdate() {
        transform.position = transform.position.RoundZToInt();
    }

    public Vector3 NextPosition {
        get;
        set;
    }

    public void OnDrawGizmos() {
        if(leader != null) { 
            Gizmos.DrawLine(transform.position, leader.transform.position);
        }

        if (IsLeader) {
            Gizmos.DrawCube(transform.position, Vector3.one);
        }
    }


}

public enum Direction {
    North,South,East,West
}