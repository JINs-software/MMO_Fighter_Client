using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager s_Instance;   // 유일성 보장
    static Manager Instance { get { Init(); return s_Instance; } }

    NetworkManager m_NetworkMgr = new NetworkManager();

    public static NetworkManager Network { get { return Instance.m_NetworkMgr; } }
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    static void Init()
    {
        if (s_Instance == null)
        {
            GameObject go = GameObject.Find("@Manager");
            if (go == null)
            {
                go = new GameObject { name = "@Manager" };
                go.AddComponent<Manager>();
            }

            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<Manager>();
        }
    }
}
