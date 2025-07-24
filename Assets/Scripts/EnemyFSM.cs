using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    // 적 캐릭터 상태 정의
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    // 적 캐릭터 현재 상태
    EnemyState m_State;

    // 플레이어 발견 거리
    public float findDistance = 8f;

    // 플레이어 트랜스폼
    Transform player;

    // 공격 가능 거리
    public float attackDistance = 2f;

    // 캐릭터 컨트롤러 컴포넌트
    CharacterController cc;

    // 이동 속도
    public float moveSpeed = 5f;

    // 기본 이동 속도 저장
    private float originalMoveSpeed;

    // 물에 닿았을 때 material 변수
    public Material waterMaterial;

    // 현재 시간
    float currentTime = 0;

    // 공격 주기 시간
    float attackDelay = 2f;

    // 적 캐릭터 공격력
    public int attackPower = 3;

    // 초기 위치와 회전값을 저장할 변수
    Vector3 originPos;
    Quaternion originRot;

    // 이동 가능 범위
    public float moveDistance = 20f;

    // 물 밖으로 나갔을 때 원래 material 변수
    private Material defaultMaterial;

    // 적 캐릭터 체력
    public int hp = 15;

    // 적 캐릭터 최대 체력
    int maxHp = 15;

    // 적 캐릭터 hp Slider 변수
    public Slider hpSlider;

    // 애니메이터 변수
    Animator anim;

    // 네비게이션 컴포넌트 변수
    NavMeshAgent smith;

    void Start()
    {
        // 시작할 때 적 캐릭터의 상태를 대기(Idle)로 설정한다.
        m_State = EnemyState.Idle;

        // 시작할 때 원래 이동 속도를 저장한다.
        originalMoveSpeed = moveSpeed;

        // 플레이어의 트랜스폼 컴포넌트를 받아온다
        player = GameObject.Find("Player").transform;

        // 캐릭터 컨트롤러 컴포넌트를 받아온다
        cc = GetComponent<CharacterController>();

        // 자신의 초기 위치와 회전값을 저장한다
        originPos = transform.position;
        originRot = transform.rotation;

        // 자신 게임오브젝트에서 애니메이터 컴포넌트를 받아온다
        anim = transform.GetComponentInChildren<Animator>();

        // 네비게이션 컴포넌트를 받아온다
        smith = GetComponent<NavMeshAgent>();

        defaultMaterial = GetComponentInChildren<SkinnedMeshRenderer>().material;

        smith.baseOffset = 0f;
    }

    void Update()
    {
        // 현재 상태를 체크하여 해당 상태의 행동을 수행한다.
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }

        // 현재 hp(%)를 hp 슬라이더의 value에 반영한다.
        // hpSlider.value = (float)hp / (float)maxHp;
    }

    // 물에 들어왔을 때 실행
    public void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.GetComponent<Water>() != null)
        {
            moveSpeed = originalMoveSpeed * 2;
            GetComponentInChildren<SkinnedMeshRenderer>().material = waterMaterial;
        }
    }

    // 물에서 나갔을 때 실행
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Water>() != null)
        {
            moveSpeed = originalMoveSpeed;
            GetComponentInChildren<SkinnedMeshRenderer>().material = defaultMaterial;
        }
    }

    void FixedUpdate()
   {
        // NavMeshAgent의 이동 속도를 설정한다.
        smith.speed = moveSpeed;

   }

    void Idle()
    {
        // 만약, 플레이어와의 거리가 발견 거리보다 가까우면 Move 상태로 전환한다.
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환: Idle -> Move");

            // 이동 애니메이션으로 전환한다
            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
        if (smith.isStopped)
        {
            smith.isStopped = false;
        }


        // 만약, 현재 위치가 초기 위치에서 이동 가능 범위를 벗어났다면...
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            // 상태를 Return 상태로 전환한다.
            m_State = EnemyState.Return;
            print("상태 전환: Move -> Return");
        }

        // 만약, 플레이어와의 거리가 공격 가능 거리보다 멀다면 플레이어를 향해 이동한다.
        else if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            // 네비게이션 에이전트의 정지 거리를 공격 가능 거리로 설정한다.
            smith.stoppingDistance = attackDistance;

            // 네비게이션 목적지를 플레이어의 위치로 설정한다.
            smith.destination = player.position;
        }
        // 그렇지 않다면, 상태를 Attack 상태로 전환한다.
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환: Move -> Attack");

            // 공격 시간을 공격 주기 시간으로 초기화한다.
            currentTime = attackDelay;

            // 공격 준비 애니메이션 실행
            anim.SetTrigger("MoveToAttackDelay");

            // 네비게이션 컴포넌트의 이동을 멈추고 경로를 초기화한다.
            smith.isStopped = true;
            smith.ResetPath();
        }
    }

    void Attack()
    {
        // 만약, 플레이어가 공격 가능 거리보다 가까이 있다면 플레이어를 공격한다.
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            // 공격 주기 시간이 지나면 플레이어를 공격한다.
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                // player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격");
                currentTime = 0;

                // 공격 애니메이션 실행
                anim.SetTrigger("StartAttack");
            }
        }
        // 그렇지 않다면, 상태를 Move 상태로 전환한다.
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환: Attack -> Move");
            currentTime = 0;

            // 이동 애니메이션 실행
            anim.SetTrigger("AttackToMove");
        }
    }

    // 플레이어의 컴포넌트에서 호출할 함수를 정의한다
    
    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }
    

    void Return()
    {
        if (smith.isStopped)
        {
            smith.isStopped = false;
        }
        // 만약, 초기 위치와의 거리가 0.1f 이상이라면 초기 위치로 이동한다.
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            // 네비게이션 목적지를 초기 위치로 설정한다.
            smith.destination = originPos;

            // 네비게이션 에이전트의 정지 거리를 0으로 설정한다.
            smith.stoppingDistance = 0;
        }
        // 그렇지 않다면, 자신의 위치를 초기 위치로 설정하고 상태를 대기 상태로 전환한다.
        else
        {
            // 네비게이션 컴포넌트의 이동을 멈추고 경로를 초기화한다.
            smith.isStopped = true;
            smith.ResetPath();

            // 위치와 회전을 초기 상태로 되돌린다.
            transform.position = originPos;
            transform.rotation = originRot;

            // hp를 다시 회복한다.
            hp = maxHp;

            m_State = EnemyState.Idle;
            print("상태 전환: Return -> Idle");

            // 대기 애니메이션으로 전환하는 트리거를 호출한다.
            anim.SetTrigger("MoveToIdle");
        }
    }

    // 데미지를 받는 함수
    public void HitEnemy(int hitPower)
    {
        // 만약, 이미 피해를 받고 있거나 죽은 상태이거나 복귀 중인 상태라면 아무 처리도 하지 않고 함수를 종료한다.
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }

        // 플레이어의 공격력만큼 적 캐릭터의 체력을 감소시킨다.
        hp -= hitPower;

        // 네비게이션 컴포넌트의 이동을 멈추고 경로를 초기화한다.
        smith.isStopped = true;
        smith.ResetPath();

        // 적 캐릭터의 체력이 0보다 크면 피해 상태로 전환한다.
        if (hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any state -> Damaged");

            // 피해 애니메이션을 실행한다.
            anim.SetTrigger("Damaged");
            Damaged();
        }
        // 그렇지 않다면, 죽음 상태로 전환한다.
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any state -> Die");

            // 죽음 애니메이션을 실행한다.
            anim.SetTrigger("Die");
            Die();
        }
    }
    

    void Damaged()
    {
        // 피해 상태를 처리하기 위한 코루틴을 실행한다.
        StartCoroutine(DamageProcess());
    }

    // 피해 처리용 코루틴 함수
    IEnumerator DamageProcess()
    {
        // 피해 상태 시간 동안 기다린다.
        yield return new WaitForSeconds(1f);

        // 상태를 이동 상태로 전환한다.
        m_State = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }

    // 죽음 처리 함수
    void Die()
    {
        // 실행 중인 모든 코루틴을 중지한다.
        StopAllCoroutines();

        // 죽음 상태를 처리하기 위한 코루틴을 실행한다.
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        // 캐릭터 컨트롤러 컴포넌트를 비활성화한다.
        cc.enabled = false;

        // 2초 동안 기다린 후 게임오브젝트를 삭제한다.
        yield return new WaitForSeconds(2f);
        print("사망!");
        Destroy(gameObject);
    }
}