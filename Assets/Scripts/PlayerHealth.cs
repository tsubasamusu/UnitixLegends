using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float playerHp=100.0f;//Playerの体力

    public float PlayerHp//PlayerHp変数用のプロパティ
    {
        get { return playerHp; }//外部からは取得処理のみ可能に
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        Debug.Log(playerHp);
    }

    /// <summary>
    /// 他のコライダーに触れた際に呼び出される
    /// </summary>
    /// <param name="hit"></param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //触れたゲームオブジェクトのタグに応じて処理を変更
        switch(hit.gameObject.tag)
        {
            case ("Grenade"):
                UpdatePlayerHp(30.0f);
                break;
            case ("TearGasGrenade"):
                AttackedByTearGasGrenade();
                break;
            case ("Knife"):
                UpdatePlayerHp(100.0f);
                break;
            case ("Bat"):
                UpdatePlayerHp(50.0f);
                break;
            case ("Assault"):
                UpdatePlayerHp(1.0f,hit);
                break;
            case ("Sniper"):
                UpdatePlayerHp(80.0f,hit);
                break;
            case ("Shotgun"):
                UpdatePlayerHp(30.0f, hit);
                break;
        }
    }

    /// <summary>
    /// PlayerのHpを更新
    /// </summary>
    private void UpdatePlayerHp(float updateValue, ControllerColliderHit hit=null, bool isAttacked = true)
    {
        //攻撃を受けたら
        if(isAttacked)
        {
            //updateValueをマイナスにする
            updateValue = -updateValue;
        }

        //playerHpに0以上100以下の値まで代入されるように制限する
        playerHp = Mathf.Clamp(playerHp+ updateValue, 0, 100);

        //引数のhitがnullではないなら
        if(hit != null)//nullエラー回避
        {
            //引数で受け取ったゲームオブジェクトを消す
            Destroy(hit.gameObject);
        }
    }

    /// <summary>
    /// 催涙弾による攻撃を受けた場合の処理
    /// </summary>
    private void AttackedByTearGasGrenade()
    {

    }
}
