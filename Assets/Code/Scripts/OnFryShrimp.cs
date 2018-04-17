using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFryShrimp : MonoBehaviour {

    public GameObject fried;

    public void OnFry()
    {
        Instantiate(fried, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
