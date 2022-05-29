using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private AudioSource playerAudio;

    
    public GameObject player;
    public Slider lifeBar;
    public GameObject gameoverUI;
    public Text scoreText;
    public AudioClip deadClip;
    public GameObject[] enemy;
    public GameObject[] enemyObject;
    public Transform[] spwanPoint;
    private float maxSpawnDelay;
    private float curSpawnDelay;
    public int ascore = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        curSpawnDelay += Time.deltaTime;
        
        if(curSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = 1.5f;
            curSpawnDelay = 0.0f;
        }

        if (player.GetComponent<Player>().life <= 0)
        {
            gameoverUI.SetActive(true);
        }

        /*
        if(enemy[0].GetComponent<Enemy>().)
        {
            score(10);
        }
        */

    }

    /*
    void score(int newScore)
    {
        ascore += newScore;
        scoreText.text = "Score " + ascore;
    }
    */

    void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, 1);
        int ranPoint = Random.Range(0, 5);
        Instantiate(enemyObject[ranEnemy], 
            spwanPoint[ranPoint].position, 
            spwanPoint[ranPoint].rotation);

        Enemy enemyLogic = enemy[ranEnemy].GetComponent<Enemy>();
        enemyLogic.target = player;
    }

    public void Dead()
    {
        PlayerLife();
        DeadSound();
        player.SetActive(false);
    }

    public void DeadSound()
    {
        playerAudio.clip = deadClip;
        playerAudio.Play();
    }

    public void Respawn()
    {
        Invoke("RespawnExe", 1.0f);
    }

    void RespawnExe()
    {
        player.transform.position = Vector3.left * 5.0f;
        player.SetActive(true);
        Invoke("RespawnEnd", 3.0f);
    }

    void RespawnEnd()
    {
        player.GetComponent<Player>().respawn = false;
    }

    public void PlayerLife()

    {
        float life = player.GetComponent<Player>().life;
        lifeBar.value -= 0.2f;
    }

}
    