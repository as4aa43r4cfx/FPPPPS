using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{

    Vector3 moveVec;

    // �̵� �ӵ� ����
    public float moveSpeed = 7f;

    // ĳ���� ��Ʈ�ѷ� ����
    CharacterController cc;

    // �߷� ����
    float gravity = -20f;

    float waterGravity = -30f;

    // ���� �ӷ� ����
    public float yVelocity = 0;

    // ������ ����
    public float jumpPower = 10f;

    public float upSwim = 0.6f;

    // ���� ���� ����
    public bool isJumping = false;

    // �÷��̾� ü�� ����
    public int hp = 20;

    // �ִ� ü�� ����
    int maxHp = 20;

    // hp �����̴� ����
    public Slider hpSlider;

    // Hit ȿ�� ������Ʈ
    public GameObject hitEffect;

    // �ִϸ����� ����
    Animator anim;

    void Start()
    {
        // ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();

        // �ִϸ����� �޾ƿ���
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {

        if (yVelocity < -5f)
        {
            yVelocity = -5f; // 공중에서 낙하 제한
        }


        // ���� ���°� "���� ��" ������ ������ ���� �����ϰ� �Ѵ�.
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        // Ű���� <W>, <A>, <S>, <D> ��ư�� �Է��ϸ� ĳ���͸� �� �������� �̵���Ű�� �ʹ�.
        // Ű���� <Space> ��ư�� �Է��ϸ� ĳ���͸� �������� ������Ű�� �ʹ�.

        // 1. ������� �Է��� �޴´�.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. �̵� ������ �����Ѵ�.

        Vector3 dir = new Vector3(h, 0, v);
        moveVec = new Vector3(h, 0, v).normalized;
        dir = dir.normalized;

        // �̵� ������ Ʈ���� ȣ���ϰ� ������ ũ�� ���� �Ѱ��ش�.
        anim.SetFloat("MoveMotion", dir.magnitude);

        // 2-1. ���� ī�޶� �������� ������ ��ȯ�Ѵ�.
        dir = Camera.main.transform.TransformDirection(dir);

        if (!GameManager.isWater)
        {
            // 2-2. ����, ���� ���̾���, �ٽ� �ٴڿ� �����ߴٸ�...
            if (isJumping && cc.collisionFlags == CollisionFlags.Below)
            {
                // ���� �� ���·� �ʱ�ȭ�Ѵ�.
                isJumping = false;
                // ĳ���� ���� �ӵ��� 0���� �����.
                yVelocity = -0;
            }

            // 2-3. ����, Ű���� <Space> ��ư�� �Է��߰�, ������ �� �� ���¶��...
            if (Input.GetButtonDown("Jump") && !isJumping)
            {
                // ĳ���� ���� �ӵ��� �������� �����ϰ� ���� ���·� �����Ѵ�.
                yVelocity = jumpPower;
                isJumping = true;
            }

            // 2-4. ĳ���� ���� �ӵ��� �߷� ���� �����Ѵ�.
            yVelocity += gravity * Time.deltaTime;
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                yVelocity = upSwim;
            }
            else
            {
                yVelocity += waterGravity * Time.deltaTime;
            }
        }


        dir.y = yVelocity;

        // 3. �̵� �ӵ��� ���� �̵��Ѵ�.
        cc.Move(dir * moveSpeed * Time.deltaTime);

        // 4. ���� �÷��̾� hp(%)�� hp �����̴��� value�� �ݿ��Ѵ�.
        //hpSlider.value = (float)hp / (float)maxHp;
        if (GameManager.isWater) // �� �� �� ��
        {
            
            if (yVelocity < -3f)
            {
                yVelocity = -3f; // 공중에서 낙하 제한
            }
            moveSpeed = 4f;
        }
        else
        {
            moveSpeed = 7f;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 12.5f;
            jumpPower = 3f;
        }
        else
        {
            moveSpeed = 7f;
            jumpPower = 4f;
        }
        if (moveVec != Vector3.zero)
        {
            // 이동 방향을 바라보게 회전
            transform.LookAt(transform.position + moveVec);
            if (GameManager.isWater)
            {
                Quaternion lookRot = Quaternion.LookRotation(moveVec);
                Vector3 euler = lookRot.eulerAngles;
                euler.x = 90f;
                transform.rotation = Quaternion.Euler(euler);
            }

            // y 회전에 -60도 추가
            Vector3 rot = transform.eulerAngles;
            rot.y -= 150f;
            transform.eulerAngles = rot;
        }
    }

    // �÷��̾��� �ǰ� �Լ�
    public void DamageAction(int damage)
    {
        // ���ʹ��� ���ݷ¸�ŭ �÷��̾��� ü���� ��´�.
        hp -= damage;

        // ����, �÷��̾��� ü���� 0���� ũ�� �ǰ� ȿ���� ����Ѵ�.
        if (hp > 0)
        {
            // �ǰ� ����Ʈ �ڷ�ƾ�� �����Ѵ�.
            StartCoroutine(PlayHitEffect());
        }
    }

    // �ǰ� ȿ�� �ڷ�ƾ �Լ�
    IEnumerator PlayHitEffect()
    {
        // 1. �ǰ� UI�� Ȱ��ȭ �Ѵ�.
        hitEffect.SetActive(true);

        // 2. 0.3�ʰ� ����Ѵ�.
        yield return new WaitForSeconds(0.3f);

        // 3. �ǰ� UI�� ��Ȱ��ȭ �Ѵ�.
        hitEffect.SetActive(false);
    }
}