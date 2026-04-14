using Benchwarp.Data;
using ItemChanger;
using ItemChanger.Containers;
using ItemChanger.Costs;
using ItemChanger.Items;
using ItemChanger.Locations;
using ItemChanger.Placements;
using ItemChanger.Serialization;
using ItemChanger.Silksong.Containers;
using ItemChanger.Silksong.Costs;
using ItemChanger.Silksong.Modules;
using ItemChanger.Silksong.RawData;
using ItemChanger.Silksong.StartDefs;
using ItemChanger.Silksong.Tags;
using ItemChanger.Silksong.UIDefs.BigUIDefs;
using ItemChanger.Tags;
using UnityEngine.Profiling;

namespace ItemChangerTesting.MiscTests;

internal class CrestsTest : Test
{
    public override TestMetadata GetMetadata() => new()
    {
        Folder = TestFolder.ItemTests,
        MenuName = "Crests",
        MenuDescription = "Tests Crests (UIDefs)",
        Revision = 2026041400,
    };

    public override void Setup(TestArgs args)
    {
        StartAt(new CoordinateStartDef() { SceneName = SceneNames.Bonetown, X = 60, Y = 7.57f });
        Profile.Modules.Add(new StartCrestReplacementModule() { ReplacementCrestID = "Warrior" });

        int pos = 60;

        Location GetNewLocation()
        {
            pos += 5;

            return new CoordinateLocation()
            {
                X = pos,
                Y = 7.57f,
                Managed = false,
                SceneName = SceneNames.Bonetown,
                Name = $"Bonetown shiny {pos}",
            };
        }

        foreach ((string name, Item item) in BaseItemList.GetBaseItems())
        {
            if (item.UIDef is not CrestUIDef) continue;

            Location loc = GetNewLocation();
            Placement pmt = loc.Wrap().Add(Finder.GetItem(name)!);
            pmt.AddTag(new PlacementItemsHintBoxTag());
            Profile.AddPlacement(pmt);
        }

        Location multiLoc = GetNewLocation().WithTag(new UnsupportedContainerTag() { ContainerType = ContainerNames.Chest });
        Placement mpmt = multiLoc.Wrap();
        mpmt.Add(Finder.GetItem(ItemNames.Crest_of_Architect)!);
        mpmt.Add(Finder.GetItem(ItemNames.Crest_of_Beast)!);
        mpmt.Add(Finder.GetItem(ItemNames.Taunt)!);
        mpmt.Add(Finder.GetItem(ItemNames.Crest_of_Hunter)!);
        mpmt.AddTag(new FixedTextHintBoxTag() { Text = new BoxedString("MultiBigUIDefs") });
        Profile.AddPlacement(mpmt);

        Placement damage = new CoordinateLocation()
        {
            Name = "Damage",
            SceneName = SceneNames.Bone_East_17,
            Y = 81.57f,
            X = 62.10f,
            FlingType = ItemChanger.Enums.FlingType.Everywhere,
            ForceDefaultContainer = true,
            Managed = false,
        }.Wrap();
        damage.Add(Finder.GetItem(ItemNames.Crest_of_Architect)!);
        damage.Add(Finder.GetItem(ItemNames.Crest_of_Beast)!);
        damage.Add(Finder.GetItem(ItemNames.Taunt)!);
        damage.Add(Finder.GetItem(ItemNames.Crest_of_Hunter)!);
        Profile.AddPlacement(damage);
    }
}
