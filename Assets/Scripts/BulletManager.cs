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

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    void Update()
    {
        //Bulletを発射する向きをカメラの向きに合わせる
        transform.eulerAngles = new Vector3(mainCamera.eulerAngles.x,mainCamera.eulerAngles.y,transform.eulerAngles.z);
    }

    /// <summary>
    /// ShotBulletゲームオブジェクトを有効化・無効化を行う
    /// </summary>
    public void SetShotBulletActiveOrPassive(bool set)
    {
        //引数を元に、ShotBulletゲームオブジェクトの有効化・無効化を切り替える
        transform.gameObject.SetActive(set);
    }

    /// <summary>
    /// 弾を発射する
    /// </summary>
    public void ShotBullet()
    {
        //TODO:GameDataから選択している武器の弾の情報を取得する処理を呼び出す

        //弾を生成
        Rigidbody bulletRb=Instantiate(bulletPrefab,transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));

        //弾を発射
        bulletRb.AddForce(transform.forward* shotSpeed);

        //Playerの死後も発射した弾は一定時間残る
        bulletRb.gameObject.transform.SetParent(temporaryObjectContainerTran);

        //残弾数を減らす
        shotCount--;

        //発射した弾を3.0秒後に消す
        Destroy(bulletRb.gameObject,3.0f);

        //TODO:SoundManagerから武器の発射音を再生する処理を呼び出す
    }
}
