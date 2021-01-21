using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private GameObject Start_UI;
    private GameObject Game_UI;
    private GameObject Shop_UI;

    private UILabel HighScore;
    private UILabel GemCount;

    private UILabel Score;
    private UILabel GemInGame;

    private GameObject PlayBottom;
    private GameObject ShopBottom;

    private PlayerControl m_PlayerControl;

    // Start is called before the first frame update
    void Start()
    {
        Start_UI = GameObject.Find("Start_UI");
        Game_UI = GameObject.Find("Game_UI");
        Shop_UI = GameObject.Find("Shop_UI");

        HighScore = GameObject.Find("HighScore").GetComponent<UILabel>();
        GemCount = GameObject.Find("GemCount").GetComponent<UILabel>();

        Score = GameObject.Find("Score").GetComponent<UILabel>();
        GemInGame = GameObject.Find("GemNum").GetComponent<UILabel>();

        PlayBottom = GameObject.Find("Play_btn");
        ShopBottom = GameObject.Find("Shop");

        m_PlayerControl = GameObject.Find("Player").GetComponent<PlayerControl>();

        UIEventListener.Get(PlayBottom).onClick = PlayOn;
        Game_UI.SetActive(false);
        Shop_UI.SetActive(false);

       
        Init();
    }
    private void PlayOn(GameObject go)
    {
        Start_UI.SetActive(false);
        Shop_UI.SetActive(false);
        Game_UI.SetActive(true);
        m_PlayerControl.StartGame();
    }
    private void Init()
    {
        HighScore.text = PlayerPrefs.GetInt("BestScore", 0)+"";
        GemCount.text = PlayerPrefs.GetInt("gem", 0)+"/100";

        Score.text = "0";
        GemInGame.text = PlayerPrefs.GetInt("gem", 0) + "/100";
    }
    public void UpdateInfor(int score,int gem)
    {
        Score.text = score.ToString();
        GemInGame.text = gem + "/100";
        GemCount.text = gem + "/100";
        HighScore.text = PlayerPrefs.GetInt("BestScore", 0) + "";
    }
    // Update is called once per frame
    public void ResetUI()
    {
        Start_UI.SetActive(true);
        Game_UI.SetActive(false);
        Score.text = "0";
    }
}
