using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyListOne : MonoBehaviour
{
    public Text nameText;
    public Text priceText;
    public Text buyCntText;

    public void Set()
    {

    }
    public void ClickCancel()
    {
        //가격 합에서도 뺀다.
        priceText.text = priceText.text.Replace("원", "");
        int price = int.Parse(priceText.text);
        Debug.Log("취소한 가격 : " + price);
        ProductCtrl._Instance.sumPrice -= price;
        //Debug.Log("최종합 가격 : " + ClickObject._Instance.sumPrice);
        ProductCtrl._Instance.sumPriceText.text = ProductCtrl._Instance.sumPrice.ToString() + "원";
        ProductCtrl._Instance.sumPriceText_buy.text = ProductCtrl._Instance.sumPrice.ToString() + "원";
        Destroy(this.gameObject);
        foreach(Transform buyListOne in ProductCtrl._Instance.buyContent.GetComponentsInChildren<Transform>())
        {
            if (buyListOne.gameObject.name == "BuyListOne(Clone)")
            {
                //Transform nameText = buyListOne.Find("NameText");
                // Transform priceText = buyListOne.Find("PriceText");
                Transform nameText = buyListOne.GetChild(0);
                Transform priceText = buyListOne.GetChild(1);
                Text t = nameText.GetComponent<Text>();
                Text t_price = priceText.GetComponent<Text>();
                Debug.Log(t_price.text + " , " + this.transform.GetChild(1).GetComponent<Text>().text);
                Debug.Log(t.text +" , " + this.transform.GetChild(0).GetComponent<Text>().text);
                if (t.text.Remove(t.text.IndexOf(" x ")) == this.transform.GetChild(0).GetComponent<Text>().text &&
                    t_price.text.Replace("원", "") == this.transform.GetChild(1).GetComponent<Text>().text)
                {
                    //t에서 x이후 다 지우기
                   /* Debug.Log(t.text + " 지우기");
                    t.text = t.text.Remove(t.text.IndexOf(" x "));//, t.text.Length-1);
                    Debug.Log(t.text + "로 바뀜");
                    //이름이 같으면
                    Debug.Log(t_price.text + "와"
                        + this.transform.GetChild(1).GetComponent<Text>().text);
              */
                    Destroy(buyListOne.gameObject);
                    break;
                }
            }
            
        }
    }
}
