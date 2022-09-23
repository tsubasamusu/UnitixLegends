using DG.Tweening;//DOTween���g�p
using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;

namespace yamap 
{
    public class ItemManager : MonoBehaviour 
    {
        public static ItemManager instance;//�C���X�^���X

        [SerializeField]
        private ItemDataSO itemDataSO;//ItemDataSO

        /// <summary>
        /// ItemDataSO�擾�p
        /// </summary>
        public ItemDataSO ItemDataSO { get => itemDataSO; }

        private UIManager uIManager;//UIManager

        private BulletManager bulletManager;//BulletManager

        [SerializeField]
        private Transform itemTrans;//�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[

        [SerializeField]
        private Transform generateItemPosPrefab;//�A�C�e���̐����ʒu�̃v���t�@�u

        private Transform playerTran;//Player�̈ʒu���

        [SerializeField]
        private float itemRotSpeed;//�A�C�e���̉�]���x

        [SerializeField]
        private int maxItemTranCount;//��������A�C�e���̐����ʒu�̍ő吔

        public List<ItemDataSO.ItemData> generatedItemDataList = new List<ItemDataSO.ItemData>();//���������A�C�e���̃f�[�^�̃��X�g

        public List<Transform> generatedItemTranList = new List<Transform>();//�A�C�e���̐����ʒu�̃��X�g

        public List<ItemDataSO.ItemData> playerItemList = new List<ItemDataSO.ItemData>(5);//Player���������Ă���A�C�e���̃��X�g

        private bool isFull;//Player�̏��L�������e�I�[�o�[���ǂ���

        /// <summary>
        /// ���e�I�[�o�[����擾�p
        /// </summary>
        public bool IsFull{ get=>isFull;}

        private int nearItemNo;//Player�̍ł��߂��ɂ���A�C�e���̔ԍ�

        /// <summary>
        /// Player�̍ł��߂��ɂ���A�C�e���̔ԍ��̎擾�p
        /// </summary>
        public int NearItemNo { get => nearItemNo;}

        private float lengthToNearItem;//�uPlayer�̍ł��߂��ɂ���A�C�e���v�ƁuPlayer�v�Ƃ̋���

        /// <summary>
        /// �uPlayer�̍ł��߂��ɂ���A�C�e���v�ƁuPlayer�v�Ƃ̋����̎擾�p
        /// </summary>
        public float LengthToNearItem { get => lengthToNearItem;}

        private int selectedItemNo = 0;//�g�p���Ă���A�C�e���̔ԍ�

        /// <summary>
        /// �g�p���Ă���A�C�e���̔ԍ��̎擾�E�ݒ�p
        /// </summary>
        public int SelectedItemNo
        {
            get { return selectedItemNo; }
            set { selectedItemNo = value; }
        }

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
        }

