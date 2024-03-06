using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    public GameObject Crosshair02_zoom;

    public GameObject Weapon01;
    public GameObject Weapon02;

    public GameObject Crosshair01;
    public GameObject Crosshair02;

    public GameObject Weapon01_R;
    public GameObject Weapon02_R;

    public GameObject firePosition;

    public GameObject bombFactory;

    public GameObject[] eff_Flash;

    public float throwPower = 15f;

    public GameObject bulletEffect;

    public Text wModeText;

    ParticleSystem ps;

    // 발사 무기 공격력
    public int weaponPower = 5;

    Animator anim;

    enum WeaponMode
    {
        Normal,
        Sniper
    }
    WeaponMode wMode;

    bool ZoomMode = false;




    void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();

        anim = GetComponentInChildren<Animator>();

        wMode = WeaponMode.Normal;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        //1.마우스 오른족 버튼을 입력 받는다
        if (Input.GetMouseButtonDown(1))
        {
            switch (wMode)
            {
                case WeaponMode.Normal:

                    GameObject bomb = Instantiate(bombFactory);
                    bomb.transform.position = firePosition.transform.position;

                    Rigidbody rb = bomb.GetComponent<Rigidbody>();

                    rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);

                    break;

                case WeaponMode.Sniper:
                    if (!ZoomMode)
                    {
                        Camera.main.fieldOfView = 15f;
                        ZoomMode = true;

                        Crosshair02_zoom.SetActive(true);
                        Crosshair02.SetActive(false);
                    }

                    else
                    {
                        Camera.main.fieldOfView = 60f;
                        ZoomMode = false;

                        Crosshair02_zoom.SetActive(false);
                        Crosshair02.SetActive(true);
                    }

                    break;

            }


            
        }

        //마우스 왼쪽 버튼을입력받는다
        if (Input.GetMouseButtonDown(0))
        {
            if (anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hitInfo = new RaycastHit();

            // 레이를 발사하고, 만일 부딪힌 물체가 있으면...
            if (Physics.Raycast(ray, out hitInfo))
            {
                // 만일 레이에 부딪힌 대상의 레이어가 "Enemy"라면 데미지 함수를 실행한다.
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }
                // 그렇지 않다면, 레이에 부딪힌 지점에 피격 이펙트를 플레이한다.
                else
                {
                    // 피격 이펙트의 위치를 레이가 부딪힌 지점으로 이동시킨다.
                    bulletEffect.transform.position = hitInfo.point;

                    // 피격 이펙트의 forward 방향을 레이가 부딪힌 지점의 법선 벡터와 일치시킨다.
                    bulletEffect.transform.forward = hitInfo.normal;

                    // 피격 이펙트를 플레이한다.
                    ps.Play();
                }
            }

            StartCoroutine(ShootEffectOn(0.05f));
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            wMode = WeaponMode.Normal;

            Camera.main.fieldOfView = 60f;

            wModeText.text = "Normal Mode";

            Weapon01.SetActive(true);
            Weapon02.SetActive(false);
            Crosshair01.SetActive(true);
            Crosshair02.SetActive(false);
            Weapon01_R.SetActive(true);
            Weapon02_R.SetActive(false);

            Crosshair02_zoom.SetActive(false);
            ZoomMode = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Sniper;

            wModeText.text = "Sniper Mode";

            Weapon01.SetActive(false);
            Weapon02.SetActive(true);
            Crosshair01.SetActive(false);
            Crosshair02.SetActive(true);
            Weapon01_R.SetActive(false);
            Weapon02_R.SetActive(true);
        }
    }

    IEnumerator ShootEffectOn(float duration)
    {
        
        int num = Random.Range(0, eff_Flash.Length - 1);
        
        eff_Flash[num].SetActive(true);
     
        yield return new WaitForSeconds(duration);
 
        eff_Flash[num].SetActive(false);
    }


}