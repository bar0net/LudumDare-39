using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSolar : Turret {
    public int dailyCharge = 20;
    public int batteryCapacity = 30;

    private void Start()
    {
        // Update GameManager power statistics when it's built
        _gm.ExpandMaxPower(dailyCharge,batteryCapacity);
    }
}
