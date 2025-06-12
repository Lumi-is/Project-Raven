using System.Collections;
using UnityEngine;

public class AOEBullet : MonoBehaviour, IBullet
{
    [SerializeField] int warningTimer;
    [SerializeField] float damage;
    [SerializeField] Collider hitbox;
    [SerializeField] Renderer visuals;

    void Awake()
    {
        hitbox.enabled = false;
        visuals.enabled = false;
    }

    public void OnBeatChange()
    {
        warningTimer--;
        if (warningTimer == 0)
        {
            AOEAttack();
        }
    }

    void AOEAttack()
    {
        hitbox.enabled = true;
        visuals.enabled = true;
        //Enable all necessary parts for the attack (hitbox and visuals)
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
