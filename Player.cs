using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // config params
    [Header("Player")]
    [SerializeField] int health = 500;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float padding = 0.5f;

    [Header("Laser")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserSpeed = 10f;
    [SerializeField] float laserFiringPeriod = 0.2f;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] [Range(0, 2)] float shootSFXVolume = 0.3f;

    [Header("Explosion")]
    [SerializeField] GameObject particlesVFX;
    [SerializeField] float particlesDuration = 1f;
    [SerializeField] AudioClip destroySFX;
    [SerializeField] [Range(0, 2)] float destroySFXVolume = 2f;

    Coroutine firingCoroutine;

    // state variables
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetMoveBoundries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        FireLaser();
    }

    private void FireLaser()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            AudioSource.PlayClipAtPoint(shootSFX, Camera.main.transform.position, shootSFXVolume);

            yield return new WaitForSeconds(laserFiringPeriod);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetMoveBoundries()
    {
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
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
            DestroyPlayer();
        }
    }

    private void DestroyPlayer()
    {
        GameObject particles = Instantiate(particlesVFX, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(destroySFX, Camera.main.transform.position, destroySFXVolume);

        Destroy(particles, particlesDuration);
        Destroy(gameObject);

        FindObjectOfType<LevelLoader>().LoadGameOver();
    }

    public int GetHealth()
    {
        if (health <= 0)
        {
            return 0;
        }
        else
        {
            return health;
        }
    }
}
