using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private int maxBulletCount;//�����ł���A�C�e��1������̒e�̍ő吔

    [SerializeField]
    private Transform mainCamera;//���C���J����

    [SerializeField]
    private Transform temporaryObjectContainerTran;//�ꎞ�I�ɃQ�[���I�u�W�F�N�g�����e����Transform

    [SerializeField]
    private ItemDataSO itemDataSO;//ItemDataSO

    private float timer;//�o�ߎ���

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
        StartCoroutine(MeasureTime1());
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
    /// �o�ߎ���1���v������
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator MeasureTime1()
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
    /// �c�i�����X�V����
    /// </summary>
    /// <param name="itemName">�A�C�e���̖��O</param>
    /// <param name="updateValue">�c�e���̕ύX��</param>
    public void UpdateBulletCount(ItemDataSO.ItemName itemName, int updateValue = 0)
    {
        //�c�e���̍X�V����0�ł͂Ȃ�������
        if (updateValue != 0)
        {
            //�󂯎�����A�C�e���̖��O�ɉ����ď�����ύX
            switch (itemName)
            {
                //��֒e�Ȃ�
                case ItemDataSO.ItemName.Grenade:
                    grenadeBulletCount = Mathf.Clamp(grenadeBulletCount + updateValue, 0, maxBulletCount);
                    break;

                //�×ܒe�Ȃ�
                case ItemDataSO.ItemName.TearGasGrenade:
                    tearGasGrenadeBulletCount=Mathf.Clamp(tearGasGrenadeBulletCount+updateValue,0,maxBulletCount);
                    break;

                //�A�T���g�Ȃ�
                case ItemDataSO.ItemName.Assault:
                    assaultBulletCount=Mathf.Clamp(assaultBulletCount+updateValue,0,maxBulletCount);
                    break;

                //�V���b�g�K���Ȃ�
                case ItemDataSO.ItemName.Shotgun:
                    shotgunBulletCount=Mathf.Clamp(shotgunBulletCount+updateValue,0,maxBulletCount);
                    break;

                //�X�i�C�p�[�Ȃ�
                case ItemDataSO.ItemName.Sniper:
                    sniperBulletCount =Mathf.Clamp(sniperBulletCount+updateValue,0,maxBulletCount);
                    break;
            }

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�󂯎�����A�C�e���̖��O�ɉ����ď�����ύX
        switch (itemName)
        {
            //��֒e�Ȃ�
            case ItemDataSO.ItemName.Grenade:
                grenadeBulletCount += itemDataSO.itemDataList[1].bulletCount;
                break;

            //�×ܒe�Ȃ�
            case ItemDataSO.ItemName.TearGasGrenade:
                tearGasGrenadeBulletCount += itemDataSO.itemDataList[2].bulletCount;
                break;

            //�A�T���g�Ȃ�
            case ItemDataSO.ItemName.Assault:
                assaultBulletCount += itemDataSO.itemDataList[5].bulletCount;
                break;

            //�V���b�g�K���Ȃ�
            case ItemDataSO.ItemName.Shotgun:
                shotgunBulletCount += itemDataSO.itemDataList[6].bulletCount;
                break;

            //�X�i�C�p�[�Ȃ�
            case ItemDataSO.ItemName.Sniper:
                sniperBulletCount += itemDataSO.itemDataList[7].bulletCount;
                break;

            //�A�T���g�p�e�Ȃ�
            case ItemDataSO.ItemName.AssaultBullet:
                assaultBulletCount += itemDataSO.itemDataList[11].bulletCount;
                break;

            //�V���b�g�K���p�e�Ȃ�
            case ItemDataSO.ItemName.ShotgunBullet:
                shotgunBulletCount += itemDataSO.itemDataList[12].bulletCount;
                break;

            //�X�i�C�p�[�p�e�Ȃ�
            case ItemDataSO.ItemName.SniperBullet:
                sniperBulletCount += itemDataSO.itemDataList[13].bulletCount;
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
        if (timer < itemData.interval||GetBulletCount(itemData.itemName)==0)
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

        //���j���Ԃ܂ő҂�
        yield return new WaitForSeconds(itemData.timeToExplode);

        //�g�p����A�C�e������֒e�Ȃ�
        if(itemData.itemName == ItemDataSO.ItemName.Grenade)
        {
            //TODO:�������鏈��
        }
        //�g�p����A�C�e�����×ܒe�Ȃ�
        else if(itemData.itemName == ItemDataSO.ItemName.TearGasGrenade)
        {
            //TODO:�K�X����o���鏈��
        }

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
}
