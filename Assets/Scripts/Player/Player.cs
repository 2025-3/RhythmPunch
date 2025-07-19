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

        //공격모드인지 방어 모드인지 판단
        NoteType _nodeType = GameManager.Instance.sheets[GameManager.Instance.SheetIndex].sheetData.notes[i].noteType;

        //공격모드
        if (_nodeType == NoteType.Attack)
        {
            //공격 성공하면 {Perfect Good Bad}
            if (type == JudgementType.Perfect || type == JudgementType.Good )
            {
                Debug.Log("\"Attack\", \"Hit\"");
                PlayAnimation("Attack", "Hit"); //플레이어 공격, 적 피격

                PlayEffect(type);

            }
            else if (type == JudgementType.Bad)
            {
                Debug.Log("Attack  Defend");

                PlayAnimation("Attack", "Defend"); //플레이어 공격, 적 방어

                PlayEffect(type);

            }
            //Miss 시에 idle
            else
            {
                Debug.Log("\"Idle\", \"Idle\"");

                PlayAnimation("Idle", "Idle"); //플레이어 idle, 적 idle

                PlayEffect(type);
            }
        }

        //방어 모드이면
        else
        {
            //방어 성공하면 {Perfect Good}
            if (type == JudgementType.Perfect || type == JudgementType.Good)
            {
                PlayAnimation("Defend", "Attack"); //플레이어 방어, 적 공격

                PlayEffect(type);

            }
            else
            {
                PlayAnimation("Hit", "Attck"); //플레이어 피격, 적 공격

                PlayEffect(type);

            }
        }

    }

    private void PlayAnimation(string player, string enemy)
    {
        /*
         * State =>
         *  Attack 1:상단 2:중단 3하단
         *  Idle 0
         *  Hit -1
         *  Defend -2
         */

        //플레이어 애니메이션
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

        //적 애니메이션
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
