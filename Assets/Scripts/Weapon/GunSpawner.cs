using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : WeaponSpawner
{
    private MusicManager musicManager;
    public void SetMusicManager(MusicManager manager)
    {
        musicManager = manager;
    }
    int effectNum = 3;
    const float spreadAngle = 15f;
    const float speed = 700f;
    const float delay = 0.07f;
    Vector2 lastDirection = Vector2.right;
    protected override IEnumerator StartAttack()
    {
        EnemySpawner enemySpawner = EnemySpawner.GetInstance();
        PlayerMove playerMove = PlayerMove.GetInstance();

        while (true)
        {
            UpdateAttackPower();
            UpdateAttackSpeed();
            musicManager.PlaySFX(musicManager.gunClip);
            if (enemySpawner.GetListCount() > 0)
            {
                for (int i = 0; i < effectNum; ++i)
                {
                    Vector2 mouseDirection = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
                    mouseDirection.Normalize();

                    // Tính toán góc thay đổi
                    float spreadAngleOffset = Random.Range(-spreadAngle, spreadAngle);

                    // Xoay hướng theo góc thay đổi
                    Quaternion spreadRotation = Quaternion.Euler(0, 0, spreadAngleOffset);

                    // Áp dụng xoay hướng cho hướng bắn
                    Vector2 modifiedDirection = spreadRotation * mouseDirection;

                    // Bắn vũ khí
                    SpawnWeapon(modifiedDirection);

                    yield return new WaitForSeconds(delay);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnWeapon(Vector2 direction)
    {
        GameObject weapon = ObjectPooling.GetObject(GetWeaponType());

        // Tính toán góc giữa vector hướng và vector (1, 0) để quay vũ khí đến hướng mong muốn
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Đặt vị trí của vũ khí
        weapon.transform.position = GetWeaponData().GetBasePosition();

        if (GetWeaponData().GetParent().Equals(WeaponData.Parent.Self))
            weapon.transform.position += Player.GetInstance().GetPosition();

        // Đặt kích thước của vũ khí
        weapon.transform.localScale = new Vector2(GetWeaponData().GetBaseScale().x * (GetAdditionalScale() / 100f), GetWeaponData().GetBaseScale().y * (GetAdditionalScale() / 100f));

        // Thiết lập các tham số của vũ khí
        weapon.GetComponent<Weapon>().SetParameters(GetWeaponData(), GetAttackPower(), GetInactiveDelay(), Direction.Self);

        // Quay vũ khí đến hướng mong muốn
        weapon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Kích hoạt vũ khí
        weapon.SetActive(true);

        // Thêm lực đẩy vào vũ khí theo hướng mong muốn
        weapon.GetComponent<Rigidbody2D>().AddForce(direction * speed, ForceMode2D.Force);
    }

    public override void LevelUp()
    {
        switch (GetLevel())
        {
            case 2:
                IncreaseAttackPower(10);
                break;
            case 3:
                DecreaseAttackSpeed(10f);
                break;
            case 4:
                IncreaseAdditionalScale(10f);
                effectNum++;
                break;
            case 5:
                DecreaseAttackSpeed(10f);
                break;
            case 6:
                IncreaseAttackPower(10);
                break;
            case 7:
                effectNum++;
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
