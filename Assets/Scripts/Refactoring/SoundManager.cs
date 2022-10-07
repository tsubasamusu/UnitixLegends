using UnityEngine;

namespace yamap 
{
    public class SoundManager : MonoBehaviour 
    {
        public static SoundManager instance;

        [SerializeField]
        private SoundDataSO soundDataSO;//SoundDataSO

        private AudioSource[] seSources;//���ʉ��p��AudioSource�̔z��

        [SerializeField]
        private AudioSource bgmSource;//BGM�p��AudioSource

        private int seAudioCount = 16;//���ʉ��̐�

        /// <summary>
        /// Start���\�b�h���O�ɌĂяo�����
        /// </summary>
        void Awake() 
        {
            //�ȉ��A�V���O���g���ɕK�{�̋L�q
            if (instance == null) 
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } 
            else 
            {
                Destroy(gameObject);
            }

            //���ʉ��p��AudioSource�̔z����쐬
            seSources = new AudioSource[seAudioCount];

            //���ʉ��̐������J��Ԃ�
            for (int i = 0; i < seAudioCount; i++) 
            {
                //TODO;����͉������Ă���H
                seSources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        /// <summary>
        /// ���ʉ���AudioSource���g���čĐ�����
        /// </summary>
        /// <param name="soundData">���ʉ��̃f�[�^</param>
        /// <param name="loop">���ʉ����J��Ԃ����ǂ���</param>
        /// <param name="volume">���ʉ��̑傫��</param>
        public void PlaySE(SeName soundEffectName, bool isLoop = false, float volume = 1f) 
        {
            //���ʉ��̃f�[�^���擾
            SeData seData = GetSoundEffectData(soundEffectName);

            //���ʉ��p��AudioSource�̔z��̗v�f��1�����o��
            foreach (var audioSource in seSources)
            {
                //AudioSource���Đ����Ȃ�
                if (audioSource.isPlaying) 
                {
                    //���̌J��Ԃ������֔�΂�
                    continue;
                } 
                //AudioSource���Đ����ł͂Ȃ��Ȃ�
                else 
                {
                    //���ʉ����J��Ԃ����ǂ�����ݒ�
                    audioSource.loop = isLoop;

                    //���ʉ����̑傫����ݒ�
                    audioSource.volume = volume;

                    //���ʉ����Đ�
                    audioSource.PlayOneShot(seData.audioClip);

                    //�J��Ԃ��������I������
                    break;
                }
            }

            /// <summary>
            /// �w�肵�����ʉ��̃f�[�^���擾����
            /// </summary>
            /// <param name="searchSeName">���ʉ��̖��O</param>
            /// <returns>�w�肵�����ʉ��̃f�[�^</returns>
            SeData GetSoundEffectData(SeName searchSeName) 
            {
                //�w�肳�ꂽ���ʉ��̃f�[�^��Ԃ�
                return soundDataSO.seList.Find(x => x.seName == searchSeName);
            }
        }

        /// <summary>
        /// AudioSource����̏�Ԃɂ���
        /// </summary>
        public void ClearAudioSource() 
        {
            //���ʉ��p��AudioSource�̔z��̗v�f��1�����o��
            for (int i = 0; i < seSources.Length; i++) 
            {
                //AudioSource���Đ����Ȃ�
                if (seSources[i].isPlaying) 
                {
                    //�N���b�v��null������
                    seSources[i].clip = null;
                }
            }      
        }

        /// <summary>
        /// BGM���Đ�����
        /// </summary>
        /// <param name="bgmName">BGM�̖��O</param>
        /// <param name="isLoop">�J��Ԃ����ǂ���</param>
        /// <param name="volume">�{�����[��</param>
        public void PlayBGM(BgmName bgmName, bool isLoop, float volume = 1f) 
        {
            //BGM�̃f�[�^���擾
            BgmData bgmData = GetBgmData(bgmName);


            /// <summary>
            /// �w�肵�����O��BGM�̃f�[�^���擾����
            /// </summary>
            /// <param name="searchbgmName">BGM�̖��O</param>
            /// <returns>�w�肵�����O��BGM�̃f�[�^</returns>
            BgmData GetBgmData(BgmName searchbgmName) 
            {
                //�w�肳�ꂽBGM�̃f�[�^��Ԃ�
                return soundDataSO.bgmList.Find(x => x.bgmName == searchbgmName);
            }

            //���ʉ����J��Ԃ����ǂ�����ݒ�
            bgmSource.loop = isLoop;

            //���ʉ����̑傫����ݒ�
            bgmSource.volume = volume;

            //�I�[�f�B�I�N���b�v��ݒ�
            bgmSource.clip = bgmData.audioClip;

            //BGM���Đ�����
            bgmSource.Play();            
        }
    }
}