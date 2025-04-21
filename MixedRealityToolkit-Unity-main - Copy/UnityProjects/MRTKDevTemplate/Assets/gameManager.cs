using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gameManager : MonoBehaviour
{
    public GameObject theDrone;
    public GameObject followPanel;
    public TMP_Text instructions;
    float wakeUpTime = 10;
    public Animator anim;
    public GameObject pianoScript1; 
    public GameObject pianoScript2; 
    public GameObject pianoScript3; 
    
    // Start is called before the first frame update
    void Awake() {
        
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        AnimContr a =theDrone.GetComponent<AnimContr>();
        StartCoroutine(wakeUpTimeF());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator wakeUpTimeF()
    {
        yield return new WaitForSeconds(wakeUpTime);
        anim.Play("Idle");
        followPanel.SetActive(true);
    }
}
