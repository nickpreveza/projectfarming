using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Persistent Singelton.
/// Inherit from this to make your class a Singleton that survives between scenes.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonPersistent<T> : MonoBehaviour where T :Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(this);
        }
    }
}
