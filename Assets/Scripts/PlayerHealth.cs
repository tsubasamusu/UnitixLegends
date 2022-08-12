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
    /// ���̃R���C�_�[�ɐG�ꂽ�ۂɌĂяo�����
    /// </summary>
    /// <param name="hit"></param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //�G�ꂽ�Q�[���I�u�W�F�N�g�̃^�O�ɉ����ď�����ύX
        switch(hit.gameObject.tag)
        {
            case ("Grenade"):
                UpdatePlayerHp(-30.0f);
                break;
            case ("TearGasGrenade"):
                AttackedByTearGasGrenade();
                break;
            case ("Knife"):
                UpdatePlayerHp(-100.0f);
                break;
            case ("Bat"):
                UpdatePlayerHp(-50.0f);
                break;
            case ("Assault"):
                UpdatePlayerHp(-1.0f,hit);
                break;
            case ("Sniper"):
                UpdatePlayerHp(-80.0f,hit);
                break;
            case ("Shotgun"):
                UpdatePlayerHp(-30.0f, hit);
                break;
        }
    }

    /// <summary>
    /// Player��Hp���X�V
    /// </summary>
    private void UpdatePlayerHp(float updateValue, ControllerColliderHit hit=null)
    {
        //�U�����󂯂��ۂ̏����Ȃ�
        if(updateValue<0)
        {
            //TODO:UIManager����_���[�W���o�̏������Ăяo��
        }
        //�񕜂���ۂ̏����Ȃ�
        else if(updateValue>0)
        {
            //TODO:UIManager����񕜉��o�̏������Ăяo��
        }

        //playerHp��0�ȏ�100�ȉ��̒l�܂ő�������悤�ɐ�������
        playerHp = Mathf.Clamp(playerHp+ updateValue, 0, 100);

        //null�G���[���
        if (hit != null)
        {
            //�����Ŏ󂯎�����Q�[���I�u�W�F�N�g������
            Destroy(hit.gameObject);
        }

        //Player�̗̑͂�0�ɂȂ�����
        if(playerHp==0.0f)
        {
            //TOD:GameManager����Q�[���I�[�o�[�̏������Ăяo��
        }
    }

    /// <summary>
    /// �×ܒe�ɂ��U�����󂯂��ꍇ�̏���
    /// </summary>
    private void AttackedByTearGasGrenade()
    {
        //TODO:UIManager���王�E����莞�ԈÂ����鏈�����Ăяo��
    }
}
