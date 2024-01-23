using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;

    //ĳ���� ��Ʈ�ѷ� ����
    CharacterController cc;

    //�߷� ����
    float gravity = -20f;

    //���� �ӷ� ����
    float yVelocity = 0;

    //������ ����
    public float JumpPower = 10f;

    //���� ���� ����
    public bool isJumping = false;

    // �÷��̾� ü�� ����
    public int hp = 20;

    int MaxHp = 20;

    public Slider hpSlider;

    private void Start()
    {
        //ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();
    }
    
    // Update is called once per frame
    void Update()
    {
        //w,a,s,dŰ�� ������ ĳ���͸� �� �������� �̵���Ű�� �ʹ�
        //spacebarŰ�� ������ ĳ���͸� �������� ������Ű�� �ʹ�

        //1.�Է����޴´�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //2.�̵� ������ �����Ѵ�.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        //2-1.���� ī�޶� �������� ������ ��ȯ�Ѵ�
        dir = Camera.main.transform.TransformDirection(dir);

        hpSlider.value = (float)hp / (float)MaxHp;

        if (cc.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                //���� �� ���·� �ʱ�ȭ�Ѵ�
                isJumping = false;
                yVelocity = 0;

            }
            
        }

        if (Input.GetButtonDown("Jump")&& !isJumping)
        {
            yVelocity = JumpPower;
            isJumping=true;
        }

        //2-2. ĳ���� ���� �ӵ��� �߷°��� �����Ѵ�
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;


        //3.�̵� �ӵ��� ���� �̵��Ѵ�.
        cc.Move(dir * moveSpeed * Time.deltaTime);

    }
    public void DamageAction(int damage)
    {
        hp -= damage;
    }
}
