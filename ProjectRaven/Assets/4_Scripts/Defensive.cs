using UnityEngine;

public class Defensive : MonoBehaviour
{
    public float HP;
    public float MaxHP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(HP < 0)
        {
            Die();
        }
    }

    public void TakeDamage(float takenDamageAmount)
    {
        
    }

    void Die()
    {

    }
}
