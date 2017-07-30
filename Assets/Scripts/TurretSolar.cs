using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSolar : Turret {
    public int dailyCharge = 20;
    public int batteryCapacity = 30;

    private void Start()
    {
        _gm.ExpandMaxPower(batteryCapacity);
    }
}
