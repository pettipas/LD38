using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

    public Animator animator;
    public AudioSource audioSource;

    public IEnumerator Start() {
        PlayerPrefs.DeleteAll();
        while (!Input.anyKey) {
            yield return null;
        }
        audioSource.Play();
        animator.SafePlay("startin");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("main");
    }
}
