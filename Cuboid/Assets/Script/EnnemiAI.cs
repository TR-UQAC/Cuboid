using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnnemiAI : MonoBehaviour {

    //La cible
    public Transform target;

    //Nombre d'update par seconde
    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;

    //Le chemin
    public Path path;

    //La vitesse
    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    //La distance maxime de l'IA d'un waypoint pour continuer au prochain waypoint
    public float nextWaypointDistance = 3;

    //Le waypoint ver lequel il se déplace
    private int currentWaypoint = 0;

    private bool searchingForPlayer = false;

    private void Start(){
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if(target == null){
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        //Commencer un nouveau chemin vers la position de la cible, retourne le resultat a la methode OnPathComplete
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    IEnumerator SearchForPlayer() {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if(sResult == null) {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        } else {
            target = sResult.transform;
            searchingForPlayer = false;
            StartCoroutine(UpdatePath());
            yield break;
        }
    }

    IEnumerator UpdatePath() {
        if(target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            yield break;
        }

        //Commencer un nouveau chemin vers la position de la cible, retourne le resultat a la methode OnPathComplete
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p){
        Debug.Log("Nous avons un path. Avons nous une erreur : " + p.error);
        if (!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate() {
        if (target == null) {
            if (!searchingForPlayer) {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        //TODO: Always look at player?
        if(path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count) {
            if (pathIsEnded)
                return;

            Debug.Log("Fin du chemin atteint");
            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;
        //Dirrection vers le prochain waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        //Move the AI
        rb.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if(dist < nextWaypointDistance) {
            currentWaypoint++;
            return;
        }
    }

}
