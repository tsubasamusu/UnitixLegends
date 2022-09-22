using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;//CinemachineFreeLookを使用

namespace yamap {

    public class CinemachineManager : MonoBehaviour {

        [SerializeField]
        private CinemachineFreeLook airplaneCamera;//飛行機視点カメラ

        [SerializeField]
        private PlayerController playerController;//PlayerController

        [SerializeField]
        private Transform miniMapBackgroundTran;//ミニマップ背景の位置

        private Transform mainCameraTran;//メインカメラの位置

        [SerializeField]
        private Transform playerCharacterTran;//Playerのキャラクターの位置

        [SerializeField]
        private GameObject playerCharacterbody;//Playerのキャラクターの体

        private float angle;//Playerのキャラクターが視点を遮る角度

        /// <summary>
        /// ゲーム開始直後に呼び出される
        /// </summary>
        private void Start() 
        {
            //メインカメラの位置情報を取得
            mainCameraTran = Camera.main.transform;

            //Playerのキャラクターと視点が被る角度を取得
            angle = playerCharacterTran.eulerAngles.y + 90f;
        }

        /// <summary>
        /// 毎フレーム呼び出される
        /// </summary>
        private void Update() 
        {
            //PlayerControllerが無効なら
            if (playerController.enabled == false) 
            {
                //以降の処理を行わない
                return;
            }

            //ミニマップの背景の位置を常にPlayerの位置に合わせる
            miniMapBackgroundTran.position = new Vector3(playerController.transform.position.x, miniMapBackgroundTran.position.y, playerController.transform.position.z);

            //Playerのキャラクターが視界を遮っているかどうか調べる
            bool set = CheckIntercepted() ? false : true;

            //Playerのキャラクターの有効化無効化を切り替える
            SetPlayerCharacterActive(set);
        }

        /// <summary>
        /// Playerのキャラクターが視界を遮っているかどうか調べる
        /// </summary>
        /// <returns>Playerのキャラクターが視界を遮っていたらtrue</returns>
        private bool CheckIntercepted() 
        {
            //カメラの角度が一定範囲内なら
            if (mainCameraTran.eulerAngles.y >= angle - 20f && mainCameraTran.eulerAngles.y <= angle + 20f) 
            {
                //trueを返す
                return true;
            }
            //カメラの角度が一定範囲内なら
            else if (mainCameraTran.eulerAngles.y >= (angle + 180f) - 20f && mainCameraTran.eulerAngles.y <= (angle + 180f) + 20f) 
            {
                //trueを返す
                return true;
            }

            //falseを返す
            return false;
        }

        /// <summary>
        /// 飛行機視点カメラの優先順位を設定
        /// </summary>
        /// <param name="airplaneCameraPriority">優先順位</param>
        public void SetAirplaneCameraPriority(int airplaneCameraPriority) 
        {
            //引数を元に、飛行機視点カメラの優先順位を設定
            airplaneCamera.Priority = airplaneCameraPriority;
        }

        /// <summary>
        /// Playerのキャラクターの有効化無効化を切り替える
        /// </summary>
        /// <param name="set">有効にするならtrue</param>
        public void SetPlayerCharacterActive(bool set) 
        {
            //引数を元に、Playerのキャラクターの有効化、無効化を切り替える
            playerCharacterbody.SetActive(set);
        }
    }
}