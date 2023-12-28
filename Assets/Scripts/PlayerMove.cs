using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;

    //캐릭터 컨트롤러 변수
    CharacterController cc;

    //중력 변수
    float gravity = -20f;

    //수직 속력 변수
    float yVelocity = 0;

    private void Start()
    {
        //캐릭터 컨트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();
    }
    
    // Update is called once per frame
    void Update()
    {
        //w,a,s,d키를 누르면 캐릭터를 그 방향으로 이동시키고 싶다

        //1.입력을받는다
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //2.이동 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        //2-1.메인 카메라를 기준으로 방향을 변환한다
        dir = Camera.main.transform.TransformDirection(dir);
        //2-2. 캐릭터 수직 속도에 중력값을 적용한다
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;


        //3.이동 속도에 맞춰 이동한다.
        cc.Move(dir * moveSpeed * Time.deltaTime);

    }
}
