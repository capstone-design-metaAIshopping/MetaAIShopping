using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ClickObject : MonoBehaviour
{
    public Button soldOutBtn;
    public Button buyBtn;
    public Text priceText;
    public Text nameText;
    public Text sumPriceText;

    public Button UpBtn;
    public Button DownBtn;
    private int BuyCnt = 0;
    public Text BuyCntText;
    public GameObject content; // 카트 리스트뷰의 Content
    public int sumPrice = 0;

    public static ClickObject _Instance;
    public GameObject buyListOnePrefab;
    public GameObject _NewSphere;
    public GameObject _CloneProduct;
    public bool isNewSpace;
    private void Awake()
    {
        _Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);
            if(hit.collider != null)
            {
                GameObject CurrentTouch = hit.transform.gameObject;
                Debug.Log("클릭한 오브젝트 이름 : " + hit.transform.gameObject.name);
                string productName = hit.transform.gameObject.name;
                productName = productName.Replace("(Clone)", "");
                //클릭한 오브젝트 이름에서 숫자, 띄어쓰기, (,) 지우고 
                //이름이 파이어베이스 안에 있는 name과 이름이 같다면, 
                if (FirebaseCtrl._Instance.productDict.ContainsKey(productName))
                {
                    Debug.Log("품절인가? " + FirebaseCtrl._Instance.productDict[productName].soldOut.ToString());
                    nameText.text = productName;
                    priceText.text = FirebaseCtrl._Instance.productDict[productName].price.ToString();
                    UICtrl._Instance.OpenClose_Product_Info();
                    //파이어베이스에서 가격, 품절인지 아닌지 받아온다.
                   if ( FirebaseCtrl._Instance.productDict[productName].soldOut.ToString() == "1")
                    {
                        //품절 버튼 활성화
                        soldOutBtn.gameObject.SetActive(true);
                        //구매버튼 비활성화
                        buyBtn.gameObject.SetActive(false);
                        //수량 체크 못하게
                        Debug.Log("해당 상품은 품절 상품입니다");
                    }
                    else
                    {
                        soldOutBtn.gameObject.SetActive(false);
                        buyBtn.gameObject.SetActive(true);
                        //구매버튼 활성화
                        Debug.Log("구매 가능한 상품입니다.");

                    }
                }
                /*if (hit.collider.gameObject.name == "티셔츠")
                {
                    //해당 오브젝트 복제
                    _NewSphere =  Instantiate(UICtrl._Instance.rollerSphere, Camera.main.transform.position, Quaternion.identity);
                    UICtrl._Instance.shoppingCenter.SetActive(false);
                    _CloneProduct = Instantiate(hit.collider.gameObject, this.transform.position + new Vector3(0.0f,1.5f,-1.0f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
                    // SceneManager.LoadScene("retro", LoadSceneMode.Additive);
                    //1,2,3층 패널을 없애고
                    UICtrl._Instance.FloorPanel.SetActive(false);
                    //뒤로가기 버튼 보이게
                    UICtrl._Instance.BackToShoppingBtn.SetActive(true);
                }
                */
                foreach(string s in ChangeSpace._Instance.spaceDict.Keys)
                {
                    Debug.Log(s); //키들
                    string[] values;
                    ChangeSpace._Instance.spaceDict.TryGetValue(s, out values);
                  
                    if(Array.Exists(values, x => x.Equals(hit.collider.gameObject.name)))
                    {
                        Debug.Log(s + "공간으로 이동");
                        //s 공간으로 이동
                        switch (s)
                        {   
                            case "집": _NewSphere = Instantiate(UICtrl._Instance.houseSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "롤러장": _NewSphere = Instantiate(UICtrl._Instance.rollerSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "도로": _NewSphere = Instantiate(UICtrl._Instance.roadSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "농구장": _NewSphere = Instantiate(UICtrl._Instance.basketballSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "겨울": _NewSphere = Instantiate(UICtrl._Instance.winterSphere, Camera.main.transform.position, Quaternion.identity); break;
                        }
                        UICtrl._Instance.shoppingCenter.SetActive(false);
                        _CloneProduct = Instantiate(hit.collider.gameObject, this.transform.position + new Vector3(0.0f, 1.5f, -1.0f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
                        _CloneProduct.GetComponent<Renderer>().materials = new Material[1] { _CloneProduct.GetComponent<Renderer>().material };
                        isNewSpace = true;
                        //1,2,3층 패널을 없애고
                        UICtrl._Instance.FloorPanel.SetActive(false);
                        //뒤로가기 버튼 보이게
                        UICtrl._Instance.BackToShoppingBtn.SetActive(true);
                        break;
                    }
                }
               
            }
        }
    }

    /*
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
            GameObject buyListOne = Instantiate(buyListOnePrefab);
            //buyListOne.transform.parent.gameObject.GetComponent<BuyListOne>();
            buyListOne.transform.SetParent(content.transform, false);
            int priceResult = int.Parse(BuyCntText.text) * int.Parse(priceText.text);
            sumPrice += priceResult;
            buyListOne.GetComponent<BuyListOne>().nameText.text = nameText.text;
            buyListOne.GetComponent<BuyListOne>().buyCntText.text = BuyCntText.text;
            buyListOne.GetComponent<BuyListOne>().priceText.text = priceResult.ToString() + "원";
            sumPriceText.text = sumPrice.ToString() + "원";
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
    */
}
