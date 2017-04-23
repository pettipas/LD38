using UnityEngine;
using System.Collections;
using System;
public class Pixelator : MonoBehaviour {

    public int max = 32;
    public int amount =12;
    RenderTexture reTex;

    void OnRenderImage(RenderTexture src, RenderTexture dest) {
        reTex = RenderTexture.GetTemporary(src.width / amount, src.width / amount);
        reTex.filterMode = FilterMode.Point;
        src.filterMode = FilterMode.Point;
        Graphics.Blit(src, reTex);
        Graphics.Blit(reTex, dest);
        RenderTexture.ReleaseTemporary(reTex);
    }

    public IEnumerator Pixelate(Action onComplete) {
        yield return Pixelate(1, 8 , 0.8f);
        yield return new WaitForSeconds(1.0f);
        yield return Pixelate(8, 1, 1.0f);
        onComplete();
    }

    public IEnumerator Pixelate(int start, int end, float delta) {
        amount = start;
        max = end;
        while (amount < max) {
            amount += 2;
            yield return new WaitForSeconds(delta);
        }
        amount = end;
        yield break;
    }


}

