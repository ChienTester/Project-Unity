using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [SerializeField] GameObject whipPlayer;
    [SerializeField] GameObject gunPlayer;
    [SerializeField] Slider hpSlider;
    [SerializeField] ParticleSystem bleeding;
    [SerializeField] GameObject GameOverWindow;
    //[SerializeField] GameObject GameMap_1Window;
    //[SerializeField] GameObject GameMap_2Window;
    static Player instance;
    float attackSpeed;
    float expAdditional;
    int luck;
    bool isColliding;
    CharacterData characterData;
    private MusicManager musicManager;
    public void SetMusicManager(MusicManager manager)
    {
        musicManager = manager;
    }
    private Player() { }

    void Awake()
    {
        Initialize();
    }
    void Start()
    {      
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        if (musicManager != null)
        {
            SetMusicManager(musicManager);
        }
    }
    protected override void Initialize()
    {
        base.Initialize();
        UpdatePlayerAppearance();
        GameOverWindow.SetActive(false);
        instance = this;
        attackSpeed = 100f;
        expAdditional = 100f;
        luck = 0;
        hpSlider.maxValue = GetHealthPoint();
        hpSlider.value = GetHealthPoint();
        isColliding = false;
        //GameMap_2Window.SetActive(false);
    }
    //void Start()
    //{
    //    StartCoroutine(SwitchMaps());
    //}
    //IEnumerator SwitchMaps()
    //{
    //    // Đợi 10 giây
    //    yield return new WaitForSeconds(10f);

    //    // Ẩn map_1 và hiển thị map_2
    //    GameMap_2Window.SetActive(false);
    //    GameMap_1Window.SetActive(true);
    //}
    public static Player GetInstance()
    {
        return instance;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetExpAdditional()
    {
        return expAdditional;
    }

    public int GetLuck()
    {
        return luck;
    }

    public void DecreaseAttackSpeed(float value)
    {
        attackSpeed -= value;
    }

    public void IncreaseExpAdditional(float value)
    {
        expAdditional += value;
    }

    public void IncreaseLuck(int value)
    {
        luck += value;
    }

    public override void Die()
    {
        musicManager.musicAudioSource.Stop();
        musicManager.PlaySFX(musicManager.dieClip);
        PlayerMove.GetInstance().isDead = true;
        StartCoroutine(DieAnimation());
    }

    protected override IEnumerator DieAnimation()
    {
        GetAnimator().SetBool("Death", true);
        yield return new WaitForSeconds(1f);
        GameOverWindow.SetActive(true);
        Time.timeScale = 0f;
    }

    void GetFirstWeapon()
    {
        switch (characterData.GetCharacterType())
        {
            case CharacterData.CharacterType.WhipPlayer:
                Inventory.GetInstance().AddWeapon(WeaponData.WeaponType.Whip);
                break;
            case CharacterData.CharacterType.GunPlayer:
                Inventory.GetInstance().AddWeapon(WeaponData.WeaponType.Gun);
                break;
        }
    }


    public override void ReduceHealthPoint(int damage)
    {
        if (!PlayerMove.GetInstance().isDead)
        {
            base.ReduceHealthPoint(damage);

            hpSlider.value = GetHealthPoint();
            bleeding.Play();




            isColliding = true;

            if (hitCoroutine == null)
                hitCoroutine = StartCoroutine(UnderAttack());
        }
    }

    public void IncreaseHealth(int amount)
    {
        if (!PlayerMove.GetInstance().isDead)
        {
            base.RecoverHealthPoint(amount);

            hpSlider.value = GetHealthPoint();

            isColliding = false;
        }
    }

    protected override IEnumerator UnderAttack()
    {
        spriteRenderer.color = Color.red;

        do
        {
            isColliding = false;
            yield return new WaitForSeconds(0.2f);
        }
        while (isColliding);

        spriteRenderer.color = Color.white;
        hitCoroutine = null;
    }

    // Phương thức để đặt dữ liệu của nhân vật
    public void SetCharacterData(CharacterData data)
    {
        characterData = data;
        // Sau khi đặt dữ liệu, cần cập nhật giao diện của người chơi
        UpdatePlayerAppearance();
    }

    // Phương thức cập nhật giao diện của người chơi dựa trên dữ liệu của nhân vật
    void UpdatePlayerAppearance()
    {
        // Kiểm tra nếu dữ liệu nhân vật không null
        if (characterData != null)
        {
            // Dựa vào loại nhân vật trong dữ liệu để cập nhật giao diện của người chơi
            switch (characterData.GetCharacterType())
            {
                case CharacterData.CharacterType.WhipPlayer:
                    whipPlayer.SetActive(true);
                    gunPlayer.SetActive(false);
                    break;
                case CharacterData.CharacterType.GunPlayer:
                    whipPlayer.SetActive(false);
                    gunPlayer.SetActive(true);
                    break;
                default:
                    break;
            }
            GetFirstWeapon();
        }
    }
}
