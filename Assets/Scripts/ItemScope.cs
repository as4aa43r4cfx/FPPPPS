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
        // 인식 범위 내에 아이템이 없을 경우
        if (ItemsInScope.Count == 0)
        {
            Player.Instance.LetterF.SetActive(false);
        }
        // 인식 범위 내에 아이템이 하나라도 있을 경우
        else
        {
            Player.Instance.LetterF.SetActive(true);
            // F키를 누르면
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
