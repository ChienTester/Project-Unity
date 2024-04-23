using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : WeaponSpawner
{
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
            musicManager.PlaySFX(musicManager.bomClip);
            SpawnWeapon(Direction.Right);

            if (GetLevel() >= 2)
                SpawnWeapon(Direction.Left);

            if (GetLevel() >= 5)
                SpawnWeapon(Direction.Up);

            if (GetLevel() >= 7)
                SpawnWeapon(Direction.Down);

            yield return new WaitForSeconds(GetAttackSpeed());
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
