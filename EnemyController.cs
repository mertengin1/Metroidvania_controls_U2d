using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private bool facingRight = true;
    private Animator animator;
    private Transform player;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    private float lastAttackTime;
    public int maxHealth = 50;  // Düşman için maksimum sağlık değeri
    private int currentHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < attackRange)
        {
            AttackPlayer();
        }
        else
        {
            MoveAlongWaypoints();
        }
    }

    void MoveAlongWaypoints()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector2 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        if (direction.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && facingRight)
        {
            Flip();
        }

        animator.SetFloat("Speed", Mathf.Abs(direction.x));

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void AttackPlayer()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            animator.SetTrigger("Attack");
            player.GetComponent<PlayerHealth>().TakeDamage(10); // Burada hata alıyorsan, PlayerHealth scriptini kontrol et
            lastAttackTime = Time.time;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        if (direction.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && facingRight)
        {
            Flip();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Düşmanın ölmesiyle ilgili işlemler burada yapılabilir
        Debug.Log("Enemy died");
        Destroy(gameObject); // Düşmanı sahneden kaldır
    }
}
