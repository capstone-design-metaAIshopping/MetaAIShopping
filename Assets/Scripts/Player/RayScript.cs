using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
public class RayScript : MonoBehaviour
{
    public GraphicRaycaster graphicRaycaster; //Canvas에 있는 GraphicRaycaster
    private List<RaycastResult> raycastResults; //Raycast로 충돌한 UI들을 담는 리스트
    private PointerEventData pointerEventData; //Canvas 상의 포인터 위치 및 정보
    LineRenderer layser;
    RaycastHit hit;
    Vector3 RayDir;
    float MaxDistance = 15f;
    public Image Info_Pannel;
    public Button soldOutBtn;
    public Button buyBtn;
    public Text name_text;
    public Text price_text;
   // public Text amount_text;
    public static bool IsPause = false;

    public Material highlight; //외곽선 material
    Material originalMaterial;
    GameObject lastHighlightedObject;


    public static RayScript _Instance;
    private void Awake()
    {
        _Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        layser = this.gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RayDir = transform.rotation * Vector3.forward;
        layser.SetPosition(0, transform.position); // 첫번째 시작점 위치
        layser.SetPosition(1, transform.position + MaxDistance * RayDir);

        // Mouse의 포지션을 Ray cast 로 변환
        RaycastHit hit;

        Ray cast = Camera.main.ScreenPointToRay(Input.mousePosition);
     /*   if (!ClickObject._Instance.isNewSpace)
        {
            if (Physics.Raycast(cast, out hit)) //마우스 레이(디버그 용)
            {
                //상품이면
                if (hit.collider.gameObject.tag == "product")
                {
                    //외곽선 보이게
                    originalMaterial = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
                    HighlightObject(hit.collider.gameObject);
                }
                else
                {
                    ClearHighlighted();
                }
                // if(highlightObj != null)
                //      highlightObj.GetComponent<MeshRenderer>().material = default;
            }
        }
     */


        if (Physics.Raycast(transform.position, RayDir, out hit, MaxDistance)) //(Ray 원점, Ray 방향, 충돌 감지할 RaycastHit, Ray 거리(길이))
        {
            //버튼 속성이면
            if (hit.collider.gameObject.GetComponent<Button>() != null)
            {
                Button btn = hit.collider.gameObject.GetComponent<Button>();
                if (btn != null)
                {
                    btn.Select();
                }
            }
            if (!ViewCtrl._Instance.isNewSpace)
            {
                //상품이면
                if (hit.collider.gameObject.tag == "product")
                {
                    //외곽선 보이게
                    originalMaterial = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
                    HighlightObject(hit.collider.gameObject);
                }
                else
                {
                    ClearHighlighted();
                }
            }
        }
        //test용
        if (Input.GetKeyDown(KeyCode.Z))
           SelectProduct_();
        
    }

    void HighlightObject(GameObject gameObject)
    {
        if (lastHighlightedObject != gameObject)
        {
            ClearHighlighted();
            Debug.Log(gameObject.name + " 하이라이트 키기");
            gameObject.GetComponent<MeshRenderer>().materials = new Material[2]{  originalMaterial, highlight };
            //gameObject.GetComponent<MeshRenderer>().material = highlight;
            lastHighlightedObject = gameObject;
        }

    }

