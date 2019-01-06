public class LocalPlayer : Entity
{
    protected vp_PlayerEventHandler m_Player = null;
    protected vp_PlayerEventHandler Player
    {
        get
        {
            if (m_Player == null)
                m_Player = (vp_PlayerEventHandler)transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
            return m_Player;
        }
    }

    private void OnEnable()
    {
        if (Player != null)
            Player.Register(this);
    }

    private void OnDisable()
    {
        if (Player != null)
            Player.Unregister(this);
    }

    //vp_FPInput 传来的消息
    private void OnStart_Attack()
    {
        entityAnimator.Attack();
        weaponHandle.Fire();
    }
}
