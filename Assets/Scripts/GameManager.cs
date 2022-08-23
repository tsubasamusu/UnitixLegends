using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadScene���g�p

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private EnemyGenerator enemyGenerator;//EnemyGenerator

    [SerializeField]
    private AirplaneController airplaneController;//AirplaneController

    [SerializeField]
    private UIManager uiManager;//UIManager

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
        //CanvasGroup���\���ɂ���
        uiManager.SetCanvasGroup(false);

        //�Q�[���X�^�[�g���o���s��
        yield return StartCoroutine(uiManager.PlayGameStart());

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
    /// <returns></returns>
    public IEnumerator MakeGameClear()
    {
        //�Q�[���I����Ԃɐ؂�ւ���
        isGameOver = true;

        //CanvasGroup���\���ɂ���
        uiManager.SetCanvasGroup(false);

        //�Q�[���N���A���o���s��
        yield return StartCoroutine(uiManager.PlayGameClear());

        SceneManager.LoadScene("Main");
    }
}
