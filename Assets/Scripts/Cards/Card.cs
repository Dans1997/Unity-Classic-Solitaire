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
        public event Action<CardFace, CardFace> CardFaceChanged;
        
        public CardType CardType { get; }
        public CardFace CardFace { get; private set; }
        public CardRank Rank { get; private set; }
        public CardSuit Suit { get; private set; }

        private readonly Image cardImage;
        private readonly string faceDownSpriteKey;
        private readonly Sprite faceDownSprite;
        private Sprite faceUpSprite;
        
        public Card(CardType cardType, Sprite faceDownSprite)
        {
            CardType = cardType;
            Rank = CardUtils.GetCardRank(cardType);
            Suit = CardUtils.GetCardSuit(cardType);
            this.faceDownSprite = faceDownSprite;
            
            cardImage = new Image();
            Add(cardImage);

            AddToClassList("card");
            clicked += () => CardClicked?.Invoke(this);
        }
        
        public void SetCardFace(CardFace newFace)
        {
            if (CardFace == newFace) return;
            var oldFace = CardFace;
            CardFace = newFace;
            cardImage.sprite = CardFace == CardFace.FaceUp ? faceUpSprite : faceDownSprite;
            pickingMode = CardFace == CardFace.FaceUp ? PickingMode.Position : PickingMode.Ignore;
            
            CardFaceChanged?.Invoke(oldFace, newFace);
        }
    
        public IEnumerator LoadCardSprites()
        {
            var faceUpSpriteHandle = Addressables.LoadAssetAsync<Sprite>(CardType.ToString());
            yield return faceUpSpriteHandle;
            
            faceUpSprite = faceUpSpriteHandle.Task.Result;
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

        public void SetSelected(bool selected)
        {
            if (selected) AddToClassList("selected");
            else RemoveFromClassList("selected");
        }
    }
}