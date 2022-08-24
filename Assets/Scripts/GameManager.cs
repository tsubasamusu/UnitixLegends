using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadScene���g�p
using System.Linq;//Where���\�b�h���g�p

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private EnemyGenerator enemyGenerator;//EnemyGenerator

    [SerializeField]
    private AirplaneController airplaneController;//AirplaneController

    [SerializeField]
    private PlayerController playerController;//PlayerControoler

    [SerializeField]
    private UIManager uiManager;//UIManager

    [SerializeField]
    private SoundManager soundManager;//SoundManager

    private bool isGameOver;//�Q�[��������Ԃ��ǂ���

    public bool IsGameOver//isGameOver�ϐ��p�̃v���p�e�B
    {
        get { return isGameOver; }//�O������͎擾�����݂̂��\��
    }

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator Start()
    {
        //�Q�[���J�n�����Đ�
        soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.GameStartSE));

        //PlayerControoler�𖳌���
        playerController.enabled = false;

        //CanvasGroup���\���ɂ���
        uiManager.SetCanvasGroup(false);

        //���b�Z�[�W�𖳌���
        uiManager.SetMessageActive(false);

        //�L������������
        GameData.instance.KillCount = 0;

        //�Q�[���X�^�[�g���o���s��
        yield return StartCoroutine(uiManager.PlayGameStart());

        //���b�Z�[�W��L����
        uiManager.SetMessageActive(true);

        //��s�@�Ɋւ���ݒ���s��
        airplaneController.SetUpAirplane();

        //Player�̍s���̐�����J�n
        StartCoroutine(airplaneController.ControlPlayerMovement());

        //Enemy�̐������J�n
        StartCoroutine(enemyGenerator.GenerateEnemy());

        //�A�C�e���X���b�g�̐ݒ蓙���s��
        uiManager.SetUpItemSlots();
    }

    /// <summary>
    /// �Q�[���N���A���o���s��
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator MakeGameClear()
    {
        //�Q�[���N���A�����Đ�
        soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.GameClearSE));

        //�Q�[�����I�����邽�߂̏������s��
        SetUpGameOver();

        //�Q�[���N���A���o���s��
        yield return StartCoroutine(uiManager.PlayGameClear());

        //Main�V�[����ǂݍ���
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// �Q�[���I�[�o�[���o���s��
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator MakeGameOver()
    {
        //�Q�[���I�[�o�[�����Đ�
        soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.GameOverSE));

        //�Q�[�����I�����邽�߂̏������s��
        SetUpGameOver();

        //�Q�[���I�[�o�[���o���s��
        yield return StartCoroutine(uiManager.PlayGameOver());

        //Main�V�[����ǂݍ���
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// �Q�[�����I�����邽�߂̏������s��
    /// </summary>
    private void SetUpGameOver()
    {
        //�Q�[���I����Ԃɐ؂�ւ���
        isGameOver = true;

        //CanvasGroup���\���ɂ���
        uiManager.SetCanvasGroup(false);

        //�S�Ẵt���[�g�\�����\���ɂ���
        uiManager.SetFloatingMessagesNotActive();

        //���b�Z�[�W�𖳌���
        uiManager.SetMessageActive(false);
    }
}
