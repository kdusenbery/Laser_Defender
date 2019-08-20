using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // config params
    [Header("Enemy")]
    [SerializeField] int health = 100;
    [SerializeField] int scoreValue = 100;
    
    [Header("Laser")]
    [SerializeField] GameObject laserPrefab;
    float shotCounter;
    [SerializeField] float laserSpeed = 10f;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] [Range(0, 2)] float shootSFXVolume = 0.3f;

    [Header("Explosion")]
    [SerializeField] GameObject particlesVFX;
    [SerializeField] float particlesDuration = 1f;
    [SerializeField] AudioClip destroySFX;
    [SerializeField] [Range(0, 2)] float destroySFXVolume = 2f;

    
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;

        if(shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -laserSpeed);
        AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSFXVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();

        if (!damageDealer) { return; }

        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();

        if (health <= 0)
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        GameObject particles = Instantiate(particlesVFX, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(destroySFX, Camera.main.transform.position, destroySFXVolume);

        Destroy(particles, particlesDuration);
        Destroy(gameObject);

        FindObjectOfType<GameSession>().AddToScore(scoreValue);
    }
}
