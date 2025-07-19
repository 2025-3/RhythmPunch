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
        GameManager.Instance.onEndGame.AddListener(() =>
        {
            PlayAnimation("Lose", "Win");
        });

        GameManager.Instance.onCounterAttacked.AddListener(CommandAttack);
    }
    private void Play(int i, JudgementType type)
    {
        moveType = InputProcessor.currentInput;

        //���ݸ������ ��� ������� �Ǵ�
        NoteType _nodeType = GameManager.Instance.sheets[GameManager.Instance.SheetIndex].sheetData.notes[i].noteType;

        //공격 모드일때
        if (_nodeType == NoteType.Attack)
        {
            PlayEffect(Hit.Enemy, type);

            //���� �����ϸ� {Perfect Good Bad}
            if (type == JudgementType.Perfect || type == JudgementType.Good )
            {
                Debug.Log("\"Attack\", \"Hit\"");
                PlayAnimation("Attack", "Hit"); //�÷��̾� ����, �� �ǰ�
            }
            else if (type == JudgementType.Bad)
            {
                Debug.Log("Attack  Defend");

                PlayAnimation("Attack", "Defend"); //�÷��̾� ����, �� ���

            }
            //Miss �ÿ� idle
            else
            {
                Debug.Log("\"Idle\", \"Idle\"");

                PlayAnimation("Idle", "Idle"); //�÷��̾� idle, �� idle

            }
        }

        //방어 모드일때
        else
        {
            PlayEffect(Hit.Player, type);

            //방어에 성공했을 때 {Perfect Good}
            if (type == JudgementType.Perfect || type == JudgementType.Good)
            {
                PlayAnimation("Defend", "Attack"); //�÷��̾� ���, �� ����

            }
            else
            {
                PlayAnimation("Hit", "Attck"); //�÷��̾� �ǰ�, �� ����

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

        if(player == "Lose"|| enemy == "Win")
        {
            playerAnimator.SetTrigger("Lose");
            enemyAnimator.SetTrigger("Win");

        }
    }

    private void CommandAttack()
    {
        moveType = InputProcessor.currentInput;

        switch (moveType)
        {
            case MoveType.High:
                playerAnimator?.SetTrigger("HighC");
                enemyAnimator.SetTrigger("Hit");
                break;
            case MoveType.Middle:
                playerAnimator?.SetTrigger("MiddleC");
                enemyAnimator.SetTrigger("Hit");

                break;
            case MoveType.Low:
                playerAnimator?.SetTrigger("LowC");
                enemyAnimator.SetTrigger("Hit");

                break;

        }
    }

    private void PlayEffect(Hit h, JudgementType type)
    {
        if (h == Hit.Player)
        {
            //방어에 성공하면
            if (type == JudgementType.Perfect || type == JudgementType.Good) 
            {
                EffectSpawner.Instance.DefendPlayer();
            }
            else
            {
                EffectSpawner.Instance.HitPlayerEffect(moveType);
            }

        }
        else if (h == Hit.Enemy)
        {
            EffectSpawner.Instance.HitEnemyEffect(moveType);
        }
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

public enum Hit { Player, Enemy}