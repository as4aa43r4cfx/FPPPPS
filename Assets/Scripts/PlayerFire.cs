using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject firePosition;

    public GameObject bombFactory;

    public float throwPower = 15f;

    public GameObject bulletEffect;

    ParticleSystem ps;

    // �߻� ���� ���ݷ�
    public int weaponPower = 5;

    Animator anim;




    void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();

        anim = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        //1.���콺 ������ ��ư�� �Է� �޴´�
        if (Input.GetMouseButtonDown(1))
        {
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;

            Rigidbody rb = bomb.GetComponent<Rigidbody>();


            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }

        //���콺 ���� ��ư���Է¹޴´�
        if (Input.GetMouseButtonDown(0))
        {
            if (anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hitInfo = new RaycastHit();

            // ���̸� �߻��ϰ�, ���� �ε��� ��ü�� ������...
            if (Physics.Raycast(ray, out hitInfo))
            {
                // ���� ���̿� �ε��� ����� ���̾ "Enemy"��� ������ �Լ��� �����Ѵ�.
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }
                // �׷��� �ʴٸ�, ���̿� �ε��� ������ �ǰ� ����Ʈ�� �÷����Ѵ�.
                else
                {
                    // �ǰ� ����Ʈ�� ��ġ�� ���̰� �ε��� �������� �̵���Ų��.
                    bulletEffect.transform.position = hitInfo.point;

                    // �ǰ� ����Ʈ�� forward ������ ���̰� �ε��� ������ ���� ���Ϳ� ��ġ��Ų��.
                    bulletEffect.transform.forward = hitInfo.normal;

                    // �ǰ� ����Ʈ�� �÷����Ѵ�.
                    ps.Play();
                }
            }
        }
    }
}