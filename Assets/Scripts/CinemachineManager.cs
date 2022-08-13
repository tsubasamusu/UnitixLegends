using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook airplaneCamera;//飛行機視点カメラ

    [SerializeField]
    private GameObject player;//Player

    [SerializeField]
    private GameObject miniMapCamera;//ミニマップカメラ

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //ミニマップカメラを常にPlayerの上空に滞在させる
        miniMapCamera.transform.position=new Vector3(player.transform.position.x,miniMapCamera. transform.position.y,player.transform.position.z);
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
