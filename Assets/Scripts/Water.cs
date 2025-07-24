using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private float waterDrag; // �� �� ���׷�
    private float originDrag; // �� �� ������ ���� ���׷�

    private Color originColor; // �� �� ������ ���� ���� Fog ����
    [SerializeField] private Color originNightColor;  // �� �� ������ ���� ���� Fog ����

    [SerializeField] private Color waterColor; // ���� �� �� Fog ����
    [SerializeField] private Color waterNightColor; // ���� �� �� Fog ����

    [SerializeField] private float waterFogDensity; // ���� �� �� Ź�� ����
    [SerializeField] private float waterNightFogDensity; // ���� �� �� Ź�� ����

    private float originFogDensity; // �� �� ������ ���� Ź�� ����
    [SerializeField] private float originNightFogDensity; // �� �� ������ ���� Ź�� ����

    [SerializeField] private float breatheTime;
    private float currentBreatheTime;

    [SerializeField] private GameObject thePlayer;

    [SerializeField] private string sound_WaterIn;
    [SerializeField] private string sound_WaterOut;
    [SerializeField] private string sound_WaterBreathe;
    void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = thePlayer.GetComponent<Rigidbody>().drag;
    }

    void Update()
    {
        if (GameManager.isWater)
        {
            currentBreatheTime += Time.deltaTime;
            if (currentBreatheTime >= breatheTime)
            {
                currentBreatheTime = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("1������������");
            GetInWater(other);  // ���� ��
            

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
            GetOutWater(other);  // ������ ����
    }




    //���� �ٲٱ�    
    private void GetInWater(Collider _player)
    {
        GameManager.isWater = true;
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;
        RenderSettings.fogColor = waterColor;
        Debug.Log(RenderSettings.fogColor);
        RenderSettings.fogDensity = waterFogDensity;
        Debug.Log("�ƾƤ���������������������");

    }

    private void GetOutWater(Collider _player)
    {

        if (GameManager.isWater)
        {
            GameManager.isWater = false;
            _player.transform.GetComponent<Rigidbody>().drag = originDrag;
            RenderSettings.fogColor = originColor;
            RenderSettings.fogDensity = originFogDensity;
        }
    }
}

