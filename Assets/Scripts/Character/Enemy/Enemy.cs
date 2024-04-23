using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] CrystalData.CrystalType crystalType;
    [SerializeField] HeartData.HeartType heartType;
    Shader shaderGUItext;
    Shader shaderSpritesDefault;
    EnemyMove enemyMove;
    Rigidbody2D rigidbody;

    void Awake()
    {
        Initialize();
    }

    void OnEnable()
    {
        InitHealthPoint();
        GetComponent<CapsuleCollider2D>().enabled = true;
        spriteRenderer.material.shader = shaderSpritesDefault;
        hitCoroutine = null;
    }

    protected override void Initialize()
    {
        base.Initialize();

        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
        enemyMove = GetComponent<EnemyMove>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void ReduceHealthPoint(int damage)
    {
        base.ReduceHealthPoint(damage);

        if (hitCoroutine == null)
            hitCoroutine = StartCoroutine(UnderAttack());

        KnockBack();
        FloatingDamage(damage);
    }

    protected override IEnumerator UnderAttack()
    {
        spriteRenderer.material.shader = shaderGUItext;

        yield return new WaitForSeconds(0.2f);

        spriteRenderer.material.shader = shaderSpritesDefault;
        hitCoroutine = null;
    }

    void FloatingDamage(int damage)
    {
        GameObject damageText = ObjectPooling.GetObject("damage");
        TextMeshPro textMesh = damageText.GetComponent<TextMeshPro>();
        RectTransform rectTransform = damageText.GetComponent<RectTransform>();

        textMesh.text = damage.ToString();

        rectTransform.position = new Vector3(transform.position.x + 1f, transform.position.y + 1f, rectTransform.position.z);

        damageText.SetActive(true);
    }

    void KnockBack()
    {
        rigidbody.AddForce(enemyMove.GetDirection() * -2f, ForceMode2D.Impulse);
    }

    public override void Die()
    {
        EnemySpawner.GetInstance().IncreaseKillCount();

        int dropChance = Random.Range(0, 100);

        if (dropChance < 5) // Nếu số ngẫu nhiên từ 0 đến 99 nhỏ hơn 5, tức là có 5% khả năng
        {
            DropHeart();
        }
        else // 95%
        {
            DropCrystral();
        }

        StartCoroutine(DieAnimation());
    }

    void DropCrystral()
    {
        GameObject crystal = ObjectPooling.GetObject(crystalType);

        crystal.transform.position = transform.position;

        crystal.SetActive(true);
    }

    void DropHeart()
    {
        GameObject heart = ObjectPooling.GetObject(heartType);

        heart.transform.position = transform.position;

        heart.SetActive(true);
    }

    protected override IEnumerator DieAnimation()
    {
        GetAnimator().SetBool("die", true);
        GetComponent<EnemyMove>().isDead = true;
        GetComponent<CapsuleCollider2D>().enabled = false;

        yield return new WaitForSeconds(0.1f);

        ObjectPooling.ReturnObject(gameObject, GetCharacterType());
        GetAnimator().SetBool("die", false);
        gameObject.SetActive(false);
    }
}
