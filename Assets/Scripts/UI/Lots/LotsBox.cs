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

    public int KeptLots => lots.Count;

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
        lot.transform.localPosition = lot.OriginalPosition;
        lot.transform.rotation = lot.OriginalRotation;
    }
}
