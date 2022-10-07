using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private UIManager uiManager;//UIManager

    [SerializeField]
    private StormController stormController;//StormController

    [SerializeField]
    private GameManager gameManager;//GameManager

    [SerializeField]
    private ItemManager itemManager;//ItemManager

    [SerializeField]
    private Skybox skybox;//Skybox

    [SerializeField]
    private Material normalSky;//通常時の空

    [SerializeField]
    private Material stormSky;//ストーム内の空

    [SerializeField, Header("1秒あたりに受けるストームによるダメージ")]
    private float stormDamage;//1秒あたりに受けるストームによるダメージ

    public float StormDamage//stormDamage変数用のプロパティ
    {
        get { return stormDamage; }//外部からは取得処理のみ可能に
    }

    private float playerHp=100.0f;//Playerの体力

    public float PlayerHp//PlayerHp変数用のプロパティ
    {
        get { return playerHp; }//外部からは取得処理のみ可能に
    }

    private int bandageCount;//包帯の数

    public int BandageCount//bandageCount変数用のプロパティ
    {
        get { return bandageCount; }//外部からは取得処理のみ可能に
    }

    private int medicinalPlantscount;//薬草の数

    public int MedicinalPlantsCount//medicinalPlantscount変数用のプロパティ
    {
        get { return medicinalPlantscount; }//外部からは取得処理のみ可能に
    }

    private int syringeCount;//注射器の数

    public int SyringeCount//syringeCount変数用のプロパティ
    {
        get { return syringeCount; }//外部からは取得処理のみ可能に
    }

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //ストームによるダメージを受けるかどうかの調査を開始
        StartCoroutine(CheckStormDamage());
    }

    /// <summary>
    /// 他のコライダーに触れた際に呼び出される
    /// </summary>
    /// <param name="hit">触れた相手</param>
    private void OnCollisionEnter(Collision hit)
    {
        //触れたゲームオブジェクトのタグに応じて処理を変更
        switch (hit.gameObject.tag)
        {
            //手榴弾なら
            case "Grenade":
                UpdatePlayerHp(-itemManager.GetItemData(ItemDataSO.ItemName.Grenade).attackPower, hit.gameObject);
                break;

            //催涙弾なら
            case "TearGasGrenade":
                UpdatePlayerHp(-itemManager.GetItemData(ItemDataSO.ItemName.TearGasGrenade).attackPower, hit.gameObject);
                AttackedByTearGasGrenade();
                break;

            //ナイフなら
            case "Knife":
                UpdatePlayerHp(-itemManager.GetItemData(ItemDataSO.ItemName.Knife).attackPower);
                break;

            //バットなら
            case "Bat":
                UpdatePlayerHp(-itemManager.GetItemData(ItemDataSO.ItemName.Bat).attackPower);
                break;

            //アサルトなら
            case "Assault":
                UpdatePlayerHp(-itemManager.GetItemData(ItemDataSO.ItemName.Assault).attackPower, hit.gameObject);
                break;

            //ショットガンなら
            case "Shotgun":
                UpdatePlayerHp(-itemManager.GetItemData(ItemDataSO.ItemName.Shotgun).attackPower, hit.gameObject);
                break;

            //スナイパーなら
            case "Sniper":
                UpdatePlayerHp(-itemManager.GetItemData(ItemDataSO.ItemName.Sniper).attackPower, hit.gameObject);
                break;
        }
    }

    /// <summary>
    /// ストームによるダメージを受けるかどうか調べる
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator CheckStormDamage()
    {
        //空の判定
        bool skyFlag = false;

        //無限に繰り返す
        while (true)
        {
            //Playerが安置内におらず、ゲーム終了状態ではないなら、繰り返される
            while (!stormController.CheckEnshrine(transform.position)&&!gameManager.IsGameOver)
            {
                //空の判定がtrueなら
                if (skyFlag)
                {
                    //悪天候に変更
                    skybox.material = stormSky;

                    //空の判定にfalseを入れる
                    skyFlag = false;
                }

                //PlayerのHpを減少させる
                UpdatePlayerHp(-stormDamage);

                //1秒待つ
                yield return new WaitForSeconds(1f);
            }

            //空の判定がfalseなら
            if (!skyFlag)
            {
                //快晴に変更
                skybox.material = normalSky;

                //空の判定にtrueを入れる
                skyFlag = true;
            }

            //次のフレームへ飛ばす（実質、Updateメソッド）
            yield return null;
        }
    }

    /// <summary>
    /// PlayerのHpを更新
    /// </summary>
    /// <param name="updateValue">Hpの更新量</param>
    /// <param name="gameObject">触れた相手</param>
    public void UpdatePlayerHp(float updateValue, GameObject gameObject=null)
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
        if (gameObject != null)
        {
            //引数で受け取ったゲームオブジェクトを消す
            Destroy(gameObject);
        }

        //Playerの体力が0になったら
        if (playerHp == 0.0f)
        {
            //ゲームオーバー演出を行う
            StartCoroutine(gameManager.MakeGameOver());
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

    /// <summary>
    /// 回復アイテムの所持数を更新する
    /// </summary>
    /// <param name="itemName">アイテムの名前</param>
    /// <param name="updateValue">所持数の更新量</param>
    public void UpdateRecoveryItemCount(ItemDataSO.ItemName itemName, int updateValue)
    {
        //アイテムの名前に応じて処理を変更
        switch(itemName)
        {
            //包帯なら
            case ItemDataSO.ItemName.Bandage:
                bandageCount = Mathf.Clamp(bandageCount + updateValue, 0, itemManager.GetItemData(ItemDataSO.ItemName.Bandage).maxBulletCount);
                break;

            //薬草なら
            case ItemDataSO.ItemName.MedicinalPlants:
                medicinalPlantscount=Mathf.Clamp(medicinalPlantscount + updateValue, 0, itemManager.GetItemData(ItemDataSO.ItemName.MedicinalPlants).maxBulletCount);
                break;

            //注射器なら
            case ItemDataSO.ItemName.Syringe:
                syringeCount=Mathf.Clamp(syringeCount+updateValue, 0, itemManager.GetItemData(ItemDataSO.ItemName.Syringe).maxBulletCount);
                break;
        }
    }

    /// <summary>
    /// 指定した回復アイテムの所持数を取得する
    /// </summary>
    /// <param name="itemName">回復アイテムの名前</param>
    /// <returns>その回復アイテムの所持数</returns>
    public int GetRecoveryItemCount(ItemDataSO.ItemName itemName)
    {
        //アイテムの名前に応じて処理を変更
        switch (itemName)
        {
            //包帯なら
            case ItemDataSO.ItemName.Bandage:
                return bandageCount;
              
            //薬草なら
            case ItemDataSO.ItemName.MedicinalPlants:
                return medicinalPlantscount;

            //注射器なら
            case ItemDataSO.ItemName.Syringe:
                return syringeCount;

            //上記以外なら
            default:
                return 0;
        }
    }
}
