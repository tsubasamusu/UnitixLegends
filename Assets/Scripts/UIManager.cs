using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using UnityEngine.UI;//UI���g�p
using DG.Tweening;//DOTween���g�p
using UnityEngine.SceneManagement;//�V�[���̃��[�h���g�p

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
    private Text txtBulletCount;//�c�e���e�L�X�g

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
    private Transform canvasTran;//Canvas��transform

    [SerializeField]
    private GameObject itemSlotSetPrefab;//�A�C�e���X���b�g�Z�b�g�̃v���t�@�u

    private List<Image> imgItemSlotList = new List<Image>();//�A�C�e���X���b�g�̃C���[�W�̃��X�g

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    private void Start()
    {
        //�A�C�e���X���b�g�𐶐�
        GenerateItemSlots(5);
    }

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�t���[�����[�g���v�Z���A�\�����X�V����
        UpdateFpsText();
    }

    /// <summary>
    /// �Q�[���X�^�[�g���o���s��
    /// </summary>
    public IEnumerator PlayGameStart()
    {
        //���E�𔒐F�ɐݒ�
        eventHorizon.color = new Color(255.0f, 255.0f, 255.0f,0.0f);

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

        //GameManager����Q�[���J�n��Ԃɐ؂�ւ���
    }

    /// <summary>
    /// �Q�[���N���A���o���s��
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayGameClear()
    {
        //���E�𔒐F�ɐݒ�
        eventHorizon.color = new Color(255.0f, 255.0f, 255.0f, 0.0f);

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

        //Main�V�[����ǂݍ���
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// �Q�[���I�[�o�[���o���s��
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayGameOver()
    {
        //���E�����F�ɐݒ�
        eventHorizon.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        //1.0�b�����Ď��E�����S�ɈÂ�����
        eventHorizon.DOFade(1.0f, 1.0f);

        //���E�����S�ɈÓ]����܂ő҂�
        yield return new WaitForSeconds(1.0f);

        //3.0�b�����āuGameOver�v��\��
        txtGameOver.DOText("GameOver",3.0f);

        //�uGameOver�̕\�����I�������ƁA�����1.0�b�ԑ҂�
        yield return new WaitForSeconds(4.0f);

        //Main�V�[����ǂݍ���
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// ���E���w�肳�ꂽ���Ԃ����Â�����
    /// </summary>
    public IEnumerator SetEventHorizonBlack(float time)
    {
        //���E�����F�ɐݒ�
        eventHorizon.color = new Color(0.0f, 0.0f, 0.0f,0.0f);

        //1.0�b�����Ď��E�����S�ɈÂ�����
        eventHorizon.DOFade(1.0f, 1.0f);

        //�����Ŏw�肳�ꂽ���Ԃ������E���Â��܂ܕۂ�
        yield return new WaitForSeconds(time);

        //0.5�b�����Ď��E�����ɖ߂�
        eventHorizon.DOFade(0.0f, 0.5f);
    }

    /// <summary>
    /// ��e�����ۂ̎��E�̏���
    /// </summary>
    public IEnumerator AttackEventHorizon()
    {
        //���E��ԐF�ɐݒ�
        eventHorizon.color = new Color(255.0f, 0.0f, 0.0f, 0.0f);

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
    public void UpdateHpSliderValue(float currentValue,float updateValue)
    {
        //0.5�b�����đ̗͗p�X���C�_�[���X�V����
        hpSlider.DOValue((currentValue + updateValue)/100.0f, 0.5f);
    }

    /// <summary>
    /// CanvasGroup�̕\���A��\����؂�ւ���
    /// </summary>
    /// <param name="set"></param>
    public void SetCanvasGroup(bool isSetting)
    {
        //���������ɁACanvasGroup�̓����x��ݒ�
        canvasGroup.alpha = isSetting ? 1.0f : 0.0f;
    }

    /// <summary>
    /// �c�e���̕\���̍X�V���s��
    /// </summary>
    public void UpdateTxtBulletCount(int bulletCount)
    {
        //���������ɁA�c�e���̃e�L�X�g��ݒ�
       txtBulletCount.text=bulletCount.ToString();
    }

    /// <summary>
    /// �t���[�g�\���̐������s��
    /// </summary>
    public IEnumerator GenerateFloatingMessage(string messageText,Color color)
    {
        //�t���[�g�\���𐶐�
        Text txtFloatingMessage = Instantiate(floatingMessagePrefab);

        //���������t���[�g�\���̐e��Canvas�ɐݒ�
        txtFloatingMessage.gameObject.transform.SetParent(canvasTran);

        //���������ɁA�t���[�g�\���̃e�L�X�g��ݒ�
        txtFloatingMessage.text = messageText;

        //���������ɁA�t���[�g�\���̐F��ݒ�
        txtFloatingMessage.color = color;

        //�t���[�g�\���̏ꏊ�������ʒu��ݒ�
        txtFloatingMessage.gameObject.transform.position= new Vector3(900.0f,200.0f,0);

        //�t���[�g�\���̑傫����������
        txtFloatingMessage.gameObject.transform.localScale = Vector3.one;

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
        //�t���[�����[�g���v�Z���A�擾����
        float fps = 1f / Time.deltaTime;

        //�\�����X�V
        txtFps.text=fps.ToString("F0")+"fps";
    }

    /// <summary>
    /// �w�肳�ꂽ�������A�C�e���X���b�g�𐶐�����
    /// </summary>
    /// <param name="generateNumber">�A�C�e���X���b�g�̐�</param>
    public void GenerateItemSlots(int generateNumber)
    {
        //�����Ŏw�肳�ꂽ�񐔂��������������J��Ԃ�
        for (int i = 0; i < generateNumber; i++)
        {
            //�A�C�e���X���b�g�𐶐�
            GameObject itemSlot = Instantiate(itemSlotSetPrefab);

            //���������A�C�e���X���b�g�̐e��canvasGroup�ɐݒ�
            itemSlot.transform.SetParent(canvasGroup.transform);

            //���������A�C�e���X���b�g�̑傫����ݒ�
            itemSlot.transform.localScale = Vector3.one;

            //���������A�C�e���X���b�g�̈ʒu��ݒ�
            itemSlot.transform.localPosition = new Vector3((-1 * (200 + (50 * (generateNumber - 5)))) + (100 * i), 0, 0);

            //���������A�C�e���X���b�g�̎q�I�u�W�F�N�g��Image���擾
            if (itemSlot.transform.GetChild(2).TryGetComponent<Image>(out Image imgItem))//null�G���[���
            {
                //�擾�����C���[�W�𓧖��ɐݒ�
                imgItem.DOFade(0.0f, 0.01f);
            }

            //���������A�C�e���X���b�g�̃C���[�W�����X�g�ɒǉ�
            imgItemSlotList.Add(imgItem);

            //Player���������Ă���A�C�e���̃��X�g�̗v�f���A�A�C�e���X���b�g�̐��������
            GameData.instance.playerItemList.Add(null);
        }
    }

    /// <summary>
    /// �A�C�e����Sprite��ݒ肷��
    /// </summary>
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
    public void SetMessageText(string text)
    {
        //���������ɁB���b�Z�[�W�̃e�L�X�g��ݒ�
        txtMessage.text=text;
    }
}
