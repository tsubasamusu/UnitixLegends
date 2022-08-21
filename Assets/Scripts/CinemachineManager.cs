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

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //ミニマップの背景の位置を常にPlayerの位置に合わせる
        miniMapBackgroundTran.position = new Vector3(playerTran.position.x,miniMapBackgroundTran.position.y,playerTran.position.z);
    }

    /// <summary>
    /// 飛行機視点カメラの優先順位を設定
    /// </summary>
    /// <param name="airplaneCameraPriority"></param>
    public void SetAirplaneCameraPriority(int airplaneCameraPriority)
    {
        //引数を元に、飛行機視点カメラの優先順位を設定
        airplaneCamera.Priority= airplaneCameraPriority;
    }
}
