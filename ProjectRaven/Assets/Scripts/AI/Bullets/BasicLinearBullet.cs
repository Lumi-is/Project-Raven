using UnityEngine;

public class BasicLinearBullet : MonoBehaviour, IBullet
{

    public bool shootAtPlayer;

    [SerializeField] GameObject model;

    [SerializeField] float speed;

    [SerializeField] float damage;

    [SerializeField] Vector3 targetPos;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {     
        Rigidbody rb = GetComponent<Rigidbody>();

        //Custom shader and color

        Transform player = FindAnyObjectByType<HealthManager>().transform;

        if(shootAtPlayer)
        {
            Vector3 bulletShootDirection = player.position - transform.position;

            bulletShootDirection = speed * bulletShootDirection.normalized;
            rb.linearVelocity = new Vector3(bulletShootDirection.x, transform.position.y, bulletShootDirection.z);
        } else {
            Vector3 bulletShootDirection = targetPos - transform.position;

            bulletShootDirection = speed * bulletShootDirection;
            rb.linearVelocity = new Vector3(bulletShootDirection.x, transform.position.y, bulletShootDirection.z);
        }
    }

    public void OnBeatChange()
    {
        Debug.Log("V1 ULTRAKILL :D");
    }  

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HealthManager playerScript = other.gameObject.GetComponent<HealthManager>();
            playerScript.TakeDamage(damage);
        }
    }
}
