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
	private CharacterController controller;//CharacterController

    [SerializeField]
	private Animator anim;//Animator

	private Vector3 moveDirection = Vector3.zero;//�i�s����

	void Start()
	{

	}

	void Update()
	{

		//print ("Ray�̐ݒ蔻��" + CheckGrounded ());

		// ray���g�����ڒn����
		if (CheckGrounded() == true)
		{

			// �O�i
			if (Input.GetAxis("Vertical") > 0.0f)
			{ // �L�����N�^�[���o�b�N���Ȃ��悤�ɂ���B
				moveDirection.z = Input.GetAxis("Vertical") * speedZ;
			}
			else
			{
				moveDirection.z = 0;
			}

			// �����]��
			transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);

			// �W�����v
			if (Input.GetButtonDown("Jump"))
			{
				moveDirection.y = jumpSpeed;
			}
		}

		// �d�͂𔭐�������
		moveDirection.y -= gravity * Time.deltaTime;

		// �ړ��̎��s
		Vector3 globalDirection = transform.TransformDirection(moveDirection);
		controller.Move(globalDirection * Time.deltaTime);

		// ���|�C���g�i�A�j���[�V�����̎��s�j
		// ���x���O�ȏ�̏ꍇ�ARun�t���O��true�ɂ���B
		anim.SetBool("Run", moveDirection.z > 0.0f);

	}

	// Ray�i�����j���g�����ڒn���胁�\�b�h
	public bool CheckGrounded()
	{

		// ray�̏����ʒu�ƌ����i�p���j
		var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

		// ray�̒T������
		var tolerance = 0.3f;

		// ray�̃q�b�g����
		return Physics.Raycast(ray, tolerance);
	}
}
