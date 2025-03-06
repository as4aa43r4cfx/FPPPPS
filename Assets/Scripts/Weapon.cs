using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{

    // 발사 무기 공격력
    public int weaponPower = 10;
    public float rate;
    public BoxCollider Area;
    public TrailRenderer trailEffect;

    void Start()
    {

    }

    void Update()
    {

    }
}


        /*
        // 마우스 왼쪽 버튼 입력을 받는다.
        if (Input.GetMouseButtonDown(0))
        {
            // 만일 이동 블랜드 트리 파라메터의 값이 0이라면, 공격 애니메이션을 실시한다.
            if (anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }

            // 레이를 생성하고 발사될 위치와 진행 방향을 설정한다.
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // 레이가 부딪힌 대상의 정보를 저장할 변수를 생성한다.
            RaycastHit hitInfo = new RaycastHit();

            // 레이를 발사하고, 만일 부딪힌 물체가 있으면...
            if (Physics.Raycast(ray, out hitInfo))
            {
                // 만일 레이에 부딪힌 대상의 레이어가 "Enemy"라면 데미지 함수를 실행한다.
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    Debug.Log("mmmmm");
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }
                // 그렇지 않다면, 레이에 부딪힌 지점에 피격 이펙트를 플레이한다.
                else
                {
                    // 피격 이펙트의 위치를 레이가 부딪힌 지점으로 이동시킨다.
                    bulletEffect.transform.position = hitInfo.point;

                    // 피격 이펙트의 forward 방향을 레이가 부딪힌 지점의 법선 벡터와 일치시킨다.
                    bulletEffect.transform.forward = hitInfo.normal;

                    // 피격 이펙트를 플레이한다.
                    ps.Play();
                }
            }
        */


