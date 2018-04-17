using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private Queue<string> sentences;
    private bool dialogActive = false;

    //private IEnumerator coroutine;

    // Use this for initialization
    void Start () {
        sentences = new Queue<string>();
	}

    void Update()
    {
        if (dialogActive)
        {
            //animator.GetCurrentAnimatorClipInfo()
        }

        if (Input.anyKeyDown && dialogActive)
        {
            EndDialogue();
            //Time.timeScale = 1f;
            //Invoke("EndDialogue", 2f);
        }
    }

    public void StartDialogue(Dialogue dialogue) {
        //TODO: arrêter le temps au début du dialogue
        dialogActive = true;
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;

        Debug.Log("StartDialogue");
        Time.timeScale = 0f;
        PauseMenu.GameIsPaused = true;
        if (sentences == null)
        {
            sentences = new Queue<string>();
        }

        sentences.Clear();

        foreach(string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if(sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        //coroutine = TypeSentence(sentence);
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence) {
       
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue() {
        Debug.Log("EndDialogue");
        Time.timeScale = 1f;
        PauseMenu.GameIsPaused = false;
        dialogActive = false;
        //TODO: Reprendre le temps quand le dialogue est terminer
        animator.SetBool("IsOpen", false);

    }

    public bool GetDialogStatus()
    {
        return animator.GetBool("IsOpen");
    }
}
