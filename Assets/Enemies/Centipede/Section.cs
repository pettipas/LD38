using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour {

    public HorizontalMotion hMotion;
    public GameObject head;
    public GameObject body;
    public GameObject bodyRoot;
    public Animator headAnimation;
    public Animator bodyAnimation;
    public float animationSpeed;
    public float startTime;
    public Transform Body {
        get {
            return bodyRoot.transform;
        }
    }

    public void Start() {
        headAnimation = head.GetComponent<Animator>();
        bodyAnimation = body.GetComponent<Animator>();
        headAnimation.speed = animationSpeed;
        bodyAnimation.speed = animationSpeed;
        headAnimation.Play("crawl",0,startTime);
        bodyAnimation.Play("body_crawl", 0, startTime);
    }

    public void Update() {
        if (hMotion.IsLeader) {
            head.SetActive(true);
            body.SetActive(false);
           if(!headAnimation.GetCurrentAnimatorStateInfo(0).IsName("crawl"))
                headAnimation.Play("crawl", 0, bodyAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
        else {
            head.SetActive(false);
            body.SetActive(true);
            if (!bodyAnimation.GetCurrentAnimatorStateInfo(0).IsName("body_crawl"))
                bodyAnimation.Play("body_crawl", 0, headAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
    }
}
