using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;

    //캐릭터 컨트롤러 변수
    CharacterController cc;

    //중력 변수
    float gravity = -20f;

    //수직 속력 변수
    float yVelocity = 0;

    //점프력 변수
    public float JumpPower = 10f;

    //점프 상태 변수
    public bool isJumping = false;

    // 플레이어 체력 변수
    public int hp = 20;

    int MaxHp = 20;

    public Slider hpSlider;

    public GameObject hitEffect;

    private void Start()
    {
        //캐릭터 컨트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();
    }
    
    // Update is called once per frame
    void Update()
    {

        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        //w,a,s,d키를 누르면 캐릭터를 그 방향으로 이동시키고 싶다
        //spacebar키를 누르면 캐릭터를 수직으로 점프시키고 싶다

        //1.입력을받는다
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //2.이동 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        //2-1.메인 카메라를 기준으로 방향을 변환한다
        dir = Camera.main.transform.TransformDirection(dir);

        hpSlider.value = (float)hp / (float)MaxHp;

        if (cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                //점프 전 상태로 초기화한다
                isJumping = false;
                yVelocity = 0;

            }
            
        }

        if (Input.GetButtonDown("Jump")&& !isJumping)
        {
            yVelocity = JumpPower;
            isJumping=true;
        }

        //2-2. 캐릭터 수직 속도에 중력값을 적용한다
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;


        //3.이동 속도에 맞춰 이동한다.
        cc.Move(dir * moveSpeed * Time.deltaTime);

    }
    public void DamageAction(int damage)
    {
        hp -= damage;
        // 만일, 플레이어의 체력이 0보다 크면 피격 효과를 출력한다.
        if (hp > 0)
        {
            // 피격 이펙트 코루틴을 시작한다.
            StartCoroutine(PlayHitEffect());
        }
    }

    // 피격 효과 코루틴 함수
    IEnumerator PlayHitEffect()
    {
        hitEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hitEffect.SetActive(false);
    }
}
