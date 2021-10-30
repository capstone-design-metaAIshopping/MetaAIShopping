using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class UICtrl : MonoBehaviour
{
    public static UICtrl _Instance;
    [Header("Product Info")]
    bool open_Product_Info;
    public GameObject Product_Info_Panel;
    public int Price;
    public int Stock_cnt;
    public int Buy_cnt;

    [Header("Buy_List")]
    bool open_Buy_List;
    public GameObject Buy_List_Panel;

    [Header("Cart_List")]
    bool open_Cart_List;
    public GameObject Cart_List_Panel;

    [Header("Settings")]
    bool open_Setting;
    public GameObject Setting_Panel;
    public int BGM_value;
    public int EffectSound_value;

    [Header("Agent")]
    public GameObject Agent_Quest_Panel;
    public GameObject Agent_voice_Panel;
    [Tooltip("Text field to display the results of streaming.")]
    public Text ResultsField;
    private void Awake()
    {
        _Instance = this;
    }
    private void Start()
    {
        //다 닫고 시작
        Product_Info_Panel.SetActive(false);
        Buy_List_Panel.SetActive(false);
        Cart_List_Panel.SetActive(false);
        Setting_Panel.SetActive(false);
    }
    public void OpenClose_Product_Info()
    {
        if (open_Product_Info)
        {
            Product_Info_Panel.SetActive(false);
            open_Product_Info = false;
        }
        else
        {
            Product_Info_Panel.SetActive(true);
            open_Product_Info = true;
            //다른거 다 닫기
            Buy_List_Panel.SetActive(false);
            Cart_List_Panel.SetActive(false);
            Setting_Panel.SetActive(false);
        }
    }

    public void OpenClose_Buy_List()
    {
        if (open_Buy_List)
        {
            Buy_List_Panel.SetActive(false);
            open_Buy_List = false;
        }
        else
        {
            Buy_List_Panel.SetActive(true);
            open_Buy_List = true;
            //다른거 다 닫기
            Product_Info_Panel.SetActive(false);
            Cart_List_Panel.SetActive(false);
            Setting_Panel.SetActive(false);
        }
    }

    public void OpenClose_Cart_List()
    {
        if (open_Cart_List)
        {
            Cart_List_Panel.SetActive(false);
            open_Cart_List = false;
        }
        else
        {
            Cart_List_Panel.SetActive(true);
            open_Cart_List = true;
            //다른거 다 닫기
            Product_Info_Panel.SetActive(false);
            Buy_List_Panel.SetActive(false);
            Setting_Panel.SetActive(false);
        }
    }

    public void OpenClose_Setting_Panel()
    {
        if (open_Setting)
        {
            Setting_Panel.SetActive(false);
            open_Setting = false;
        }
        else
        {
            Setting_Panel.SetActive(true);
            open_Setting = true;
            //다른거 다 닫기
            Product_Info_Panel.SetActive(false);
            Buy_List_Panel.SetActive(false);
            Cart_List_Panel.SetActive(false);
        }
    }

    public void OpenClose_AQuest_Panel()
    {
        Agent_Quest_Panel.SetActive(!Agent_Quest_Panel.activeSelf);
    }
    public void OpenClose_Voice_Panel()
    {
        Agent_voice_Panel.SetActive(!Agent_voice_Panel.activeSelf);
        if (Agent_voice_Panel.activeSelf)
        {
            ResultsField.gameObject.SetActive(true);
        }
        else
        {
            ResultsField.gameObject.SetActive(false);
            ViewCtrl._Instance.transform.GetComponent<NavMeshAgent>().isStopped = true;
            ViewCtrl._Instance.transform.GetComponent<NavMeshAgent>().ResetPath();
            ViewCtrl._Instance.transform.GetComponent<LineRenderer>().enabled = false;
        }
    }
    //일시정지 취소(상품 정보 패널에서 뒤로가기 누르면)
    public void Click_BackBtn()
    {
        /*        //일시정지 풀리고
                if (RayScript.IsPause == true)
                {
                    RayScript.IsPause = false;
                    Time.timeScale = 1;
                }*/
        //다 닫기
        Product_Info_Panel.SetActive(false);
        Buy_List_Panel.SetActive(false);
        Cart_List_Panel.SetActive(false);
        Setting_Panel.SetActive(false);
    }



}
