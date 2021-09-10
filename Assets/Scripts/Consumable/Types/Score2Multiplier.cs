using UnityEngine;
using System;
using System.Collections;

public class Score2Multiplier : Consumable
{
    public bool isBadCoin = false;
    public override string GetConsumableName()
    {
        return "x2";
    }

    public override ConsumableType GetConsumableType()
    {
        if (isBadCoin)
        {
            return ConsumableType.BAD_COIN;
        }
        else
        {
            return ConsumableType.SCORE_MULTIPLAYER;
        }
    }

    public override int GetPrice()
    {
        return 750;
    }

	public override int GetPremiumCost()
	{
		return 0;
	}

	public override IEnumerator Started(CharacterInputController c)
    {
        yield return base.Started(c);

        m_SinceStart = 0;

        c.trackManager.modifyMultiply += MultiplyModify;
        if (isBadCoin)
        {
            TrackManager.instance.characterController.premium -= 10;
        }
        else
        {
            GameController.Instance.isMult = true;
            GameController.Instance.SetCoinTo2x(true);
        }
    }

    public override void Ended(CharacterInputController c)
    {
        base.Ended(c);

        c.trackManager.modifyMultiply -= MultiplyModify;
        GameController.Instance.isMult = false;
        GameController.Instance.SetCoinTo2x(false);
    }

    protected int MultiplyModify(int multi)
    {
        return multi * 2;
    }
}
