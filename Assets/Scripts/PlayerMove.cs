using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;

    //ĳ���� ��Ʈ�ѷ� ����
    CharacterController cc;

    //�߷� ����
    float gravity = -20f;

    //���� �ӷ� ����
    float yVelocity = 0;

    private void Start()
    {
        //ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();
    }
    
    // Update is called once per frame
    void Update()
    {
        //w,a,s,dŰ�� ������ ĳ���͸� �� �������� �̵���Ű�� �ʹ�

        //1.�Է����޴´�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //2.�̵� ������ �����Ѵ�.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        //2-1.���� ī�޶� �������� ������ ��ȯ�Ѵ�
        dir = Camera.main.transform.TransformDirection(dir);
        //2-2. ĳ���� ���� �ӵ��� �߷°��� �����Ѵ�
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;


        //3.�̵� �ӵ��� ���� �̵��Ѵ�.
        cc.Move(dir * moveSpeed * Time.deltaTime);

    }
}
