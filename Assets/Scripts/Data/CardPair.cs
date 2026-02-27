using UnityEngine;

[System.Serializable]
public class CardPair
{
    public Card first;
    public Card second;

    public CardPair(Card a, Card b)
    {
        first = a;
        second = b;
    }
}