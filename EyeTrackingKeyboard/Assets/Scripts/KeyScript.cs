using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyScript : MonoBehaviour {
    public string keyCharacter;
    public TMP_Text textComponent;
    public GameObject keyMesh;
    private bool isSelected = false;

    void Start() {
        textComponent.text = keyCharacter;
        keyMesh.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 1);
    }

    void Update() {

    }

    public void SetSelected(bool selected) {
        this.isSelected = selected;
        if (selected) {
            keyMesh.GetComponent<Renderer>().material.color = new Color(1, 1, 0, 1);
        } else {
            keyMesh.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 1);
        }
    }
}
