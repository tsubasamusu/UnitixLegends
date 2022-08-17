using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private int shotCount;//（仮）

    [SerializeField]
    private Rigidbody bulletPrefab;//（仮）

    [SerializeField]
    private float shotSpeed;//（仮）

    [SerializeField]
    private Transform mainCamera;//メインカメラ

    [SerializeField]
    private Transform temporaryObjectContainerTran;//一時的にゲームオブジェクトを収容するTransform

    [SerializeField]
    private ItemDataSO itemDataSO;//ItemDataSO

    private float timer;//経過時間１

    private float timer2;//経過時間２

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
    /// 毎フレーム呼び出される
    /// </summary>
   　private void Update()
    {
        //Bulletを発射する向きをカメラの向きに合わせる
        transform.eulerAngles = new Vector3(mainCamera.eulerAngles.x, mainCamera.eulerAngles.y, transform.eulerAngles.z);
    }

    /// <summary>
    /// 残弾数を更新する
    /// </summary>
    /// <param name="itemName">アイテムの名前r</param>
    public void UpdateBulletCount(ItemDataSO.ItemName itemName)
    {
        //受け取ったアイテムの名前に応じて処理を変更
        switch (itemName)
        {
            //アサルトなら
            case (ItemDataSO.ItemName.Assault):
                assaultBulletCount += itemDataSO.itemDataList[5].bulletCount;
                break;

            //ショットガンなら
            case (ItemDataSO.ItemName.Shotgun):
                shotgunBulletCount += itemDataSO.itemDataList[6].bulletCount;
                break;

            //スナイパーなら
            case (ItemDataSO.ItemName.Sniper):
                sniperBulletCount += itemDataSO.itemDataList[7].bulletCount;
                break;

            //アサルト用弾なら
            case (ItemDataSO.ItemName.AssaultBullet):
                assaultBulletCount += itemDataSO.itemDataList[11].bulletCount;
                break;

            //ショットガン用弾なら
            case (ItemDataSO.ItemName.ShotgunBullet):
                shotgunBulletCount += itemDataSO.itemDataList[12].bulletCount;
                break;

            //スナイパー用弾なら
            case (ItemDataSO.ItemName.SniperBullet):
                sniperBulletCount += itemDataSO.itemDataList[13].bulletCount;
                break;
        }
    }

    /// <summary>
    /// 弾を発射する
    /// </summary>
    /// <param name="itemData">使用するアイテムのデータ</param>
    public void ShotBullet(ItemDataSO.ItemData itemData)
    {
        //経過時間を計測
        timer += Time.deltaTime;

        //経過時間が連射間隔より小さいなら
        if (timer < itemData.interval)
        {
            //以降の処理を行わない
            return;
        }

        //弾を生成
        Rigidbody bulletRb = Instantiate(itemData.bulletPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));

        //弾を発射
        bulletRb.AddForce(transform.forward * itemData.shotSpeed);

        //Playerの死後も発射した弾は一定時間残る
        bulletRb.gameObject.transform.SetParent(temporaryObjectContainerTran);

        //経過時間を初期化
        timer = 0;

        //使用するアイテムが、手榴弾でも催涙弾でもないなら
        if (itemData.itemName != ItemDataSO.ItemName.Grenade && itemData.itemName != ItemDataSO.ItemName.TearGasGrenade)
        {
            //発射した弾を3.0秒後に消す
            Destroy(bulletRb.gameObject, 3.0f);

            //以降の処理を行わない
            return;
        }

        //経過時間を計測
        timer2 += Time.deltaTime;

        //経過時間が爆破時間に達したら
        if(timer2>=itemData.timeToExplode)
        {
            //TODO:爆発する処理

            Debug.Log("爆発");

            //発射した弾を消す
            Destroy(bulletRb.gameObject);

            //経過時間を初期化
            timer2 = 0;
        }
    }
}
