using UnityEngine;
using System;

public class Attackable : MonoBehaviour 
{
    public float HP = 100; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(HP <= 0)
        {
            AttackManager attackScript = FindAnyObjectByType<AttackManager>();
            MovementController movement = FindAnyObjectByType<MovementController>();

            attackScript.isInAttackRange = false;
            movement.gazeFollowedAttackable = null;
            gameObject.SetActive(false);
        }
    }

    public void takeDamage(float Amount)
    {
        HP = HP - Amount;
    }
}
