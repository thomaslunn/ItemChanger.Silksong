using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger.Locations;
using Silksong.FsmUtil;

namespace ItemChanger.Silksong.Locations;

public class BallowLocation : AutoLocation
{
    protected override void DoLoad()
    {
        Using(new FsmEditGroup()
        {
            { new (SceneName!, "Ballow Diving Bell NPC", "Dialogue"), HookBallow }
        });
    }

    protected override void DoUnload() { }

    private void HookBallow(PlayMakerFSM fsm)
    {
        FsmState postFirstDiveState = fsm.MustGetState("Post Ver?");
        postFirstDiveState.InsertMethod(0, () =>
        {
            if (!Placement!.AllObtained())
            {
                fsm.SendEvent("FALSE");
            }
        });
        
        FsmState givenKeyState = fsm.MustGetState("Given Key?");
        givenKeyState.RemoveActionsOfType<PlayerDataVariableTest>();
        givenKeyState.InsertMethod(0, () =>
        {
            if (Placement!.AllObtained())
            {
                fsm.SendEvent("TRUE");
            }
            else
            {
                fsm.SendEvent("FALSE");
            }
        });

        FsmState giveKeyState = fsm.MustGetState("Give Key");
        giveKeyState.RemoveActionsOfType<CollectableItemCollect>();
        giveKeyState.InsertMethod(3, GiveAll);
    }
}