using System.Collections;
using UnityEngine;

public class AOEBullet : MonoBehaviour
{
    [SerializeField] int warningTimer;
    [SerializeField] float damage;
    [SerializeField] float size;

    void Awake()
    {
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
