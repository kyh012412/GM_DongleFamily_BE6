using System.Runtime.InteropServices;
using UnityEngine;

public class Dongle : MonoBehaviour
{
    public bool isDrag; //기본값 false drag를 통해 true를 거쳐서 drop 후 다시 false

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(isDrag){
            // Vector3 mousePos = Input.mousePosition; // 스크린 좌표계
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // x축 경계 설정
            float border = 4.2f - transform.localScale.x /2f;
            mousePos.x = Mathf.Clamp(mousePos.x,-border,border);

            mousePos.y = 8;
            mousePos.z = 0;

            // transform.position = mousePos; // 너무 달라 붙으므로 lerp를 사용
            //현재위치, 목표위치, 따라가는 강도(0~1)
            transform.position = Vector3.Lerp(transform.position,mousePos,0.2f);
        }
    }

    public void Drag(){
        isDrag = true;
    }

    public void Drop(){
        isDrag = false;
        rigid.simulated = true;
    }
}
