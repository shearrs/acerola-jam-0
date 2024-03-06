using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotsBox : MonoBehaviour
{
    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private int lotsPerRow;
    [SerializeField] private Vector2 horizontalPadding;
    [SerializeField] private Vector2 verticalPadding;
    private readonly List<Lot> lots = new();

    public int KeptLots => lots.Count;

    public void KeepLot(Lot lot)
    {
        lot.OriginalPosition = lot.transform.localPosition;
        lot.OriginalRotation = lot.transform.localRotation;

        int row = lots.Count / lotsPerRow;
        int amountInRow = lots.Count - (row * lotsPerRow);
        Vector3 hPadding = new(horizontalPadding.x, 0, horizontalPadding.y);
        Vector3 vPadding = new(verticalPadding.x, 0, verticalPadding.y);

        lot.transform.localPosition = start + (amountInRow * hPadding) + (row * vPadding);
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
