using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage; 

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Defensive>() != null)
        {
            Defensive playerScript = other.gameObject.GetComponent<Defensive>();
            playerScript.PlayerDamage(damage);
        }
    }
}
