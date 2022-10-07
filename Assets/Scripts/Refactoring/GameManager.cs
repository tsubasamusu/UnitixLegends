using System.Collections;//IEnumerator���g�p
using UnityEngine;
using UnityEngine.SceneManagement;//LoadScene���g�p

namespace yamap 
{
    public class GameManager : MonoBehaviour 
    {
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
            SoundManager.instance.PlaySE(SeName.GameStartSE);

            //PlayerControoler�𖳌���
            playerController.enabled = false;

            //TODO:PlayerController��񊈐������̂ɁA�����ݒ�ł���́H
            playerController.SetUpPlayer(uiManager);

            //UIManager�̏����ݒ���s��
            uiManager.SetUpUIManager(bulletManager, playerController.PlayerHealth);

            //CanvasGroup���\���ɂ���
            uiManager.SetCanvasGroup(false);

            //���b�Z�[�W�𖳌���
            uiManager.SetMessageActive(false);

            //BulletManager�̏����ݒ���s��
            bulletManager.SetUpBulletManager(playerController, temporaryObjectContainerTran);       

            //ItemManager�̏����ݒ���s��
            ItemManager.instance.SetUpItemManager(uiManager, playerController, bulletManager);

            //�L������������
            GameData.instance.KillCount = 0;

            //�Q�[���X�^�[�g���o���I���܂ő҂�
            yield return uiManager.PlayGameStart();

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

            //�X�g�[���ɂ��_���[�W�̊Ď����J�n����
            StartCoroutine(CheckStormDamageAsync());
        }

        /// <summary>
        /// ��s�@�����э~���
        /// </summary>
        public void FallAirPlane() 
        {     
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
        private IEnumerator CheckStormDamageAsync() 
        {
            //��̔���p
            bool skyFlag = false;

            //Player�����S���Ă��Ȃ��Ȃ�J��Ԃ�
            while (playerController.PlayerHealth.PlayerHp > 0) 
            {
                //Player�����u���ɂ��Ȃ��Ȃ�
                while (!stormController.CheckEnshrine(playerController.transform.position)) 
                {
                    //��̔��肪true�Ȃ�
                    if (skyFlag) 
                    {
                        //���V��ɕύX
                        stormController.ChangeSkyBox(PlayerStormState.InStorm);

                        //��̔����false������i���ʂȏ�����h���j
                        skyFlag = false;
                    }

                    //Player��Hp������������
                    playerController.PlayerHealth.UpdatePlayerHp(-stormController.StormDamage);

                    //1�b�҂�
                    yield return new WaitForSeconds(1f);

                    //Player��Hp��0�ȉ��ɂȂ�����
                    if (playerController.PlayerHealth.PlayerHp <= 0) 
                    {
                        //�J��Ԃ��������甲���o��
                        break;
                    }
                }

                //��̔��肪false�Ȃ�
                if (!skyFlag) 
                {
                    //�����ɕύX
                    stormController.ChangeSkyBox(PlayerStormState.OutStorm);

                    //��̔����true������
                    skyFlag = true;
                }

                //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                yield return null;
            }

            //�Q�[���I�[�o�[�������J�n����
            StartCoroutine(MakeGameOverAsync());
        }

        /// <summary>
        /// �Q�[���N���A���o���s��
        /// </summary>
        /// <returns>�҂�����</returns>
        public IEnumerator MakeGameClear() 
        {
            //�Q�[���N���A�����Đ�
            SoundManager.instance.PlaySE(SeName.GameClearSE);

            //�Q�[�����I�����邽�߂̏������s��
            SetUpGameOver();

            //�Q�[���N���A���o���I���܂ő҂�
            yield return StartCoroutine(uiManager.PlayGameClear());

            //Main�V�[����ǂݍ���
            SceneManager.LoadScene("Main");
        }

        /// <summary>
        /// �Q�[���I�[�o�[���o���s��
        /// </summary>
        /// <returns>�҂�����</returns>
        public IEnumerator MakeGameOverAsync() 
        {
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
        private void SetUpGameOver() 
        {
            //�Q�[���I����Ԃɐ؂�ւ���
            isGameOver = true;

            //GameOver��\������
            uiManager.DisplayGameOver();

            //PlayerControoler�𖳌���
            playerController.enabled = false;
        }
    }
}