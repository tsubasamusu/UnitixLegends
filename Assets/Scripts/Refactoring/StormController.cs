using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTween���g�p

namespace yamap 
{
    /// <summary>
    /// Player�̃X�g�[���Ƃ̊֌W
    /// </summary>
    public enum PlayerStormState 
    {
        InStorm,//�X�g�[����
        OutStorm,//�X�g�[���O
    }

    public class StormController : MonoBehaviour 
    {
        [SerializeField]
        private float timeLimit;//��������

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

        private Vector3 firstStormScale;//�X�g�[���̑傫���̏����l

        private float currentScaleRate = 100f;//���݂̃X�g�[���̑傫���̊���(%)

        /// <summary>
        /// �Q�[���J�n����ɌĂяo�����
        /// </summary>
        private void Start() 
        {
            //�X�g�[���̑傫���̏����l��ݒ�
            firstStormScale = transform.localScale;

            //�X�g�[���̏k�����J�n����
            MakeStormSmaller();
        }

        /// <summary>
        /// �X�g�[���̏k�����J�n����
        /// </summary>
        private void MakeStormSmaller() 
        {
            //�������ԓ��ɓ����Łu�X�g�[���̑傫���̊����v��100%����0%�ɂ���
            DOTween.To(() => currentScaleRate, (x) => currentScaleRate = x, 0f, timeLimit).SetEase(Ease.Linear);
        }

        /// <summary>
        /// ���t���[���Ăяo�����
        /// </summary>
        private void Update() 
        {
            //�����ɉ����ăX�g�[�����k��������
            transform.localScale = new Vector3((firstStormScale.x * (currentScaleRate / 100f)), firstStormScale.y, (firstStormScale.z * (currentScaleRate / 100f)));
        }

        /// <summary>
        /// ���g�����u���ɋ��邩�ǂ������ׂ�
        /// </summary>
        /// <param name="myPos">���g�̍��W</param>
        /// <returns>���g�����u���ɂ�����true</returns>
        public bool CheckEnshrine(Vector3 myPos) 
        {
            //���g�̍��W��x-z���ʏ�ŕ\��
            Vector3 pos = Vector3.Scale(myPos, new Vector3(1f, 0f, 1f));

            //�X�g�[���̒����̍��W��(0,0,0)�ɐݒ�
            Vector3 centerPos = Vector3.zero;

            //�X�g�[���̒����܂ł̋����ix-z���ʏ�j���擾
            float length = (pos - centerPos).magnitude;

            //���g�����u���ɂ�����true�A���u�O�ɂ���Ȃ�false��Ԃ�
            return length <= transform.localScale.x / 2f ? true : false;
        }

        /// <summary>
        /// SkyBox �̕ύX
        /// </summary>
        /// <param name="playerStormState">Player�ƃX�g�[���Ƃ̊֌W</param>
        public void ChangeSkyBox(PlayerStormState playerStormState) 
        {
            //��̃}�e���A����ݒ�
            skybox.material = GetMaterialFromStormState(playerStormState);

            /// <summary>
            /// ��̃}�e���A�����擾����
            /// </summary>
            /// <param name="playerStormState">Player�ƃX�g�[���Ƃ̊֌W</param>
            /// <returns>�}�e���A��</returns>
            Material GetMaterialFromStormState(PlayerStormState playerStormState) 
            {
                //Player�ƃX�g�[���Ƃ̊֌W�ɉ����āA�قȂ�����̃}�e���A����Ԃ�
                return playerStormState == PlayerStormState.InStorm ? stormSky : normalSky;
            }
        }        
    }
}