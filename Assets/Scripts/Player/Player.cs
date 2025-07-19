using Sheets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Player : MonoBehaviour
{
    public Animator playerAnimator;

    public Animator enemyAnimator;

    MoveType moveType;
    private void Start()
    {
        GameManager.Instance.onNoteDestroyed.AddListener(Play);
    }
    private void Play(int i, JudgementType type)
    {
        moveType = InputProcessor.currentInput;

        //���ݸ������ ��� ������� �Ǵ�
        NoteType _nodeType = GameManager.Instance.sheets[GameManager.Instance.SheetIndex].sheetData.notes[i].noteType;

        //���ݸ��
        if (_nodeType == NoteType.Attack)
        {
            //���� �����ϸ� {Perfect Good Bad}
            if (type == JudgementType.Perfect || type == JudgementType.Good )
            {
                Debug.Log("\"Attack\", \"Hit\"");
                PlayAnimation("Attack", "Hit"); //�÷��̾� ����, �� �ǰ�

                PlayEffect(type);

            }
            else if (type == JudgementType.Bad)
            {
                Debug.Log("Attack  Defend");

                PlayAnimation("Attack", "Defend"); //�÷��̾� ����, �� ���

                PlayEffect(type);

            }
            //Miss �ÿ� idle
            else
            {
                Debug.Log("\"Idle\", \"Idle\"");

                PlayAnimation("Idle", "Idle"); //�÷��̾� idle, �� idle

                PlayEffect(type);
            }
        }

        //��� ����̸�
        else
        {
            //��� �����ϸ� {Perfect Good}
            if (type == JudgementType.Perfect || type == JudgementType.Good)
            {
                PlayAnimation("Defend", "Attack"); //�÷��̾� ���, �� ����

                PlayEffect(type);

            }
            else
            {
                PlayAnimation("Hit", "Attck"); //�÷��̾� �ǰ�, �� ����

                PlayEffect(type);

            }
        }

    }

    private void PlayAnimation(string player, string enemy)
    {
        /*
         * State =>
         *  Attack 1:��� 2:�ߴ� 3�ϴ�
         *  Idle 0
         *  Hit -1
         *  Defend -2
         */

        //�÷��̾� �ִϸ��̼�
        if(player == "Attack")
        {
            switch (moveType)
            {
                case MoveType.High:
                    playerAnimator?.SetTrigger("High");
                    break;
                case MoveType.Middle:
                    playerAnimator?.SetTrigger("Middle");
                    break;
                case MoveType.Low:
                    playerAnimator?.SetTrigger("Low");
                    break;

            }
        }
        else if(player == "Defend")
        {
            playerAnimator?.SetTrigger("Defend");

        }
        else if (player == "Hit")
        {
            playerAnimator?.SetTrigger("Hit");

        }
        else if(player == "Idle")
        {
            playerAnimator.SetInteger("State",0);

        }

        //�� �ִϸ��̼�
        if (enemy == "Attack")
        {
            switch (moveType)
            {

                case MoveType.High:
                    enemyAnimator?.SetTrigger("High");
                    break;
                case MoveType.Middle:
                    enemyAnimator?.SetTrigger("Middle");
                    break;
                case MoveType.Low:
                    enemyAnimator?.SetTrigger("Low");
                    break;

            }
        }
        else if (enemy == "Defend")
        {
            enemyAnimator?.SetTrigger("Defend");

        }
        else if (enemy == "Hit")
        {
            enemyAnimator?.SetTrigger("Hit");

        }
        else if (enemy == "Idle")
        {
            enemyAnimator?.SetInteger("State", 0);

        }
    }

    private void PlayEffect(JudgementType type)
    {

        EffectSpawner.Instance.SpawnEffect(moveType);
    }

    private void PlaySound()
    {

    }
    public void ReturnToIdle()
    {
        playerAnimator.SetInteger("State", 0);
        enemyAnimator.SetInteger("State", 0);

    }
}
