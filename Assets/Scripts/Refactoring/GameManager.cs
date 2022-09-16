using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//LoadScene���g�p
using System.Linq;//Where���\�b�h���g�p
using DG.Tweening.Core.Easing;

namespace yamap {

    public class GameManager : MonoBehaviour {

        [SerializeField]
        private EnemyGenerator enemyGenerator;//EnemyGenerator

        [SerializeField]
        private AirPlaneController airplaneController;//AirplaneController

        [SerializeField]
        private PlayerController playerController;//PlayerControoler

        [SerializeField]
        private UIManager uiManager;//UIManager

        [SerializeField]
        private StormController stormController;//StormController

        [SerializeField]
        private BulletManager bulletManager;

        [SerializeField]
        private Transform temporaryObjectContainerTran;

        //[SerializeField]
        //private SoundManager soundManager;//SoundManager

        private bool isGameOver;//�Q�[��������Ԃ��ǂ���

        public bool IsGameOver//isGameOver�ϐ��p�̃v���p�e�B
        {
            get { return isGameOver; }//�O������͎擾�����݂̂��\��
        }

        /// <summary>
        /// �Q�[���J�n����ɌĂяo�����
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator Start() {
            //�Q�[���J�n�����Đ�
            SoundManager.instance.PlaySE(SeName.GameStartSE);

            //PlayerControoler�𖳌���
            playerController.enabled = false;
            playerController.SetUpPlayer(uiManager);

            uiManager.SetUpUIManager(bulletManager, playerController.PlayerHealth);

            //CanvasGroup���\���ɂ���
            uiManager.SetCanvasGroup(false);

            //���b�Z�[�W�𖳌���
            uiManager.SetMessageActive(false);

            bulletManager.SetUpBulletManager(playerController, temporaryObjectContainerTran);       

            ItemManager.instance.SetUpItemManager(uiManager, playerController, bulletManager);

            //�L������������
            GameData.instance.KillCount = 0;

            //�Q�[���X�^�[�g���o���s��
            yield return StartCoroutine(uiManager.PlayGameStart());

            Debug.Log("�X�^�[�g���o�I��");

            //���b�Z�[�W��L����
            uiManager.SetMessageActive(true);

            //��s�@�Ɋւ���ݒ���s��
            airplaneController.SetUpAirplane();

            //���b�Z�[�W��\��
            uiManager.SetMessageText("Tap\n'Space'\nTo Fall", Color.blue);

            //Player�̍s���̐�����J�n
            StartCoroutine(airplaneController.ControlPlayerMovement(this, playerController));

            //Enemy�̐������J�n
            StartCoroutine(enemyGenerator.GenerateEnemy(uiManager, playerController));

            //�A�C�e���X���b�g�̐ݒ蓙���s��
            uiManager.SetUpItemSlots();

            // HP �Ď�
            StartCoroutine(CheckStormDamageAsync());
        }

        /// <summary>
        /// ��s�@�����э~���
        /// </summary>
        /// <returns>�҂�����</returns>
        public void FallAirPlane() {     
            //���b�Z�[�W�̃e�L�X�g����ɂ���
            uiManager.SetMessageText("", Color.black);

            //PlayerController��L����
            playerController.enabled = true;

            //CanvasGroup��\��
            uiManager.SetCanvasGroup(true);

            //�e�L�X�g�̕\���̍X�V���J�n
            StartCoroutine(uiManager.UpdateText());
        }

        /// <summary>
        /// �X�g�[���ɂ��_���[�W���󂯂邩�ǂ������ׂ�
        /// </summary>
        /// <returns>�҂�����</returns>
        private IEnumerator CheckStormDamageAsync() {
            Debug.Log("Hp �̊Ď��X�^�[�g");

            //��̔���
            bool skyFlag = false;

            //�Q�[���I����Ԃł͂Ȃ��Ȃ�A�J��Ԃ����
            while (playerController.PlayerHealth.PlayerHp > 0) {
                //Player�����u���ɂ��炸
                while (!stormController.CheckEnshrine(playerController.transform.position)) {
                    //��̔��肪true�Ȃ�
                    if (skyFlag) {

                        //���V��ɕύX
                        stormController.ChangeSkyBox(PlayerStormState.InStorm);

                        //��̔����false������
                        skyFlag = false;
                    }

                    //Player��Hp������������
                    playerController.PlayerHealth.UpdatePlayerHp(-stormController.StormDamage);

                    //1�b�҂�
                    yield return new WaitForSeconds(1f);

                    // �c�� HP �m�F
                    if (playerController.PlayerHealth.PlayerHp <= 0) {
                        break;
                    }
                }

                //��̔��肪false�Ȃ�
                if (!skyFlag) {
                    //�����ɕύX
                    stormController.ChangeSkyBox(PlayerStormState.OutStorm);

                    //��̔����true������
                    skyFlag = true;
                }

                //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                yield return null;
            }

            Debug.Log("Hp �̊Ď��I��");

            //Player�̗̑͂�0�ɂȂ�����Q�[���I�[�o�[���o���s��
            StartCoroutine(MakeGameOverAsync());
        }

        /// <summary>
        /// �Q�[���N���A���o���s��
        /// </summary>
        /// <returns>�҂�����</returns>
        public IEnumerator MakeGameClear() {
            //�Q�[���N���A�����Đ�
            SoundManager.instance.PlaySE(SeName.GameClearSE);

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
        public IEnumerator MakeGameOverAsync() {
            //�Q�[���I�[�o�[�����Đ�
            SoundManager.instance.PlaySE(SeName.GameOverSE);

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
        private void SetUpGameOver() {
            //�Q�[���I����Ԃɐ؂�ւ���
            isGameOver = true;

            // UIManager ��3�񖽗߂��Ă���̂ŁA��p�̃��\�b�h���P�AUIManager ���ɍ���ẮH
            // ��������΁A�����炩��̖��߂�1��ōς�
            uiManager.DisplayGameOver();

            //PlayerControoler�𖳌���
            playerController.enabled = false;
        }
    }
}