using UnityEngine;

public class SimpleScenarioController_Chara : Controller
{

	[Header("Contolled Items")]
	public Fence fenceA;
    //public Fence fenceB;

    [Header("Inputs")]
	public FloorSwitch_Chara floorSwitchA;
    //public FloorSwitch_Chara floorSwitchB;

    void Start()
	{
        floorSwitchA.on_toggled((switch_state) => { if (switch_state) fenceA.open(); else fenceA.close(); });
        //floorSwitchB.on_toggled((switch_state) => { if (switch_state) fenceB.open(); else fenceB.close(); });
    }

}