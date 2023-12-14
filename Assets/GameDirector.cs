using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    // 「もう一度」ボタン
    GameObject RetryButton;

    // Start is called before the first frame update
    void Start()
    {
        this.RetryButton = GameObject.Find("RetryButton");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 「もう一度」を押下したらシーンをリロード
    public void RetryButtonDown()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
