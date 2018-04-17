using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChopFish : MonoBehaviour {

    public GameObject chopped;
    public int numPieces = 4;

    public void OnChop()
    {
        for( int piece=0; piece < numPieces; piece++)
        {
            Instantiate(chopped, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

}
