using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    Transform m_Transform;
    Transform m_Gem;
    // Start is called before the first frame update
    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Gem = m_Transform.Find("gem 3").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Gem.Rotate(new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)));
    }
}
