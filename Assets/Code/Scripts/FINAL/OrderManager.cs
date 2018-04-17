using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour {

    public bool hasCustomer = false;
    [SerializeField]
    private Customer customer;

    private int tempuraLeft;
    private int makiLeft;
    private int sushiLeft;

    public List<Transform> currentlyInOrder;

    public bool isReady;
    public GameObject deliveryPlate;
    public Transform deliveryPoint;
    public AudioClip victoryClip;

	// Use this for initialization
	void Start () {
        currentlyInOrder = new List<Transform>();
        if( customer != null)
        {
            hasCustomer = true;
        }
    }

    public void SetCustomer( Customer person )
    {
        if (!hasCustomer)
        {
            customer = person;
            if(customer.Order) DetermineAmounts(customer.Order);
            hasCustomer = true;
        }
    }

    public void DetermineAmounts( Order request )
    {
        tempuraLeft = request.TempuraCount;
        makiLeft = request.MakiCount;
        sushiLeft = request.SushiCount;
    }

    public void RemoveFromOrder( GameObject item )
    {
        if (currentlyInOrder.Contains(item.transform))
        {
            currentlyInOrder.Remove(item.transform);
        }

        if (item.CompareTag("tempura")) tempuraLeft++;
        if (item.CompareTag("maki")) makiLeft++;
        if (item.CompareTag("sushi")) sushiLeft++;
    }

    public void AddToOrder( GameObject item )
    {
        currentlyInOrder.Add(item.transform);

        if (item.CompareTag("tempura")) tempuraLeft--;
        if (item.CompareTag("maki")) makiLeft--;
        if (item.CompareTag("sushi")) sushiLeft--;
    }

    public void CompleteOrder()
    {
        bool isReady = (tempuraLeft <= 0 && makiLeft <= 0 && sushiLeft <= 0);
        if (hasCustomer && isReady)
        {
            DeliverToCustomer();
            Invoke("PlayVictorySound", 2f);
        }
    }

    public void DeliverToCustomer()
    {
        GameObject plateToDeliver = Instantiate(deliveryPlate, deliveryPoint.position, deliveryPoint.rotation);
        AttachToPlate(plateToDeliver.transform);
        plateToDeliver.SendMessage("OnDeliver", customer.headPosition.position, SendMessageOptions.DontRequireReceiver);
        customer = null;
        hasCustomer = false;
    }

    public void AttachToPlate( Transform plate )
    {
        foreach (Transform current in currentlyInOrder)
        {
            current.SetParent(plate);
            current.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            current.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    public void DettachFromPlate()
    {
        foreach (Transform current in currentlyInOrder)
        {
            current.parent = null;
            current.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            current.GetComponent<Rigidbody>().useGravity = true;

            float range = 0.2f;
            Vector3 randomForcePosition = new Vector3(  Random.Range(-range, range), 
                                                        Random.Range(-range, 0.0f), 
                                                        Random.Range(-range, range)
                                                        );
            current.GetComponent<Rigidbody>().AddExplosionForce(5.0f, current.transform.position + randomForcePosition, 0.5f, 0.0f, ForceMode.VelocityChange);
        }
    }

    public void PlayVictorySound()
    {
        GetComponent<AudioSource>().clip = victoryClip;
        GetComponent<AudioSource>().Play();
    }
}
