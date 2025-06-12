using UnityEngine;

// 플레이어의 생존 스탯을 관리하는 클래스
public class PlayerStatus : MonoBehaviour
{
    [Header("스탯 최댓값")]
    [SerializeField] private float maxHealth = 100f; // 최대 체력
    [SerializeField] private float maxHunger = 100f; // 최대 배고픔
    [SerializeField] private float maxThirst = 100f; // 최대 목마름

    [Header("스탯 감소 속도")]
    [SerializeField] private float hungerDecayRate = 1f; // 배고픔 감소 속도 (초당)
    [SerializeField] private float thirstDecayRate = 2f; // 목마름 감소 속도 (초당)

    [Header("현재 스탯")]
    [SerializeField] private float playerHealth; // 현재 체력
    [SerializeField] private float playerHunger; // 현재 배고픔
    [SerializeField] private float playerThirst; // 현재 목마름

    public float Health => playerHealth; // 현재 체력 (읽기 전용)
    public float Hunger => playerHunger; // 현재 배고픔 (읽기 전용)
    public float Thirst => playerThirst; // 현재 목마름 (읽기 전용)

    public float MaxHealth => maxHealth; // 최대 체력 (읽기 전용)
    public float MaxHunger => maxHunger; // 최대 배고픔 (읽기 전용)
    public float MaxThirst => maxThirst; // 최대 목마름 (읽기 전용)


    private void Start()
    {
        playerHealth = maxHealth; // 시작 시 체력 초기화
        playerHunger = maxHunger; // 시작 시 배고픔 초기화
        playerThirst = maxThirst; // 시작 시 목마름 초기화
    }

    private void Update()
    {
        UpdateStatus(); //생존 스텟 업데이트 처리
    }

    private void UpdateStatus()
    {
        playerHunger -= hungerDecayRate * Time.deltaTime; //배고픔 감소
        playerThirst -= thirstDecayRate * Time.deltaTime;//목마름 감소

        playerHunger = Mathf.Clamp(playerHunger, 0f, maxHunger); //배고픔 제한
        playerThirst = Mathf.Clamp(playerThirst, 0f, maxThirst); //목마름 제한

        //조건문 배고픔과 목마름이 모두 0일때
        if (playerHunger <= 0f || playerThirst <= 0f)
        {
            playerHealth -= 5f * Time.deltaTime; //체력 감소
            playerHealth = Mathf.Clamp(playerHealth, 0f, maxHealth); //체력 제한
        }


        //조건문 플레이어 체력이 0과 같거나 작을 때
        if (playerHealth == 0f)
        {
            Die();//사망
        }
    }

    private void Die()
    {
        EventBus.PublishVoid("OnPlayerDie"); //사망 이벤트 발생
        // TODO : FSM 상태 전환, UI 표시 등 사망 처리
    }

    public void Heal(float amount)
    {
        playerHealth = Mathf.Clamp(playerHealth + amount, 0f, maxHealth); // 체력 회복
    }

    public void Eat(float amount)
    {
        playerHunger = Mathf.Clamp(playerHunger + amount, 0f, maxHunger); // 배고픔 회복
    }

    public void Drink(float amount)
    {
        playerThirst = Mathf.Clamp(playerThirst + amount, 0f, maxThirst); // 목마름 회복
    }
}
