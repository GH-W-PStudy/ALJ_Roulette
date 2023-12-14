using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RouletteController : MonoBehaviour
{
    float rotSpeed = 0; // 回転速度
    Boolean isRightClicked = false; //右クリックチェック
    Boolean isStopped = false; //停止チェック


    // Start is called before the first frame update
    // Startメソッド：スクリプト起動直後に一度だけ実行
    void Start()
    {
        // フレームレートを60に固定(デバイスによる性能差をなくす)
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    // Updateメソッド：フレームごとに実行
    void Update()
    {
        // マウスで左クリックされたらルーレットを回す
        if (Input.GetMouseButtonDown(0) && this.rotSpeed == 0)
        {
            // 回転速度をセット
            setRotSpeed();
        }

        // 回転速度ぶん、ルーレットを回転させる
        transform.Rotate(0, 0, this.rotSpeed);


        // マウスで右クリックされたらルーレットを減速→停止
        if (Input.GetMouseButtonDown(1))
        {
            this.isRightClicked = true;
        }
        if (isRightClicked == true)
        {
            SlowDown();
        }

        // ルーレット再開
        if (isStopped == true && Input.GetMouseButtonDown(0))
        {
            this.isRightClicked = false;
            setRotSpeed();
        }

    }


    void setRotSpeed()
    {
        this.rotSpeed = 30;
    }

    void SlowDown()
    {
        if (this.rotSpeed >= 0.1)
        {
            // ルーレットを減速させていく
            this.rotSpeed *= 0.975f;

        }
        else
        {
            isStopped = true;
            this.rotSpeed = 0;
        }
    }

    // 以下未実装
    //// 「もう一度」を押下したらシーンをリロード
    //public void RetryButtonDown()
    //{
    //    GameObject director = GameObject.Find("GameDirector");
    //    director.GetComponent<GameDirector>().RetryButtonDown();
    //}
}
