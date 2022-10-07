using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;//CinemachineFreeLook���g�p

namespace yamap {

    public class CinemachineManager : MonoBehaviour {

        [SerializeField]
        private CinemachineFreeLook airplaneCamera;//��s�@���_�J����

        [SerializeField]
        private PlayerController playerController;//PlayerController

        [SerializeField]
        private Transform miniMapBackgroundTran;//�~�j�}�b�v�w�i�̈ʒu

        private Transform mainCameraTran;//���C���J�����̈ʒu

        [SerializeField]
        private Transform playerCharacterTran;//Player�̃L�����N�^�[�̈ʒu

        [SerializeField]
        private GameObject playerCharacterbody;//Player�̃L�����N�^�[�̑�

        private float angle;//Player�̃L�����N�^�[�����_���Ղ�p�x

        /// <summary>
        /// �Q�[���J�n����ɌĂяo�����
        /// </summary>
        private void Start() 
        {
            //���C���J�����̈ʒu�����擾
            mainCameraTran = Camera.main.transform;

            //Player�̃L�����N�^�[�Ǝ��_�����p�x���擾
            angle = playerCharacterTran.eulerAngles.y + 90f;
        }

        /// <summary>
        /// ���t���[���Ăяo�����
        /// </summary>
        private void Update() 
        {
            //PlayerController�������Ȃ�
            if (playerController.enabled == false) 
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�~�j�}�b�v�̔w�i�̈ʒu�����Player�̈ʒu�ɍ��킹��
            miniMapBackgroundTran.position = new Vector3(playerController.transform.position.x, miniMapBackgroundTran.position.y, playerController.transform.position.z);

            //Player�̃L�����N�^�[�����E���Ղ��Ă��邩�ǂ������ׂ�
            bool set = CheckIntercepted() ? false : true;

            //Player�̃L�����N�^�[�̗L������������؂�ւ���
            SetPlayerCharacterActive(set);
        }

        /// <summary>
        /// Player�̃L�����N�^�[�����E���Ղ��Ă��邩�ǂ������ׂ�
        /// </summary>
        /// <returns>Player�̃L�����N�^�[�����E���Ղ��Ă�����true</returns>
        private bool CheckIntercepted() 
        {
            //�J�����̊p�x�����͈͓��Ȃ�
            if (mainCameraTran.eulerAngles.y >= angle - 20f && mainCameraTran.eulerAngles.y <= angle + 20f) 
            {
                //true��Ԃ�
                return true;
            }
            //�J�����̊p�x�����͈͓��Ȃ�
            else if (mainCameraTran.eulerAngles.y >= (angle + 180f) - 20f && mainCameraTran.eulerAngles.y <= (angle + 180f) + 20f) 
            {
                //true��Ԃ�
                return true;
            }

            //false��Ԃ�
            return false;
        }

        /// <summary>
        /// ��s�@���_�J�����̗D�揇�ʂ�ݒ�
        /// </summary>
        /// <param name="airplaneCameraPriority">�D�揇��</param>
        public void SetAirplaneCameraPriority(int airplaneCameraPriority) 
        {
            //���������ɁA��s�@���_�J�����̗D�揇�ʂ�ݒ�
            airplaneCamera.Priority = airplaneCameraPriority;
        }

        /// <summary>
        /// Player�̃L�����N�^�[�̗L������������؂�ւ���
        /// </summary>
        /// <param name="set">�L���ɂ���Ȃ�true</param>
        public void SetPlayerCharacterActive(bool set) 
        {
            //���������ɁAPlayer�̃L�����N�^�[�̗L�����A��������؂�ւ���
            playerCharacterbody.SetActive(set);
        }
    }
}