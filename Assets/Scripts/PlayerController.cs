using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
	private float gravity;//�d��

    [SerializeField]
	private float speedZ;//�O�i���x

    [SerializeField]
	private float jumpSpeed;//�W�����v���x

	[SerializeField]
	private KeyCode jumpKey;//�W�����v�{�^��

    [SerializeField]
	private CharacterController controller;//CharacterController

    [SerializeField]
	private Animator anim;//Animator

	private Vector3 moveDirection = Vector3.zero;//�i�s����

	void Start()
	{

	}

	void Update()
	{
		//Player���ڒn���Ă���Ȃ�
		if (CheckGrounded())
		{
			//�uW,S�v�őO��ړ�
			if (Input.GetAxis("Vertical") != 0.0f)//W��S��������Ă����
			{
				moveDirection.z = Input.GetAxis("Vertical") * speedZ;
			}
			

			// �����]��
			transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);

			//�W�����v
			if (Input.GetKeyDown(jumpKey))//�W�����v�{�^���������ꂽ��
			{
				moveDirection.y = jumpSpeed;
			}
		}

		//�d�͂𔭐�������
		moveDirection.y -= gravity * Time.deltaTime;

		// �ړ��̎��s
		Vector3 globalDirection = transform.TransformDirection(moveDirection);
		controller.Move(globalDirection * Time.deltaTime);

		// ���|�C���g�i�A�j���[�V�����̎��s�j
		// ���x���O�ȏ�̏ꍇ�ARun�t���O��true�ɂ���B
		anim.SetBool("Run", moveDirection.z > 0.0f);

	}

	/// <summary>
	/// Player���ڒn���Ă�����true��Ԃ�
	/// </summary>
	/// <returns></returns>
	public bool CheckGrounded()
	{
		//ray�̏����ʒu�ƌ����i�p���j��ݒ�
		var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

		//ray�̒T�������i�����j��ݒ�
		var tolerance = 0.3f;

		//ray�̃q�b�g����ibool�^�j��Ԃ�
		return Physics.Raycast(ray, tolerance);
	}
}
