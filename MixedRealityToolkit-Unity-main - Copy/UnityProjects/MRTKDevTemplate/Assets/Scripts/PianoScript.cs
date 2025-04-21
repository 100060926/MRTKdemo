using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class PianoScript : MonoBehaviour
{
    [SerializeField] private UnityEvent Script1Complete;
    [SerializeField] private UnityEvent Script2Complete;
    [SerializeField] private UnityEvent Script3Complete;
    // public UnityEvent Script1Complete => Script1Complete;
    // public UnityEvent Script2Complete => Script2Complete;
    // public UnityEvent Script3Complete => Script3Complete;
    public GameObject WonPanel;
    public GameObject LostPanel;
    string[] piano = new string[4];
    string[] piano2 = {"C","EFlat", "E", "G"};
    string[] piano3 = {"G","E", "EFlat", "D"};
    string[] piano4 = {"B","CSharp", "A", "D"};
    int numOfClicks = 0;
    int maxClicks = 4;
    public TMP_Text a;
    public TMP_Text numOfC;
    bool piano2bool;
    bool piano3bool;
    bool piano4bool;
    public TMP_Text description;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (numOfClicks == 1) {
            WonPanel.gameObject.SetActive(false);
            LostPanel.gameObject.SetActive(false);
        }
        if (numOfClicks == maxClicks) {
            numOfClicks = 0;
            
            if (checkArrays(piano2)) {
                WonPanel.gameObject.SetActive(true);
                piano2bool = true;
                Script1Complete?.Invoke();
            } else if (checkArrays(piano3)){
                WonPanel.gameObject.SetActive(true);
                piano3bool = true;
                Script2Complete?.Invoke();
            } else if (checkArrays(piano4)){
                WonPanel.gameObject.SetActive(true);
                piano4bool = true;
                Script3Complete?.Invoke();
            } else {
                LostPanel.gameObject.SetActive(true);
            }
        }
    }
    bool checkArrays(string[] a) {
        for (int i=0; i<a.Length; i++) {
            if (!(piano[i] == a[i])) {
                return false;
            }
        }
        return true;
    }
    public void after1() {
        a.text = "The code for the padlock is:\n\n5*6*";
        description.text = "Congrats, you have typed the first sequence correctly, now try to find the next sheet. (Hint: listen to the sound)";
    }
    public void after2() {
        a.text = "The code for the padlock is:\n\n5*67";
        description.text = "Wow!, you have typed the second sequence correctly, now try to find the last sheet. (Hint: listen to the sound)";
    }
    public void after3() {
        a.text = "The code for the padlock is:\n\n5567";
        description.text = "You are amazing!, now that you have typed finished the piano sequence game, you could type the password on the padlock";
    }
    string showArray() {
        return (piano[0] + piano[1] + piano[2] + piano[3]);
    }
    public void A() {
        piano[numOfClicks] = "A";
        numOfClicks++;
        numOfC.text = ""+numOfClicks;
    }
    public void B() {
        piano[numOfClicks] = "B";
        numOfClicks++;
        numOfC.text = ""+numOfClicks;
    }
    public void BFlat() {
        piano[numOfClicks] = "BFlat";
        numOfClicks++;
        numOfC.text = ""+numOfClicks;
    }
    public void C() {
        piano[numOfClicks] = "C";
        numOfClicks++;
        numOfC.text = ""+numOfClicks;
    }
    public void CSharp() {
        piano[numOfClicks] = "CSharp";
        numOfClicks++;
        numOfC.text = ""+numOfClicks;
    }
    public void D() {
        piano[numOfClicks] = "D";
        numOfClicks++;
        numOfC.text = ""+numOfClicks;
    }
    public void E() {
        piano[numOfClicks] = "E";
        numOfClicks++;
        numOfC.text = ""+numOfClicks;
    }
    public void EFlat() {
        piano[numOfClicks] = "EFlat";
        numOfClicks++;
        numOfC.text = ""+numOfClicks;
    }
    public void F() {
        piano[numOfClicks] = "F";
        numOfClicks++;
        numOfC.text = ""+numOfClicks;
    }
    public void FSharp() {
        piano[numOfClicks] = "FSharp";
        numOfClicks++;
        numOfC.text = ""+numOfClicks;
    }
    public void G() {
        piano[numOfClicks] = "G";
        numOfClicks++;
        numOfC.text = ""+numOfClicks;
    }
    public void GSharp() {
        piano[numOfClicks] = "GSharp";
        numOfClicks++;
        numOfC.text = ""+numOfClicks;
    }

}
