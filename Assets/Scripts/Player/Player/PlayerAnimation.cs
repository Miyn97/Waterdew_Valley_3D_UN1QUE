using UnityEngine;

// 애니메이션 파라미터 제어 클래스
public class PlayerAnimation : MonoBehaviour
{
    private Vector2 flowDirection = Vector2.zero;
    private float directionLerpSpeed = 8f;
    private Animator animator;

    private readonly int hashIsMoving = Animator.StringToHash("IsMoving");
    private readonly int hashIsJumping = Animator.StringToHash("IsJumping");
    private readonly int hashIsRunning = Animator.StringToHash("IsRunning");
    private readonly int hashIsFishing = Animator.StringToHash("IsFishing");
    private readonly int hashIsBuilding = Animator.StringToHash("IsBuilding");
    private readonly int hashIsCrafting = Animator.StringToHash("IsCrafting");
    private readonly int hashIsAttacking = Animator.StringToHash("IsAttacking");
    private readonly int hashIsDead = Animator.StringToHash("IsDead");
    private readonly int hashIsSwimming = Animator.StringToHash("IsSwimming");
    private readonly int hashHorizontal = Animator.StringToHash("Horizontal");
    private readonly int hashVertical = Animator.StringToHash("Vertical");

    private readonly int hashLookAround = Animator.StringToHash("LookAround");
    private readonly int hashRelaxed = Animator.StringToHash("Relaxed");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetDirection(0f, 0f);
    }

    public void UpdateFlowDirection()
    {
        if (!IsAnimatorReady()) return;

        flowDirection = Vector2.Lerp(flowDirection, flowDirection, Time.deltaTime * directionLerpSpeed);
        animator.SetFloat(hashHorizontal, flowDirection.x);
        animator.SetFloat(hashVertical, flowDirection.y);
    }

    private bool IsAnimatorReady()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        return animator != null && animator.isActiveAndEnabled && animator.runtimeAnimatorController != null;
    }

    public void SetMove(bool isMoving)
    {
        if (!IsAnimatorReady()) return;
        animator.SetBool(hashIsMoving, isMoving);
    }

    public void SetJump(bool isJumping)
    {
        if (!IsAnimatorReady()) return;
        animator.SetBool(hashIsJumping, isJumping);
    }

    public void SetRun(bool isRunning)
    {
        if (!IsAnimatorReady()) return;
        animator.SetBool(hashIsRunning, isRunning);
    }

    public void SetFishing(bool isFishing)
    {
        if (!IsAnimatorReady()) return;
        animator.SetBool(hashIsFishing, isFishing);
    }

    public void SetBuilding(bool isBuilding)
    {
        if (!IsAnimatorReady()) return;
        animator.SetBool(hashIsBuilding, isBuilding);
    }

    public void SetCrafting(bool isCrafting)
    {
        if (!IsAnimatorReady()) return;
        animator.SetBool(hashIsCrafting, isCrafting);
    }

    public void SetAttacking(bool isAttacking)
    {
        if (!IsAnimatorReady()) return;
        animator.SetBool(hashIsAttacking, isAttacking);
    }

    public void SetDead(bool isDead)
    {
        if (!IsAnimatorReady()) return;
        animator.SetBool(hashIsDead, isDead);
    }

    public void SetSwimming(bool isSwimming)
    {
        if (!IsAnimatorReady()) return;
        animator.SetBool(hashIsSwimming, isSwimming);
    }

    public void SetDirection(float horizontal, float vertical)
    {
        Vector2 target = new Vector2(horizontal, vertical);
        flowDirection = Vector2.ClampMagnitude(target, 1f);
    }

    public void TriggerLookAround()
    {
        if (!IsAnimatorReady()) return;
        animator.SetTrigger(hashLookAround);
    }

    public void TriggerRelaxed()
    {
        if (!IsAnimatorReady()) return;
        animator.SetTrigger(hashRelaxed);
    }
}
