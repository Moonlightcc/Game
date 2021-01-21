using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smashing_Spikes : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform m_Transform;
    private Transform son_Transform;
    Vector3 init_Pos = new Vector3();
    Vector3 end_Pos = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        son_Transform = m_Transform.Find("smashing_spikes_b").GetComponent<Transform>();
        init_Pos = son_Transform.position;
        end_Pos = init_Pos + new Vector3(0, 0.6f, 0);
        StartCoroutine("UpAndDown");
    }

    private IEnumerator UpAndDown()
    {
        while (true)
        {
            StopCoroutine("Down");
            StartCoroutine("Up");
            yield return new WaitForSeconds(1f);
            StopCoroutine("Up");
            StartCoroutine("Down");
            yield return new WaitForSeconds(1f);
        }
    }
    // Update is called once per frame
    private IEnumerator Up()
    {
        while (true)
        {
            son_Transform.position = Vector3.Lerp(son_Transform.position, end_Pos, Time.deltaTime * 25);
            yield return null;
        }
    }

    private IEnumerator Down()
    {
        while (true)
        {
            son_Transform.position = Vector3.Lerp(son_Transform.position, init_Pos, Time.deltaTime * 25);
            yield return null;

        }

    }
}
