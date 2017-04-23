using System.Collections.Generic;
using UnityEngine;

public class Centipede : MonoBehaviour {

    public List<Section> sections = new List<Section>();
    public float startAimationSpeed = 4.0f;

    public void Awake() {
        for (int i = 0; i < sections.Count;i++) {
            sections[i].animationSpeed = startAimationSpeed;
        }
    }

    public void Dye(Color c) {
        MeshRenderer[] renderers = this.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0;i < renderers.Length;i++) {
            renderers[i].material.color = c;
        }
    }
}
