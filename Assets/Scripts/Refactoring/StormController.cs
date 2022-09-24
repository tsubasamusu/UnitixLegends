using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTweenを使用

namespace yamap 
{
    /// <summary>
    /// Playerのストームとの関係
    /// </summary>
    public enum PlayerStormState 
    {
        InStorm,//ストーム内
        OutStorm,//ストーム外
    }

    public class StormController : MonoBehaviour 
    {
        [SerializeField]
        private float timeLimit;//制限時間

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

        private Vector3 firstStormScale;//ストームの大きさの初期値

        private float currentScaleRate = 100f;//現在のストームの大きさの割合(%)

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
            DOTween.To(() => currentScaleRate, (x) => currentScaleRate = x, 0f, timeLimit).SetEase(Ease.Linear);
        }

        /// <summary>
        /// 毎フレーム呼び出される
        /// </summary>
        private void Update() 
        {
            //割合に応じてストームを縮小させる
            transform.localScale = new Vector3((firstStormScale.x * (currentScaleRate / 100f)), firstStormScale.y, (firstStormScale.z * (currentScaleRate / 100f)));
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
            float length = (pos - centerPos).magnitude;

            //自身が安置内にいたらtrue、安置外にいるならfalseを返す
            return length <= transform.localScale.x / 2f ? true : false;
        }

        /// <summary>
        /// SkyBox の変更
        /// </summary>
        /// <param name="playerStormState">Playerとストームとの関係</param>
        public void ChangeSkyBox(PlayerStormState playerStormState) 
        {
            //空のマテリアルを設定
            skybox.material = GetMaterialFromStormState(playerStormState);

            /// <summary>
            /// 空のマテリアルを取得する
            /// </summary>
            /// <param name="playerStormState">Playerとストームとの関係</param>
            /// <returns>マテリアル</returns>
            Material GetMaterialFromStormState(PlayerStormState playerStormState) 
            {
                //Playerとストームとの関係に応じて、異なった空のマテリアルを返す
                return playerStormState == PlayerStormState.InStorm ? stormSky : normalSky;
            }
        }        
    }
}