using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;    
    public float damage;    
    public float lifeTime = 3f;  

    private IObjectPool<GameObject> pool;  

    public void Launch(Vector3 direction, IObjectPool<GameObject> objectPool)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }

        pool = objectPool; 
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.OnDamage(damage, transform.position, -transform.forward);
            }

            ReturnToPool();
        }
        else if (!other.CompareTag("Player") && !other.CompareTag("Bullet"))
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (pool != null)
        {
            pool.Release(gameObject); // Object Pool에 반환
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}
