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

    [SerializeField]
    private Transform mainCameraTran;//���C���J�����̈ʒu

    [SerializeField]
    private Transform playerCharacterTran;//Player�̃L�����N�^�[�̈ʒu

    private float angle;//Player�̃L�����N�^�[�Ǝ��_�����p�x

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    private void Start()
    {
        //Player�̃L�����N�^�[�Ǝ��_�����p�x���擾
        angle = playerCharacterTran.eulerAngles.y + 90f;
    }

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�~�j�}�b�v�̔w�i�̈ʒu�����Player�̈ʒu�ɍ��킹��
        miniMapBackgroundTran.position = new Vector3(playerTran.position.x, miniMapBackgroundTran.position.y, playerTran.position.z);

        //Player�̃L�����N�^�[���ˌ��̎ז��ɂȂ�Ȃ��悤�ɂ���
        SetPlayerCharacter();
    }

    /// <summary>
    /// �J�����̊p�x�ɉ�����Player�̃L�����N�^�[�̗L�����A��������؂�ւ���
    /// </summary>
    private void SetPlayerCharacter()
    {
        //�J�����̊p�x�����͈͓��Ȃ�
        if (mainCameraTran.eulerAngles.y >= angle - 20f && mainCameraTran.eulerAngles.y <= angle + 20f)
        {
            //Player�̃L�����N�^�[�𖳌���
            playerCharacterTran.gameObject.SetActive(false);

            //�ȍ~�̏������s��Ȃ�
            return;
        }
        //�J�����̊p�x�����͈͓��Ȃ�
        else if (mainCameraTran.eulerAngles.y >= (angle + 180f) - 20f && mainCameraTran.eulerAngles.y <= (angle + 180f) + 20f)
        {
            //Player�̃L�����N�^�[�𖳌���
            playerCharacterTran.gameObject.SetActive(false);

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //Player�̃L�����N�^�[��L����
        playerCharacterTran.gameObject.SetActive(true);
    }

    /// <summary>
    /// ��s�@���_�J�����̗D�揇�ʂ�ݒ�
    /// </summary>
    /// <param name="airplaneCameraPriority">�D�揇��</param>
    public void SetAirplaneCameraPriority(int airplaneCameraPriority)
    {
        //���������ɁA��s�@���_�J�����̗D�揇�ʂ�ݒ�
        airplaneCamera.Priority= airplaneCameraPriority;
    }
}
