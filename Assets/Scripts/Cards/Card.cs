using System;
using System.Collections;
using Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using Utils;

namespace Cards
{
    public class Card : Button
    {
        public event Action<Card> CardClicked;
        public CardType CardType { get; }
        public CardFace CardFace { get; private set; }
        public CardRank Rank { get; private set; }
        public CardSuit Suit { get; private set; }

        private readonly Image cardImage;
        private readonly string faceDownSpriteKey;
        private Sprite faceUpSprite;
        private Sprite faceDownSprite;

        public Card(CardType cardType, string faceDownSpriteKey)
        {
            CardType = cardType;
            Rank = CardUtils.GetCardRank(cardType);
            Suit = CardUtils.GetCardSuit(cardType);
            this.faceDownSpriteKey = faceDownSpriteKey;
            
            cardImage = new Image();
            Add(cardImage);

            AddToClassList("card");
            clicked += () => CardClicked?.Invoke(this);
        }
        
        public void SetCardFace(CardFace newFace)
        {
            if (CardFace == newFace) return;
            
            CardFace = newFace;
            cardImage.sprite = CardFace == CardFace.FaceUp ? faceUpSprite : faceDownSprite;
        }
    
        public IEnumerator LoadCardSprites()
        {
            var faceUpSpriteHandle = Addressables.LoadAssetAsync<Sprite>(CardType.ToString());
            yield return faceUpSpriteHandle;
            
            var faceDownSpriteHandle = Addressables.LoadAssetAsync<Sprite>(faceDownSpriteKey);
            yield return faceDownSpriteHandle;
            
            faceUpSprite = faceUpSpriteHandle.Task.Result;
            faceDownSprite = faceDownSpriteHandle.Task.Result;
            SetCardFace(CardFace.FaceDown);
        }

        public override string ToString()
        {
            return $"{CardType}";
        }

        public void SetHeightPercentage(float heightPercentage)
        {
            style.height = new StyleLength(Length.Percent(heightPercentage));
        }
    }
}