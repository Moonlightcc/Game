using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    private Transform m_Transform;
    private Vector3 iniPos;
    private Transform m_Player_Transform;

    public bool StartFollow = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        iniPos = m_Transform.position;
        m_Player_Transform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StartFollow)
        {
            Vector3 nextPoint = new Vector3(m_Transform.position.x, m_Player_Transform.position.y + 1.8f, m_Player_Transform.position.z-0.4f);
            m_Transform.position = Vector3.Lerp(m_Transform.position, nextPoint, 2*Time.deltaTime);

        }
    }
    public void ResetCamera()
    {
        m_Transform.position = iniPos;
    }
}
