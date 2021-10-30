using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class Product
{
    public static Material origin_mat;
    public static bool highlight_distance;
}*/
public class ObjectHighlight : MonoBehaviour
{
    public Material highlight; //외곽선 material
    public float highlight_distance = 2.0f;

    private Material origin_mat; //원래 material
    private GameObject Player;
    private bool highlight_bool; //외곽선이 있는지 없는지

    // Start is called before the first frame update
    void Start()
    {
        highlight_bool = false; //처음엔 외곽선 없다.
                                // highlight_distance = 3.0f;
        Player = GameObject.FindWithTag("Player");
        origin_mat = this.GetComponent<MeshRenderer>().material; //처음 material 저장
        this.GetComponent<Renderer>().materials = new Material[2] { origin_mat, origin_mat };
    }

    // Update is called once per frame
    void Update()
    {
        setOutline(getDistance());

    }
    //거리계산함수
    float getDistance()
    {
        float distance = Vector3.Distance(Player.transform.position, this.transform.position);

        return distance;
    }
    //외곽선 설정(거리가 3이하면)
    void setOutline(float distance)
    {
        //거리가 3이하이고 현재 외곽선 없으면
        if (distance < highlight_distance && !highlight_bool)
        {
            Debug.Log("불빛");
            //meshRenderer material을 외곽선,원래 mat으로 설정
            // 외곽선 mat이 먼저 있고 그 위에 원래 mat을 덮어 씌우는 형식
            this.GetComponent<MeshRenderer>().material = highlight;
            highlight_bool = true;
        }
        //거리가 3 이상이고 현재 외곽선 있으면
        else if (distance >= highlight_distance && highlight_bool)
        {
            this.GetComponent<MeshRenderer>().material = origin_mat;
            highlight_bool = false;
        }
    }
    /* public Material highlight; //외곽선 material
     public float highlight_distance = 3.0f;

     private Material origin_mat; //원래 material
     private GameObject[] Product;
     private bool[] highlight_bool; //외곽선이 있는지 없는지

     // Start is called before the first frame update
     void Start()
     {
        // highlight_distance = 3.0f;
         Product = GameObject.FindGameObjectsWithTag("product");
         highlight_bool = new bool[Product.Length];
         for (int i = 0; i < Product.Length; i++)
         {
            // Product[i] = new Product();
             highlight_bool[i] = false; //처음엔 외곽선 없다.
             origin_mat = Product[i].GetComponent<MeshRenderer>().material; //처음 material 저장
             Product[i].GetComponent<Renderer>().materials = new Material[2] { origin_mat, origin_mat };
         }

     }

     // Update is called once per frame
     void Update()
     {
         //setOutline(getDistance());
         for (int i = 0; i < Product.Length; i++)
         {
             setOutline(getDistance(Product[i]), Product[i], highlight_bool[i]);
         }
     }
     //거리계산함수
     float getDistance(GameObject gameObject)
     {
         float distance  = Vector3.Distance(gameObject.transform.position, this.transform.position);

         return distance;
     }
     //외곽선 설정(거리가 3이하면)
     void setOutline(float distance, GameObject gameObject, bool highlight_bool)
     {
         Debug.Log("distance: " + distance + ", highlight : " + highlight_bool);
         //거리가 3이하이고 현재 외곽선 없으면
         if (distance < highlight_distance && !highlight_bool)
         {
             Debug.Log("불빛");
             //meshRenderer material을 외곽선,원래 mat으로 설정
             // 외곽선 mat이 먼저 있고 그 위에 원래 mat을 덮어 씌우는 형식
             gameObject.GetComponent<MeshRenderer>().material = highlight;
             highlight_bool = true;
         }
         //거리가 3 이상이고 현재 외곽선 있으면
         else if(distance >= highlight_distance && highlight_bool)
         {
             gameObject.GetComponent<MeshRenderer>().material = origin_mat;
             highlight_bool = false;
         }
     }
    */
}
