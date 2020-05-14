using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // CONFIG VARIABLES
    [Header("Enemy stats")]
    [SerializeField] int health = 100;
    [SerializeField] int score = 100;

    [Header("Projectile stats")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float minShotInterval = 0.2f;
    [SerializeField] float variationShotInterval = 0.7f;
    

    [Header("VFX")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float explosionDeathDelay = 1f;

    [Header("SFX")]
    [SerializeField] AudioClip fireSFX;
    [SerializeField] [Range(0, 1)] float fireVolume = 0.3f;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0,1)] float deathVolume = 1f;

    // variables
    float shotTimerCounter;


    // Start is called before the first frame update
    void Start()
    {
        defineShotInterval();
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void OnTriggerEnter2D(Collider2D otherObject)
    {
        DamageDealer damageDealer = otherObject.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        HandleDamage(damageDealer);
    }

    private void HandleDamage(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathVolume);
        FindObjectOfType<GameSession>().AddToScore(score);
        Explosion();
        Destroy(gameObject);
    }

    private void defineShotInterval()
    {
        if (variationShotInterval != 0f)
        {
            shotTimerCounter = Random.Range(       // random value in case there is a possible variation in shot interval
                    minShotInterval,
                    minShotInterval + variationShotInterval);
        }
        else
        {
            shotTimerCounter = minShotInterval;
        }
    }

    private void CountDownAndShoot()
    {
        shotTimerCounter -= Time.deltaTime;
        if (shotTimerCounter <= 0f)
        {
            Fire();
            defineShotInterval();
        }
    }

    private void Fire()
    {
        if (projectilePrefab)
        {
            AudioSource.PlayClipAtPoint(fireSFX, Camera.main.transform.position, fireVolume);
            var projectile = Instantiate(projectilePrefab,
                                        transform.position,
                                        Quaternion.identity) as GameObject;
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -(projectileSpeed));
        }
    }

    private void Explosion()
    {
        var explosion = Instantiate(deathVFX,
                                    transform.position,
                                    Quaternion.identity) as GameObject;
        Destroy(explosion, explosionDeathDelay);
    }

}
