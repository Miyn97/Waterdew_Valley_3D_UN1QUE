using UnityEngine;

// 애니메이션 파라미터 제어 클래스
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator; // Animator 컴포넌트 참조 캐싱용

    private readonly int hashIsMoving = Animator.StringToHash("IsMoving");   // 이동 상태 해시값
    private readonly int hashIsJumping = Animator.StringToHash("IsJumping"); // 점프 상태 해시값
    private readonly int hashIsRunning = Animator.StringToHash("IsRunning"); // 달리기 상태 해시값

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
}
