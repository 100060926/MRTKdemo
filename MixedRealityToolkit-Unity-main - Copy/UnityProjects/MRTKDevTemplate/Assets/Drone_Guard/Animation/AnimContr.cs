using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimContr : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("WakeUp");
    }

    // Update is called once per frame
    void Update()
    {

        

    }
    public void IdlePlay() {
        anim.Play("Idle");
    }
}
