using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
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
    private Slider hpSlider;//�̗̓X���C�_�[

    [SerializeField]
    private CanvasGroup canvasGroup;//CanvasGroup

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
}
