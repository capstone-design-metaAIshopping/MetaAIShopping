using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

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
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                Debug.Log(snapshot.ChildrenCount);
                // Do something with snapshot...
                foreach (DataSnapshot data in snapshot.Children)
                {
                    IDictionary productInfo = (IDictionary)data.Value;
                    Debug.Log("이름: " + productInfo["name"] + ", 품절유무 : " + productInfo["soldOut"]);
                    productDict.Add(productInfo["name"], new Info(productInfo["price"], productInfo["soldOut"]));
                }
                if (productDict.Count == 0)
                {
                    Debug.Log("데이터 안받아졌어요~~");
                }
                //Debug.Log(productDict["milk"].price);
            }
        });
    }

    IEnumerator DataToDict()
    {
        Debug.Log("DataToDict 시작");
        while (productDict.Count == 0) //데이터 안받아졌으면
        {
            Debug.Log("productDict.Count가 0임/");

            foreach (DataSnapshot data in snapshot.Children)
            {
                IDictionary productInfo = (IDictionary)data.Value;
                Debug.Log("이름: " + productInfo["name"] + ", 품절유무 : " + productInfo["soldOut"]);
                productDict.Add(productInfo["name"], new Info(productInfo["price"], productInfo["soldOut"]));
                yield return null;
            }
            if (productDict.Count == 0)
            {
                Debug.Log("데이터 안받아졌어요~~");
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

}
