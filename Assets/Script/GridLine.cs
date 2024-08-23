using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i <= 100; i++)
        {
            GameObject horizontal = Resources.Load<GameObject>("Prefabs/HorizontalLine");
            Instantiate(horizontal, new Vector3(0, i * 64, 0), Quaternion.identity).transform.SetParent(transform);
            GameObject vertical = Resources.Load<GameObject>("Prefabs/VerticalLine");
            Instantiate(vertical, new Vector3(i * 64, 0, 0), Quaternion.identity).transform.SetParent(transform);
        }
    }
}