        /// <summary>
        /// ItemManager�̏����ݒ���s��
        /// </summary>
        /// <param name="uiManager">UIManager</param>
        /// <param name="playerController">PlayerController</param>
        /// <param name="bulletManager">BulletManager</param>
        public void SetUpItemManager(UIManager uiManager, PlayerController playerController, BulletManager bulletManager)
        { 
            //UIManager���擾
            this.uIManager = uiManager;

            //Player�̈ʒu�����擾
            playerTran = playerController.transform;

            //BulletManager���擾
            this.bulletManager = bulletManager;

            //�v���C���[�ƃG�l�~�[�̃A�C�e���̎�ނ��d��������
            (List<int> playerItemNums, List<int> enemyItemNums) sortingItemNums = GetSotringItemNums();

            /// <summary>
            /// �v���C���[�ƃG�l�~�[�̃A�C�e���ƂŎd�����������X�g���쐬����
            /// </summary>
            /// <returns>�v���C���[�ƃG�l�~�[�̃A�C�e���ƂŎd�����������X�g</returns>
            (List<int> playerNums, List<int> enemyNums) GetSotringItemNums() 
            {

                List<int> playerList = new();
                List<int> enemyList = new();

                //�A�C�e���̐������J��Ԃ�
                for (int i = 0; i < itemDataSO.itemDataList.Count; i++) 
                {
                    //�ŏ��̃A�C�e����None�Ȃ̂Ŕ�΂�
                    if (i == 0) 
                    {
                        //���̌J��Ԃ������֔��
                        continue;
                    }

                    //�G�l�~�[�̎g�p�ł���A�C�e���Ȃ�
                    if (itemDataSO.itemDataList[i].enemyCanUse) 
                    {
                        //�G�l�~�[�p�̃��X�g�ɒǉ�
                        enemyList.Add(itemDataSO.itemDataList[i].itemNo);
                    } 
                    //�G�l�~�[���g�p�ł��Ȃ��A�C�e���Ȃ�
                    else
                    {
                        //�v���C���[�p�̃��X�g�ɒǉ�
                        playerList.Add(itemDataSO.itemDataList[i].itemNo);
                    }
                }

                //�쐬����2�̃��X�g��Ԃ�
                return (playerList, enemyList);
            }

            //�A�C�e���𐶐�����
            GenerateItem(sortingItemNums.playerItemNums, sortingItemNums.enemyItemNums);
        }

        /// <summary>
        /// ���t���[���Ăяo�����
        /// </summary>
        private void Update() 
        {
            //Player�����݂��Ȃ��Ȃ�inull�G���[����j
            if (!playerTran) 
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //Player�̍ł��߂��ɂ���A�C�e���̏����擾
            GetInformationOfNearItem(playerTran.position);
        }

        /// <summary>
        /// �w�肵���A�C�e���̃f�[�^���擾����
        /// </summary>
        /// <param name="itemName">�A�C�e���̖��O</param>
        /// <returns>�A�C�e���̃f�[�^</returns>
        public ItemDataSO.ItemData GetItemData(ItemDataSO.ItemName itemName) 
        {
            //�w�肵���A�C�e���̃f�[�^��Ԃ�
            return itemDataSO.itemDataList.Find(x => x.itemName == itemName);
        }

        /// <summary>
        /// �A�C�e���𐶐�����
        /// </summary>
        /// <param name="playerItemNums">Player�p�̃A�C�e���̃��X�g</param>
        /// <param name="enemyItemNums">Enemy�p�̃A�C�e���̃��X�g</param>
        public void GenerateItem(List<int> playerItemNums, List<int> enemyItemNums) 
        {
            //�A�C�e���̐����ʒu�𐶐�����
            GenerateItemTran();

            //�A�C�e���̐����ʒu�̃��X�g���쐬
            CreateGeneratedItemTranList();

            //�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[�̎q�I�u�W�F�N�g�����J��Ԃ�
            for (int i = 0; i < itemTrans.childCount; i++) 
            {
                //0����2�܂ł̃����_���Ȑ������擾
                int px = Random.Range(0, 3);

                //�擾����������0�ł͂Ȃ��Ȃ�i2/3�̊m����Enemy���g���镐����o��������j
                if (px != 0) 
                {
                    //�����_���Ȑ������擾
                    int py = enemyItemNums[Random.Range(0, enemyItemNums.Count)];
                    
                    //�w�肵���ʒu�Ƀ����_���ȃA�C�e���𐶐����A�A�j���[�V�������J�n
                    StartCoroutine(PlayItemAnimation(Instantiate(itemDataSO.itemDataList[py].itemPrefab, generatedItemTranList[i])));

                    //���������A�C�e���̃f�[�^�����X�g�ɒǉ�
                    generatedItemDataList.Add(itemDataSO.itemDataList[py]);

                    //���̌J��Ԃ������ֈڂ�
                    continue;
                }

                //�����_���Ȑ������擾
                int pz = playerItemNums[Random.Range(0, playerItemNums.Count)];
                
                //�w�肵���ʒu�Ƀ����_���ȃA�C�e���𐶐����A�A�j���[�V�������J�n
                StartCoroutine(PlayItemAnimation(Instantiate(itemDataSO.itemDataList[pz].itemPrefab, generatedItemTranList[i])));

                //���������A�C�e���̃f�[�^�����X�g�ɒǉ�
                generatedItemDataList.Add(itemDataSO.itemDataList[pz]);
            }
        }

