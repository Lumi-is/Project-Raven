using UnityEditor.Build;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Defensive>() != null)
        {
            Defensive playerScript = other.gameObject.GetComponent<Defensive>();
            playerScript.PlayerDamage(damage);
        }
    }
}
