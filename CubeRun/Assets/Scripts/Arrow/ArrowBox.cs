using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBox : MonoBehaviour
{
    private Transform m_Transform;
    private Transform map_Transform;
    private GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        map_Transform = GameObject.Find("MapManager").GetComponent<Transform>();
        arrow = Resources.Load<GameObject>("Arrow");
        InvokeRepeating("CreateArrow", 1f, 1.5f);
    }


    public void CreateArrow()
    {
        GameObject tempArrow = GameObject.Instantiate(arrow, m_Transform.position + new Vector3(0, 0, 0.04f), m_Transform.rotation);
        tempArrow.GetComponent<Arrow>().initialDirection = m_Transform.right;
        tempArrow.GetComponent<Transform>().SetParent(map_Transform);
    }
}
