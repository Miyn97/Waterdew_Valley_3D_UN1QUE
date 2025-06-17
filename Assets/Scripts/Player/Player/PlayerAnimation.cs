using UnityEngine;

// 애니메이션 파라미터 제어 클래스
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator; // Animator 컴포넌트 참조 캐싱용

    private readonly int hashIsMoving = Animator.StringToHash("IsMoving");   // 이동 상태 해시값
    private readonly int hashIsJumping = Animator.StringToHash("IsJumping"); // 점프 상태 해시값
    private readonly int hashIsRunning = Animator.StringToHash("IsRunning"); // 달리기 상태 해시값
    private readonly int hashIsFishing = Animator.StringToHash("IsFishing"); // 낚시 상태 해시값
    private readonly int hashIsBuilding = Animator.StringToHash("IsBuilding"); // 건축 상태 해시값
    private readonly int hashIsCrafting = Animator.StringToHash("IsCrafting"); // 조합 상태 해시값
    private readonly int hashIsAttacking = Animator.StringToHash("IsAttacking"); // 공격 상태 해시값
    private readonly int hashIsDead = Animator.StringToHash("IsDead"); // 사망 상태 해시값
    private readonly int hashIsSwimming = Animator.StringToHash("IsSwimming"); // 수영 상태 해시값 추가
    private readonly int hashHorizontal = Animator.StringToHash("Horizontal"); // 좌우 입력
    private readonly int hashVertical = Animator.StringToHash("Vertical");     // 전후 입력
    private void Awake()
    {
        animator = GetComponent<Animator>(); // Animator 컴포넌트 초기화
    }
    public void SetMove(bool isMoving)
    {
        animator.SetBool(hashIsMoving, isMoving); // "IsMoving" 파라미터 설정
    }
    public void SetJump(bool isJumping)
    {
        animator.SetBool(hashIsJumping, isJumping); // "IsJumping" 파라미터 설정
    }
    public void SetRun(bool isRunning)
    {
        animator.SetBool(hashIsRunning, isRunning); // "IsRunning" 파라미터 설정
    }
    public void SetFishing(bool isFishing)
    {
        animator.SetBool(hashIsFishing, isFishing); // "IsFishing" 파라미터 설정
    }
    public void SetBuilding(bool isBuilding)
    {
        animator.SetBool(hashIsBuilding, isBuilding); // "IsBuilding" 파라미터 설정
    }
    public void SetCrafting(bool isCrafting)
    {
        animator.SetBool(hashIsCrafting, isCrafting); // "IsCrafting" 파라미터 설정
    }
    public void SetAttacking(bool isAttacking)
    {
        animator.SetBool(hashIsAttacking, isAttacking); // "IsAttacking" 파라미터 설정
    }
    public void SetDead(bool isDead)
    {
        animator.SetBool(hashIsDead, isDead); // "IsDead" 파라미터 설정
    }
    private void EnsureInitialized()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }
    public void SetSwimming(bool isSwimming)
    {
        EnsureInitialized(); // null일 경우에만 초기화
        animator.SetBool(hashIsSwimming, isSwimming);
    }
    // 방향 파라미터 설정 (Blend Tree용)
    public void SetDirection(float horizontal, float vertical)
    {
        animator.SetFloat(hashHorizontal, horizontal); // "Horizontal" 파라미터 설정
        animator.SetFloat(hashVertical, vertical);     // "Vertical" 파라미터 설정
    }
}
