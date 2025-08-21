using UnityEngine;
using UnityEngine.UI;

public class Switch1 : MonoBehaviour
{
    [SerializeField] private GameObject Player;   // 플레이어 오브젝트
    [SerializeField] private Text actionText;     // UI 텍스트 (E/F 안내)
    private bool isOnBlock = false;               // 블럭 위에 있는지 체크
    private bool isBig = false;
    void Start()
    {
        actionText.gameObject.SetActive(false);   // 시작 시 꺼두기
    }

    void Update()
    {
        if (isOnBlock)  // 블럭 위에 있을 때만 실행
        {
            actionText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isBig)
                {
                    Player.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                    isBig = false;
                }
                else
                {
                    Player.transform.localScale = new Vector3(6f, 6f, 6f);
                    isBig = true;
                }
            }
        }
        else
        {
            actionText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            isOnBlock = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            isOnBlock = false;
        }
    }
}