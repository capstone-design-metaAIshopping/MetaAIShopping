using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;
public class Info
{
    public object price;
    public object soldOut;
    public Info(object _price, object _soldOut)
    {
        this.price = _price;
        this.soldOut = _soldOut;
    }

}
public class FirebaseCtrl : MonoBehaviour
{
    public Dictionary<object, Info> productDict;
    public static FirebaseCtrl _Instance;
    DataSnapshot snapshot;
    private bool IsReceivedData;
    private void Awake()
    {
        _Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        productDict = new Dictionary<object, Info>();
        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance.GetReference("product").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("데이터 가져오기 실패");
                UICtrl._Instance.debugText.text += "\n파이어베이스 데이터 가져오기 실패";
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                Debug.Log(snapshot.ChildrenCount);
                // Do something with snapshot...
                foreach (DataSnapshot data in snapshot.Children)
                {
                    try
                    {
                        IDictionary productInfo = (IDictionary)data.Value;
                        Debug.Log("이름: " + productInfo["name"] + ", 품절유무 : " + productInfo["soldOut"]);
                        productDict.Add(productInfo["name"], new Info(productInfo["price"], productInfo["soldOut"]));
                        IsReceivedData = true;
                    }
                    catch(Exception ex)
                    {
                        Debug.Log(ex.ToString());
                    }
                    
                }
                if (productDict.Count == 0)
                {
                    Debug.Log("데이터 안받아졌어요~~");
                    UICtrl._Instance.debugText.text += "\n파이어베이스 안 받아짐";
                    IsReceivedData = false;
                }
                if (productDict.ContainsKey("water"))
                {
                    Debug.Log("영어로 받아졌어요~~");
                    UICtrl._Instance.debugText.text += "\n파이어베이스 안 받아짐";
                    IsReceivedData = false;
                    productDict.Clear();
                }
                if (!IsReceivedData)
                {
                    NoticeCtrl._Instance.CreateNotice("상품 데이터가 올바르게 업데이트되지 않았습니다.\n네트워크를 확인하고 다시 실행해주세요.", true);
                }
                //Debug.Log(productDict["milk"].price);
            }
        });
        InvokeRepeating("DataToDict", 3.0f ,3.0f );
    }
    private void DataToDict()
    {
        Debug.Log("DataToDict 시작");
        UICtrl._Instance.debugText.text += "\nDataToDict 시작";
        if (!IsReceivedData) //데이터 안받아졌으면
        {
            Debug.Log("productDict.Count가 0임/");

            foreach (DataSnapshot data in snapshot.Children)
            {
                IDictionary productInfo = (IDictionary)data.Value;
                Debug.Log("이름: " + productInfo["name"] + ", 품절유무 : " + productInfo["soldOut"]);
                productDict.Add(productInfo["name"], new Info(productInfo["price"], productInfo["soldOut"]));
            }
            if (productDict.Count == 0)
            {
                Debug.Log("데이터 안받아졌어요~~");
                IsReceivedData = false;
            }
            if (productDict.ContainsKey("water"))
            {
                Debug.Log("영어로 받아졌어요~~");
                IsReceivedData = false;
                productDict.Clear();
            }
            Debug.Log(IsReceivedData);
        }
        if (IsReceivedData)
        {
            CancelInvoke("DataToDict");
        }
  
    }

}
