using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScriptMessageHandler
{
    public static List<Type> GetIBullets()
    {
        var allTypes = typeof(ScriptMessageHandler).Assembly.GetTypes();
        var searchFor = typeof(IBullet);

        List<Type> IBulletTypes = new();
        foreach (Type type in allTypes)
        {
            if (searchFor.IsAssignableFrom(type))
            {
                IBulletTypes.Add(type);
            }
        }
        return IBulletTypes;
    }
    public static void CallIBullets(List<Type> types)
    {
        foreach (Type type in types)
        {
            GameObject[] instanceList = GameObject.FindGameObjectsWithTag("Bullet");

            foreach (GameObject gameObject in instanceList)
            {
                Debug.Log(type.Name);
                IBullet instance = (IBullet)gameObject.GetComponent(type.Name);
                instance?.OnBeatChange();
            }
        }
    }
}
