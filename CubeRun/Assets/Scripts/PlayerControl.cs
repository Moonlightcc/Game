using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public int x1 = 3;
    private int x2 = 2;
    private int x3;

    private Color color_Light = new Color(122/255f,85/255f,179/255f);
    private Color color_Deep = new Color(126/255f,96/255f,183/255f);

    private Transform m_Transform;
    
    private MapManager m_MapManager;
    private UIManager m_UIManager;

    private bool playerCanMove = false;

    private int gemCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        gemCount = PlayerPrefs.GetInt("gem", 0);
        m_Transform = gameObject.GetComponent<Transform>();
        m_UIManager = GameObject.Find("UI Root").GetComponent<UIManager>();
        x3 = x1;
        m_MapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
    }
    private void SaveData()
    {
        PlayerPrefs.SetInt("gem", gemCount);
        if (PlayerPrefs.GetInt("BestScore", 0) <= (x1 - x3))
            PlayerPrefs.SetInt("BestScore", x1 - x3);

    }
    public void PlayerInShop()
    {
        ChangePosition();
    }
    public void StartGame()
    {
        playerCanMove = true;
        Camera.main.GetComponent<CameraFollower>().StartFollow = true;
        ChangePosition();
        m_MapManager.StartDown();
    }
    // Update is called once per frame
    void Update()
    {
        MoveControl();
        ExtendMap();
    }

    void MoveControl()
    {
        if (playerCanMove)
        {
            if (Input.GetKeyDown(KeyCode.A))
                MoveLeft();
            if (Input.GetKeyDown(KeyCode.D))
                MoveRight();
        }
    }

    void ChangePosition()
    {
        GameObject tempObject = m_MapManager.mapData[x1][x2];
        string tempTag = tempObject.tag;
        Transform tempPlace = tempObject.GetComponent<Transform>();
        MeshRenderer tempMesh = null;
        m_Transform.position = tempPlace.position + new Vector3(0, 0.127f, 0);
        m_Transform.rotation = tempPlace.rotation;
        if (tempTag == "Tile")
            tempMesh = tempPlace.Find("normal_a2").GetComponent<MeshRenderer>();
        else if (tempTag == "Smashing_Spike")
            tempMesh = tempPlace.Find("smashing_spikes_a2").GetComponent<MeshRenderer>();
        else if (tempTag == "Moving_Spike")
            tempMesh = tempPlace.Find("moving_spikes_a2").GetComponent<MeshRenderer>();

        if (tempMesh != null)
            if (x1 % 2 == 0)
                tempMesh.material.color = color_Deep;
            else
                tempMesh.material.color = color_Light;
        else
        {
            gameObject.AddComponent<Rigidbody>();
            StartCoroutine("GameOver", true);
        }
        m_UIManager.UpdateInfor(x1 - x3, gemCount);

    }
    void MoveLeft()
    {
        if (x1 % 2 == 0 || x2 != 0)
        {
            x1++;
            if (x1 % 2 == 1)
                x2--;
            ChangePosition();
        }
    }
    void MoveRight()
    {
        if (x1 % 2 == 0 || x2 != 4)
        {
            x1++;
            if (x1 % 2 == 0)
                x2++;
            ChangePosition();
        }
    }

    private void ExtendMap()
    {
        if(m_MapManager.mapData.Count - x1 <=13)
        {
            m_MapManager.AddPr();
            m_MapManager.canCreateBox = true;
            m_MapManager.CreateMapElement(m_MapManager.setPoint);
        }
    }

    public IEnumerator GameOver(bool b)
    {
        m_MapManager.StopDown();
        playerCanMove = false;
        if (b)
            yield return new WaitForSeconds(0.5f);
        
        Camera.main.GetComponent<CameraFollower>().StartFollow = false;
        SaveData();
        StartCoroutine("ReturnToStart");
        
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Spike_Attack")
        {
            StartCoroutine("GameOver",false);
        }
        else if (coll.tag == "Gem")
        {
            AddGem();
            GameObject.Destroy(coll.gameObject.GetComponent<Transform>().parent.gameObject);
        }
    }
    private void AddGem()
    {
        gemCount++;
        m_UIManager.UpdateInfor(x1 - x3, gemCount);
    }
    private IEnumerator ReturnToStart()
    {
        yield return new WaitForSeconds(2f);
        ResetPlayer();
        m_UIManager.UpdateInfor(x1 - x3, gemCount);
        m_UIManager.ResetUI();
        
    }   
    private void ResetPlayer()
    {
        if(gameObject.GetComponent<Rigidbody>()!=null)
            GameObject.Destroy(gameObject.GetComponent<Rigidbody>());
        x1 = 3;
        x2 = 2;
        x3 = x1;
        m_MapManager.ResetMap();
        Camera.main.GetComponent<CameraFollower>().ResetCamera();
    }
}
