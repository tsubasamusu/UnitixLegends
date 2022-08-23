using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTween���g�p

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private Transform mainCamera;//���C���J����

    [SerializeField]
    private Transform temporaryObjectContainerTran;//�ꎞ�I�ɃQ�[���I�u�W�F�N�g�����e����Transform

    [SerializeField]
    private ItemDataSO itemDataSO;//ItemDataSO

    [SerializeField]
    private PlayerController playerController;//PlayerController

    [SerializeField]
    private ItemManager itemManager;//ItemManager

    private float timer;//�o�ߎ���

    private bool stopFlag;//�d��������h��

    private int grenadeBulletCount;//��֒e�̎c��̐�

    public int GrenadeBulletCount//grenadeBulletCount�ϐ��p�̃v���p�e�B
    {
        get { return grenadeBulletCount; }//�O������͎擾�����݂̂��\��
    }

    private int tearGasGrenadeBulletCount;//�×ܒe�̎c��̐�

    public int TearGasGrenadeBulletCount//tearGasGrenadeBulletCount�ϐ��p�̃v���p�e�B
    {
        get { return tearGasGrenadeBulletCount; }//�O������͎擾�����݂̂��\��
    }

    private int assaultBulletCount;//�A�T���g�p�e�̎c�e��

    public int AssaultBulletCount//assaultBulletCount�ϐ��p�̃v���p�e�B
    {
        get { return assaultBulletCount; }//�O������͎擾�����݂̂��\��
    }

    private int sniperBulletCount;//�X�i�C�p�[�p�e�̎c�e��

    public int SniperBulletCount//sniperBulletCount�ϐ��p�̃v���p�e�B
    {
        get { return sniperBulletCount; }//�O������͎擾�����݂̂��\��
    }

    private int shotgunBulletCount;//�V���b�g�K���p�e�̎c�e��

    public int ShotgunBulletCount//shotgunBulletCount�ϐ��p�̃v���p�e�B
    {
        get { return shotgunBulletCount; }//�O������͎擾�����݂̂��\��
    }

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    private void Start()
    {
        //�o�ߎ��Ԃ̌v�����J�n
        StartCoroutine(MeasureTime());
    }

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
   �@private void Update()
    {
        //Bullet�𔭎˂���������J�����̌����ɍ��킹��
        transform.eulerAngles = new Vector3(mainCamera.eulerAngles.x, mainCamera.eulerAngles.y, transform.eulerAngles.z);
    }

    /// <summary>
    /// �o�ߎ��Ԃ��v������
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator MeasureTime()
    {
        //�������[�v
        while (true)
        {
            //�o�ߎ��Ԃ��v��
            timer += Time.deltaTime;

            //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
            yield return null;
        }
    }

    /// <summary>
    /// �c�e�����X�V����
    /// </summary>
    /// <param name="itemName">�A�C�e���̖��O</param>
    /// <param name="updateValue">�c�e���̕ύX��</param>
    public void UpdateBulletCount(ItemDataSO.ItemName itemName, int updateValue)
    {
        //�󂯎�����A�C�e���̖��O�ɉ����ď�����ύX
        switch (itemName)
        {
            //��֒e�Ȃ�
            case ItemDataSO.ItemName.Grenade:
                grenadeBulletCount = Mathf.Clamp(grenadeBulletCount + updateValue, 0, itemDataSO.itemDataList[1].maxBulletCount);
                break;

            //�×ܒe�Ȃ�
            case ItemDataSO.ItemName.TearGasGrenade:
                tearGasGrenadeBulletCount = Mathf.Clamp(tearGasGrenadeBulletCount + updateValue, 0, itemDataSO.itemDataList[2].maxBulletCount);
                break;

            //�A�T���g�Ȃ�
            case ItemDataSO.ItemName.Assault:
                assaultBulletCount = Mathf.Clamp(assaultBulletCount + updateValue, 0, itemDataSO.itemDataList[5].maxBulletCount);
                break;

            //�V���b�g�K���Ȃ�
            case ItemDataSO.ItemName.Shotgun:
                shotgunBulletCount = Mathf.Clamp(shotgunBulletCount + updateValue, 0, itemDataSO.itemDataList[6].maxBulletCount);
                break;

            //�X�i�C�p�[�Ȃ�
            case ItemDataSO.ItemName.Sniper:
                sniperBulletCount = Mathf.Clamp(sniperBulletCount + updateValue, 0, itemDataSO.itemDataList[7].maxBulletCount);
                break;
        }
    }

    /// <summary>
    /// �e�𔭎˂���
    /// </summary>
    /// <param name="itemData">�g�p����A�C�e���̃f�[�^</param>
    /// <returns>�҂�����</returns>
    public IEnumerator ShotBullet(ItemDataSO.ItemData itemData)
    {
        //�o�ߎ��Ԃ��A�ˊԊu��菬�������A�c�e����0�Ȃ�
        if (timer < itemData.interval || GetBulletCount(itemData.itemName) == 0)
        {
            //�ȍ~�̏������s��Ȃ�
            yield break;
        }

        //�e�𐶐�
        Rigidbody bulletRb = Instantiate(itemData.bulletPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));

        //�e�𔭎�
        bulletRb.AddForce(transform.forward * itemData.shotSpeed);

        //Player�̎�������˂����e�͈�莞�Ԏc��
        bulletRb.gameObject.transform.SetParent(temporaryObjectContainerTran);

        //�c�e�������炷
        UpdateBulletCount(itemData.itemName, -1);

        //�o�ߎ��Ԃ�������
        timer = 0;

        //�g�p����A�C�e�����A��֒e�ł��×ܒe�ł��Ȃ��Ȃ�
        if (itemData.itemName != ItemDataSO.ItemName.Grenade && itemData.itemName != ItemDataSO.ItemName.TearGasGrenade)
        {
            //���˂����e��3.0�b��ɏ���
            Destroy(bulletRb.gameObject, 3.0f);

            //�ȍ~�̏������s��Ȃ�
            yield break;
        }

        //��֒e�̎c��̐���0���A�I�����Ă���A�C�e������֒e�Ȃ�
        if (grenadeBulletCount == 0&&itemManager.GetSelectedItemData().itemName==ItemDataSO.ItemName.Grenade)
        {
            //�I�����Ă���A�C�e����j������
            itemManager.DiscardItem(playerController.SelectedItemNo - 1);
        }
        //�×ܒe�̎c��̐���0���A�I�����Ă���A�C�e�����×ܒe�Ȃ�
        else if (tearGasGrenadeBulletCount==0&& itemManager.GetSelectedItemData().itemName == ItemDataSO.ItemName.TearGasGrenade)
        {
            //�I�����Ă���A�C�e����j������
            itemManager.DiscardItem(playerController.SelectedItemNo - 1);
        }

        //���j���Ԃ܂ő҂�
        yield return new WaitForSeconds(itemData.timeToExplode);

        //�G�t�F�N�g�𐶐����A�e��ShoBullet�ɐݒ�
        GameObject effect = Instantiate(itemData.effect, transform);

        //���������G�t�F�N�g��3�b��ɏ���
        Destroy(effect, 3f);

        //���˂����e������
        Destroy(bulletRb.gameObject);
    }

    /// <summary>
    /// �w�肵���A�C�e���̎c�e�����擾����
    /// </summary>
    /// <param name="itemName">���̒e���g�p����A�C�e��</param>
    /// <returns>���̃A�C�e�����g�p����e�̎c�e��</returns>
    public int GetBulletCount(ItemDataSO.ItemName itemName)
    {
        //�I�����Ă���A�C�e���̖��O�ɉ����ď�����ύX
        switch (itemName)
        {
            //��֒e�Ȃ�
            case ItemDataSO.ItemName.Grenade:
                return grenadeBulletCount;

            //�×ܒe�Ȃ�
            case ItemDataSO.ItemName.TearGasGrenade:
                return tearGasGrenadeBulletCount;

            //�A�T���g�Ȃ�
            case ItemDataSO.ItemName.Assault:
                return assaultBulletCount;

            //�X�i�C�p�[�Ȃ�
            case ItemDataSO.ItemName.Sniper:
                return sniperBulletCount;

            //�V���b�g�K���Ȃ�
            case ItemDataSO.ItemName.Shotgun:
                return shotgunBulletCount;

            //��L�ȊO�Ȃ�
            default:
                return 0;
        }
    }

    /// <summary>
    /// �ߐڕ�����g�p����
    /// </summary>
    /// <param name="itemData">�g�p����A�C�e���̃f�[�^</param>
    /// <returns>�҂�����</returns>
    public IEnumerator UseHandWeapon(ItemDataSO.ItemData itemData)
    {
        //stopFlag��true�Ȃ�
        if (stopFlag)
        {
            //�ȍ~�̏������s��Ȃ�
            yield break;
        }

        //�A�C�e���𐶐�
        Rigidbody itemRb = Instantiate(itemData.bulletPrefab, transform);

        //�A�C�e���̈ʒu��ݒ�
        itemRb.gameObject.transform.localPosition = Vector3.zero;

        //�d��������h��
        stopFlag = true;

        //���������A�C�e������BoxCollider�̏����擾�ł��Ȃ�������
        if (!itemRb.TryGetComponent(out BoxCollider boxCollider))//null�G���[���
        {
            //�����
            Debug.Log("BoxCollider�̎擾�Ɏ��s");

            //�ȍ~�̏������s��Ȃ�
            yield break;
        }

        //�g�p����A�C�e�����i�C�t�Ȃ�
        if (itemData.itemName == ItemDataSO.ItemName.Knife)
        {
            //�i�C�t�̃A�j���[�V�������J�n�i�O��ړ��j
            itemRb.gameObject.transform.DOLocalMoveZ(2f,0.5f).SetLoops(2,LoopType.Yoyo);

            //�i�C�t�̃A�j���[�V�������I���܂ő҂�
            yield return new WaitForSeconds(1f);

            //���������A�C�e��������
            Destroy(itemRb.gameObject);

            //�d��������h��
            stopFlag = false;
        }
        //�g�p����A�C�e�����o�b�g�Ȃ�
        else if (itemData.itemName == ItemDataSO.ItemName.Bat)
        {
            //�o�b�g�̃A�j���[�V�������J�n�i�O��ړ��j
            itemRb.gameObject.transform.DOLocalMoveZ(2f, 0.5f).SetLoops(2, LoopType.Yoyo);

            itemRb.gameObject.transform.DOLocalRotate(new Vector3(60f,0f,0f),0.5f).SetLoops(2, LoopType.Yoyo);

            //�o�b�g�̃A�j���[�V�������I���܂ő҂�
            yield return new WaitForSeconds(1f);

            //���������A�C�e��������
            Destroy(itemRb.gameObject);

            //�d��������h��
            stopFlag = false;
        }
    }
}
