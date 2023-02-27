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
    public GameObject content; // īƮ ����Ʈ���� Content
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
                Debug.Log("Ŭ���� ������Ʈ �̸� : " + hit.transform.gameObject.name);
                string productName = hit.transform.gameObject.name;
                productName = productName.Replace("(Clone)", "");
                //Ŭ���� ������Ʈ �̸����� ����, ����, (,) ����� 
                //�̸��� ���̾�̽� �ȿ� �ִ� name�� �̸��� ���ٸ�, 
                if (FirebaseCtrl._Instance.productDict.ContainsKey(productName))
                {
                    Debug.Log("ǰ���ΰ�? " + FirebaseCtrl._Instance.productDict[productName].soldOut.ToString());
                    nameText.text = productName;
                    priceText.text = FirebaseCtrl._Instance.productDict[productName].price.ToString();
                    UICtrl._Instance.OpenClose_Product_Info();
                    //���̾�̽����� ����, ǰ������ �ƴ��� �޾ƿ´�.
                   if ( FirebaseCtrl._Instance.productDict[productName].soldOut.ToString() == "1")
                    {
                        //ǰ�� ��ư Ȱ��ȭ
                        soldOutBtn.gameObject.SetActive(true);
                        //���Ź�ư ��Ȱ��ȭ
                        buyBtn.gameObject.SetActive(false);
                        //���� üũ ���ϰ�
                        Debug.Log("�ش� ��ǰ�� ǰ�� ��ǰ�Դϴ�");
                    }
                    else
                    {
                        soldOutBtn.gameObject.SetActive(false);
                        buyBtn.gameObject.SetActive(true);
                        //���Ź�ư Ȱ��ȭ
                        Debug.Log("���� ������ ��ǰ�Դϴ�.");

                    }
                }
                /*if (hit.collider.gameObject.name == "Ƽ����")
                {
                    //�ش� ������Ʈ ����
                    _NewSphere =  Instantiate(UICtrl._Instance.rollerSphere, Camera.main.transform.position, Quaternion.identity);
                    UICtrl._Instance.shoppingCenter.SetActive(false);
                    _CloneProduct = Instantiate(hit.collider.gameObject, this.transform.position + new Vector3(0.0f,1.5f,-1.0f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
                    // SceneManager.LoadScene("retro", LoadSceneMode.Additive);
                    //1,2,3�� �г��� ���ְ�
                    UICtrl._Instance.FloorPanel.SetActive(false);
                    //�ڷΰ��� ��ư ���̰�
                    UICtrl._Instance.BackToShoppingBtn.SetActive(true);
                }
                */
                foreach(string s in ChangeSpace._Instance.spaceDict.Keys)
                {
                    Debug.Log(s); //Ű��
                    string[] values;
                    ChangeSpace._Instance.spaceDict.TryGetValue(s, out values);
                  
                    if(Array.Exists(values, x => x.Equals(hit.collider.gameObject.name)))
                    {
                        Debug.Log(s + "�������� �̵�");
                        //s �������� �̵�
                        switch (s)
                        {   
                            case "��": _NewSphere = Instantiate(UICtrl._Instance.houseSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "�ѷ���": _NewSphere = Instantiate(UICtrl._Instance.rollerSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "����": _NewSphere = Instantiate(UICtrl._Instance.roadSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "����": _NewSphere = Instantiate(UICtrl._Instance.basketballSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "�ܿ�": _NewSphere = Instantiate(UICtrl._Instance.winterSphere, Camera.main.transform.position, Quaternion.identity); break;
                        }
                        UICtrl._Instance.shoppingCenter.SetActive(false);
                        _CloneProduct = Instantiate(hit.collider.gameObject, this.transform.position + new Vector3(0.0f, 1.5f, -1.0f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
                        _CloneProduct.GetComponent<Renderer>().materials = new Material[1] { _CloneProduct.GetComponent<Renderer>().material };
                        isNewSpace = true;
                        //1,2,3�� �г��� ���ְ�
                        UICtrl._Instance.FloorPanel.SetActive(false);
                        //�ڷΰ��� ��ư ���̰�
                        UICtrl._Instance.BackToShoppingBtn.SetActive(true);
                        break;
                    }
                }
               
            }
        }
    }

    /*
    //UP��ư ������ ���ż��� �ø���
    public void Up_Click()
    {
        BuyCnt = int.Parse(BuyCntText.text);
        BuyCnt++;
        BuyCntText.text = BuyCnt.ToString();
    }
    //Down��ư ������ ���ż��� ���̱�
    public void Down_Click()
    {
        BuyCnt = int.Parse(BuyCntText.text);
        if (BuyCnt > 0)
        {
            BuyCnt--;
        }
        else
        {
            Debug.Log("���ż����� 0 ���ϰ� �Ұ����մϴ�.");
        }
        BuyCntText.text = BuyCnt.ToString();

    }


    public void ClickBuyButton()
    {
        if (BuyCnt > 0)
        {
            //������ �߰������Ѵ����� �θ� �佺ũ�� Content�� �Ѵ�.
            //GameObject buyListOne = Instantiate(Resources.Load<GameObject>("Prefabs/BuyListOne"));
            GameObject buyListOne = Instantiate(buyListOnePrefab);
            //buyListOne.transform.parent.gameObject.GetComponent<BuyListOne>();
            buyListOne.transform.SetParent(content.transform, false);
            int priceResult = int.Parse(BuyCntText.text) * int.Parse(priceText.text);
            sumPrice += priceResult;
            buyListOne.GetComponent<BuyListOne>().nameText.text = nameText.text;
            buyListOne.GetComponent<BuyListOne>().buyCntText.text = BuyCntText.text;
            buyListOne.GetComponent<BuyListOne>().priceText.text = priceResult.ToString() + "��";
            sumPriceText.text = sumPrice.ToString() + "��";
        }
        else
        {
            Debug.Log("���ż����� 0�Դϴ�.");
        }
        UICtrl._Instance.Product_Info_Panel.SetActive(false);

        //  buyListOne.SetParent(content);
        //��ٱ��Ͽ� �� ����Ʈ�Ѱ��� �̸�, ���� �ִ´�!
        //���⼭�� ��ҹ�ư ������ �ֵ���..?
        //������ ��ü ������ ��� �ؾ��ҵ�

    }
    */
}
