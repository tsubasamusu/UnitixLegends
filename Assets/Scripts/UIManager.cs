using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using UnityEngine.UI;//UI���g�p
using DG.Tweening;//DOTween���g�p
using UnityEngine.SceneManagement;//LOadScen���g�p

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image eventHorizon;//���E

    [SerializeField]
    private Image logo;//���S

    [SerializeField]
    private Sprite gameStart;//�Q�[���X�^�[�g���S

    [SerializeField]
    private Sprite gameClear;//�Q�[���N���A���S

    [SerializeField]
    private Text txtGameOver;//�Q�[���I�[�o�[�e�L�X�g

    [SerializeField]
    private Text txtItemCount;//�c�e���e�L�X�g

    [SerializeField]
    private Text txtOtherCount;//���̐��̃e�L�X�g

    [SerializeField]
    private Text txtFps;//FPS�̃e�L�X�g

    [SerializeField]
    private Text txtMessage;//���b�Z�[�W�̃e�L�X�g

    [SerializeField]
    private Text floatingMessagePrefab;//�t���[�g�\���̃v���t�@�u

    [SerializeField]
    private Slider hpSlider;//�̗̓X���C�_�[

    [SerializeField]
    private CanvasGroup canvasGroup;//CanvasGroup

    [SerializeField]
    private Transform itemSlot;//�A�C�e���X���b�g�̐e

    [SerializeField]
    private ItemDataSO itemDataSO;//ItemDataSO

    [SerializeField]
    private BulletManager bulletManager;//BulletManager

    [SerializeField]
    private ItemManager itemManager;//ItemManager

    [SerializeField]
    private PlayerHealth playerHealth;//PlayerHealth

    [SerializeField]
    private Transform floatingMessagesTran;//�t���[�g�\���̐e

    [SerializeField]
    private Transform enemies;//�S�Ă�Enemy�̐e

    [SerializeField]
    private GameObject itemSlotSetPrefab;//�A�C�e���X���b�g�Z�b�g�̃v���t�@�u

    [SerializeField]
    private GameObject scope;//�X�R�[�v

    [HideInInspector]
    public List<Image> imgItemSlotList = new List<Image>();//�A�C�e���X���b�g�̃C���[�W�̃��X�g

    [HideInInspector]
    public List<Image> imgItemSlotBackgroundList= new List<Image>();//�A�C�e���X���b�g�̔w�i�̃C���[�W�̃��X�g

    /// <summary>
    /// �e�L�X�g�̕\���̍X�V����ɍs��
    /// </summary>
    /// <returns></returns>
    public IEnumerator UpdateText()
    {
        //�����ɌJ��Ԃ�
        while(true)
        {
            //�t���[�����[�g���v�Z���A�\�����X�V����
            UpdateFpsText();

            //�c�e���̕\�����X�V����
            UpdateTxtBulletCount();

            //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
            yield return null;
        }
    }

    /// <summary>
    /// �A�C�e���X���b�g�̐ݒ蓙���s��
    /// </summary>
    public void SetUpItemSlots()
    {
        //�A�C�e���X���b�g�𐶐�
        GenerateItemSlots(5);

        //��ԉE�̃A�C�e���X���b�g�̔w�i��ݒ�
        SetItemSlotBackgroundColor(1, Color.red);
    }

    /// <summary>
    /// ���̐��̕\�����X�V
    /// </summary>
    /// <param name="enemyNumber">Enemy�̐�</param>
    public void UpdateTxtOtherCount(int enemyNumber)
    {
        txtOtherCount.text = (enemyNumber+1).ToString()+"Players\n"+GameData.instance.KillCount.ToString()+"Kills";
    }

    /// <summary>
    /// �Q�[���X�^�[�g���o���s��
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator PlayGameStart()
    {
        //���E�𔒐F�ɐݒ�
        SetEventHorizonColor(Color.white);

        //���E�̐F���n�b�L���ƕ\��
        eventHorizon.DOFade(1f, 0f);

        //���S���Q�[���X�^�[�g�ɐݒ�
        logo.sprite = gameStart;

        //2.0�b�����ă��S��\��
        logo.DOFade(1.0f,2.0f);

        //���S�̕\�����I���܂ő҂�
        yield return new WaitForSeconds(2.0f);

        //���E��1.0�b�|���ē����ɂ���
        eventHorizon.DOFade(0.0f, 1.0f);

        //���S��1.0�b�����ď���
        logo.DOFade(0.0f, 1.0f);

        //���E�ƃ��S�̉��o���I���܂ő҂�
        yield return new WaitForSeconds(1.0f);
    }

    /// <summary>
    /// �Q�[���N���A���o���s��
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator PlayGameClear()
    {
        //���E�𔒐F�ɐݒ�
        SetEventHorizonColor(Color.white);

        //���E�̐F���n�b�L���ƕ\��
        eventHorizon.DOFade(1f, 0f);

        //���S���Q�[���N���A�ɐݒ�
        logo.sprite = gameClear;

        //2.0�b�����ă��S��\��
        logo.DOFade(1.0f, 2.0f);

        //���S�̕\�����I���܂ő҂�
        yield return new WaitForSeconds(2.0f);

        //���S��1.0�b�����ď���
        logo.DOFade(0.0f, 1.0f);

        //���E�ƃ��S�̉��o���I���܂ő҂�
        yield return new WaitForSeconds(1.0f);
    }

    /// <summary>
    /// �Q�[���I�[�o�[���o���s��
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator PlayGameOver()
    {
        //���E�����F�ɐݒ�
        SetEventHorizonColor(Color.black);

        //1.0�b�����Ď��E�����S�ɈÂ�����
        eventHorizon.DOFade(1.0f, 1.0f);

        //���E�����S�ɈÓ]����܂ő҂�
        yield return new WaitForSeconds(1.0f);

        //3.0�b�����ē����ŁuGameOver�v��\��
        txtGameOver.DOText("GameOver",3.0f).SetEase(Ease.Linear);

        //�uGameOver�̕\�����I�������ƁA�����1.0�b�ԑ҂�
        yield return new WaitForSeconds(4.0f);
    }

    /// <summary>
    /// ���E���w�肳�ꂽ���Ԃ����Â�����
    /// </summary>
    /// <param name="time">�Â����鎞��</param>
    /// <returns>�҂�����</returns>
    public IEnumerator SetEventHorizonBlack(float time)
    {
        //���E�����F�ɐݒ�
        SetEventHorizonColor(Color.black);

        //1.0�b�����Ď��E�����S�ɈÂ�����
        eventHorizon.DOFade(1.0f, 1.0f);

        //�����Ŏw�肳�ꂽ���Ԃ������E���Â��܂ܕۂ�
        yield return new WaitForSeconds(time);

        //0.5�b�����Ď��E�����ɖ߂�
        eventHorizon.DOFade(0.0f, 0.5f);
    }

    /// <summary>
    /// ���E�̐F��ݒ肷��
    /// </summary>
    /// <param name="color">���E�̐F</param>
    public void SetEventHorizonColor(Color color)
    {
        //���������ɁA���E�̐F��ݒ�
        eventHorizon.color=color;
    }

    /// <summary>
    /// ��e�����ۂ̎��E�̏���
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator AttackEventHorizon()
    {
        //���E��ԐF�ɐݒ�
        SetEventHorizonColor(Color.red);

        //0.25�b�����Ď��E�������Ԃ�����
        eventHorizon.DOFade(0.5f, 0.25f);

        //���E�������Ԃ��Ȃ�܂ő҂�
        yield return new WaitForSeconds(0.25f);

        //0.25�b�����Ď��E�����ɖ߂�
        eventHorizon.DOFade(0.0f, 0.25f);
    }

    /// <summary>
    /// �̗͗p�X���C�_�[���X�V����
    /// </summary>
    /// <param name="currentValue">���݂̗̑�</param>
    /// <param name="updateValue">�ύX����̗͗�</param>
    public void UpdateHpSliderValue(float currentValue,float updateValue)
    {
        //0.5�b�����đ̗͗p�X���C�_�[���X�V����
        hpSlider.DOValue((currentValue + updateValue)/100f, 0.5f);
    }

    /// <summary>
    /// CanvasGroup�̕\���A��\����؂�ւ���
    /// </summary>
    /// <param name="set">�\������Ȃ�true</param>
    public void SetCanvasGroup(bool isSetting)
    {
        //���������ɁACanvasGroup�̓����x��ݒ�
        canvasGroup.alpha = isSetting ? 1.0f : 0.0f;
    }

    /// <summary>
    /// �A�C�e���̐��̕\���̍X�V���s��
    /// </summary>
    private void UpdateTxtBulletCount()
    {
        //�I�����Ă���A�C�e������ѓ���Ȃ�
        if (itemManager.GetSelectedItemData().isMissile)
        {
            //�I������Ă����ѓ���̎c�e�����e�L�X�g�ɐݒ�
            txtItemCount.text = bulletManager.GetBulletCount(itemManager.GetSelectedItemData().itemName).ToString();
        }
        //�I�����Ă���A�C�e���ɉ񕜌��ʂ�����Ȃ�
        else if(itemManager.GetSelectedItemData().restorativeValue>0)
        {
            //�I������Ă���񕜃A�C�e���̏��������e�L�X�g�ɐݒ�
            txtItemCount.text = playerHealth.GetRecoveryItemCount(itemManager.GetSelectedItemData().itemName).ToString();
        }
        //�I�����Ă���A�C�e�����A��ѓ���ł��񕜃A�C�e���ł��Ȃ��Ȃ�
        else
        {
            //�e�L�X�g����ɂ���
            txtItemCount.text = "";
        }
    }

    /// <summary>
    /// �S�Ẵt���[�g�\�����\���ɂ���
    /// </summary>
    public void SetFloatingMessagesNotActive()
    {
        //�t���[�g�\���̐e�𖳌���
        floatingMessagesTran.gameObject.SetActive(false);
    }

    /// <summary>
    /// �t���[�g�\���𐶐�����
    /// </summary>
    /// <param name="messageText">�\���������e�L�X�g</param>
    /// <param name="color">�\������F</param>
    /// <returns>�҂�����</returns>
    public IEnumerator GenerateFloatingMessage(string messageText,Color color)
    {
        //�t���[�g�\���𐶐�
        Text txtFloatingMessage = Instantiate(floatingMessagePrefab);

        //���������t���[�g�\���̐e��ݒ�
        txtFloatingMessage.gameObject.transform.SetParent(floatingMessagesTran);

        //���������ɁA�t���[�g�\���̃e�L�X�g��ݒ�
        txtFloatingMessage.text = messageText;

        //���������ɁA�t���[�g�\���̐F��ݒ�
        txtFloatingMessage.color = color;

        //�t���[�g�\���̏ꏊ�������ʒu��ݒ�
        txtFloatingMessage.gameObject.transform.localPosition = Vector3.zero;

        //�t���[�g�\���̑傫����������
        txtFloatingMessage.gameObject.transform.localScale = new Vector3(3f,3f,3f);

        //���������t���[�g�\����3.0�b��ɏ���
        Destroy(txtFloatingMessage.gameObject, 3.0f);

        //�t���[�g�\����2.0�b�����āA��Ɉړ�������
        txtFloatingMessage.gameObject.transform.DOLocalMoveY(100.0f,2.0f);

        //�t���[�g�\���̈ړ����I���܂ő҂�
        yield return new WaitForSeconds(2.0f);

        //�t���[�g�\����1.0�b�����Ĕ�\���ɂ���
        txtFloatingMessage.DOFade(0.0f, 1.0f);
    }

    /// <summary>
    /// �t���[�����[�g���v�Z���A�\�����X�V����
    /// </summary>
    private void UpdateFpsText()
    {
        //�\�����X�V
        txtFps.text=(1f/Time.deltaTime).ToString("F0");
    }

    /// <summary>
    /// �w�肳�ꂽ�������A�C�e���X���b�g�𐶐�����
    /// </summary>
    /// <param name="generateNumber">�A�C�e���X���b�g�̐�</param>
    private void GenerateItemSlots(int generateNumber)
    {
        //�����Ŏw�肳�ꂽ�񐔂��������������J��Ԃ�
        for (int i = 0; i < generateNumber; i++)
        {
            //�A�C�e���X���b�g�𐶐�
            GameObject itemSlot = Instantiate(itemSlotSetPrefab);

            //���������A�C�e���X���b�g�̐e��itemSlot�ɐݒ�
            itemSlot.transform.SetParent(this.itemSlot);

            //���������A�C�e���X���b�g�̑傫����ݒ�
            itemSlot.transform.localScale = Vector3.one;

            //���������A�C�e���X���b�g�̈ʒu��ݒ�
            itemSlot.transform.localPosition = new Vector3((-1 * (200 + (50 * (generateNumber - 5)))) + (100 * i), -100, 0);

            //���������A�C�e���X���b�g�̎q�I�u�W�F�N�g��Image���擾
            if (itemSlot.transform.GetChild(2).TryGetComponent<Image>(out Image imgItem))//null�G���[���
            {
                //���������A�C�e���X���b�g�̃C���[�W�����X�g�ɒǉ�
                imgItemSlotList.Add(imgItem);

                //�擾�����C���[�W�𓧖��ɐݒ�
                imgItem.DOFade(0.0f, 0f);
            }

            //���������A�C�e���X���b�g�̎q�I�u�W�F�N�g�i�w�i�j��Image���擾
            if (itemSlot.transform.GetChild(0).TryGetComponent<Image>(out Image imgBackGround))//null�G���[���
            {
                //���������A�C�e���X���b�g�̃C���[�W�i�w�i�j�����X�g�ɒǉ�
                imgItemSlotBackgroundList.Add(imgBackGround);

                //�w�i�𔼓����ɐݒ�
                imgBackGround.DOFade(0.3f, 0f);
            }

            //Player���������Ă���A�C�e���̃��X�g�̗v�f���A�A�C�e���X���b�g�̐��������
            itemManager.playerItemList.Add(itemDataSO.itemDataList[0]);
        }

        //�A�C�e���X���b�g�S�̂̑傫����2�{�ɂ���
        itemSlot.localScale = new Vector3(2f, 2f, 2f);

        //�A�C�e���X���b�g�S�̂̈ʒu�𒲐�
        itemSlot.localPosition= new Vector3(0f,270f,0f);
    }

    /// <summary>
    /// �A�C�e����Sprite��ݒ肷��
    /// </summary>
    /// <param name="itemNo">�A�C�e���̔ԍ�</param>
    /// <param name="itemSprite">�A�C�e����Sprite</param>
    public void SetItemSprite(int itemNo,Sprite itemSprite)
    {
        //���������ɁA�w�肳�ꂽ�A�C�e����Sprite��ݒ肷��
        imgItemSlotList[itemNo-1].sprite = itemSprite;

        //�w�肳�ꂽ�C���[�W������
        imgItemSlotList[itemNo - 1].DOFade(1.0f, 0.25f);
    }

    /// <summary>
    /// ���b�Z�[�W�̃e�L�X�g��ݒ肵�A�\������
    /// </summary>
    /// <param name="text">����</param>
    /// <param name="color">�F</param>
    public void SetMessageText(string text,Color color)
    {
        //���������ɁA���b�Z�[�W�̃e�L�X�g��ݒ�
        txtMessage.text=text;

        //���������ɁA�e�L�X�g�̐F��ݒ�
        txtMessage.color=color;
    }

   
    /// <summary>
    /// ���b�Z�[�W�̕\���A��\����؂�ւ���
    /// </summary>
    /// <param name="isSetting">�\������Ȃ�true</param>
    public void SetMessageActive(bool isSetting)
    {
        //���������ɁA���b�Z�[�W�̓����x���擾
        float value = isSetting ? 1f : 0f;

        //�擾���������x�����b�Z�[�W�ɔ��f����
        txtMessage.DOFade(value, 0f);
    }

   /// <summary>
   /// �w�肳�ꂽ�A�C�e���X���b�g�̔w�i�F��ݒ肷��
   /// </summary>
   /// <param name="itemslotNo">�A�C�e���X���b�g�̔ԍ�</param>
   /// <param name="color">�F</param>
    public void SetItemSlotBackgroundColor(int itemslotNo,�@Color color)
    {
        //�A�C�e���X���b�g�̔w�i�̃C���[�W�̃��X�g�̗v�f�������J��Ԃ�
        for(int i = 0; i < imgItemSlotBackgroundList.Count; i++)
        {
            //�w�肳�ꂽ�A�C�e���X���b�g�̔w�i�F��ݒ肵�A����ȊO�͍��F�ɐݒ肷��
            imgItemSlotBackgroundList[i].color =i == (itemslotNo - 1) ? color:Color.black;

            //�w�i�𔼓����ɂ���
            imgItemSlotBackgroundList[i].DOFade(0.3f, 0f);
        }
    }

    /// <summary>
    /// �X�R�[�v��`��
    /// </summary>
    public void PeekIntoTheScope()
    {
        //CanvasGroup���\���ɂ���
        SetCanvasGroup(false);

        //�X�R�[�v��L����
        scope.gameObject.SetActive(true);
    }

    /// <summary>
    /// �X�R�[�v��`���̂���߂�
    /// </summary>
    public void NotPeekIntoTheScope()
    {
        //CanvasGroup��\������
        SetCanvasGroup(true);

        //�X�R�[�v�𖳌���
        scope.gameObject.SetActive(false);
    }
}
