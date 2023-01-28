using System;
using System.Collections.Generic;
using System.Linq;
using BaseMod;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160141 : PassiveAbilityBase
    {
        private int accumulated;
        public override void OnLoseHp(int dmg)
        {
            base.OnLoseHp(dmg);
            accumulated += dmg;
            int stack = accumulated / 7;
            accumulated -= 7 * stack;
            BattleUnitBuf_Blood.AddBuf(owner, stack);
        }
        public override void OnWaveStart()
        {
            base.OnWaveStart();
            List<BookModel> EquipedBook = owner.equipment.book.GetEquipedBookList(true);
            foreach (BookModel model in EquipedBook)
            {
                if (model.BookId == Tools.MakeLorId(2160011))
                    owner.allyCardDetail.AddNewCardToDeck(Tools.MakeLorId(2161406));
            }
            owner.allyCardDetail.Shuffle();
        }
    }
}
