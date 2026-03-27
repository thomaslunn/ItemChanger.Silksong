using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger.Serialization;
using ItemChanger.Silksong.Serialization;
using Silksong.FsmUtil;
using Silksong.FsmUtil.Actions;

namespace ItemChanger.Silksong.UIDefs.BigUIDefs;

/// <summary>
/// Class that modifies the default big UI def by taking an existing state and replacing some of the language strings.
/// </summary>
internal class MutatedDefaultBigUIDef : DefaultBigUIDefBase
{
    /// <summary>
    /// The state to modify.
    /// </summary>
    public required string BaseStateName { get; init; }

    public List<(LanguageString orig, IValueProvider<string> replacement)> Replacements { get; init; } = [];

    protected override void ApplyCustomModifications(PlayMakerFSM fsm)
    {
        string newStateName = "Mutated " + BaseStateName;
        FsmState newState = fsm.CopyState(BaseStateName, newStateName);

        // Add transition to the new state
        string eventName = "MUTATED " + BaseStateName;
        FsmEvent newEvent = fsm.AddTransition("Init", eventName, newStateName);
        StringSwitch sw = fsm.MustGetState("Init").GetFirstActionOfType<StringSwitch>()!;
        sw.compareTo = sw.compareTo.Prepend(newStateName).ToArray();
        sw.sendEvent = sw.sendEvent.Prepend(newEvent).ToArray();

        fsm.FindStringVariable("Item")!.Value = newStateName;

        // Replace language strings
        newState.ReplaceActionsOfType<GetLanguageString>(ReplaceGetLanguageString);
    }

    private FsmStateAction ReplaceGetLanguageString(GetLanguageString origAction)
    {
        string sheet = origAction.sheetName.Value;
        string key = origAction.convName.Value;
        FsmString storeValue = origAction.storeValue;

        foreach ((LanguageString orig, IValueProvider<string> replacement) in Replacements)
        {
            if (orig.Sheet == sheet && orig.Key == key)
            {
                return new LambdaAction()
                {
                    Method = () => storeValue.Value = replacement.Value
                };
            }
        }
        return origAction;
    }
}
