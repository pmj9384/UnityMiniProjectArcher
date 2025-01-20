using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour // 싱글톤이기에 destory해도 삭제되면안됨 그리고 어디든 씬에 붙여놓으면안됨
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool applicationIsQuitting = false;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance of {typeof(T)} is null because application is quitting.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogError($"[Singleton] Multiple instances of {typeof(T)} found!");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject($"(singleton) {typeof(T)}");
                        _instance = singleton.AddComponent<T>();
                        DontDestroyOnLoad(singleton);

                        Debug.Log($"[Singleton] An instance of {typeof(T)} was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log($"[Singleton] Using existing instance: {_instance.gameObject.name}");
                    }
                }

                return _instance;
            }
        }
    }

    private void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }
}
