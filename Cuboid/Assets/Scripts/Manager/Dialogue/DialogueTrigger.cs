using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

    public Dialogue dialogue;
    private GameObject DialogUI;
    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("UpgradeDialogUI"))
        {
            DialogUI = GameObject.FindGameObjectWithTag("UpgradeDialogUI");
            //DialogUI.SetActive(false);
        }
    }

    public void TriggerDialogue() {
        DialogUI.GetComponent<DialogueManager>().StartDialogue(dialogue);
    }
}
