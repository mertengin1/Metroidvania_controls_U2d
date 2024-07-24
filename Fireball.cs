using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f; // Ateş topunun hızı
    public int damage = 10; // Ateş topunun vereceği hasar
    public float lifetime = 5f; // Ateş topunun ömrü (saniye cinsinden)

    void Start()
    {
        Destroy(gameObject, lifetime); // Ateş topunu belirtilen süre sonra yok et
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed; // Ateş topunun hareket yönü
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(gameObject); // Ateş topunu yok et
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject); // Ateş topunu yok et
        }
    }
}
