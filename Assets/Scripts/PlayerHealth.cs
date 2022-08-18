using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private UIManager uiManager;//UIManager

    [SerializeField]
    private ItemDataSO ItemDataSO;//ItemDataSO

    private float playerHp=100.0f;//Player�̗̑�

    public float PlayerHp//PlayerHp�ϐ��p�̃v���p�e�B
    {
        get { return playerHp; }//�O������͎擾�����̂݉\��
    }

    /// <summary>
    /// ���̃R���C�_�[�ɐG�ꂽ�ۂɌĂяo�����
    /// </summary>
    /// <param name="hit">�G�ꂽ����</param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //�G�ꂽ�Q�[���I�u�W�F�N�g�̃^�O�ɉ����ď�����ύX
        switch(hit.gameObject.tag)
        {
            //��֒e�Ȃ�
            case ("Grenade"):
                UpdatePlayerHp(-30.0f,hit);
                break;

            //�×ܒe�Ȃ�
            case ("TearGasGrenade"):
                UpdatePlayerHp(0,hit);
                AttackedByTearGasGrenade();
                break;

            //�i�C�t�Ȃ�
            case ("Knife"):
                UpdatePlayerHp(-100.0f,hit);
                break;

            //�o�b�g�Ȃ�
            case ("Bat"):
                UpdatePlayerHp(-50.0f,hit);
                break;

            //�A�T���g�Ȃ�
            case ("Assault"):
                UpdatePlayerHp(-1.0f,hit);
                break;

            //�X�i�C�p�[�Ȃ�
            case ("Sniper"):
                UpdatePlayerHp(-80.0f,hit);
                break;

            //�V���b�g�K���Ȃ�
            case ("Shotgun"):
                UpdatePlayerHp(-30.0f, hit);
                break;
        }
    }

    /// <summary>
    /// Player��Hp���X�V
    /// </summary>
    /// <param name="updateValue">Hp�̍X�V��</param>
    /// <param name="hit">�G�ꂽ����</param>
    public void UpdatePlayerHp(float updateValue, ControllerColliderHit hit = null)
    {
        //�U�����󂯂��ۂ̏����Ȃ�
        if(updateValue<0.0f)
        {
            //��e�����ۂ̎��E�̏������s��
            StartCoroutine(uiManager.AttackEventHorizon());
        }

        //�̗͗p�X���C�_�[���X�V
        uiManager.UpdateHpSliderValue(playerHp, updateValue);

        //playerHp��0�ȏ�100�ȉ��̒l�܂ő�������悤�ɐ�������
        playerHp = Mathf.Clamp(playerHp + updateValue, 0, 100);

        //null�G���[���
        if (hit != null)
        {
            //�����Ŏ󂯎�����Q�[���I�u�W�F�N�g������
            Destroy(hit.gameObject);
        }

        //Player�̗̑͂�0�ɂȂ�����
        if (playerHp == 0.0f)
        {
            //TOD:GameManager����Q�[���I�[�o�[�̏������Ăяo��
        }
    }

    /// <summary>
    /// �×ܒe�ɂ��U�����󂯂��ꍇ�̏���
    /// </summary>
    private void AttackedByTearGasGrenade()
    {
        //���E��5.0�b�ԈÂ�����
        StartCoroutine( uiManager.SetEventHorizonBlack(5.0f));
    }
}
