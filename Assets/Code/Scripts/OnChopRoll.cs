using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChopRoll : MonoBehaviour {

    public GameObject maki;
    [SerializeField]
    private Transform[] makiPieces;

    public void OnChop()
    {
        foreach (Transform makiPiece in makiPieces)
        {
            Instantiate(maki, makiPiece.position, makiPiece.rotation);
            Destroy(makiPiece.gameObject);
        }
        Destroy(gameObject);
    }

}