        /// <summary>
        /// �A�C�e���̐����ʒu�𐶐�����
        /// </summary>
        private void GenerateItemTran() 
        {
            //��������A�C�e���̐����ʒu�̍ő吔�����J��Ԃ�
            for (int i = 0; i < maxItemTranCount; i++) 
            {
                //�A�C�e���̐����ʒu��ݒ�
                Transform generateItemPosTran = Instantiate(generateItemPosPrefab);

                //���������A�C�e���̐����ʒu�̐e��ݒ�
                generateItemPosTran.SetParent(itemTrans);

                //0����3�܂ł̃����_���Ȑ������擾
                int px = Random.Range(0, 4);

                //-120����120�܂ł̃����_���ȏ������擾
                float py = Random.Range(-120f, 120f);

                //px�̒l�ɉ����ď�����ύX
                generateItemPosTran.localPosition = px switch 
                {
                    0 => new (py, 0f, -120f),
                    1 => new (120f, 0f, py),
                    2 => new (py, 0f, 120f),
                    3 => new (-120f, 0f, py),
                    _ => Vector3.zero,
                };
            }
        }

        /// <summary>
        /// �A�C�e���̐����ʒu�̃��X�g���쐬����
        /// </summary>
        private void CreateGeneratedItemTranList() 
        {
            //�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[�̎q�I�u�W�F�N�g�̐������J��Ԃ�
            for (int i = 0; i < itemTrans.childCount; i++) 
            {
                //�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[�̎q�I�u�W�F�N�g�̈ʒu�������X�g�ɒǉ����Ă���
                generatedItemTranList.Add(itemTrans.GetChild(i).transform);
            }
        }

        /// <summary>
        /// �ł��߂��ɂ���A�C�e���̔ԍ��ƈʒu���𓾂�
        /// </summary>
        /// <param name="myPos">�������g�̍��W</param>
        /// <param name="isPlayerPos">��������Player�̍��W���ǂ���</param>
        public void GetInformationOfNearItem(Vector3 myPos) 
        {
            //null�G���[���
            if (generatedItemTranList.Count <= 0) 
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�A�C�e���̔ԍ�
            int itemNo = 0;

            //���X�g��0�Ԃ̗v�f�̍��W��nearPos�ɉ��ɓo�^
            Vector3 nearPos = generatedItemTranList[0].position;

            //���X�g�̗v�f�������J��Ԃ�
            for (int i = 0; i < generatedItemTranList.Count; i++) 
            {
                //�J��Ԃ������œ����v�f��null�Ȃ�
                if (generatedItemTranList[i] == null) 
                {
                    //���̌J��Ԃ������Ɉڂ�
                    continue;
                }

                //���X�g��i�Ԃ̗v�f�̍��W��pos�ɓo�^
                Vector3 pos = generatedItemTranList[i].position;

                //���o�^�����v�f�ƁAfor���œ����v�f�́AmyPos�Ƃ̋������r
                if (Vector3.Scale((pos - myPos), new Vector3(1, 0, 1)).magnitude < Vector3.Scale((nearPos - myPos), new Vector3(1, 0, 1)).magnitude) 
                {
                    //Player�̍ł��߂��ɂ���A�C�e���̔ԍ���i�œo�^
                    itemNo = i;

                    //nearPos���ēo�^
                    nearPos = pos;
                }
            }

            //Player�̍ł��߂��ɂ���A�C�e���̔ԍ���o�^
            nearItemNo = itemNo;

            //�uPlayer�̍ł��߂��ɂ���A�C�e���v�ƁuPlayer�v�Ƃ̋�����o�^
            lengthToNearItem = Vector3.Scale((nearPos - myPos), new Vector3(1, 0, 1)).magnitude;
        }
        
