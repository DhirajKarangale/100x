using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    /// On awake, we initialize our instance. Make sure to call base.Awake() in override if you need awake.
    protected virtual void Awake()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        instance = this as T;
    }
}