    void ClearHighlighted()
    {
        if (lastHighlightedObject != null)
        {
           // Debug.Log(lastHighlightedObject.name + " 하이라이트 없애기");
            if(lastHighlightedObject.GetComponent<MeshRenderer>().materials.Length > 1)
                lastHighlightedObject.GetComponent<Renderer>().materials = new Material[1] { originalMaterial };
            lastHighlightedObject = null;
        }
    }
    public void SelectProduct(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.blue, 10f); //씬에서 레이를 보이게 하는 용도(이거 없으면 안보임)
            if (Physics.Raycast(transform.position, RayDir, out hit, MaxDistance)) //(Ray 원점, Ray 방향, 충돌 감지할 RaycastHit, Ray 거리(길이))
            {
                Debug.Log("Ray충돌감지" + hit.transform.name);
                UICtrl._Instance.debugText.text += "\nRay충돌감지";
                if (hit.collider.tag == "product")
                {
                    UICtrl._Instance.debugText.text += "\n충돌한거 태그가 product임";
                    GameObject CurrentTouch = hit.collider.transform.gameObject;
                    Debug.Log("클릭한 오브젝트 이름 : " + hit.collider.transform.gameObject.name);
                    string productName = hit.collider.transform.gameObject.name;
                    productName = productName.Replace("(Clone)", "");
                    UICtrl._Instance.debugText.text += "\n클릭 오브젝트 이름 " + productName + " 임";
                    //클릭한 오브젝트 이름에서 숫자, 띄어쓰기, (,) 지우고 
                    //이름이 파이어베이스 안에 있는 name과 이름이 같다면, 
                    if (FirebaseCtrl._Instance.productDict.ContainsKey(productName))
                    {
                        UICtrl._Instance.debugText.text += "\n파이어베이스 안에 있음";
                        Debug.Log("품절인가? " + FirebaseCtrl._Instance.productDict[productName].soldOut.ToString());
                        name_text.text = productName;
                        price_text.text = FirebaseCtrl._Instance.productDict[productName].price.ToString();
                        UICtrl._Instance.OpenClose_Product_Info();
                        //파이어베이스에서 가격, 품절인지 아닌지 받아온다.
                        if (FirebaseCtrl._Instance.productDict[productName].soldOut.ToString() == "1")
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
                    else
                    {
                        UICtrl._Instance.debugText.text += "\n파이어베이스 안에 없음";
                    }
                    
                    foreach (string s in ChangeSpace._Instance.spaceDict.Keys)
                    {
                        string[] values;
                        ChangeSpace._Instance.spaceDict.TryGetValue(s, out values);

                        if (Array.Exists(values, x => x.Equals(hit.collider.gameObject.name)))
                        {
                            Debug.Log(s + "공간으로 이동");
                            //s 공간으로 이동
                            switch (s)
                            {
                                case "집": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.houseSphere, Camera.main.transform.position, Quaternion.identity); break;
                                case "롤러장": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.rollerSphere, Camera.main.transform.position, Quaternion.identity); break;
                                case "도로": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.roadSphere, Camera.main.transform.position, Quaternion.identity); break;
                                case "농구장": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.basketballSphere, Camera.main.transform.position, Quaternion.identity); break;
                                case "겨울": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.winterSphere, Camera.main.transform.position, Quaternion.identity); break;
                            }
                            UICtrl._Instance.shoppingCenter.SetActive(false);
                            ViewCtrl._Instance._CloneProduct = Instantiate(hit.collider.gameObject, this.transform.position + new Vector3(0.0f, 0.0f, -1.0f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
                            ViewCtrl._Instance._CloneProduct.GetComponent<Renderer>().materials = new Material[1] { ViewCtrl._Instance._CloneProduct.GetComponent<Renderer>().material };
                            ViewCtrl._Instance.isNewSpace = true;
                            //1,2,3층 패널을 없애고
                            UICtrl._Instance.FloorPanel.SetActive(false);
                            PlayerCtrl._Instance.boyoung.SetActive(false);
                            //뒤로가기 버튼 보이게
                            UICtrl._Instance.BackToShoppingBtn.SetActive(true);
                            break;
                        }
                    }

                }
                //버튼 속성이라면
                if (hit.collider.gameObject.GetComponent<Button>() != null)
                {
                    Debug.Log("버튼 클릭");
                    Button btn = hit.collider.gameObject.GetComponent<Button>();
                    if (btn != null)
                    {
                        btn.Select();
                        btn.onClick.Invoke();
                    }
                }
            }
        }
    }
    public void SelectProduct_()
    {
        Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.blue, 10f); //씬에서 레이를 보이게 하는 용도(이거 없으면 안보임)
        if (Physics.Raycast(transform.position, RayDir, out hit, MaxDistance)) //(Ray 원점, Ray 방향, 충돌 감지할 RaycastHit, Ray 거리(길이))
        {
            Debug.Log("Ray충돌감지" + hit.transform.name);
            if (hit.collider.tag == "product")
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
                    name_text.text = productName;
                    price_text.text = FirebaseCtrl._Instance.productDict[productName].price.ToString();
                    UICtrl._Instance.OpenClose_Product_Info();
                    //파이어베이스에서 가격, 품절인지 아닌지 받아온다.
                    if (FirebaseCtrl._Instance.productDict[productName].soldOut.ToString() == "1")
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
                

                foreach (string s in ChangeSpace._Instance.spaceDict.Keys)
                {
                    string[] values;
                    ChangeSpace._Instance.spaceDict.TryGetValue(s, out values);

                    if (Array.Exists(values, x => x.Equals(hit.collider.gameObject.name)))
                    {
                        Debug.Log(s + "공간으로 이동");
                        //s 공간으로 이동
                        switch (s)
                        {
                            case "집": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.houseSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "롤러장": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.rollerSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "도로": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.roadSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "농구장": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.basketballSphere, Camera.main.transform.position, Quaternion.identity); break;
                            case "겨울": ViewCtrl._Instance._NewSphere = Instantiate(UICtrl._Instance.winterSphere, Camera.main.transform.position, Quaternion.identity); break;
                        }
                        UICtrl._Instance.shoppingCenter.SetActive(false);
                        ViewCtrl._Instance._CloneProduct = Instantiate(hit.collider.gameObject, this.transform.position + new Vector3(0.0f, 1.5f, -1.0f), Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f)));
                        ViewCtrl._Instance._CloneProduct.GetComponent<Renderer>().materials = new Material[1] { ViewCtrl._Instance._CloneProduct.GetComponent<Renderer>().material };
                        ViewCtrl._Instance.isNewSpace = true;
                        //1,2,3층 패널을 없애고
                        UICtrl._Instance.FloorPanel.SetActive(false);
                        //뒤로가기 버튼 보이게
                        UICtrl._Instance.BackToShoppingBtn.SetActive(true);
                        break;
                    }
                }

            }
            //버튼 속성이라면
            if (hit.collider.gameObject.GetComponent<Button>() != null)
            {
                Debug.Log("버튼 클릭");
                Button btn = hit.collider.gameObject.GetComponent<Button>();
                if (btn != null)
                {
                    btn.Select();
                    btn.onClick.Invoke();
                }
            }
        }
        
    }
    /*
        Debug.DrawRay(transform.position, transform.forward * MaxDistance, Color.blue, 10f); //씬에서 레이를 보이게 하는 용도(이거 없으면 안보임)
        if (Physics.Raycast(transform.position, RayDir, out hit, MaxDistance)) //(Ray 원점, Ray 방향, 충돌 감지할 RaycastHit, Ray 거리(길이))
        {
            Debug.Log("Ray충돌감지" + hit.transform.name);
            if (hit.collider.tag == "product")
            {
                Info_Pannel.gameObject.SetActive(true);
                Debug.Log("상품 선택");
                Debug.Log("클릭한 오브젝트 이름 : " + hit.transform.gameObject.name);
                if (FirebaseCtrl._Instance.productDict.ContainsKey(hit.transform.name))
                {
                    Debug.Log("품절인가? " + FirebaseCtrl._Instance.productDict[hit.transform.gameObject.name].soldOut.ToString());
                    name_text.text = hit.transform.name;
                    price_text.text = FirebaseCtrl._Instance.productDict[hit.transform.name].price.ToString();
                    // amount_text.text = FirebaseCtrl.productDict[hit.transform.name].amount.ToString();

                    //파이어베이스에서 가격, 품절인지 아닌지 받아온다.
                    if (FirebaseCtrl._Instance.productDict[hit.transform.gameObject.name].soldOut.ToString() == "1")
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

            }
            //버튼 속성이라면
            if (hit.collider.gameObject.GetComponent<Button>() != null)
            {
                Debug.Log("버튼 클릭");
                Button btn = hit.collider.gameObject.GetComponent<Button>();
                if (btn != null)
                {
                    btn.Select();
                    btn.onClick.Invoke();
                }
            }
        }

    }
    */
    //레이가 닿았는데 태그가 product면 빛나게
}