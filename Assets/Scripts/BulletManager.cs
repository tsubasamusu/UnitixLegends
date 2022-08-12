using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCamera;//メインカメラ

    public List<GameObject> bulletPrefabList = new List<GameObject>();//弾のプレファブのリスト

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    void Update()
    {
        //Bulletを発射する向きをカメラの向きに合わせる
        transform.eulerAngles = new Vector3(mainCamera.transform.eulerAngles.x,mainCamera.transform.eulerAngles.y,transform.eulerAngles.z);
    }

    /// <summary>
    /// ShotBulletゲームオブジェクトを有効化・無効化を行う
    /// </summary>
    public void SetShotBulletActiveOrPassive(bool set)
    {
        //引数を元に、ShotBulletゲームオブジェクトの有効化・無効化を切り替える
        transform.gameObject.SetActive(set);
    }
}
