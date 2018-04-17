using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour {

    private OrderManager orderManager;
    public Transform headPosition;

    public bool hasToOrder = true;

    [SerializeField]
    private Order order;
    public Order Order
    {
        get
        {
            return order;
        }
    }

    [SerializeField]
    protected GameObject queryChan;
    [SerializeField]
    QuerySDMecanimController.QueryChanSDAnimationType defaultAnimType = QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_IDLE;
    [SerializeField]
    QuerySDEmotionalController.QueryChanSDEmotionalType defaultFaceType = QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_DEFAULT;
    [SerializeField]
    QuerySDEmotionalController.QueryChanSDEmotionalType angryFaceType = QuerySDEmotionalController.QueryChanSDEmotionalType.NORMAL_ANGER;

    // Use this for initialization
    void Start () {
        orderManager = GameObject.FindObjectOfType<OrderManager>();
        ChangeAnimation((int)defaultAnimType);
        ChangeFace(defaultFaceType);
    }

    // Update is called once per frame
    void Update () {
        if (hasToOrder)
        {
            OnOrder();
            hasToOrder = false;
        }
    }

    void OnOrder()
    {
        orderManager.gameObject.SendMessage("SetCustomer", this, SendMessageOptions.DontRequireReceiver);
    }

    void OnEat( GameObject food )
    {
        Destroy(food.gameObject);
        ChangeAnimation((int)QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_ITEMGET);
        Invoke("ResetAnimation", 1.0f);

        if(order) order.ShowCompleteMessage();

        Invoke("OnHungry", 1.0f);

    }

    void OnHungry()
    {
        if(order) order.Reset();
        hasToOrder = true;
    }

    void OnHit( GameObject item )
    {
        ChangeFace(angryFaceType);
        Invoke("ResetFace", 1.0f);
        ChangeAnimation((int)QuerySDMecanimController.QueryChanSDAnimationType.NORMAL_POSE_SIT);
        Invoke("ResetAnimation", 1.0f);
    }

    void ResetAnimation()
    {
        ChangeAnimation((int)defaultAnimType);
    }

    void ResetFace()
    {
        ChangeFace(defaultFaceType);
    }

    void ChangeFace(QuerySDEmotionalController.QueryChanSDEmotionalType faceNumber)
    {
        queryChan.GetComponent<QuerySDEmotionalController>().ChangeEmotion(faceNumber);
    }

    void ChangeAnimation(int animNumber)
    {
        queryChan.GetComponent<QuerySDMecanimController>().ChangeAnimation((QuerySDMecanimController.QueryChanSDAnimationType)animNumber);
    }
}
