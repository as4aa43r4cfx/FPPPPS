using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class waterswitch2 : MonoBehaviour
{
    [SerializeField] private GameObject Switch;
    [SerializeField] private GameObject Water1;
    private float dist;
    [SerializeField]
    private Text actionText;


    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(Switch.transform.position, transform.position);
        if (dist < 4)
        {
            actionText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Vector3 temp = Water1.transform.position;
                temp.y = 0;
                Water1.transform.position = temp;
            }
        }
        else
        {
            actionText.gameObject.SetActive(false);
        }

    }
}
