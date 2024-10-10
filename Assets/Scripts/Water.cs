using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private float waterDrag; // 물 속 저항력
    private float originDrag; // 물 밖 세상의 원래 저항력

    private Color originColor; // 물 밖 세상의 낮의 원래 Fog 색깔
    [SerializeField] private Color originNightColor;  // 물 밖 세상의 밤의 원래 Fog 색깔

    [SerializeField] private Color waterColor; // 낮의 물 속 Fog 색깔
    [SerializeField] private Color waterNightColor; // 밤의 물 속 Fog 색깔

    [SerializeField] private float waterFogDensity; // 낮의 물 속 탁한 정도
    [SerializeField] private float waterNightFogDensity; // 낮의 물 속 탁한 정도

    private float originFogDensity; // 물 밖 세상의 낮의 탁한 정도
    [SerializeField] private float originNightFogDensity; // 물 밖 세상의 밤의 탁한 정도

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
            Debug.Log("1차차ㅏㅏㅏㅏ");
            GetInWater(other);  // 물에 들어감
            

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
            GetOutWater(other);  // 물에서 나옴
    }




    //여기 바꾸기    
    private void GetInWater(Collider _player)
    {
        GameManager.isWater = true;
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;
        RenderSettings.fogColor = waterColor;
        Debug.Log(RenderSettings.fogColor);
        RenderSettings.fogDensity = waterFogDensity;
        Debug.Log("아아ㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏㅏ");

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

