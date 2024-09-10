using UnityEngine;

public class Attacker : MonoBehaviour
{
    public BulletPreset bulletStatistics;

    GameObject bulletObject;

    public bool isInEditor;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(isInEditor) return;
        
        GameObject model = bulletStatistics.model;
        bulletObject = Instantiate(model, transform.position, transform.rotation);
        bulletObject.transform.localScale = bulletObject.transform.localScale * bulletStatistics.size;

        Bullet bullet = bulletObject.GetComponent<Bullet>();
        Rigidbody rb = bulletObject.GetComponent<Rigidbody>();

        //Custom shader and color

        Transform player = FindAnyObjectByType<Defensive>().transform;

        if(bulletStatistics.shootAtPlayer)
        {
            Vector3 bulletShootDirection = player.position - transform.position;

            bulletShootDirection = bulletStatistics.speed * bulletShootDirection;
            rb.linearVelocity = new Vector3(bulletShootDirection.x, transform.position.y, bulletShootDirection.z);
        } else {
            Vector3 bulletShootDirection = bulletStatistics.EndPosition - transform.position;

            bulletShootDirection = bulletStatistics.speed * bulletShootDirection;
            rb.linearVelocity = new Vector3(bulletShootDirection.x, transform.position.y, bulletShootDirection.z);
        }

        bullet.damage = bulletStatistics.damage;

        Invoke(nameof(KillBullet),bulletStatistics.lifeTime);
    }
    void KillBullet()
    {
        Destroy(bulletObject);
    }
}