        /// <summary>
        /// �A�C�e���̃A�j���[�V�������s��
        /// </summary>
        /// <param name="item">�A�C�e��</param>
        /// <returns>�҂�����</returns>
        private IEnumerator PlayItemAnimation(ItemDetail item) 
        {
            //null�G���[���
            if (item != null) 
            {
                //�A�C�e�����㉺�ɖ����ɉ^��������
                item.transform.DOLocalMoveY(0.5f, 2.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(item.gameObject);
            }

            //null�G���[���
            while (item != null) 
            {
                //�A�C�e������]������
                item.transform.Rotate(0, itemRotSpeed, 0);

                //���̃t���[���ɔ�ԁi����Update���\�b�h�j
                yield return null;
            }
        }

        /// <summary>
        /// �A�C�e�����擾����
        /// </summary>
        /// <param name="nearItemNo">�ł��߂��ɂ���A�C�e���̔ԍ�</param>
        /// <param name="isPlayer">�A�C�e���擾�҂�Player���ǂ���</param>
        /// <param name="playerHealth">PlayerHealth</param>
        public void GetItem(int nearItemNo, bool isPlayer, PlayerHealth playerHealth = null) 
        {
            //�A�C�e���̎擾�҂�Player�ł͂Ȃ��Ȃ�
            if (!isPlayer) 
            {
                //�߂��̃A�C�e�������X�g����폜����
                RemoveItemList(nearItemNo);

                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�擾����A�C�e�����e�̃A�C�e���ł͂Ȃ��Ȃ�
            if (generatedItemDataList[nearItemNo].itemType != ItemDataSO.ItemType.Bullet)
            {
                //Player���������Ă���A�C�e���̃��X�g�̗v�f�̐������J��Ԃ�
                for (int i = 0; i < playerItemList.Count; i++) 
                {
                    //i�Ԗڂ̗v�f����Ȃ�
                    if (CheckTheElement(i))
                    {
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
            else 
            {
                //���e�I�[�o�[���ǂ������ׂ�
                CheckIsFull();
            }

            //���e�I�[�o�[���A�擾����A�C�e�����e�ł͂Ȃ��Ȃ�
            if (isFull && generatedItemDataList[nearItemNo].itemType != ItemDataSO.ItemType.Bullet)
            {
                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�擾����A�C�e������ѓ���Ȃ�
            if (generatedItemDataList[nearItemNo].itemType == ItemDataSO.ItemType.Missile || generatedItemDataList[nearItemNo].itemType == ItemDataSO.ItemType.Bullet) 
            {
                //�c�e�����X�V
                bulletManager.UpdateBulletCount(generatedItemDataList[nearItemNo].itemName, generatedItemDataList[nearItemNo].bulletCount);
            }
            //�擾����A�C�e���ɉ񕜌��ʂ�����Ȃ�
            else if (generatedItemDataList[nearItemNo].restorativeValue > 0) 
            {
                //�񕜃A�C�e���̏��������X�V
                playerHealth.UpdateRecoveryItemCount(generatedItemDataList[nearItemNo].itemName, generatedItemDataList[nearItemNo].bulletCount);
            }

            //�S�ẴA�C�e���X���b�g��Sprite���Đݒ肷��
            SetIAlltemSlotSprite();

            //�t���[�g�\���𐶐�
            StartCoroutine(uIManager.GenerateFloatingMessage(generatedItemDataList[nearItemNo].itemName.ToString(), Color.blue));

            //�߂��̃A�C�e�������X�g����폜����
            RemoveItemList(nearItemNo);
        }

        /// <summary>
        /// PlayerItemList�̎w�肵���ԍ��̗v�f���󂢂Ă��邩�ǂ����𒲂ׂ�
        /// </summary>
        /// <param name="elementNo">�v�f�̔ԍ�</param>
        /// <returns> PlayerItemList�̎w�肵���ԍ��̗v�f���󂢂Ă�����true��Ԃ�</returns>
        public bool CheckTheElement(int elementNo)
        {
            //PlayerItemList�̎w�肵���ԍ��̗v�f���󂢂Ă�����true��Ԃ�
            return playerItemList[elementNo].itemName == ItemDataSO.ItemName.None ? true : false;
        }

        /// <summary>
        /// �w�肵���ԍ��̃A�C�e�������X�g����폜����
        /// </summary>
        /// <param name="itemNo">�A�C�e���̔ԍ�</param>
        private void RemoveItemList(int itemNo) 
        {
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
        public ItemDataSO.ItemData GetSelectedItemData() 
        {
            //�I������Ă���A�C�e���̃f�[�^�����X�g����擾���ĕԂ�
            return playerItemList[SelectedItemNo];
        }

        /// <summary>
        /// �A�C�e����j������
        /// </summary>
        /// <param name="itemNo">�j������A�C�e���̔ԍ�</param>
        public void DiscardItem(int itemNo) 
        {
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
        public void SetIAlltemSlotSprite() 
        {
            //Player���������Ă���A�C�e���̃��X�g�̗v�f�̐������J��Ԃ�
            for (int i = 0; i < playerItemList.Count; i++) 
            {
                //Player�̏������Ă���A�C�e����i�Ԗڂ���Ȃ�
                if (playerItemList[i].itemName == ItemDataSO.ItemName.None) 
                {
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
        public void CheckIsFull() 
        {
            //���ɋ��e�I�[�o�[�̏�ԂƂ��ēo�^����
            isFull = true;

            //Player���������Ă���A�C�e���̃��X�g�̗v�f�̐������J��Ԃ�
            for (int i = 0; i < playerItemList.Count; i++) 
            {
                //i�Ԗڂ̗v�f����Ȃ�
                if (CheckTheElement(i)) 
                {
                    //�܂����e�I�[�o�[�ł͂Ȃ�
                    isFull = false;
                }
            }
        }

       /// <summary>
       /// �A�C�e�����g�p����
       /// </summary>
       /// <param name="itemData">�A�C�e���̃f�[�^</param>
       /// <param name="playerHealth">PlayerHealth</param>
        public void UseItem(ItemDataSO.ItemData itemData, PlayerHealth playerHealth) 
        {
            //�g�p����A�C�e������ѓ���Ȃ�
            if (itemData.itemType == ItemDataSO.ItemType.Missile) 
            {
                //�e�𔭎�
                bulletManager.ShotBullet(itemData);
            }
            //�g�p����A�C�e���ɉ񕜌��ʂ�����A���N���b�N���ꂽ��
            else if (itemData.restorativeValue > 0 && Input.GetKeyDown(KeyCode.Mouse0)) 
            {
                //���ʉ����Đ�
                SoundManager.instance.PlaySE(SeName.RecoverySE);

                //Player��Hp���X�V
                playerHealth.UpdatePlayerHp(itemData.restorativeValue);

                //���̉񕜃A�C�e���̏�������1���炷
                playerHealth.UpdateRecoveryItemCount(itemData.itemName, -1);

                //�I�����Ă���񕜃A�C�e���̏�������0�ɂȂ�����
                if (playerHealth.GetRecoveryItemCount(GetSelectedItemData().itemName) == 0) 
                {
                    //�I�����Ă���A�C�e���̗v�f������
                    DiscardItem(SelectedItemNo);
                }
            }
            //�g�p����A�C�e�����ߐڕ��킩�A���N���b�N���ꂽ��
            else if (itemData.itemType == ItemDataSO.ItemType.HandWeapon && Input.GetKeyDown(KeyCode.Mouse0)) 
            {
                //�ߐڕ�����g�p����
                bulletManager.PrepareUseHandWeapon(itemData);
            }
        }
    }
}
