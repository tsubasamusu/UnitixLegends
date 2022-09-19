using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTweenを使用

public class StormController : MonoBehaviour
{
    [SerializeField]
    private float timeLimit;//制限時間

    private Vector3 firstStormScale;//ストームの大きさの初期値

    private float currentScaleRate=100f;//現在のストームの大きさの割合(%)

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //ストームの大きさの初期値を設定
        firstStormScale = transform.localScale;

        //ストームの縮小を開始する
        MakeStormSmaller();
    }

    /// <summary>
    /// ストームの縮小を開始する
    /// </summary>
    private void MakeStormSmaller()
    {
        //制限時間内に等速で「ストームの大きさの割合」を100%から0%にする
        DOTween.To(() => currentScaleRate,(x) => currentScaleRate = x,0f,timeLimit).SetEase(Ease.Linear);
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //割合に応じてストームを縮小させる
        transform.localScale = new Vector3((firstStormScale.x*(currentScaleRate/100f)),firstStormScale.y,(firstStormScale.z*(currentScaleRate/100f)));
    }

    /// <summary>
    /// 自身が安置内に居るかどうか調べる
    /// </summary>
    /// <param name="myPos">自身の座標</param>
    /// <returns>自身が安置内にいたらtrue</returns>
    public bool CheckEnshrine(Vector3 myPos)
    {
        //自身の座標をx-z平面上で表す
        Vector3 pos = Vector3.Scale(myPos, new Vector3(1f, 0f, 1f));

        //ストームの中央の座標を(0,0,0)に設定
        Vector3 centerPos = Vector3.zero;

        //ストームの中央までの距離（x-z平面上）を取得
        float length =(pos - centerPos).magnitude;

        //自身が安置内にいたらtrue、安置外にいるならfalseを返す
        return length <=transform.localScale.x/2f?true:false;
    }
}
