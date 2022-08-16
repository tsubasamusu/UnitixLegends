using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private int shotCount;//�i���j

    [SerializeField]
    private Rigidbody bulletPrefab;//�i���j

    [SerializeField]
    private float shotSpeed;//�i���j

    [SerializeField]
    private Transform mainCamera;//���C���J����

    [SerializeField]
    private Transform temporaryObjectContainerTran;//�ꎞ�I�ɃQ�[���I�u�W�F�N�g�����e����Transform

    [SerializeField]
    private ItemDataSO itemDataSO;//ItemDataSO

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
    /// ���t���[���Ăяo�����
    /// </summary>
   �@private void Update()
    {
        //Bullet�𔭎˂���������J�����̌����ɍ��킹��
        transform.eulerAngles = new Vector3(mainCamera.eulerAngles.x,mainCamera.eulerAngles.y,transform.eulerAngles.z);
    }

    /// <summary>
    /// �c�e�����X�V����
    /// </summary>
    /// <param name="itemName">�A�C�e���̖��Or</param>
    public void UpdateBulletCount(ItemDataSO.ItemName itemName)
    {
        //�󂯎�����A�C�e���̖��O�ɉ����ď�����ύX
        switch (itemName)
        {
            //�A�T���g�Ȃ�
            case (ItemDataSO.ItemName.Assault):
                assaultBulletCount += itemDataSO.itemDataList[5].bulletCount;
                break;

            //�V���b�g�K���Ȃ�
            case (ItemDataSO.ItemName.Shotgun):
                shotgunBulletCount+=itemDataSO.itemDataList[6].bulletCount;
                break;

            //�X�i�C�p�[�Ȃ�
            case (ItemDataSO.ItemName.Sniper):
                sniperBulletCount+=itemDataSO.itemDataList[7].bulletCount;
                break;

            //�A�T���g�p�e�Ȃ�
            case (ItemDataSO.ItemName.AssaultBullet):
                assaultBulletCount += itemDataSO.itemDataList[11].bulletCount;
                break;

            //�V���b�g�K���p�e�Ȃ�
            case (ItemDataSO.ItemName.ShotgunBullet):
                shotgunBulletCount += itemDataSO.itemDataList[12].bulletCount;
                break;

            //�X�i�C�p�[�p�e�Ȃ�
            case (ItemDataSO.ItemName.SniperBullet):
                sniperBulletCount += itemDataSO.itemDataList[13].bulletCount;
                break;
        }
    }

   /// <summary>
   /// �e�𔭎˂���
   /// </summary>
   /// <param name="bulletPrefab">���˂���e�̃v���t�@�u</param>
   /// <param name="itemData">�g�p����A�C�e���̃f�[�^</param>
    public void ShotBullet(Rigidbody bulletPrefab,ItemDataSO.ItemData itemData)
    {
        //�e�𐶐�
        Rigidbody bulletRb=Instantiate(bulletPrefab,transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));

        //�e�𔭎�
        bulletRb.AddForce(transform.forward* shotSpeed);

        //Player�̎�������˂����e�͈�莞�Ԏc��
        bulletRb.gameObject.transform.SetParent(temporaryObjectContainerTran);

        //�c�e�������炷
        shotCount--;

        //���˂����e��3.0�b��ɏ���
        Destroy(bulletRb.gameObject,3.0f);
    }
}
