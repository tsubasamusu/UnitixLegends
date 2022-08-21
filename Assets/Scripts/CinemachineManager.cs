using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;//CinemachineFreeLookを使用

public class CinemachineManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook airplaneCamera;//飛行機視点カメラ

    [SerializeField]
    private Transform miniMapBackgroundTran;//ミニマップ背景の位置

    [SerializeField]
    private Transform playerTran;//Playerの位置

    [SerializeField]
    private Transform mainCameraTran;//メインカメラの位置

    [SerializeField]
    private Transform playerCharacterTran;//Playerのキャラクターの位置

    private float angle;//Playerのキャラクターと視点が被る角度

    /// <summary>
    /// ゲーム開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //Playerのキャラクターと視点が被る角度を取得
        angle = playerCharacterTran.eulerAngles.y + 90f;
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //ミニマップの背景の位置を常にPlayerの位置に合わせる
        miniMapBackgroundTran.position = new Vector3(playerTran.position.x, miniMapBackgroundTran.position.y, playerTran.position.z);

        //Playerのキャラクターが射撃の邪魔にならないようにする
        SetPlayerCharacter();
    }

    /// <summary>
    /// カメラの角度に応じてPlayerのキャラクターの有効化、無効化を切り替える
    /// </summary>
    private void SetPlayerCharacter()
    {
        //カメラの角度が一定範囲内なら
        if (mainCameraTran.eulerAngles.y >= angle - 20f && mainCameraTran.eulerAngles.y <= angle + 20f)
        {
            //Playerのキャラクターを無効化
            playerCharacterTran.gameObject.SetActive(false);

            //以降の処理を行わない
            return;
        }
        //カメラの角度が一定範囲内なら
        else if (mainCameraTran.eulerAngles.y >= (angle + 180f) - 20f && mainCameraTran.eulerAngles.y <= (angle + 180f) + 20f)
        {
            //Playerのキャラクターを無効化
            playerCharacterTran.gameObject.SetActive(false);

            //以降の処理を行わない
            return;
        }

        //Playerのキャラクターを有効化
        playerCharacterTran.gameObject.SetActive(true);
    }

    /// <summary>
    /// 飛行機視点カメラの優先順位を設定
    /// </summary>
    /// <param name="airplaneCameraPriority">優先順位</param>
    public void SetAirplaneCameraPriority(int airplaneCameraPriority)
    {
        //引数を元に、飛行機視点カメラの優先順位を設定
        airplaneCamera.Priority= airplaneCameraPriority;
    }
}
