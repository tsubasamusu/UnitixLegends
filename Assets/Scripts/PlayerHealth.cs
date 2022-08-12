using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float playerHp=100.0f;//Player�̗̑�

    public float PlayerHp//PlayerHp�ϐ��p�̃v���p�e�B
    {
        get { return playerHp; }//�O������͎擾�����̂݉\��
    }

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        Debug.Log(playerHp);
    }

    /// <summary>
    /// ���̃R���C�_�[�ɐG�ꂽ�ۂɌĂяo�����
    /// </summary>
    /// <param name="hit"></param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //�G�ꂽ�Q�[���I�u�W�F�N�g�̃^�O�ɉ����ď�����ύX
        switch(hit.gameObject.tag)
        {
            case ("Grenade"):
                UpdatePlayerHp(30.0f);
                break;
            case ("TearGasGrenade"):
                AttackedByTearGasGrenade();
                break;
            case ("Knife"):
                UpdatePlayerHp(100.0f);
                break;
            case ("Bat"):
                UpdatePlayerHp(50.0f);
                break;
            case ("Assault"):
                UpdatePlayerHp(1.0f,hit);
                break;
            case ("Sniper"):
                UpdatePlayerHp(80.0f,hit);
                break;
            case ("Shotgun"):
                UpdatePlayerHp(30.0f, hit);
                break;
        }
    }

    /// <summary>
    /// Player��Hp���X�V
    /// </summary>
    private void UpdatePlayerHp(float updateValue, ControllerColliderHit hit=null, bool isAttacked = true)
    {
        //�U�����󂯂���
        if(isAttacked)
        {
            //updateValue���}�C�i�X�ɂ���
            updateValue = -updateValue;
        }

        //playerHp��0�ȏ�100�ȉ��̒l�܂ő�������悤�ɐ�������
        playerHp = Mathf.Clamp(playerHp+ updateValue, 0, 100);

        //������hit��null�ł͂Ȃ��Ȃ�
        if(hit != null)//null�G���[���
        {
            //�����Ŏ󂯎�����Q�[���I�u�W�F�N�g������
            Destroy(hit.gameObject);
        }
    }

    /// <summary>
    /// �×ܒe�ɂ��U�����󂯂��ꍇ�̏���
    /// </summary>
    private void AttackedByTearGasGrenade()
    {

    }
}
