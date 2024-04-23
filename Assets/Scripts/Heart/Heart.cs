using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private MusicManager musicManager;
    public void SetMusicManager(MusicManager manager)
    {
        musicManager = manager;
    }
    [SerializeField] HeartData heartData;
    [SerializeField] PlayerMove player;
    Rigidbody2D rigidbody;
    Coroutine coroutine;
    Sprite sprite;
    float bloodValue;
    bool isCollided;
    int speed;

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
    internal virtual void Initialize()
    {
        sprite = heartData.GetSprite();
        bloodValue = heartData.GetBloodValue();
        GetComponent<SpriteRenderer>().sprite = GetSprite();
        rigidbody = GetComponent<Rigidbody2D>();
        isCollided = false;
        speed = 7;
    }

    public float GetBloodValue()
    {
        return bloodValue;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 3)
        {
            musicManager.PlaySFX(musicManager.heartClip);
            if (coroutine == null)
                coroutine = StartCoroutine(HeartAnimation());

            if (isCollided)
                GetHeart();
        }
    }

    IEnumerator HeartAnimation()
    {
        rigidbody.AddForce(new Vector2(player.GetHorizontal(), player.GetVertical()) * speed, ForceMode2D.Impulse);

        yield return new WaitForSecondsRealtime(0.4f);

        isCollided = true;

        StartCoroutine(Disable());

        while (true)
        {
            Vector2 direction = player.transform.position - transform.position;
            rigidbody.MovePosition(rigidbody.position + direction.normalized * Time.deltaTime * speed++);

            yield return null;
        }
    }

    void GetHeart()
    {
        ObjectPooling.ReturnObject(gameObject, heartData.GetHeartType());
        gameObject.SetActive(false);
        player.GetComponent<Player>().IncreaseHealth((int)bloodValue);
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(0.5f);

        GetHeart();
    }
}
