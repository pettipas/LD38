using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

    public float time = 3.0f;

    public IEnumerator Start() {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
