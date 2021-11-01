using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Management;

public class PlayerCtrl : MonoBehaviour
{
    public float MoveSpeed = 3.0f;
    private float v;
    float deltaS;

    private Vector2 lastPosInput;
    //private GameObject MainCamera_;
    private float gravity = 19.6f;

    // Start is called before the first frame update
    void Start()
    {
        deltaS = MoveSpeed * Time.deltaTime;
        lastPosInput = Vector2.zero;
        // MainCamera_ = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Move(); //이 함수 내용은 세희가 바꿔쥬쎼용~~ 지금 이게 너무 안좋아서,,

        var EulerY = Camera.main.transform.eulerAngles.y;
        //   Debug.Log(Camera.main.transform.eulerAngles.y);
        transform.position += Quaternion.Euler(0, EulerY, 0) * (new Vector3(lastPosInput.x, 0, lastPosInput.y) * deltaS);


    }

    //Move함수 내용은 세희가 바꿔쥬쎼용~~ 지금 이게 너무 안좋아서,,
    void Move()
    {


        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.Translate(Vector3.back * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (v < 0) //뒤로 갈때 회전반대
                this.transform.Rotate(0, 10 * MoveSpeed * Time.deltaTime, 0);
            else
                this.transform.Rotate(0, -10 * MoveSpeed * Time.deltaTime, 0);

        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (v < 0) //뒤로갈때 회전반대
                this.transform.Rotate(0, -10 * MoveSpeed * Time.deltaTime, 0);
            else
                this.transform.Rotate(0, 10 * MoveSpeed * Time.deltaTime, 0);
        }
        
    }

   public void OnMove_(InputAction.CallbackContext context)
    {
        Debug.Log("컨트롤러 이용 움직임");
        var v2 = context.ReadValue<Vector2>();
        if ((System.Math.Abs(v2.x) < 0.5) && (System.Math.Abs(v2.y) < 0.5))
        {
            lastPosInput.x = 0;
            lastPosInput.y = 0;
        }
        else
        {
            lastPosInput = context.ReadValue<Vector2>().normalized;
        }
   
    }

}
