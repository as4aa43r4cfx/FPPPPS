using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    [SerializeField] private GameObject Switchobj;
    [SerializeField] private GameObject Gate1;
    [SerializeField] private GameObject Gate2;
    private float dist;
    [SerializeField]
    private Text actionText;


    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(Switchobj.transform.position, transform.position);
        if (dist < 5.4f)
        {
            actionText.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Gate1.SetActive(!Gate1.activeSelf);
                Gate2.SetActive(!Gate2.activeSelf);
            }
        }
        else
        {
            actionText.gameObject.SetActive(false);
        }

    }
}
