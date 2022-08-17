using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private int maxBulletCount;//所持できるアイテム1つ当たりの弾の最大数

    [SerializeField]
    private Transform mainCamera;//メインカメラ

    [SerializeField]
    private Transform temporaryObjectContainerTran;//一時的にゲームオブジェクトを収容するTransform

    [SerializeField]
    private ItemDataSO itemDataSO;//ItemDataSO

    private float timer;//経過時間

    private int grenadeBulletCount;//手榴弾の残りの数

    public int GrenadeBulletCount//grenadeBulletCount変数用のプロパティ
    {
        get { return grenadeBulletCount; }//外部からは取得処理のみを可能に
    }

    private int tearGasGrenadeBulletCount;//催涙弾の残りの数

    public int TearGasGrenadeBulletCount//tearGasGrenadeBulletCount変数用のプロパティ
    {
        get { return tearGasGrenadeBulletCount; }//外部からは取得処理のみを可能に
    }

    private int assaultBulletCount;//アサルト用弾の残弾数

    public int AssaultBulletCount//assaultBulletCount変数用のプロパティ
    {
        get { return assaultBulletCount; }//外部からは取得処理のみを可能に
    }

    private int sniperBulletCount;//スナイパー用弾の残弾数

    public int SniperBulletCount//sniperBulletCount変数用のプロパティ
    {
        get { return sniperBulletCount; }//外部からは取得処理のみを可能に
    }

    private int shotgunBulletCount;//ショットガン用弾の残弾数

    public int ShotgunBulletCount//shotgunBulletCount変数用のプロパティ
    {
        get { return shotgunBulletCount; }//外部からは取得処理のみを可能に
    }

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //経過時間の計測を開始
        StartCoroutine(MeasureTime1());
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
   　private void Update()
    {
        //Bulletを発射する向きをカメラの向きに合わせる
        transform.eulerAngles = new Vector3(mainCamera.eulerAngles.x, mainCamera.eulerAngles.y, transform.eulerAngles.z);
    }

    /// <summary>
    /// 経過時間1を計測する
    /// </summary>
    /// <returns>待ち時間</returns>
    private IEnumerator MeasureTime1()
    {
        //無限ループ
        while (true)
        {
            //経過時間を計測
            timer += Time.deltaTime;

            //次のフレームへ飛ばす（実質、Updateメソッド）
            yield return null;
        }
    }

    /// <summary>
    /// 残段数を更新する
    /// </summary>
    /// <param name="itemName">アイテムの名前</param>
    /// <param name="updateValue">残弾数の変更量</param>
    public void UpdateBulletCount(ItemDataSO.ItemName itemName, int updateValue = 0)
    {
        //残弾数の更新数が0ではなかったら
        if (updateValue != 0)
        {
            //受け取ったアイテムの名前に応じて処理を変更
            switch (itemName)
            {
                //手榴弾なら
                case ItemDataSO.ItemName.Grenade:
                    grenadeBulletCount = Mathf.Clamp(grenadeBulletCount + updateValue, 0, maxBulletCount);
                    break;

                //催涙弾なら
                case ItemDataSO.ItemName.TearGasGrenade:
                    tearGasGrenadeBulletCount=Mathf.Clamp(tearGasGrenadeBulletCount+updateValue,0,maxBulletCount);
                    break;

                //アサルトなら
                case ItemDataSO.ItemName.Assault:
                    assaultBulletCount=Mathf.Clamp(assaultBulletCount+updateValue,0,maxBulletCount);
                    break;

                //ショットガンなら
                case ItemDataSO.ItemName.Shotgun:
                    shotgunBulletCount=Mathf.Clamp(shotgunBulletCount+updateValue,0,maxBulletCount);
                    break;

                //スナイパーなら
                case ItemDataSO.ItemName.Sniper:
                    sniperBulletCount =Mathf.Clamp(sniperBulletCount+updateValue,0,maxBulletCount);
                    break;
            }

            //以降の処理を行わない
            return;
        }

        //受け取ったアイテムの名前に応じて処理を変更
        switch (itemName)
        {
            //手榴弾なら
            case ItemDataSO.ItemName.Grenade:
                grenadeBulletCount += itemDataSO.itemDataList[1].bulletCount;
                break;

            //催涙弾なら
            case ItemDataSO.ItemName.TearGasGrenade:
                tearGasGrenadeBulletCount += itemDataSO.itemDataList[2].bulletCount;
                break;

            //アサルトなら
            case ItemDataSO.ItemName.Assault:
                assaultBulletCount += itemDataSO.itemDataList[5].bulletCount;
                break;

            //ショットガンなら
            case ItemDataSO.ItemName.Shotgun:
                shotgunBulletCount += itemDataSO.itemDataList[6].bulletCount;
                break;

            //スナイパーなら
            case ItemDataSO.ItemName.Sniper:
                sniperBulletCount += itemDataSO.itemDataList[7].bulletCount;
                break;

            //アサルト用弾なら
            case ItemDataSO.ItemName.AssaultBullet:
                assaultBulletCount += itemDataSO.itemDataList[11].bulletCount;
                break;

            //ショットガン用弾なら
            case ItemDataSO.ItemName.ShotgunBullet:
                shotgunBulletCount += itemDataSO.itemDataList[12].bulletCount;
                break;

            //スナイパー用弾なら
            case ItemDataSO.ItemName.SniperBullet:
                sniperBulletCount += itemDataSO.itemDataList[13].bulletCount;
                break;
        }
    }

    /// <summary>
    /// 弾を発射する
    /// </summary>
    /// <param name="itemData">使用するアイテムのデータ</param>
    /// <returns>待ち時間</returns>
    public IEnumerator ShotBullet(ItemDataSO.ItemData itemData)
    {
        //経過時間が連射間隔より小さいか、残弾数が0なら
        if (timer < itemData.interval||GetBulletCount(itemData.itemName)==0)
        {
            //以降の処理を行わない
            yield break;
        }

        //弾を生成
        Rigidbody bulletRb = Instantiate(itemData.bulletPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));

        //弾を発射
        bulletRb.AddForce(transform.forward * itemData.shotSpeed);

        //Playerの死後も発射した弾は一定時間残る
        bulletRb.gameObject.transform.SetParent(temporaryObjectContainerTran);

        //残弾数を減らす
        UpdateBulletCount(itemData.itemName, -1);

        //経過時間を初期化
        timer = 0;

        //使用するアイテムが、手榴弾でも催涙弾でもないなら
        if (itemData.itemName != ItemDataSO.ItemName.Grenade && itemData.itemName != ItemDataSO.ItemName.TearGasGrenade)
        {
            //発射した弾を3.0秒後に消す
            Destroy(bulletRb.gameObject, 3.0f);

            //以降の処理を行わない
            yield break;
        }

        //爆破時間まで待つ
        yield return new WaitForSeconds(itemData.timeToExplode);

        //使用するアイテムが手榴弾なら
        if(itemData.itemName == ItemDataSO.ItemName.Grenade)
        {
            //TODO:爆発する処理
        }
        //使用するアイテムが催涙弾なら
        else if(itemData.itemName == ItemDataSO.ItemName.TearGasGrenade)
        {
            //TODO:ガスを放出する処理
        }

        //発射した弾を消す
        Destroy(bulletRb.gameObject);
    }

    /// <summary>
    /// 指定したアイテムの残弾数を取得する
    /// </summary>
    /// <param name="itemName">その弾を使用するアイテム</param>
    /// <returns>そのアイテムが使用する弾の残弾数</returns>
    public int GetBulletCount(ItemDataSO.ItemName itemName)
    {
        //選択しているアイテムの名前に応じて処理を変更
        switch (itemName)
        {
            //手榴弾なら
            case ItemDataSO.ItemName.Grenade:
                return grenadeBulletCount;

            //催涙弾なら
            case ItemDataSO.ItemName.TearGasGrenade:
                return tearGasGrenadeBulletCount;

            //アサルトなら
            case ItemDataSO.ItemName.Assault:
                return assaultBulletCount;

            //スナイパーなら
            case ItemDataSO.ItemName.Sniper:
                return sniperBulletCount;

            //ショットガンなら
            case ItemDataSO.ItemName.Shotgun:
                return shotgunBulletCount;

            //上記以外なら
            default:
                return 0;
        }
    }
}
