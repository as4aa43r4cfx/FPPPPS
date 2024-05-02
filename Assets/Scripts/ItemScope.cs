using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScope : MonoBehaviour
{

    private void Awake()
    {
        ItemsInScope = new List<Item>();
    }

    private void Update()
    {
        // �ν� ���� ���� �������� ���� ���
        if (ItemsInScope.Count == 0)
        {
            Player.Instance.LetterF.SetActive(false);
        }
        // �ν� ���� ���� �������� �ϳ��� ���� ���
        else
        {
            Player.Instance.LetterF.SetActive(true);
            // FŰ�� ������
            if (Input.GetKeyDown(KeyCode.F))
            {
              
            }
        }
    }

    private void OnTriggerEnter3D(Collider3D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null)
            ItemsInScope.Add(item);
    }

    private void OnTriggerExit3D(Collider3D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null)
            ItemsInScope.Remove(item);
    }
}
