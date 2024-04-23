using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipSpawner : WeaponSpawner
{
    [SerializeField] Animator whipAnimator;
    private MusicManager musicManager;
    public void SetMusicManager(MusicManager manager)
    {
        musicManager = manager;
    }
    protected override IEnumerator StartAttack()
    {
        while (true)
        {
            UpdateAttackPower();
            UpdateAttackSpeed();
            if (musicManager != null && musicManager.whipClip != null)
                musicManager.PlaySFX(musicManager.whipClip);
            SpawnWeapon(Direction.Self);

            yield return new WaitForSeconds(0.1f);

            if (GetLevel() >= 2)
                SpawnWeapon(Direction.Opposite);

            yield return new WaitForSeconds(GetAttackSpeed());
        }
    }

    public override void LevelUp()
    {
        switch (GetLevel())
        {
            case 3:
                IncreaseAttackPower(5);
                break;
            case 4:
                IncreaseAttackPower(5);
                IncreaseAdditionalScale(10f);
                break;
            case 5:
                IncreaseAttackPower(5);
                DecreaseAttackSpeed(10f);
                whipAnimator.speed -= whipAnimator.speed * 10 / 100f;
                break;
            case 6:
                IncreaseAttackPower(5);
                IncreaseAdditionalScale(10f);
                break;
            case 7:
                IncreaseAttackPower(10);
                DecreaseAttackSpeed(10f);
                IncreaseAdditionalScale(10f);
                whipAnimator.speed -= whipAnimator.speed * 10 / 100f;
                break;
        }
    }
    void Start()
    {
        MusicManager musicManager = FindObjectOfType<MusicManager>();
        if (musicManager != null)
        {
            SetMusicManager(musicManager);
        }
    }
}
