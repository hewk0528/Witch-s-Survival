using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Animator animator;
    private AudioSource playerAudio;
    private CircleCollider2D playerCollider;

    public GameManager manager;
    public AudioClip shootClip;
    public int life;
    public bool respawn;
    public GameObject bullet;
    public float speed;
    public float power;
    public float attackSpeed;
    private float curDelay;
    private bool isTouchTop, isTouchBottom, isTouchRight, isTouchLeft;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
        Reload();
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        if ((isTouchRight && x == 1) || (isTouchLeft && x == -1))
            x = 0;

        float y = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && y == 1) || (isTouchBottom && y == -1))
            y = 0;

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(x, y, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;        
    }

    void Shoot()
    {
        if (Input.GetKey(KeyCode.LeftControl) && curDelay >= attackSpeed)
        {
            switch (power)
            {
                case 1:
                    GameObject bullet = Instantiate(this.bullet, transform.position + Vector3.right, transform.rotation);
                    Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
                    rigidbody.AddForce(Vector3.right * 10, ForceMode2D.Impulse);
                    break;
                case 2:
                    GameObject bulletU = Instantiate(this.bullet, transform.position + Vector3.right + Vector3.up * 0.1f, transform.rotation);
                    GameObject bulletD = Instantiate(this.bullet, transform.position + Vector3.right + Vector3.down * 0.1f + Vector3.back, transform.rotation);
                    Rigidbody2D rigidbodyU = bulletU.GetComponent<Rigidbody2D>();
                    Rigidbody2D rigidbodyD = bulletD.GetComponent<Rigidbody2D>();
                    rigidbodyU.AddForce(Vector3.right * 10, ForceMode2D.Impulse);
                    rigidbodyD.AddForce(Vector3.right * 10, ForceMode2D.Impulse);
                    break;
                case 3:
                    GameObject bulletUU = Instantiate(this.bullet, transform.position + Vector3.right + Vector3.up * 0.15f, transform.rotation);
                    GameObject bulletMM = Instantiate(this.bullet, transform.position + Vector3.right + Vector3.right * 0.2f + Vector3.back, transform.rotation);
                    GameObject bulletDD = Instantiate(this.bullet, transform.position + Vector3.right + Vector3.down * 0.15f, transform.rotation);
                    Rigidbody2D rigidbodyUU = bulletUU.GetComponent<Rigidbody2D>();
                    Rigidbody2D rigidbodyMM = bulletMM.GetComponent<Rigidbody2D>();
                    Rigidbody2D rigidbodyDD = bulletDD.GetComponent<Rigidbody2D>();
                    rigidbodyUU.AddForce(Vector3.right * 10, ForceMode2D.Impulse);
                    rigidbodyMM.AddForce(Vector3.right * 10, ForceMode2D.Impulse);
                    rigidbodyDD.AddForce(Vector3.right * 10, ForceMode2D.Impulse);
                    break;
            }
            curDelay = 0;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetBool("ShootB", true);
            ShootAudio();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            animator.SetBool("ShootB", false);
        }
    }

    void Reload()
    {
        curDelay += Time.deltaTime;
    }

    void Gameover()
    {
        gameObject.SetActive(false);
        Cursor.visible = true;
    }

    void ShootAudio()
    {
        playerAudio.clip = shootClip;
        playerAudio.Play();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
            }
        }

        if ((collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet") && respawn == false)
        {
            
            if (life <= 1)
            {
                manager.PlayerLife();
                manager.DeadSound();
                Gameover();
                life -= 1;
            } else
            {
                life -= 1;
                respawn = true;
                manager.Dead();
                manager.Respawn();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
            }
        }
    }
}
