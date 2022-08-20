using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private UIManager uiManager;//UIManager

    [SerializeField]
    private ItemDataSO itemDataSO;//ItemDataSO

    private float playerHp=100.0f;//Player�̗̑�

    public float PlayerHp//PlayerHp�ϐ��p�̃v���p�e�B
    {
        get { return playerHp; }//�O������͎擾�����̂݉\��
    }

    private int bandageCount;//��т̐�

    public int BandageCount//bandageCount�ϐ��p�̃v���p�e�B
    {
        get { return bandageCount; }//�O������͎擾�����̂݉\��
    }

    private int medicinalPlantscount;//�򑐂̐�

    public int MedicinalPlantsCount//medicinalPlantscount�ϐ��p�̃v���p�e�B
    {
        get { return medicinalPlantscount; }//�O������͎擾�����̂݉\��
    }

    private int syringeCount;//���ˊ�̐�

    public int SyringeCount//syringeCount�ϐ��p�̃v���p�e�B
    {
        get { return syringeCount; }//�O������͎擾�����̂݉\��
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
            case "Grenade":
                UpdatePlayerHp(-itemDataSO.itemDataList[1].attackPower,hit);
                break;

            //�×ܒe�Ȃ�
            case "TearGasGrenade":
                UpdatePlayerHp(-itemDataSO.itemDataList[2].attackPower,hit);
                AttackedByTearGasGrenade();
                break;

            //�i�C�t�Ȃ�
            case "Knife":
                UpdatePlayerHp(-itemDataSO.itemDataList[3].attackPower);
                break;

            //�o�b�g�Ȃ�
            case "Bat":
                UpdatePlayerHp(-itemDataSO.itemDataList[4].attackPower);
                break;

            //�A�T���g�Ȃ�
            case "Assault":
                UpdatePlayerHp(-itemDataSO.itemDataList[5].attackPower,hit);
                break;

            //�V���b�g�K���Ȃ�
            case "Shotgun":
                UpdatePlayerHp(-itemDataSO.itemDataList[6].attackPower, hit);
                break;

            //�X�i�C�p�[�Ȃ�
            case "Sniper":
                UpdatePlayerHp(-itemDataSO.itemDataList[7].attackPower,hit);
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

    /// <summary>
    /// �񕜃A�C�e���̏��������X�V����
    /// </summary>
    /// <param name="itemName">�A�C�e���̖��O</param>
    /// <param name="updateValue">�������̍X�V��</param>
    public void UpdateRecoveryItemCount(ItemDataSO.ItemName itemName, int updateValue)
    {
        //�A�C�e���̖��O�ɉ����ď�����ύX
        switch(itemName)
        {
            //��тȂ�
            case ItemDataSO.ItemName.Bandage:
                bandageCount = Mathf.Clamp(bandageCount + updateValue, 0, itemDataSO.itemDataList[8].maxBulletCount);
                break;

            //�򑐂Ȃ�
            case ItemDataSO.ItemName.MedicinalPlants:
                medicinalPlantscount=Mathf.Clamp(medicinalPlantscount + updateValue, 0,itemDataSO.itemDataList[9].maxBulletCount);
                break;

            //���ˊ�Ȃ�
            case ItemDataSO.ItemName.Syringe:
                syringeCount=Mathf.Clamp(syringeCount+updateValue, 0, itemDataSO.itemDataList[10].maxBulletCount);
                break;
        }
    }

    /// <summary>
    /// �w�肵���񕜃A�C�e���̏��������擾����
    /// </summary>
    /// <param name="itemName">�񕜃A�C�e���̖��O</param>
    /// <returns>���̉񕜃A�C�e���̏�����</returns>
    public int GetRecoveryItemCount(ItemDataSO.ItemName itemName)
    {
        //�A�C�e���̖��O�ɉ����ď�����ύX
        switch (itemName)
        {
            //��тȂ�
            case ItemDataSO.ItemName.Bandage:
                return bandageCount;
              
            //�򑐂Ȃ�
            case ItemDataSO.ItemName.MedicinalPlants:
                return medicinalPlantscount;

            //���ˊ�Ȃ�
            case ItemDataSO.ItemName.Syringe:
                return syringeCount;

            //��L�ȊO�Ȃ�
            default:
                return 0;
        }
    }
}
