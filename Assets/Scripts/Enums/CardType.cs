using System;

namespace Enums
{
    [Flags]
    public enum CardType
    {
        AceOfSpades = (CardSuit.Spades << 8) | CardRank.Ace,
        TwoOfSpades = (CardSuit.Spades << 8) | CardRank.Two,
        ThreeOfSpades = (CardSuit.Spades << 8) | CardRank.Three,
        FourOfSpades = (CardSuit.Spades << 8) | CardRank.Four,
        FiveOfSpades = (CardSuit.Spades << 8) | CardRank.Five,
        SixOfSpades = (CardSuit.Spades << 8) | CardRank.Six,
        SevenOfSpades = (CardSuit.Spades << 8) | CardRank.Seven,
        EightOfSpades = (CardSuit.Spades << 8) | CardRank.Eight,
        NineOfSpades = (CardSuit.Spades << 8) | CardRank.Nine,
        TenOfSpades = (CardSuit.Spades << 8) | CardRank.Ten,
        JackOfSpades = (CardSuit.Spades << 8) | CardRank.Jack,
        QueenOfSpades = (CardSuit.Spades << 8) | CardRank.Queen,
        KingOfSpades = (CardSuit.Spades << 8) | CardRank.King,

        AceOfHearts = (CardSuit.Hearts << 8) | CardRank.Ace,
        TwoOfHearts = (CardSuit.Hearts << 8) | CardRank.Two,
        ThreeOfHearts = (CardSuit.Hearts << 8) | CardRank.Three,
        FourOfHearts = (CardSuit.Hearts << 8) | CardRank.Four,
        FiveOfHearts = (CardSuit.Hearts << 8) | CardRank.Five,
        SixOfHearts = (CardSuit.Hearts << 8) | CardRank.Six,
        SevenOfHearts = (CardSuit.Hearts << 8) | CardRank.Seven,
        EightOfHearts = (CardSuit.Hearts << 8) | CardRank.Eight,
        NineOfHearts = (CardSuit.Hearts << 8) | CardRank.Nine,
        TenOfHearts = (CardSuit.Hearts << 8) | CardRank.Ten,
        JackOfHearts = (CardSuit.Hearts << 8) | CardRank.Jack,
        QueenOfHearts = (CardSuit.Hearts << 8) | CardRank.Queen,
        KingOfHearts = (CardSuit.Hearts << 8) | CardRank.King,

        AceOfDiamonds = (CardSuit.Diamonds << 8) | CardRank.Ace,
        TwoOfDiamonds = (CardSuit.Diamonds << 8) | CardRank.Two,
        ThreeOfDiamonds = (CardSuit.Diamonds << 8) | CardRank.Three,
        FourOfDiamonds = (CardSuit.Diamonds << 8) | CardRank.Four,
        FiveOfDiamonds = (CardSuit.Diamonds << 8) | CardRank.Five,
        SixOfDiamonds = (CardSuit.Diamonds << 8) | CardRank.Six,
        SevenOfDiamonds = (CardSuit.Diamonds << 8) | CardRank.Seven,
        EightOfDiamonds = (CardSuit.Diamonds << 8) | CardRank.Eight,
        NineOfDiamonds = (CardSuit.Diamonds << 8) | CardRank.Nine,
        TenOfDiamonds = (CardSuit.Diamonds << 8) | CardRank.Ten,
        JackOfDiamonds = (CardSuit.Diamonds << 8) | CardRank.Jack,
        QueenOfDiamonds = (CardSuit.Diamonds << 8) | CardRank.Queen,
        KingOfDiamonds = (CardSuit.Diamonds << 8) | CardRank.King,

        AceOfClubs = (CardSuit.Clubs << 8) | CardRank.Ace,
        TwoOfClubs = (CardSuit.Clubs << 8) | CardRank.Two,
        ThreeOfClubs = (CardSuit.Clubs << 8) | CardRank.Three,
        FourOfClubs = (CardSuit.Clubs << 8) | CardRank.Four,
        FiveOfClubs = (CardSuit.Clubs << 8) | CardRank.Five,
        SixOfClubs = (CardSuit.Clubs << 8) | CardRank.Six,
        SevenOfClubs = (CardSuit.Clubs << 8) | CardRank.Seven,
        EightOfClubs = (CardSuit.Clubs << 8) | CardRank.Eight,
        NineOfClubs = (CardSuit.Clubs << 8) | CardRank.Nine,
        TenOfClubs = (CardSuit.Clubs << 8) | CardRank.Ten,
        JackOfClubs = (CardSuit.Clubs << 8) | CardRank.Jack,
        QueenOfClubs = (CardSuit.Clubs << 8) | CardRank.Queen,
        KingOfClubs = (CardSuit.Clubs << 8) | CardRank.King
    }
}