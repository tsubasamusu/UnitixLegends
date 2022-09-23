using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private UIManager uiManager;//UIManager

    [SerializeField]
    private StormController stormController;//StormController

    [SerializeField]
    private GameManager gameManager;//GameManager

    [SerializeField]
    private ItemManager itemManager;//ItemManager

    [SerializeField]
    private Skybox skybox;//Skybox

    [SerializeField]
    private Material normalSky;//�ʏ펞�̋�

    [SerializeField]
    private Material stormSky;//�X�g�[�����̋�

    [SerializeField, Header("1�b������Ɏ󂯂�X�g�[���ɂ��_���[�W")]
    private float stormDamage;//1�b������Ɏ󂯂�X�g�[���ɂ��_���[�W

    public float StormDamage//stormDamage�ϐ��p�̃v���p�e�B
    {
        get { return stormDamage; }//�O������͎擾�����̂݉\��
    }

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
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    private void Start()
    {
        //�X�g�[���ɂ��_���[�W���󂯂邩�ǂ����̒������J�n
        StartCoroutine(CheckStormDamage());
    }

    /// <summary>
    /// ���̃R���C�_�[�ɐG�ꂽ�ۂɌĂяo�����
    /// </summary>
    /// <param name="hit">�G�ꂽ����</param>
    private void OnCollisionEnter(Collision hit)
    {
        //�G�ꂽ�Q�[���I�u�W�F�N�g�̃^�O�ɉ����ď�����ύX
        switch (hit.gameObject.tag)
        {
            //��֒e�Ȃ�
            case "Grenade":
                UpdatePlayerHp(-itemManager.GetItemData(ItemDataSO.ItemName.Grenade).attackPower, hit.gameObject);
                break;

            //�×ܒe�Ȃ�
            case "TearGasGrenade":
                UpdatePlayerHp(-itemManager.GetItemData(ItemDataSO.ItemName.TearGasGrenade).attackPower, hit.gameObject);
                AttackedByTearGasGrenade();
                break;

            //�i�C�t�Ȃ�
            case "Knife":
                UpdatePlayerHp(-itemManager.GetItemData(ItemDataSO.ItemName.Knife).attackPower);
                break;

            //�o�b�g�Ȃ�
            case "Bat":
                UpdatePlayerHp(-itemManager.GetItemData(ItemDataSO.ItemName.Bat).attackPower);
                break;

            //�A�T���g�Ȃ�
            case "Assault":
                UpdatePlayerHp(-itemManager.GetItemData(ItemDataSO.ItemName.Assault).attackPower, hit.gameObject);
                break;

            //�V���b�g�K���Ȃ�
            case "Shotgun":
                UpdatePlayerHp(-itemManager.GetItemData(ItemDataSO.ItemName.Shotgun).attackPower, hit.gameObject);
                break;

            //�X�i�C�p�[�Ȃ�
            case "Sniper":
                UpdatePlayerHp(-itemManager.GetItemData(ItemDataSO.ItemName.Sniper).attackPower, hit.gameObject);
                break;
        }
    }

    /// <summary>
    /// �X�g�[���ɂ��_���[�W���󂯂邩�ǂ������ׂ�
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator CheckStormDamage()
    {
        //��̔���
        bool skyFlag = false;

        //�����ɌJ��Ԃ�
        while (true)
        {
            //Player�����u���ɂ��炸�A�Q�[���I����Ԃł͂Ȃ��Ȃ�A�J��Ԃ����
            while (!stormController.CheckEnshrine(transform.position)&&!gameManager.IsGameOver)
            {
                //��̔��肪true�Ȃ�
                if (skyFlag)
                {
                    //���V��ɕύX
                    skybox.material = stormSky;

                    //��̔����false������
                    skyFlag = false;
                }

                //Player��Hp������������
                UpdatePlayerHp(-stormDamage);

                //1�b�҂�
                yield return new WaitForSeconds(1f);
            }

            //��̔��肪false�Ȃ�
            if (!skyFlag)
            {
                //�����ɕύX
                skybox.material = normalSky;

                //��̔����true������
                skyFlag = true;
            }

            //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
            yield return null;
        }
    }

    /// <summary>
    /// Player��Hp���X�V
    /// </summary>
    /// <param name="updateValue">Hp�̍X�V��</param>
    /// <param name="gameObject">�G�ꂽ����</param>
    public void UpdatePlayerHp(float updateValue, GameObject gameObject=null)
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
        if (gameObject != null)
        {
            //�����Ŏ󂯎�����Q�[���I�u�W�F�N�g������
            Destroy(gameObject);
        }

        //Player�̗̑͂�0�ɂȂ�����
        if (playerHp == 0.0f)
        {
            //�Q�[���I�[�o�[���o���s��
            StartCoroutine(gameManager.MakeGameOver());
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
                bandageCount = Mathf.Clamp(bandageCount + updateValue, 0, itemManager.GetItemData(ItemDataSO.ItemName.Bandage).maxBulletCount);
                break;

            //�򑐂Ȃ�
            case ItemDataSO.ItemName.MedicinalPlants:
                medicinalPlantscount=Mathf.Clamp(medicinalPlantscount + updateValue, 0, itemManager.GetItemData(ItemDataSO.ItemName.MedicinalPlants).maxBulletCount);
                break;

            //���ˊ�Ȃ�
            case ItemDataSO.ItemName.Syringe:
                syringeCount=Mathf.Clamp(syringeCount+updateValue, 0, itemManager.GetItemData(ItemDataSO.ItemName.Syringe).maxBulletCount);
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