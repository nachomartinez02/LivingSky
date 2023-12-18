using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class ActionNode : BehaviorNode
{
    private IAController iaController;
    private int countRangeYellow;
    private int countRangeRed;
    private int countRangePowerUps;
    private int totalHealth_ia_1;

    public ActionNode(IAController iaController, int countRangeYellow, int countRangeRed, int countRangePowerUps)
    {
        this.iaController = iaController;
        this.countRangeYellow = countRangeYellow;
        this.countRangeRed = countRangeRed;
        this.countRangePowerUps = countRangePowerUps;
    }

    public override NodeStatus Execute()
    {
        // Lógica del arbol de comportamiento

        if (publicVariables.myTurn)
        {
            if (RandomMove())
            {
                iaController.RandomMove();
            }
            else
            {
                if (LookMove())
                {
                    if (PowerUpInRange())
                    {
                        iaController.PowerUpMove();
                    }
                    else
                    {
                        iaController.LookMove();
                    }
                }
                
                else
                {
                    if (AttackMove())
                    {
                        iaController.AttackMove();
                    }
                    else
                    {
                        iaController.RandomMove();
                    }
                }
            }
            return NodeStatus.Success;

        }
        return NodeStatus.Failure;

    }
        
    private bool RandomMove()
    {
        bool checkMove = false;
        if (countRangeYellow == 0 && countRangePowerUps == 0)
        {
            checkMove = true;
        }
        else
        {
            checkMove = false;
        }
        return checkMove; // Cambia esto según tu lógica
    }
    private bool LookMove()
    {
        bool checkMove = false;
        if (countRangeRed == 0)
        {
            checkMove = true;
        } 
        else
        {
            checkMove = false;
        }
        return checkMove; // Cambia esto según tu lógica
    }

    private bool AttackMove()
    {
        bool checkAttack = false;
        if (countRangeRed > 0)
        {
            checkAttack = true;
        }
        else
        {
            checkAttack = false;
        }

        return checkAttack;
    }

    private bool PowerUpInRange()
    {
        bool checkPowerUp = false;
        if (countRangePowerUps > 0)
        {
            checkPowerUp = true;
        }
        else
        {
            checkPowerUp = false;
        }
        return checkPowerUp;
    }
}
