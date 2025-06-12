using System.Collections;
using UnityEngine;

public class AOEBullet : MonoBehaviour
{
    [SerializeField] int warningTimer;
    [SerializeField] float damage;
    [SerializeField] Collider hitbox;
    [SerializeField] Renderer visuals;

    void Awake()
    {
        hitbox.enabled = false;
        visuals.enabled = false;
        StartCoroutine(AOEWarning());
    }

    IEnumerator AOEWarning()
    {
        for (int i = 0; i < warningTimer; i++)
        {
            //Flash the warning
            yield return new WaitForSeconds(LevelEventManager.timeBetweenBeats);
        }

        AOEAttack();
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
