using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using DG.Tweening;//DOTween���g�p

public class GameData : MonoBehaviour
{
    public static GameData instance;//�C���X�^���X

    [SerializeField]
    private Transform itemTrans;//�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[

    [SerializeField]
    private ItemDataSO ItemDataSO;//ItemDataSO

    [SerializeField]
    private Transform playerTran;//Player�̈ʒu���

    [SerializeField]
    private float itemRotSpeed;//�A�C�e���̉�]���x

    [HideInInspector]
    public List<ItemDataSO.ItemData> generatedItemDataList = new List<ItemDataSO.ItemData>();//���������A�C�e���̃f�[�^�̃��X�g

    [HideInInspector]
    public List<Transform> generatedItemTranList = new List<Transform>();//�A�C�e���̐����ʒu�̃��X�g

    private int nearItemNo;//Player�̍ł��߂��ɂ���A�C�e���̔ԍ�

    private float lengthToNearItem;//�uPlayer�̍ł��߂��ɂ���A�C�e���v�ƁuPlayer�v�Ƃ̋���

    /// <summary>
    /// Start���\�b�h���O�ɌĂяo�����i�ȉ��A�V���O���g���ɕK�{�̋L�q�j
    /// </summary>
    private void Awake()
    {
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

    private void Start()
    {
        //�A�C�e���𐶐�
        GenerateItem();
    }

    private void Update()
    {
        GetInformationOfNearItem(playerTran.position, true);
    }

    /// <summary>
    /// �A�C�e���𐶐�����
    /// </summary>
    public void GenerateItem()
    {
        //�A�C�e���̐����ʒu�̃��X�g���쐬
        CreateGeneratedItemTranList();

        //�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[�̎q�I�u�W�F�N�g�̐������J��Ԃ�
        for (int i = 0; i < itemTrans.childCount; i++)
        {
            //�����_���Ȑ������擾
            int px = Random.Range(0, 13);

            //�w�肵���ʒu�Ƀ����_���ȃA�C�e���𐶐����A�A�j���[�V�������J�n
           StartCoroutine(PlayItemAnimation( Instantiate(ItemDataSO.itemDataList[px].prefab, generatedItemTranList[i])));

            //���������A�C�e���̃f�[�^�����X�g�ɒǉ�
            generatedItemDataList.Add(ItemDataSO.itemDataList[px]);
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
    /// �ł��߂��ɂ���A�C�e���̏��𓾂�
    /// </summary>
    public void GetInformationOfNearItem(Vector3 myPos, bool isPlayerPos)
    {
        //null�G���[���
        if (generatedItemTranList.Count <= 0)
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�A�C�e���̔ԍ�
        int itemNo=0;

        //���X�g��0�Ԃ̗v�f�̍��W��nearPos�ɉ��ɓo�^
        Vector3 nearPos = generatedItemTranList[0].position;

        //���X�g�̗v�f�������J��Ԃ�
        for (int i = 0; i < generatedItemTranList.Count; i++)
        {
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

        //myPos��Player�̍��W�Ȃ�
        if (isPlayerPos)
        {
            //Player�̍ł��߂��ɂ���A�C�e���̔ԍ���o�^
            nearItemNo = itemNo;

            //�uPlayer�̍ł��߂��ɂ���A�C�e���v�ƁuPlayer�v�Ƃ̋�����o�^
            lengthToNearItem = Vector3.Scale((nearPos - myPos), new Vector3(1, 0, 1)).magnitude;
        }
    }

    /// <summary>
    /// �A�C�e���̃A�j���[�V�������s��
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayItemAnimation(GameObject itemPrefab)
    {
        //null�G���[���
        if (itemPrefab != null)
        {
            //�A�C�e�����㉺�ɖ����ɉ^��������
            itemPrefab.transform.DOLocalMoveY(0.5f, 2.0f).SetEase(Ease.Linear). SetLoops(-1, LoopType.Yoyo);
        }

        //null�G���[���
        while (itemPrefab != null)
        {
            //�A�C�e������]������
            itemPrefab.transform.Rotate(0, itemRotSpeed, 0);

            yield return null;
        }
    }
}
