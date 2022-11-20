using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EyeTrackingKeyboard : MonoBehaviour {

    public TMP_Text outputTextComponent;
    public GameObject keyboardMesh;
    public GameObject keysParent;
    public Transform eyeTransform;
    public float selectionTime = 0.5f;

    private string outputString = "";
    private KeyScript[] keys;
    private KeyScript? lastSelectedKey = null;
    private float currentSelectionTime = 0;

    private bool isUsingUppercase = false;
    private bool isUsingSecondary = false;

    private void ClearSelection() {
        foreach (KeyScript key in keys) {
            key.SetSelected(false);
        }
    }

    private void SetUseSecondary(bool useSecondary) {
        this.isUsingSecondary = useSecondary;
        foreach (KeyScript key in keys) {
            key.SetUseSecondary(useSecondary);
        }
    }

    private void SetUseUppercase(bool useUppercase) {
        this.isUsingUppercase = useUppercase;
        foreach (KeyScript key in keys) {
            key.SetUseUppercase(useUppercase);
        }
    }

    void Start() {
        this.keys = keysParent.GetComponentsInChildren<KeyScript>();
        keyboardMesh.GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 0.4f, 0.25f);
        outputTextComponent.text = "";
        ClearSelection();
        SetUseUppercase(false);
        SetUseSecondary(false);
    }

    void Update() {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, eyeTransform.position.y - 0.1f, gameObject.transform.position.z);

        if (lastSelectedKey != null) {
            lastSelectedKey.SetSelected(false);
        }

        RaycastHit hit;
        Ray ray = new Ray(eyeTransform.position, eyeTransform.forward);

        if (Physics.Raycast(ray, out hit, 10)) {

            KeyScript? key = hit.transform.parent.GetComponent<KeyScript>();

            if (key != null) {
                key.SetSelected(true);

                if (lastSelectedKey != null && lastSelectedKey.keyCharacter == key.keyCharacter) {
                    currentSelectionTime += Time.deltaTime;

                    if (currentSelectionTime >= selectionTime) {
                        currentSelectionTime = 0;

                        if (key.keyCharacter == "Backspace") {
                            if (!System.String.IsNullOrEmpty(this.outputString)) {
                                this.outputString = outputString.Remove(outputString.Length - 1, 1);
                            }
                        } else if (key.keyCharacter == "Shift") {
                            SetUseUppercase(!this.isUsingUppercase);
                        } else if (key.keyCharacter == "Enter") {
                            this.outputString += "\n";
                        } else if (key.keyCharacter == "123") {
                            SetUseSecondary(!this.isUsingSecondary);
                        } else {
                            if (this.isUsingUppercase) {
                                if (this.isUsingSecondary) {
                                    this.outputString += key.uppercaseSecondaryKeyCharacter;
                                } else {
                                    this.outputString += key.uppercaseKeyCharacter;
                                }
                                
                                SetUseUppercase(false);
                            } else {
                                if (this.isUsingSecondary) {
                                    this.outputString += key.secondaryKeyCharacter;
                                } else {
                                    this.outputString += key.keyCharacter;
                                }
                            }
                        }

                        outputTextComponent.text = outputString;
                        gameObject.GetComponent<AudioSource>().Play();
                    }
                } else {
                    lastSelectedKey = key;
                    currentSelectionTime = 0;
                }  
            }
        }
    }
}
