using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;

    public Vector3 initialDirection;
    private bool goForward = true;
    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();
        m_Rigidbody.AddTorque(initialDirection * 30, ForceMode.Impulse);
    }

    void Update()
    {
        if(goForward)
            m_Transform.Translate(initialDirection*0.05f,Space.World);
    }
    
    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Wall")
        {
            gameObject.tag = "Untagged";
            goForward = false;
            m_Rigidbody.AddForce(-initialDirection , ForceMode.Impulse);
            m_Rigidbody.AddRelativeTorque(Vector3.forward * Random.Range(10, 30), ForceMode.Impulse);
            m_Rigidbody.useGravity = true;
            GameObject.Destroy(gameObject, 1.3f);
        }
        if(coll.tag == "Spike_Attack")
        {
            gameObject.tag = "Untagged";
            goForward = false;
            m_Rigidbody.AddRelativeTorque(Vector3.forward * Random.Range(10, 30), ForceMode.Impulse);
            m_Rigidbody.useGravity = true;
            GameObject.Destroy(gameObject, 1.3f);
        }
        if(coll.tag == "Player")
        {
            goForward = false;
            m_Rigidbody.AddForce(-initialDirection*0.5f, ForceMode.Acceleration);
        }


        
    }
}
