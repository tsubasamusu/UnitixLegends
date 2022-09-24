using UnityEngine;

namespace yamap 
{
    public class SoundManager : MonoBehaviour 
    {
        public static SoundManager instance;

        [SerializeField]
        private SoundDataSO soundDataSO;//SoundDataSO

        private AudioSource[] seSources;//効果音用のAudioSourceの配列

        [SerializeField]
        private AudioSource bgmSource;//BGM用のAudioSource

        private int seAudioCount = 16;//効果音の数

        /// <summary>
        /// Startメソッドより前に呼び出される
        /// </summary>
        void Awake() 
        {
            //以下、シングルトンに必須の記述
            if (instance == null) 
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } 
            else 
            {
                Destroy(gameObject);
            }

            //効果音用のAudioSourceの配列を作成
            seSources = new AudioSource[seAudioCount];

            //効果音の数だけ繰り返す
            for (int i = 0; i < seAudioCount; i++) 
            {
                //TODO;これは何をしている？
                seSources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        /// <summary>
        /// 効果音をAudioSourceを使って再生する
        /// </summary>
        /// <param name="soundData">効果音のデータ</param>
        /// <param name="loop">効果音を繰り返すかどうか</param>
        /// <param name="volume">効果音の大きさ</param>
        public void PlaySE(SeName soundEffectName, bool isLoop = false, float volume = 1f) 
        {
            //効果音のデータを取得
            SeData seData = GetSoundEffectData(soundEffectName);

            //効果音用のAudioSourceの配列の要素を1つずつ取り出す
            foreach (var audioSource in seSources)
            {
                //AudioSourceが再生中なら
                if (audioSource.isPlaying) 
                {
                    //次の繰り返し処理へ飛ばす
                    continue;
                } 
                //AudioSourceが再生中ではないなら
                else 
                {
                    //効果音を繰り返すかどうかを設定
                    audioSource.loop = isLoop;

                    //効果音音の大きさを設定
                    audioSource.volume = volume;

                    //効果音を再生
                    audioSource.PlayOneShot(seData.audioClip);

                    //繰り返し処理を終了する
                    break;
                }
            }

            /// <summary>
            /// 指定した効果音のデータを取得する
            /// </summary>
            /// <param name="searchSeName">効果音の名前</param>
            /// <returns>指定した効果音のデータ</returns>
            SeData GetSoundEffectData(SeName searchSeName) 
            {
                //指定された効果音のデータを返す
                return soundDataSO.seList.Find(x => x.seName == searchSeName);
            }
        }

        /// <summary>
        /// AudioSourceを空の状態にする
        /// </summary>
        public void ClearAudioSource() 
        {
            //効果音用のAudioSourceの配列の要素を1つずつ取り出す
            for (int i = 0; i < seSources.Length; i++) 
            {
                //AudioSourceが再生中なら
                if (seSources[i].isPlaying) 
                {
                    //クリップにnullを入れる
                    seSources[i].clip = null;
                }
            }      
        }

        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="bgmName">BGMの名前</param>
        /// <param name="isLoop">繰り返すかどうか</param>
        /// <param name="volume">ボリューム</param>
        public void PlayBGM(BgmName bgmName, bool isLoop, float volume = 1f) 
        {
            //BGMのデータを取得
            BgmData bgmData = GetBgmData(bgmName);


            /// <summary>
            /// 指定した名前のBGMのデータを取得する
            /// </summary>
            /// <param name="searchbgmName">BGMの名前</param>
            /// <returns>指定した名前のBGMのデータ</returns>
            BgmData GetBgmData(BgmName searchbgmName) 
            {
                //指定されたBGMのデータを返す
                return soundDataSO.bgmList.Find(x => x.bgmName == searchbgmName);
            }

            //効果音を繰り返すかどうかを設定
            bgmSource.loop = isLoop;

            //効果音音の大きさを設定
            bgmSource.volume = volume;

            //オーディオクリップを設定
            bgmSource.clip = bgmData.audioClip;

            //BGMを再生する
            bgmSource.Play();            
        }
    }
}