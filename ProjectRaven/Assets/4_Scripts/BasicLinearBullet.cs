using UnityEngine;

public class BasicLinearBullet : MonoBehaviour
{
    public BulletStats bulletStatistics;

    public bool shootAtPlayer;

    GameObject bulletObject;

    [SerializeField] GameObject model;

    [SerializeField] float speed;

    [SerializeField] Vector3 targetPos;


    public bool isInEditor;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(isInEditor) return;
        
        bulletObject = Instantiate(model, transform.position, transform.rotation);
        bulletObject.transform.localScale = bulletObject.transform.localScale * bulletStatistics.size;

        Rigidbody rb = bulletObject.GetComponent<Rigidbody>();

        //Custom shader and color

        Transform player = FindAnyObjectByType<Defensive>().transform;

        if(shootAtPlayer)
        {
            Vector3 bulletShootDirection = player.position - transform.position;

            bulletShootDirection = speed * bulletShootDirection;
            rb.linearVelocity = new Vector3(bulletShootDirection.x, transform.position.y, bulletShootDirection.z);
        } else {
            Vector3 bulletShootDirection = targetPos - transform.position;

            bulletShootDirection = speed * bulletShootDirection;
            rb.linearVelocity = new Vector3(bulletShootDirection.x, transform.position.y, bulletShootDirection.z);
        }

        Invoke(nameof(KillBullet),bulletStatistics.lifeTime);
    }

    

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Defensive>() != null)
        {
            Defensive playerScript = other.gameObject.GetComponent<Defensive>();
            playerScript.TakeDamage(bulletStatistics.dmg);
        }
    }

    void KillBullet()
    {
        Destroy(bulletObject);
    }
}
