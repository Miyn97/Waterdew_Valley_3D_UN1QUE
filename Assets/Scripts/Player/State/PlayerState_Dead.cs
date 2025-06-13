public class PlayerState_Dead : IState
{
    private readonly Player player;

    public PlayerState_Dead(Player player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.AnimatorWrapper.SetDead(true); // 죽음 애니메이션 (Animator에 SetDead 필요)
    }

    public void Update() { }

    public void FixedUpdate() { }

    public void Exit() { }
}
