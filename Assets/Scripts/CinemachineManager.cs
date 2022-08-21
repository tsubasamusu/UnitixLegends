using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;//CinemachineFreeLook���g�p

public class CinemachineManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook airplaneCamera;//��s�@���_�J����

    [SerializeField]
    private Transform miniMapBackgroundTran;//�~�j�}�b�v�w�i�̈ʒu

    [SerializeField]
    private Transform playerTran;//Player�̈ʒu

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�~�j�}�b�v�̔w�i�̈ʒu�����Player�̈ʒu�ɍ��킹��
        miniMapBackgroundTran.position = new Vector3(playerTran.position.x,miniMapBackgroundTran.position.y,playerTran.position.z);
    }

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
