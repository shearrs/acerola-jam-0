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
    [SerializeField] private int maxLots;
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
        if (LotsCount >= maxLots)
        {
            lot.IsKept = false;
            lot.IsLocked = false;
            CombatManager.Instance.RetireLot(lot);

            return;
        }

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
            lots[i].IsLocked = true;
            KeepLot(lots[i]);
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

    public List<Lot> ReleaseLotsOfType(LotType type, int amount = -1)
    {
        if (amount == -1)
            amount = GetAmountOfType(type);
        
        List<Lot> typedLots = new(amount);
        int counter = 0;

        foreach(Lot lot in lots)
        {
            if (counter == amount)
                break;

            if (lot.Type == type)
            {
                typedLots.Add(lot);
                counter++;
            }
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

    public void SortLots()
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
        ReleaseLotsOfType(LotType.TEMPTATION, 3);

        Sin sin = null;
        SinType type = GetRandomSin();
        switch (type)
        {
            case SinType.PRIDE:
                sin = new Pride();
                break;
            case SinType.GREED:
                sin = new Greed();
                break;
            case SinType.LUST:
                sin = new Lust();
                break;
            case SinType.ENVY:
                sin = new Envy();
                break;
            case SinType.GLUTTONY:
                sin = new Gluttony();
                break;
            case SinType.WRATH:
                sin = new Wrath();
                break;
            case SinType.SLOTH:
                sin = new Sloth();
                break;
        }

        Level.Instance.Player.AddSin(sin);
    }

    private SinType GetRandomSin()
    {
        Player player = Level.Instance.Player;

        List<SinType> sins = new(7)
        {
            SinType.PRIDE,
            SinType.GREED,
            SinType.LUST,
            SinType.ENVY,
            SinType.GLUTTONY,
            SinType.WRATH,
            SinType.SLOTH
        };

        foreach(Sin sin in player.Sins)
        {
            sins.Remove(sin.GetSinType());
        }

        int sinIndex = Random.Range(0, sins.Count);

        return sins[sinIndex];
    }
}
