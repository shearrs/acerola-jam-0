using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotsBox : MonoBehaviour
{
    [Header("Positioning")]
    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private int lotsPerRow;
    [SerializeField] private Vector3 horizontalPadding;
    [SerializeField] private Vector3 verticalPadding;
    private readonly List<Lot> lots = new();

    public List<Lot> Lots => lots;
    public int LotsCount => lots.Count;

    public void Empty()
    {
        int count = lots.Count;

        for (int i = 0; i < count; i++)
        {
            ReleaseLot(lots[0], true);
        }
    }

    public void KeepLot(Lot lot)
    {
        lot.OriginalPosition = lot.transform.localPosition;
        lot.OriginalRotation = lot.transform.localRotation;

        int row = lots.Count / lotsPerRow;
        int amountInRow = lots.Count - (row * lotsPerRow);

        lot.transform.localPosition = start + (amountInRow * horizontalPadding) + (row * verticalPadding);
        lot.transform.localRotation = Quaternion.Euler(rotation);
        lot.BringToFront();

        lots.Add(lot);
        lot.IsKept = true;
    }

    public void KeepFinalLots(List<Lot> lots)
    {
        for (int i = 0; i < lots.Count; i++)
        {
            KeepLot(lots[i]);
            lots[i].IsLocked = true;
        }

        LockLots();

        if (GetAmountOfType(LotType.TEMPTATION) >= 3)
            GenerateSin();
    }

    public void ReleaseLot(Lot lot, bool retire = false)
    {
        lots.Remove(lot);
        lot.IsKept = false;
        lot.IsLocked = false;

        if (retire)
            CombatManager.Instance.RetireLot(lot);
        else
            SortLots();
    }

    public int GetAmountOfType(LotType type)
    {
        int count = 0;

        foreach(Lot lot in lots)
        {
            if (lot.Type == type)
                count++;
        }

        return count;
    }

    public List<Lot> ReleaseLotsOfType(LotType type)
    {
        List<Lot> typedLots = new(GetAmountOfType(type));

        foreach(Lot lot in lots)
        {
            if (lot.Type == type)
                typedLots.Add(lot);
        }

        foreach (Lot lot in typedLots)
            ReleaseLot(lot, true);

        SortLots();

        return typedLots;
    }

    private void LockLots()
    {
        foreach (Lot lot in lots)
            lot.IsLocked = true;
    }

    private void SortLots()
    {
        for (int i = 0; i < lots.Count; i++)
        {
            int row = i / lotsPerRow;
            int amountInRow = i - (lotsPerRow * row);
            Lot lot = lots[i];

            lot.transform.SetLocalPositionAndRotation
                (
                start + (amountInRow * horizontalPadding) + (row * verticalPadding), 
                Quaternion.Euler(rotation)
                );
        }
    }

    private void GenerateSin()
    {
        // randomize value
        Debug.Log("generate sin");
        ReleaseLotsOfType(LotType.TEMPTATION);

        Level.Instance.Player.AddSin(new Greed());
    }
}
