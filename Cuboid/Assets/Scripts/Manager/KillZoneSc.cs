using UnityEngine;

public class KillZoneSc : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerCharacter2D p = collision.GetComponent<PlayerCharacter2D>();
            GameMaster.KillJoueur(p);
        }
    }
}
