using Cards;
using Enums;

namespace Utils
{
    public static class CardUtils
    {
        public static bool IsRedSuit(this Card card)
        {
            return card.Suit is CardSuit.Hearts or CardSuit.Diamonds;
        }
        
        public static bool IsOppositeColor(Card card1, Card card2)
        {
            return (card1.IsRedSuit() && !card2.IsRedSuit()) || (!card1.IsRedSuit() && card2.IsRedSuit());
        }
        
        public static CardSuit GetCardSuit(CardType cardType)
        {
            return (CardSuit)((int)cardType >> 8);
        }

        public static CardRank GetCardRank(CardType cardType)
        {
            return (CardRank)((int)cardType & 0xFF);
        }
    }
}