using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook airplaneCamera;//��s�@���_�J����

    /// <summary>
    /// ��s�@���_�J�����̗D�揇�ʂ�ݒ�
    /// </summary>
    /// <param name="airplaneCameraPriority"></param>
    public void SetAirplaneCameraPriority(int airplaneCameraPriority)
    {
        //���������ɁA��s�@���_�J�����̗D�揇�ʂ�ݒ�
        airplaneCamera.Priority= airplaneCameraPriority;
    }
}
