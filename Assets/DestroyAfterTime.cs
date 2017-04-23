using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

    public IEnumerator Start() {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
