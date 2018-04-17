using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour {

    public const int maxPerItem = 4;

    [SerializeField]
    private Text tempuraTextCount;  //Tempura
    private int tempuraCount;
    public int TempuraCount
    {
        get
        {
            return tempuraCount;
        }
    }

    [SerializeField]
    private Text makiTextAmount;     //Maki
    private int makiCount;
    public int MakiCount
    {
        get
        {
            return makiCount;
        }
    }

    [SerializeField]
    private Text sushiTextAmount;    //Sushi
    private int sushiCount;
    public int SushiCount
    {
        get
        {
            return sushiCount;
        }
    }

    public GameObject completeMessage;

    void Awake()
    {
        GetAmounts();
    }

    void GetAmounts()
    {
        tempuraCount = int.Parse(tempuraTextCount.text);
        makiCount = int.Parse(makiTextAmount.text);
        sushiCount = int.Parse(sushiTextAmount.text);
    }

    void SetAmounts()
    {
        tempuraTextCount.text = Random.Range(1, maxPerItem+1).ToString();
        makiTextAmount.text = Random.Range(1, maxPerItem+1).ToString();
        sushiTextAmount.text = Random.Range(1, maxPerItem+1).ToString();
    }

    public void Reset()
    {
        HideCompleteMessage();
        SetAmounts();
        GetAmounts();
    }

    public void ShowCompleteMessage()
    {
        completeMessage.SetActive(true);
    }

    public void HideCompleteMessage()
    {
        completeMessage.SetActive(false);
    }

}
