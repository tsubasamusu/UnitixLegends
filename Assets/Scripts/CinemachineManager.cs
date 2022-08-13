using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook airplaneCamera;//��s�@���_�J����

    [SerializeField]
    private GameObject player;//Player

    [SerializeField]
    private GameObject miniMapCamera;//�~�j�}�b�v�J����

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�~�j�}�b�v�J���������Player�̏��ɑ؍݂�����
        miniMapCamera.transform.position=new Vector3(player.transform.position.x,miniMapCamera. transform.position.y,player.transform.position.z);
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
