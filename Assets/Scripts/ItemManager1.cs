using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yamap {

    public class ItemManager : MonoBehaviour {

        public static ItemManager instance;

        [SerializeField]
        private ItemDataSO itemDataSO;//ItemDataSO

        [SerializeField]
        private UIManager uIManager;//UIManager

        [SerializeField]
        private BulletManager bulletManager;//BulletManager

        [SerializeField]
        private PlayerController playerController;//PlayerController

        [SerializeField]
        private PlayerHealth playerHealth;//PlayerHealth

        [SerializeField]
        private Transform itemTrans;//�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[

        [SerializeField]
        private Transform generateItemPosPrefab;//�A�C�e���̐����ʒu�̃v���t�@�u

        [SerializeField]
        private Transform playerTran;//Player�̈ʒu���

        [SerializeField]
        private float itemRotSpeed;//�A�C�e���̉�]���x

        [SerializeField]
        private int maxItemTranCount;//��������A�C�e���̐����ʒu�̍ő吔

        [HideInInspector]
        public List<ItemDataSO.ItemData> generatedItemDataList = new List<ItemDataSO.ItemData>();//���������A�C�e���̃f�[�^�̃��X�g

        [HideInInspector]
        public List<Transform> generatedItemTranList = new List<Transform>();//�A�C�e���̐����ʒu�̃��X�g

        [HideInInspector]
        public List<ItemDataSO.ItemData> playerItemList = new List<ItemDataSO.ItemData>();//Player���������Ă���A�C�e���̃��X�g

        private bool isFull;//Player�̏��L�������e�I�[�o�[���ǂ���

        public bool IsFull//isFull�ϐ��p�̃v���p�e�B
        {
            get { return isFull; }//�O������͎擾�����݂̂��\��
        }

        private int nearItemNo;//Player�̍ł��߂��ɂ���A�C�e���̔ԍ�

        public int NearItemNo//nearItemNo�ϐ��p�̃v���p�e�B
        {
            get { return nearItemNo; }//�O������͎擾�����݂̂��\��
        }

        private float lengthToNearItem;//�uPlayer�̍ł��߂��ɂ���A�C�e���v�ƁuPlayer�v�Ƃ̋���

        public float LengthToNearItem//lengthToNearItem�ϐ��p�̃v���p�e�B
        {
            get { return lengthToNearItem; }//�O������͎擾�����݂̂��\��
        }

        private int selectedItemNo = 1;//�g�p���Ă���A�C�e���̔ԍ�

        public int SelectedItemNo//useItemNo�ϐ��p�̃v���p�e�B
        {
            get { return selectedItemNo; }//�O������͎擾�����̂݉\��
        }


        void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }


        /// <summary>
        /// �Q�[���J�n����ɌĂяo�����
        /// </summary>
        private void Start() {
            //�A�C�e���𐶐�
            GenerateItem();
        }

        /// <summary>
        /// ���t���[���Ăяo�����
        /// </summary>
        private void Update() {
            //Player�̍ł��߂��ɂ���A�C�e���̏����擾
            GetInformationOfNearItem(playerTran.position, true);
        }

        /// <summary>
        /// �w�肵���A�C�e���̃f�[�^���擾����
        /// </summary>
        /// <param name="itemName">�A�C�e���̖��O</param>
        /// <returns>�A�C�e���̃f�[�^</returns>
        public ItemDataSO.ItemData GetItemData(ItemDataSO.ItemName itemName) {
            //�A�C�e���̃f�[�^�̃��X�g����v�f��1�����o��
            foreach (ItemDataSO.ItemData itemData in itemDataSO.itemDataList) {
                //���o�����v�f�̖��O�������Ɠ�����������
                if (itemData.itemName == itemName) {
                    //���o�����v�f��Ԃ�
                    return itemData;
                }
            }

            //null��Ԃ�
            return null;
        }

        /// <summary>
        /// �A�C�e���𐶐�����
        /// </summary>
        public void GenerateItem() {
            //�A�C�e���̐����ʒu�𐶐�����
            GenerateItemTran();

            //�A�C�e���̐����ʒu�̃��X�g���쐬
            CreateGeneratedItemTranList();

            //�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[�̎q�I�u�W�F�N�g�����J��Ԃ�
            for (int i = 0; i < itemTrans.childCount; i++) {
                //0����2�܂ł̃����_���Ȑ������擾
                int px = Random.Range(0, 3);

                //�擾����������0�ł͂Ȃ��Ȃ�i2/3�̊m����Enemy���g���镐����o��������j
                if (px != 0) {
                    //5����7�܂ł̃����_���Ȑ������擾
                    int py = Random.Range(5, 8);

                    //�w�肵���ʒu�Ƀ����_���ȃA�C�e���𐶐����A�A�j���[�V�������J�n
                    StartCoroutine(PlayItemAnimation(Instantiate(itemDataSO.itemDataList[py].prefab, generatedItemTranList[i])));

                    //���������A�C�e���̃f�[�^�����X�g�ɒǉ�
                    generatedItemDataList.Add(itemDataSO.itemDataList[py]);

                    //���̌J��Ԃ������ֈڂ�
                    continue;
                }

                //Enemy���g�p�ł��Ȃ��A�C�e���̔ԍ�
                int[] randomNumbers = { 1, 2, 3, 4, 8, 9, 10, 11, 12, 13 };

                //��L�̔z�񂩂烉���_���ȗv�f���擾
                int pz = randomNumbers[Random.Range(0, 10)];

                //�w�肵���ʒu�Ƀ����_���ȃA�C�e���𐶐����A�A�j���[�V�������J�n
                StartCoroutine(PlayItemAnimation(Instantiate(itemDataSO.itemDataList[pz].prefab, generatedItemTranList[i])));

                //���������A�C�e���̃f�[�^�����X�g�ɒǉ�
                generatedItemDataList.Add(itemDataSO.itemDataList[pz]);
            }
        }

        /// <summary>
        /// �A�C�e���̐����ʒu�𐶐�����
        /// </summary>
        private void GenerateItemTran() {
            //��������A�C�e���̐����ʒu�̍ő吔�����J��Ԃ�
            for (int i = 0; i < maxItemTranCount; i++) {
                //�A�C�e���̐����ʒu��ݒ�
                Transform generateItemPosTran = Instantiate(generateItemPosPrefab);

                //���������A�C�e���̐����ʒu�̐e��ݒ�
                generateItemPosTran.SetParent(itemTrans);

                //0����3�܂ł̃����_���Ȑ������擾
                int px = Random.Range(0, 4);

                //-120����120�܂ł̃����_���ȏ������擾
                float py = Random.Range(-120f, 120f);

                //px�̒l�ɉ����ď�����ύX
                switch (px) {
                    case 0:
                        generateItemPosTran.localPosition = new Vector3(py, 0f, -120f);
                        break;

                    case 1:
                        generateItemPosTran.localPosition = new Vector3(120f, 0f, py);
                        break;

                    case 2:
                        generateItemPosTran.localPosition = new Vector3(py, 0f, 120f);
                        break;

                    case 3:
                        generateItemPosTran.localPosition = new Vector3(-120f, 0f, py);
                        break;
                }
            }
        }

        /// <summary>
        /// �A�C�e���̐����ʒu�̃��X�g���쐬����
        /// </summary>
        private void CreateGeneratedItemTranList() {
            //�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[�̎q�I�u�W�F�N�g�̐������J��Ԃ�
            for (int i = 0; i < itemTrans.childCount; i++) {
                //�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[�̎q�I�u�W�F�N�g�̈ʒu�������X�g�ɒǉ����Ă���
                generatedItemTranList.Add(itemTrans.GetChild(i).transform);
            }
        }

        /// <summary>
        /// �ł��߂��ɂ���A�C�e���̔ԍ��ƈʒu���𓾂�
        /// </summary>
        /// <param name="myPos">�������g�̍��W</param>
        /// <param name="isPlayerPos">��������Player�̍��W���ǂ���</param>
        public void GetInformationOfNearItem(Vector3 myPos, bool isPlayerPos) {
            //null�G���[���
            if (generatedItemTranList.Count <= 0) {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�A�C�e���̔ԍ�
            int itemNo = 0;

            //���X�g��0�Ԃ̗v�f�̍��W��nearPos�ɉ��ɓo�^
            Vector3 nearPos = generatedItemTranList[0].position;

            //���X�g�̗v�f�������J��Ԃ�
            for (int i = 0; i < generatedItemTranList.Count; i++) {
                //�J��Ԃ������œ����v�f��null�Ȃ�
                if (generatedItemTranList[i] == null) {
                    //���̌J��Ԃ������Ɉڂ�
                    continue;
                }

                //���X�g��i�Ԃ̗v�f�̍��W��pos�ɓo�^
                Vector3 pos = generatedItemTranList[i].position;

                //���o�^�����v�f�ƁAfor���œ����v�f�́AmyPos�Ƃ̋������r
                if (Vector3.Scale((pos - myPos), new Vector3(1, 0, 1)).magnitude < Vector3.Scale((nearPos - myPos), new Vector3(1, 0, 1)).magnitude) {
                    //Player�̍ł��߂��ɂ���A�C�e���̔ԍ���i�œo�^
                    itemNo = i;

                    //nearPos���ēo�^
                    nearPos = pos;
                }
            }

            //myPos��Player�̍��W�Ȃ�
            if (isPlayerPos) {
                //Player�̍ł��߂��ɂ���A�C�e���̔ԍ���o�^
                nearItemNo = itemNo;

                //�uPlayer�̍ł��߂��ɂ���A�C�e���v�ƁuPlayer�v�Ƃ̋�����o�^
                lengthToNearItem = Vector3.Scale((nearPos - myPos), new Vector3(1, 0, 1)).magnitude;
            }
        }

        /// <summary>
        /// �A�C�e���̃A�j���[�V�������s��
        /// </summary>
        /// <param name="itemPrefab">�A�C�e���̃v���t�@�u</param>
        /// <returns>�҂�����</returns>
        private IEnumerator PlayItemAnimation(GameObject itemPrefab) {
            //null�G���[���
            if (itemPrefab != null) {
                //�A�C�e�����㉺�ɖ����ɉ^��������
                itemPrefab.transform.DOLocalMoveY(0.5f, 2.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            }

            //null�G���[���
            while (itemPrefab != null) {
                //�A�C�e������]������
                itemPrefab.transform.Rotate(0, itemRotSpeed, 0);

                //���̃t���[���ɔ�ԁi����Update���\�b�h�j
                yield return null;
            }
        }

        /// <summary>
        /// �A�C�e�����擾����
        /// </summary>
        /// <param name="nearItemNo">�ł��߂��ɂ���A�C�e���̔ԍ�</param>
        /// <param name="isPlayer">�A�C�e���̎擾�҂�Player���ǂ���</param>
        public void GetItem(int nearItemNo, bool isPlayer) {
            //�A�C�e���̎擾�҂�Player�ł͂Ȃ��Ȃ�
            if (!isPlayer) {
                //�߂��̃A�C�e�������X�g����폜����
                RemoveItemList(nearItemNo);

                //�ȍ~�̏������s��Ȃ�
                return;
            }


/********************/


            //�擾����A�C�e�����e�̃A�C�e���ł͂Ȃ��Ȃ�
            if (generatedItemDataList[nearItemNo].itemType != ItemDataSO.ItemType.Bullet)  // .isNotBullet
            {
                //Player���������Ă���A�C�e���̃��X�g�̗v�f�̐������J��Ԃ�
                for (int i = 0; i < playerItemList.Count; i++) {
                    //i�Ԗڂ̗v�f����Ȃ�
                    if (CheckTheElement(i)) {
                        //Player���������Ă���A�C�e���̃��X�g�̋󂢂Ă���v�f�ɁA�A�C�e���̏�����
                        playerItemList[i] = generatedItemDataList[nearItemNo];

                        //�܂����e�I�[�o�[�ł͂Ȃ�
                        isFull = false;

                        //�J��Ԃ��������I������
                        break;
                    }
                }
            }
            //�擾����A�C�e�����e�̃A�C�e���Ȃ�
            else {
                //���e�I�[�o�[���ǂ������ׂ�
                CheckIsFull();
            }

            //���e�I�[�o�[�ł͂Ȃ����A�擾����A�C�e�����e�Ȃ�
            if (!IsFull || generatedItemDataList[nearItemNo].itemType == ItemDataSO.ItemType.Bullet)  // .isNotBullet
            {
                //�c�e�����X�V
                //bulletManager.UpdateBulletCount(generatedItemDataList[nearItemNo].itemName);

                bulletManager.UpdateBulletCount(generatedItemDataList[nearItemNo].itemName, generatedItemDataList[nearItemNo].bulletCount);

                //�S�ẴA�C�e���X���b�g��Sprite���Đݒ肷��
                SetIAlltemSlotSprite();

                //�t���[�g�\���𐶐�
                StartCoroutine(uIManager.GenerateFloatingMessage(generatedItemDataList[nearItemNo].itemName.ToString(), Color.blue));

                //�߂��̃A�C�e�������X�g����폜����
                RemoveItemList(nearItemNo);
            }

            //�擾����A�C�e������������ł͂Ȃ��Ȃ�
            if (!generatedItemDataList[nearItemNo].isThrowingWeapon) {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�擾����A�C�e������֒e���APlayer���������Ă���A�C�e���̃��X�g�Ɏ�֒e�����ɂ���Ȃ�
            if (generatedItemDataList[nearItemNo].itemName == ItemDataSO.ItemName.Grenade && playerItemList.Contains(itemDataSO.itemDataList[1])) {
                //TODO:��֒e�̎c�e���𑝂₷����
            }
            //�擾����A�C�e�����×ܒe���APlayer���������Ă���A�C�e���̃��X�g�ɍ×ܒe�����ɂ���Ȃ�
            else if (generatedItemDataList[nearItemNo].itemName == ItemDataSO.ItemName.TearGasGrenade && playerItemList.Contains(itemDataSO.itemDataList[2])) {
                //TODO:�×ܒe�̎c�e���𑝂₷����
            }



/********************/


        }

        /// <summary>
        /// PlayerItemList�̎w�肵���ԍ��̗v�f���󂢂Ă��邩�ǂ����𒲂ׂ�
        /// </summary>
        /// <param name="elementNo">�v�f�̔ԍ�</param>
        /// <returns> PlayerItemList�̎w�肵���ԍ��̗v�f���󂢂Ă�����true��Ԃ�</returns>
        public bool CheckTheElement(int elementNo) {
            //PlayerItemList�̎w�肵���ԍ��̗v�f���󂢂Ă�����true��Ԃ�
            return playerItemList[elementNo].itemName == ItemDataSO.ItemName.None ? true : false;
        }

        /// <summary>
        /// �w�肵���ԍ��̃A�C�e�������X�g����폜����
        /// </summary>
        /// <param name="itemNo">�A�C�e���̔ԍ�</param>
        private void RemoveItemList(int itemNo) {
            //�A�C�e���̃f�[�^�̃��X�g����v�f���폜
            generatedItemDataList.RemoveAt(itemNo);

            //�A�C�e���̈ʒu���̃��X�g����v�f���폜
            generatedItemTranList.RemoveAt(itemNo);

            //�A�C�e���̃Q�[���I�u�W�F�N�g������
            Destroy(itemTrans.GetChild(itemNo).gameObject);
        }

        /// <summary>
        /// �I������Ă���A�C�e���̃f�[�^���擾����
        /// </summary>
        /// <returns>�I������Ă���A�C�e���̃f�[�^</returns>
        //public ItemDataSO.ItemData GetSelectedItemData() {
        //    //�I������Ă���A�C�e���̃f�[�^�����X�g����擾���ĕԂ�
        //    return playerItemList[playerController.SelectedItemNo - 1];
        //}

        /// <summary>
        /// �A�C�e����j������
        /// </summary>
        /// <param name="itemNo">�j������A�C�e���̔ԍ�</param>
        public void DiscardItem(int itemNo) {
            //���e�I�[�o�[���ǂ������ׂ�
            CheckIsFull();

            //�w�肳�ꂽ�A�C�e�������X�g����폜����
            playerItemList.RemoveAt(itemNo);

            //Player���������Ă���A�C�e���̃��X�g�̗v�f�̐������ɕۂ�
            playerItemList.Add(itemDataSO.itemDataList[0]);

            //�S�ẴA�C�e���X���b�g��Sprite���Đݒ肷��
            SetIAlltemSlotSprite();
        }

        /// <summary>
        /// Player���������Ă���A�C�e���̃��X�g�����ɁA�S�ẴA�C�e���X���b�g��Sprite��ݒ肷��
        /// </summary>
        public void SetIAlltemSlotSprite() {
            //Player���������Ă���A�C�e���̃��X�g�̗v�f�̐������J��Ԃ�
            for (int i = 0; i < playerItemList.Count; i++) {
                //Player�̏������Ă���A�C�e����i�Ԗڂ���Ȃ�
                if (playerItemList[i].itemName == ItemDataSO.ItemName.None) {
                    //�A�C�e���X���b�g�̃C���[�W�𓧖��ɂ���
                    uIManager.imgItemSlotList[i].DOFade(0f, 0f);

                    //�ȍ~�̏������s��Ȃ�
                    return;
                }

                //�S�ẴA�C�e���X���b�g��Sprite��ݒ肷��
                uIManager.SetItemSprite(i + 1, playerItemList[i].sprite);
            }
        }

        /// <summary>
        /// ���e�I�[�o�[���ǂ������ׂ�
        /// </summary>
        public void CheckIsFull() {
            //���ɋ��e�I�[�o�[�̏�ԂƂ��ēo�^����
            isFull = true;

            //Player���������Ă���A�C�e���̃��X�g�̗v�f�̐������J��Ԃ�
            for (int i = 0; i < playerItemList.Count; i++) {
                //i�Ԗڂ̗v�f����Ȃ�
                if (CheckTheElement(i)) {
                    //�܂����e�I�[�o�[�ł͂Ȃ�
                    isFull = false;
                }
            }
        }

        /// <summary>
        /// �A�C�e�����g�p����
        /// </summary>
        /// <param name="itemData">�g�p����A�C�e���̃f�[�^</param>
        public void UseItem(ItemDataSO.ItemData itemData) {
            //�g�p����A�C�e�����e�Ί�Ȃ�
            if (itemData.isFirearms) {
                //�e�𔭎�
                //StartCoroutine(bulletManager.ShotBullet(itemData));
                bulletManager.ShotBullet(itemData);
            }
            //�g�p����A�C�e���ɉ񕜌��ʂ���������
            else if (itemData.restorativeValue > 0) {
                //Player��Hp���X�V
                playerHealth.UpdatePlayerHp(itemData.restorativeValue);
            }
        }

        ///// <summary>
        ///// �A�C�e�����g�p����
        ///// </summary>
        ///// <param name="itemData">�g�p����A�C�e���̃f�[�^</param>
        //public void UseItem(ItemDataSO.ItemData itemData) {
        //    //�g�p����A�C�e������ѓ���Ȃ�
        //    if (itemData.isMissile) {
        //        //�e�𔭎�
        //        StartCoroutine(bulletManager.ShotBullet(itemData));
        //    }
        //    //�g�p����A�C�e���ɉ񕜌��ʂ�����A���N���b�N���ꂽ��
        //    else if (itemData.restorativeValue > 0 && Input.GetKeyDown(KeyCode.Mouse0)) {
        //        //���ʉ����Đ�
        //        soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.RecoverySE));

        //        //Player��Hp���X�V
        //        playerHealth.UpdatePlayerHp(itemData.restorativeValue);

        //        //���̉񕜃A�C�e���̏�������1���炷
        //        playerHealth.UpdateRecoveryItemCount(itemData.itemName, -1);

        //        //�I�����Ă���񕜃A�C�e���̏�������0�ɂȂ�����
        //        if (playerHealth.GetRecoveryItemCount(GetSelectedItemData().itemName) == 0) {
        //            //�I�����Ă���A�C�e���̗v�f������
        //            DiscardItem(playerController.SelectedItemNo - 1);
        //        }
        //    }
        //    //�g�p����A�C�e�����ߐڕ��킩�A���N���b�N���ꂽ��
        //    else if (itemData.isHandWeapon && Input.GetKeyDown(KeyCode.Mouse0)) {
        //        //�ߐڕ�����g�p����
        //        StartCoroutine(bulletManager.UseHandWeapon(itemData));
        //    }
        //}
    }
}
