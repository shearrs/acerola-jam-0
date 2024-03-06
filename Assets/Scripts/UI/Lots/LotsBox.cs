using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotsBox : MonoBehaviour
{
    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private int lotsPerRow;
    [SerializeField] private Vector3 horizontalPadding;
    [SerializeField] private Vector3 verticalPadding;
    private readonly List<Lot> lots = new();

    public List<Lot> Lots => lots;
    public int LotsCount => lots.Count;

    public void KeepLot(Lot lot)
    {
        lot.OriginalPosition = lot.transform.localPosition;
        lot.OriginalRotation = lot.transform.localRotation;

        int row = lots.Count / lotsPerRow;
        int amountInRow = lots.Count - (row * lotsPerRow);

        lot.transform.localPosition = start + (amountInRow * horizontalPadding) + (row * verticalPadding);
        lot.transform.localRotation = Quaternion.Euler(rotation);

        lots.Add(lot);
        lot.IsKept = true;
    }

    public void ReleaseLot(Lot lot)
    {
        lots.Remove(lot);
        lot.IsKept = false;
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
            ReleaseLot(lot);

        return typedLots;
    }
}
