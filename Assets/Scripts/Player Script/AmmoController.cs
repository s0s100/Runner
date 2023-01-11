using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text ammoText;
    private int ammoQuantity = 3;

    private void Start()
    {
        ammoText.text = ammoQuantity.ToString();
    }

    public void AddAmmo()
    {
        ammoQuantity++;
        ammoText.text = ammoQuantity.ToString();
    }

    public void AddAmmo(int amount)
    {
        ammoQuantity += amount;
        ammoText.text = ammoQuantity.ToString();
    }

    public void RemoveAmmo()
    {
        ammoQuantity--;
        ammoText.text = ammoQuantity.ToString();
    }

    public bool IsAttackPossible()
    {
        if (ammoQuantity > 0)
        {
            return true;
        }

        return false;
    }
}
