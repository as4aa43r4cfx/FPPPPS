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

    // �߻� ���� ���ݷ�
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
        //1.���콺 ������ ��ư�� �Է� �޴´�
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