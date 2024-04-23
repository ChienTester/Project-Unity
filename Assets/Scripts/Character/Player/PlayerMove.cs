using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] GameObject Whip;
    [SerializeField] GameObject Gun;
    Vector3 Whipoff;
    Vector3 Gunoff;
    static PlayerMove instance;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Player character;
    float horizontal;
    float vertical;
    bool lookingLeft;
    public bool isDead;
    [HideInInspector]
    public Vector2 moveDir;
    void Awake()
    {
        character = GetComponent<Player>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lookingLeft = false;
        instance = this;
        isDead = false;
        if (Whip != null)
        {
            Whipoff = Whip.transform.localPosition;
        }
        if (Gun != null)
        {
            Gunoff = Gun.transform.localPosition;
        }
    }

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (!Level.GetIsLevelUpTime())
            {

                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = Input.GetAxisRaw("Vertical");
                moveDir = new Vector2(horizontal, vertical).normalized;
            }
            if (Mathf.Abs(horizontal) >= 0.7f && Mathf.Abs(vertical) >= 0.7f)
            {
                horizontal = Mathf.Clamp(horizontal, -0.7f, 0.7f);
                vertical = Mathf.Clamp(vertical, -0.7f, 0.7f);
            }

            if (horizontal != 0f || vertical != 0f)
            {
                animator.SetInteger("AnimState", 1);

                if (horizontal > 0f)
                {
                    spriteRenderer.flipX = false;
                    lookingLeft = false;
                }
                else if (horizontal < 0f)
                {
                    spriteRenderer.flipX = true;
                    lookingLeft = true;
                }
            }
            else
            {
                animator.SetInteger("AnimState", 0);
            }

            // Thay doi vi tri whip
            if (horizontal != 0f)
            {
                // Kiểm tra nếu con dao tồn tại
                if (Whip != null)
                {
                    // Cập nhật hướng của con dao dựa trên hướng flip của người chơi
                    Whip.transform.localScale = new Vector3(spriteRenderer.flipX ? -1 : 1, 1, 1);

                    // Cập nhật vị trí của con dao dựa trên vị trí ban đầu và hướng flip
                    Whip.transform.localPosition = new Vector3(spriteRenderer.flipX ? -Whipoff.x : Whipoff.x, Whipoff.y, Whipoff.z);
                }
            }
            // Thay doi vi tri gun
            if (horizontal != 0f)
            {
                // Kiểm tra nếu con dao tồn tại
                if (Gun != null)
                {
                    // Cập nhật hướng của con dao dựa trên hướng flip của người chơi
                    Gun.transform.localScale = new Vector3(spriteRenderer.flipX ? -1 : 1, 1, 1);

                    // Cập nhật vị trí của con dao dựa trên vị trí ban đầu và hướng flip
                    Gun.transform.localPosition = new Vector3(spriteRenderer.flipX ? -Gunoff.x : Gunoff.x, Gunoff.y, Gunoff.z);
                }
            }

            if (!isDead)
            {
                transform.Translate(Vector2.right * horizontal * character.GetSpeed() / 10f * Time.deltaTime);
                transform.Translate(Vector2.up * vertical * character.GetSpeed() / 10f * Time.deltaTime);
            }
        }
    }

    public static PlayerMove GetInstance()
    {
        return instance;
    }

    public bool GetLookingLeft()
    {
        return lookingLeft;
    }

    public float GetHorizontal()
    {
        return horizontal;
    }

    public float GetVertical()
    {
        return vertical;
    }
}
