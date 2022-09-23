using System;
using UnityEngine;

public class ground : ObjectAnchor
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public override void attach_to(HandController hand_controller, Vector3 direction, bool magic_grab)
    {
        // do nothing
    }

    public override void detach_from(HandController hand_controller)
    {
        //do nothing
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool is_available()
    {
        //do nothing 
        return false;
    }

    public override float get_grasping_radius()
    {
        return 0.0f;
    }

    public override bool can_be_grasped_by(MainPlayerController player)
    {
        //do nothing
        return false;
    }

}
