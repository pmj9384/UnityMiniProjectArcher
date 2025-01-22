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
        
        
        ReturnToPool();
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
           
            damageable.OnDamage(damage, transform.position, -transform.forward);
      
  
        }

  
    }
}


    private void ReturnToPool()
    {
        if (pool != null)
        {
            pool.Release(gameObject); // 오브젝트 풀에 반환
        }
        else
        {
            Destroy(gameObject);  // 풀 객체가 없다면 삭제
        }
    }

}
