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
}
