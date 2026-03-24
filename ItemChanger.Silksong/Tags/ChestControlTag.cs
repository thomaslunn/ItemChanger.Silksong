using ItemChanger.Silksong.Containers;
using ItemChanger.Tags;
using ItemChanger.Tags.Constraints;

namespace ItemChanger.Silksong.Tags;

[LocationTag]
[PlacementTag]
public class ChestControlTag : Tag
{
    public ChestContainer.ChestControlInfo ChestInfo { get; set; } = new();
}
