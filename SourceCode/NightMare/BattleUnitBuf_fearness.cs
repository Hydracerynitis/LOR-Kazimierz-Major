using System;

namespace KazimierzMajor
{
	public class BattleUnitBuf_fearness : BattleUnitBuf
	{
		public bool Boost = false;
		public bool Nerf = false;
		public override string keywordId
		{
			get
			{
				return "Fearness";
			}
		}

		public override string keywordIconId
		{
			get
			{
				return "Fearness";
			}
		}
        public override string bufActivatedText => string.Format(BattleEffectTextsXmlList.Instance.GetEffectTextDesc("Fearness"),getMax().ToString(),getDamage().ToString()) ;
        public override void BeforeRollDice(BattleDiceBehavior behavior)
		{
			behavior.ApplyDiceStatBonus(new DiceStatBonus
			{
				max = getMax(),
				dmgRate = -getDamage(),
				breakRate = -getDamage()
			});
		}
		public int getMax()
        {
			int output = -3;		
			if(Nerf)
				output = 0;
			return output;
		}
		public int getDamage()
        {
			int damage = 50;
			if (Boost)
				damage = 100;
			if (Nerf)
				damage = 25;
			return damage;
        }
		public override void OnRoundEnd()
		{
			Boost = false;
		}
		public BattleUnitBuf_fearness()
		{
			stack = 0;
		}
	}
}
