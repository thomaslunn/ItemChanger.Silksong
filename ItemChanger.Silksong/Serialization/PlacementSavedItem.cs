using ItemChanger.Items;
using ItemChanger.Placements;

namespace ItemChanger.Silksong.Serialization;

/// <summary>
/// An implementation of SavedItem that hosts an IC placement.
/// </summary>
public class PlacementSavedItem : SavedItem
{
    public required Placement Placement { get; set; }
    public required GiveInfo GiveInfo { get; set; }

    public override void Get(bool showPopup = true)
    {
        Placement.GiveAll(GiveInfo);
    }

    public override bool CanGetMore()
    {
        return !Placement.AllObtained();
    }
}