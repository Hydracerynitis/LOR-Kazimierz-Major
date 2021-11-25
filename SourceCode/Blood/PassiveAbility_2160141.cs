using System;
using System.Collections.Generic;
using System.Linq;
using BaseMod;
using System.Text;
using System.Threading.Tasks;

namespace KazimierzMajor
{
    public class PassiveAbility_2160141 : PassiveAbility_2160041
    {
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
