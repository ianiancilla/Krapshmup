using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class Player : MonoBehaviour
{
    // configuration parameters
    [Header("Player stats")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float movementBoundaryMargin = 0f;
    [SerializeField] int health = 200;
    [SerializeField] bool invincible = false;

    [Header("Player projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPause = 0.2f;

    [Header("VFX")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float explosionDeathDelay = 1f;

    [Header("SFX")]
    [SerializeField] AudioClip fireSFX;
    [SerializeField] [Range(0, 1)] float fireVolume = 1f;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathVolume = 1f;
    [SerializeField] AudioClip damagedSFX;
    [SerializeField] [Range(0, 1)] float damagedVolume = 1f;

    // variables

    Coroutine firingCoroutine;

    // boundaries for player movement
    float xMin;
    float xMax;
    float yMin;
    float yMax;
    float xPadding;
    float yPadding;

    // cache
    GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        SetupMoveBoundaries();

        // cache
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }



    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            // istantiates a new laser and gives projectileSpeed velocity
            AudioSource.PlayClipAtPoint(fireSFX, Camera.main.transform.position, fireVolume);
            GameObject laser = Instantiate(laserPrefab,
                                transform.position,
                                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

            // waits before yielding new projectile
            yield return new WaitForSeconds(projectileFiringPause);
        }

    }

    private void Move()
    {
        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;  // TODO consider Raw
        float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;  // TODO consider Raw


        float newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        float newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetupMoveBoundaries()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        xPadding = renderer.bounds.size.x / 2 + movementBoundaryMargin;
        yPadding = renderer.bounds.size.y / 2 + movementBoundaryMargin;

        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xPadding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yPadding;

    }

    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        DamageDealer damageDealer = otherObject.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        HandleDamage(damageDealer);
    }

    private void HandleDamage(DamageDealer damageDealer)
    {
        var damage = damageDealer.GetDamage();
        health -= damage;
        gameSession.SubtractFromScore(damageDealer.GetDamage());

        damageDealer.Hit();
        AudioSource.PlayClipAtPoint(damagedSFX, Camera.main.transform.position, damagedVolume);
        if (health <= 0)
        {
            if (invincible) { return; }
            Die();
        }
    }

    private void Explosion()
    {
        var explosion = Instantiate(deathVFX,
                                    transform.position,
                                    Quaternion.identity) as GameObject;
        Destroy(explosion, explosionDeathDelay);
    }


    private void Die()
    {
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathVolume);
        Explosion();
        Destroy(gameObject);
        FindObjectOfType<SceneLoader>().LoadGameOver();
    }

}
