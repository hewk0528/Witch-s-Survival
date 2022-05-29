using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;

    public int health;
    public GameObject target;
    public GameObject bullet;
    private float attackSpeed = 1.0f;
    private float curDelay;
    private float speed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        speed = Random.Range(2.0f, 4.0f);
        rigid.velocity = Vector2.left * speed;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        Reload();
    }

    void Shoot()
    {
        if (curDelay >= attackSpeed)
        {
            GameObject bullet = Instantiate(this.bullet, transform.position + Vector3.left, transform.rotation);
            Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
            Vector3 dirVec;
            if (target != null)
            {
                dirVec = target.transform.position - transform.position;
            }
            else
            {
                target = GameObject.Find("Player");
                dirVec = target.transform.position - transform.position;
            }
            rigidbody.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
            curDelay = 0;
        }
    }

    void Reload()
    {
        curDelay += Time.deltaTime;
    }

    void OnHit(int dmg)
    {
        DecreaseSprite();
        Invoke("IncreaseSprite", 0.1f);

        health -= dmg;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void DecreaseSprite()
    {
        Color color = spriteRenderer.color;
        color.a -= 0.25f;
        spriteRenderer.color = color;
    }

    void IncreaseSprite()
    {
        Color color = spriteRenderer.color;
        color.a += 0.25f;
        spriteRenderer.color = color;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.damage);

            Destroy(collision.gameObject);
        }
    }

}
