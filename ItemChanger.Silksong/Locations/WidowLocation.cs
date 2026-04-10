using Benchwarp.Data;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger.Locations;
using Silksong.FsmUtil;

namespace ItemChanger.Silksong.Locations;

public class WidowLocation : AutoLocation
{
    protected override void DoLoad()
    {
        Using(new FsmEditGroup()
        {
            { new (SceneName!, "Spinner Boss", "Control"), HookWidow },
        });
    }
    
    protected override void DoUnload() { }

    private void HookWidow(PlayMakerFSM fsm)
    {
        // Remove granting the Needolin ability
        FsmState finalBindBurstState = fsm.MustGetState("Final Bind Burst");
        finalBindBurstState.RemoveFirstActionMatching(
            (action) => 
                action is HutongGames.PlayMaker.Actions.SetPlayerDataBool setPDAction
            && setPDAction.boolName.Value == "hasNeedolin");
        
        // Remove the Needolin popup
        FsmState getNeedolinState = fsm.MustGetState("Get Needolin");
        getNeedolinState.RemoveLastActionOfType<SpawnPowerUpGetMsg>();
        
        // Instead, grant the item
        getNeedolinState.AddLambdaMethod(GiveAll);
        
        // Remove an unnecessarily long wait time
        FsmState fadeToBlackState = fsm.MustGetState("Fade To Black");
        fadeToBlackState.RemoveLastActionOfType<Wait>();
        
        // Avoid going to the memory scene entirely
        // It would be a softlock without movement + Needolin
        FsmState toMemorySceneState = fsm.MustGetState("To Memory Scene");
        toMemorySceneState.RemoveLastActionOfType<BeginSceneTransition>();
        
        // Instead, reload the room as if Hornet has just been through the memory sequence
        toMemorySceneState.AddAction(new BeginSceneTransition()
        {
            sceneName = SceneNames.Belltown_Shrine,
            entryGateName = "door_wakeOnGround",
            entryDelay = 0f,
            visualization = GameManager.SceneLoadVisualizations.ThreadMemory,
            preventCameraFadeOut = true,
        });
        
    }
}