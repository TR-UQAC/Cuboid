using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class boss : Personnages
{

    private void Awake()
    {
        //  set les variable avant d'être actif
    }

    // Use this for initialization
    void Start ()
    {
		//  lancé l'intro du boss
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void DommagePerso(int dommage)
    {

        /*if (!ennemiStats.immortel)
        {
            ennemiStats.immortel = true;
            StartCoroutine(ChangeImmortel());
            ennemiStats.vie -= dommage;

            if (ennemiStats.vie <= 0)
            {
                GameMaster.KillEnnemi(this);
                //TimeManager.DoSlowMotion();
            }
        }*/
    }
}
