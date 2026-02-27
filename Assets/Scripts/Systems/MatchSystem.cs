using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MatchSystem : MonoBehaviour
{
    public event Action OnMatch;
    public event Action OnMismatch;

    private List<Card> selectedCards = new List<Card>();
    private bool processing;

    public void RegisterCard(Card card)
    {
        if (processing || card.IsMatched)
            return;

        if (selectedCards.Contains(card))
            return;

        selectedCards.Add(card);

        if (selectedCards.Count == 2)
            StartCoroutine(CheckMatch());
    }

    private IEnumerator CheckMatch()
    {
        processing = true;

        yield return new WaitForSeconds(0.6f);

        if (selectedCards.Count < 2)
        {
            ResetState();
            yield break;
        }

        Card first = selectedCards[0];
        Card second = selectedCards[1];

        if (first == null || second == null)
        {
            ResetState();
            yield break;
        }

        if (first.CardID == second.CardID)
        {
            first.SetMatched();
            second.SetMatched();
            OnMatch?.Invoke();
        }
        else
        {
            first.ShowBack();
            second.ShowBack();
            OnMismatch?.Invoke();
        }

        ResetState();
    }
    public bool CanAcceptInput()
    {
        return !processing;
    }
    private void ResetState()
    {
        selectedCards.Clear();
        processing = false;
    }

    public void StopProcessing()
    {
        StopAllCoroutines();
        selectedCards.Clear();
        processing = false;
    }
}