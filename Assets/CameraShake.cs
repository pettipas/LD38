using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    public static CameraShake Instance;

    public Transform camTransform;
    public float shake = 0f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    Vector3 originalPos;

    void Awake() {
        Instance = this;
        if (camTransform == null) {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable() {
        originalPos = camTransform.localPosition;
    }

    public void Shake() {
        shake = shakeAmount;
    }

    public void Shake(float amount) {
        shake = amount;
    }

    void Update() {
        if (shake > 0) {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shake -= Time.deltaTime * decreaseFactor;
        }
        else {
            shake = 0f;
            camTransform.localPosition = originalPos;
        }
    }
}