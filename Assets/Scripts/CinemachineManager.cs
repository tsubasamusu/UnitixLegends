using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook airplaneCamera;//飛行機視点カメラ

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
