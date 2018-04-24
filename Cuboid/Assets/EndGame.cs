using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {

    private BoxCollider2D trigger;
    private bool takingOff = false;
    Vector3 endPos;

    void Awake()
    {
        trigger = GetComponent<BoxCollider2D>();
        trigger.enabled = false;
        endPos = transform.parent.position + new Vector3(transform.parent.position.x, transform.parent.position.y + 18f, transform.parent.position.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go = other.gameObject;
        if (go.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>().timerRunning = false;

            go.SetActive(false);
            transform.parent.GetComponent<Animator>().Play("ShipEnter");
            InvokeRepeating("TakeOff", 3f, 0.001f);     
        }
    }

    private void TakeOff()
    {
        takingOff = true;    
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, endPos, Time.deltaTime);

        if (transform.parent.position == endPos)
        {
            //FindObjectOfType<AudioManager>().Stop("EscapeMusic");
            if (FindObjectOfType<AudioManager>() != null) {
                FindObjectOfType<AudioManager>().ChangeMusique("MusiqueFin");
            }
            CancelInvoke();
            //Application.LoadLevelAsync(4);
            SceneManager.LoadSceneAsync(4);
        }

        //TODO: mettre les crédits et changer la musique et retourner au menu principale.
        
    }

    public void Enable()
    {
        trigger.enabled = true;
    }
}
