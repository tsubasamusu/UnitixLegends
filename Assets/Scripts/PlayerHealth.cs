using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private UIManager uiManager;//UIManager

    private float playerHp=100.0f;//Playerの体力

    public float PlayerHp//PlayerHp変数用のプロパティ
    {
        get { return playerHp; }//外部からは取得処理のみ可能に
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
                UpdatePlayerHp(-30.0f,hit);
                break;
            case ("TearGasGrenade"):
                UpdatePlayerHp(0,hit);
                AttackedByTearGasGrenade();
                break;
            case ("Knife"):
                UpdatePlayerHp(-100.0f,hit);
                break;
            case ("Bat"):
                UpdatePlayerHp(-50.0f,hit);
                break;
            case ("Assault"):
                UpdatePlayerHp(-1.0f,hit);
                break;
            case ("Sniper"):
                UpdatePlayerHp(-80.0f,hit);
                break;
            case ("Shotgun"):
                UpdatePlayerHp(-30.0f, hit);
                break;
        }
    }

    /// <summary>
    /// PlayerのHpを更新
    /// </summary>
    private void UpdatePlayerHp(float updateValue, ControllerColliderHit hit = null)
    {
        //攻撃を受けた際の処理なら
        if(updateValue<0.0f)
        {
            //被弾した際の視界の処理を行う
            StartCoroutine( uiManager.AttackEventHorizon());
        }

        //体力用スライダーを更新
        uiManager.UpdateHpSliderValue(playerHp, updateValue);

        //playerHpに0以上100以下の値まで代入されるように制限する
        playerHp = Mathf.Clamp(playerHp + updateValue, 0, 100);

        //nullエラー回避
        if (hit != null)
        {
            //引数で受け取ったゲームオブジェクトを消す
            Destroy(hit.gameObject);
        }

        //Playerの体力が0になったら
        if (playerHp == 0.0f)
        {
            //TOD:GameManagerからゲームオーバーの処理を呼び出す
        }
    }

    /// <summary>
    /// 催涙弾による攻撃を受けた場合の処理
    /// </summary>
    private void AttackedByTearGasGrenade()
    {
        //視界を5.0秒間暗くする
        StartCoroutine( uiManager.SetEventHorizonBlack(5.0f));
    }
}
