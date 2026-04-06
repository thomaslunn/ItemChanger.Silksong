using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger.Locations;
using Silksong.FsmUtil;

namespace ItemChanger.Silksong.Locations;

public class WeaverBurialSpireLocation : AutoLocation
{
    protected override void DoLoad()
    {
        Using(new FsmEditGroup()
        {
            { new(SceneName!, "Shrine Weaver Ability", "Inspection"), HookWeaverSpire }
        });
    }
    
    protected override void DoUnload() { }

    private void HookWeaverSpire(PlayMakerFSM fsm)
    {
        // Make shrine available depending on whether placement has any items
        FsmState collectedCheckState = fsm.MustGetState("Collected Check");
        collectedCheckState.RemoveActionsOfType<PlayerDataBoolTest>();
        collectedCheckState.AddMethod(() =>
        {
            if (Placement!.AllObtained())
            {
                fsm.SendEvent("COLLECTED");
            }
        });
        
        // Skip long fade-to-black, auto-equip logic, ability-get text
        FsmState fadeToBlackState = fsm.MustGetState("Fade To Black");
        fadeToBlackState.AddTransition("SKIP_MSG", "Heal");
        fadeToBlackState.RemoveActionsOfType<Wait>();
        fadeToBlackState.AddMethod(() =>
        {
            fsm.SendEvent("SKIP_MSG");
        });
        
        // Give ability slightly early - so the pickup shows during the stand-up animation
        FsmState getUpState = fsm.MustGetState("Get Up Pause");
        getUpState.AddMethod(GiveAll);
        
        // Remove original ability gain
        FsmState animationFinishedState = fsm.MustGetState("End");
        animationFinishedState.RemoveActionsOfType<HutongGames.PlayMaker.Actions.SetPlayerDataBool>();
        animationFinishedState.RemoveActionsOfType<CallStaticMethod>();
    }
}