using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class EntityAnimator : MonoBehaviour
{
    // booleans
    //private int IsAttacking;
    //private int IsClimbing;
    //private int IsCrouching;
    //private int IsGrounded;
    private int IsMoving;
    //private int IsOutOfControl;
    //private int IsReloading;
    private int IsRunning;
    //private int IsSettingWeapon;
    //private int IsZooming;
    //private int IsFirstPerson;

    private Transform mtransform = null;
    public Transform Transform
    {
        get
        {
            if (mtransform == null)
                mtransform = transform;
            return mtransform;
        }
    }

    private vp_PlayerEventHandler player = null;
    public vp_PlayerEventHandler Player
    {
        get
        {
            if (player == null)
                player = (vp_PlayerEventHandler)transform.root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
            return player;
        }
    }

    private Animator animator;
    public Animator Animator
    {
        get
        {
            if (animator == null)
                animator = GetComponent<Animator>();
            return animator;
        }
    }

    private void OnEnable()
    {
        if (Player != null)
        {
            Player.Register(this);
        }
    }

    private void OnDisable()
    {
        if (Player != null)
        {
            Player.Unregister(this);
        }
    }

    private void InitHashIDs()
    {
        //// floats
        //ForwardAmount = Animator.StringToHash("Forward");
        //PitchAmount = Animator.StringToHash("Pitch");
        //StrafeAmount = Animator.StringToHash("Strafe");
        //TurnAmount = Animator.StringToHash("Turn");
        //VerticalMoveAmount = Animator.StringToHash("VerticalMove");

        //// booleans
        //IsAttacking = Animator.StringToHash("IsAttacking");
        //IsClimbing = Animator.StringToHash("IsClimbing");
        //IsCrouching = Animator.StringToHash("IsCrouching");
        //IsGrounded = Animator.StringToHash("IsGrounded");
        IsMoving = Animator.StringToHash("IsMoving");
        //IsOutOfControl = Animator.StringToHash("IsOutOfControl");
        //IsReloading = Animator.StringToHash("IsReloading");
        IsRunning = Animator.StringToHash("IsRunning");
        //IsSettingWeapon = Animator.StringToHash("IsSettingWeapon");
        //IsZooming = Animator.StringToHash("IsZooming");
        //IsFirstPerson = Animator.StringToHash("IsFirstPerson");

        //// triggers
        //StartClimb = Animator.StringToHash("StartClimb");
        //StartOutOfControl = Animator.StringToHash("StartOutOfControl");
        //StartReload = Animator.StringToHash("StartReload");

        //// enum indices
        //WeaponGripIndex = Animator.StringToHash("WeaponGrip");
        //WeaponTypeIndex = Animator.StringToHash("WeaponType");
    }

    private void Awake()
    {
        InitHashIDs();
    }

    private void Start()
    {

    }

    private void LateUpdate()
    {
        UpdateAnimator();
    }

    /// <summary>
    /// updates variables on the mecanim animator object
    /// </summary>
    private void UpdateAnimator()
    {
        // --- booleans used to transition between blend states ---
        // TODO: these should be moved to event callbacks on the next optimization run

        //Animator.SetBool(IsRunning, Player.Run.Active && GetIsMoving());
        //Animator.SetBool(IsCrouching, Player.Crouch.Active);
        //Animator.SetInteger(WeaponTypeIndex, Player.CurrentWeaponType.Get());
        //Animator.SetInteger(WeaponGripIndex, Player.CurrentWeaponGrip.Get());
        //Animator.SetBool(IsSettingWeapon, Player.SetWeapon.Active);
        //Animator.SetBool(IsReloading, Player.Reload.Active);
        //Animator.SetBool(IsOutOfControl, Player.OutOfControl.Active);
        //Animator.SetBool(IsClimbing, Player.Climb.Active);
        //Animator.SetBool(IsZooming, Player.Zoom.Active);
        //Animator.SetBool(IsGrounded, m_Grounded);
        //Animator.SetBool(IsMoving, GetIsMoving());
        //Animator.SetBool(IsFirstPerson, Player.IsFirstPerson.Get());

        // --- floats used inside blend states to blend between animations ---

        //Animator.SetFloat(TurnAmount, m_CurrentTurn);
        //Animator.SetFloat(ForwardAmount, m_CurrentForward);
        //Animator.SetFloat(StrafeAmount, m_CurrentStrafe);
        //Animator.SetFloat(PitchAmount, (-Player.Rotation.Get().x) / 90.0f);

        //if (m_Grounded)
        //    Animator.SetFloat(VerticalMoveAmount, 0.0f);
        //else
        //{
        //    if (Player.Velocity.Get().y < 0.0f)
        //        Animator.SetFloat(VerticalMoveAmount, Mathf.Lerp(Animator.GetFloat(VerticalMoveAmount), -1.0f, Time.deltaTime * 3));
        //    else
        //        Animator.SetFloat(VerticalMoveAmount, Player.MotorThrottle.Get().y * 10.0f);
        //}
    }

    //private bool GetIsMoving()
    //{
    //    return (Vector3.Scale(Player.MotorThrottle.Get(), (Vector3.right + Vector3.forward))).magnitude >
    //        ((vp_Input.Instance.ControlType == 0) ? // use different sensitivity depending on input hardware
    //        0.01f   // keyboard (digital)
    //        :
    //        0.0f);  // joystick (analog)
    //}

    public void MovingStart()
    {
        Animator.SetBool(IsMoving, true);
    }

    public void MovingOver()
    {
        Animator.SetBool(IsMoving, false);
    }

    public void RunStart()
    {
        Animator.SetBool(IsRunning, true);
    }

    public void RunOver()
    {
        Animator.SetBool(IsRunning, false);
    }
}
