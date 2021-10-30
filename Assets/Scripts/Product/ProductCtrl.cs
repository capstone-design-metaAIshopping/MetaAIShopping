using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//버튼 누르면 수량 변화하는 스크립트
//Product_Info_Pannel 관련 스크립트
public class ProductCtrl : MonoBehaviour
{
    public Button UpBtn;
    public Button DownBtn;
    private int BuyCnt;
    public Text BuyCntText;
    public Text priceText;
    public Text nameText;
    public Text sumPriceText;
    public Text sumPriceText_buy;
    public GameObject cartContent; // 카트 리스트뷰의 Content
    public GameObject buyContent; // 구매 리스트뷰의 Content
    public int sumPrice = 0;

    public static ProductCtrl _Instance;
    public GameObject cartListOnePrefab;
    public GameObject buyListOnePrefab;

    private void Awake()
    {
        _Instance = this;
    }
 
    //UP버튼 눌러서 구매수량 늘리기
    public void Up_Click()
    {
        BuyCnt = int.Parse(BuyCntText.text);
        BuyCnt++;
        BuyCntText.text = BuyCnt.ToString();
    }
    //Down버튼 눌러서 구매수량 줄이기
    public void Down_Click()
    {
        BuyCnt = int.Parse(BuyCntText.text);
        if (BuyCnt > 0)
        {
            BuyCnt--;
        }
        else
        {
            Debug.Log("구매수량은 0 이하가 불가능합니다.");
        }
        BuyCntText.text = BuyCnt.ToString();

    }

    public void ClickBuyButton()
    {
        if (BuyCnt > 0)
        {
            //프리팹 추가생성한다음에 부모를 뷰스크롤 Content로 한다.
            //GameObject buyListOne = Instantiate(Resources.Load<GameObject>("Prefabs/BuyListOne"));
            GameObject cartListOne = Instantiate(cartListOnePrefab);
            GameObject buyListOne = Instantiate(buyListOnePrefab);
            //buyListOne.transform.parent.gameObject.GetComponent<BuyListOne>();
            cartListOne.transform.SetParent(cartContent.transform, false);
            buyListOne.transform.SetParent(buyContent.transform, false);
            int priceResult = int.Parse(BuyCntText.text) * int.Parse(priceText.text);
            sumPrice += priceResult;
            cartListOne.GetComponent<BuyListOne>().nameText.text = nameText.text;
            buyListOne.GetComponent<BuyListOne>().nameText.text = nameText.text + " x " + BuyCntText.text;
            cartListOne.GetComponent<BuyListOne>().buyCntText.text = BuyCntText.text;
            cartListOne.GetComponent<BuyListOne>().priceText.text = priceResult.ToString() + "원";
            buyListOne.GetComponent<BuyListOne>().priceText.text = priceResult.ToString() + "원";
            sumPriceText.text = sumPrice.ToString() + "원";
            sumPriceText_buy.text = sumPrice.ToString() + "원";
        }
        else
        {
            Debug.Log("구매수량이 0입니다.");
        }
        UICtrl._Instance.Product_Info_Panel.SetActive(false);

        //  buyListOne.SetParent(content);
        //장바구니에 들어갈 리스트한개에 이름, 가격 넣는다!
        //여기서도 취소버튼 누를수 있도록..?
        //구매한 전체 수량도 계산 해야할듯

    }



}
