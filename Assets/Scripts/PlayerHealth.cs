using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private UIManager uiManager;//UIManager

    [SerializeField]
    private ItemDataSO itemDataSO;//ItemDataSO

    private float playerHp=100.0f;//Playerの体力

    public float PlayerHp//PlayerHp変数用のプロパティ
    {
        get { return playerHp; }//外部からは取得処理のみ可能に
    }

    private int bandageCount;//包帯の数

    public int BandageCount
    {
        get { return bandageCount; }
        set { bandageCount = value; }
    }

    /// <summary>
    /// 他のコライダーに触れた際に呼び出される
    /// </summary>
    /// <param name="hit">触れた相手</param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //触れたゲームオブジェクトのタグに応じて処理を変更
        switch(hit.gameObject.tag)
        {
            //手榴弾なら
            case ("Grenade"):
                UpdatePlayerHp(itemDataSO.itemDataList[1].attackPower,hit);
                break;

            //催涙弾なら
            case ("TearGasGrenade"):
                UpdatePlayerHp(itemDataSO.itemDataList[2].attackPower,hit);
                AttackedByTearGasGrenade();
                break;

            //ナイフなら
            case ("Knife"):
                UpdatePlayerHp(itemDataSO.itemDataList[3].attackPower,hit);
                break;

            //バットなら
            case ("Bat"):
                UpdatePlayerHp(itemDataSO.itemDataList[4].attackPower,hit);
                break;

            //アサルトなら
            case ("Assault"):
                UpdatePlayerHp(itemDataSO.itemDataList[5].attackPower,hit);
                break;

            //ショットガンなら
            case ("Shotgun"):
                UpdatePlayerHp(itemDataSO.itemDataList[6].attackPower, hit);
                break;

            //スナイパーなら
            case ("Sniper"):
                UpdatePlayerHp(itemDataSO.itemDataList[7].attackPower,hit);
                break;
        }
    }

    /// <summary>
    /// PlayerのHpを更新
    /// </summary>
    /// <param name="updateValue">Hpの更新量</param>
    /// <param name="hit">触れた相手</param>
    public void UpdatePlayerHp(float updateValue, ControllerColliderHit hit = null)
    {
        //攻撃を受けた際の処理なら
        if(updateValue<0.0f)
        {
            //被弾した際の視界の処理を行う
            StartCoroutine(uiManager.AttackEventHorizon());
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
