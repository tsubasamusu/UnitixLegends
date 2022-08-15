using System.Collections;
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;//�C���X�^���X

    [SerializeField]
    private Transform itemTrans;//�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[

    [SerializeField]
    private ItemDataSO ItemDataSO;//ItemDataSO

    [SerializeField]
    private Transform playerTran;//Player�̈ʒu���

    [HideInInspector]
    public List<ItemDataSO.ItemData> generatedItemDataList = new List<ItemDataSO.ItemData>();//���������A�C�e���̃f�[�^�̃��X�g

    [HideInInspector]
    public List<Transform> generatedItemTranlist=new List<Transform>();//�A�C�e���̐����ʒu�̃��X�g

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
        //�A�C�e���̐����ʒu�̃��X�g���쐬
        CreateGeneratedItemTranList();

        //�A�C�e���𐶐�
        GenerateItem();
    }

    /// <summary>
    /// �A�C�e���𐶐�����
    /// </summary>
    public void GenerateItem()
    {
        //�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[�̎q�I�u�W�F�N�g�̐������J��Ԃ�
        for (int i = 0; i < itemTrans.childCount; i++)
        {
            //�����_���Ȑ������擾
            int px = Random.Range(0, 13);

            //�w�肵���ʒu�Ƀ����_���ȃA�C�e���𐶐�
            Instantiate(ItemDataSO.itemDataList[px].prefab, generatedItemTranlist[i]);

            //���������A�C�e���̃f�[�^�����X�g�ɒǉ�
            generatedItemDataList.Add(ItemDataSO.itemDataList[px]);   
        }
    }

    /// <summary>
    /// �A�C�e���̐����ʒu�̃��X�g���쐬����
    /// </summary>
    public void CreateGeneratedItemTranList()
    {
        //�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[�̎q�I�u�W�F�N�g�̐������J��Ԃ�
        for (int i = 0; i < itemTrans.childCount; i++)
        {
            //�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[�̎q�I�u�W�F�N�g�̈ʒu�������X�g�ɒǉ����Ă���
            generatedItemTranlist.Add(itemTrans.GetChild(i).transform);
        }
    }

    /// <summary>
    /// �ł��߂��ɂ���A�C�e���̔ԍ���Ԃ�
    /// </summary>
    /// <param name="myPos">�����̈ʒu</param>
    /// <returns></returns>
    public int InvestigateNearItemNumber(Vector3 myPos)
    {
        return 1;//�i���j
    }
}
