using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitioning : MonoState {

    public Vector3 gotoPos;
    public HorizontalMotion leader;
    public HorizontalMotion mine;
    public MonoState verticalMotion;
    public float speed;
    public int startSign;
    public Vector3 lastKnownLeaderPosition;
    public float distance;
    public Vector3 myDir;
    public Vector3 leaderDir;

    public void OnEnable() {
        distance = 0;
        if (mine == null) {
            mine = GetComponent<HorizontalMotion>();
        }
        mine.Evading = false;
        gotoPos = leader.transform.position.Round();
        myDir = leader.Dir;
        startSign = (int)Mathf.Sign(Vector3.Dot(leader.transform.position,transform.position));
        if (leader != null) {
            lastKnownLeaderPosition = leader.transform.position;
        }
    }

    public Vector3 Dir {
        get {
            return myDir;
        }
        set {
            myDir = value;
        }
    }

    public void Update() {
     
        leaderDir = leader.Dir;

        if (leader != null) {
            lastKnownLeaderPosition = leader.transform.position;
        }
      
        if (((int)Mathf.Sign(Vector3.Dot(lastKnownLeaderPosition, transform.position)) != startSign)
            || Vector3.Distance(transform.position, gotoPos) < 0.2f
            || leader.Dir != mine.Dir) {
            mine.Evading = true;
            this.GotoState(this, verticalMotion);
        }
       
        Vector3 nextPosition = Time.smoothDeltaTime * speed * Dir;
        transform.position += nextPosition;
    }

    public void OnDisable() {
        transform.position = gotoPos;
    }

}
