using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    // ���� ����Ʈ ������ ����
    public GameObject bombEffect;

    // ����ź ������
    public int attackPower = 10;

    // ���� ȿ�� �ݰ�
    public float explosionRadius = 15f;

    // �浹���� ���� ó��
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("bomb");
        
        // ���� ȿ�� �ݰ� ������ ���̾ "Enemy"�� ��� ���� ������Ʈ���� Collider ������Ʈ�� �迭�� �����Ѵ�.
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 10);
        Debug.Log(cols);


        // ����� Collider �迭�� �ִ� ��� ���ʹ̿��� ����ź �������� �����Ѵ�.
        for (int i = 0; i < cols.Length; i++)
        {
            Debug.Log(cols[i]);
            cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
        }

        // ����Ʈ �������� �����Ѵ�.
        GameObject eff = Instantiate(bombEffect);

        // ����Ʈ �������� ��ġ�� ����ź ������Ʈ �ڽ��� ��ġ�� �����ϴ�.
        eff.transform.position = transform.position;

        // �ڱ� �ڽ��� �����Ѵ�.
        Destroy(gameObject);
    }
}