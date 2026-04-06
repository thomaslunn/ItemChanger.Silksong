using Benchwarp.Data;
using ItemChanger.Locations;
using ItemChanger.Silksong.Locations;

namespace ItemChanger.Silksong.RawData;

internal static partial class BaseLocationList
{
    public static Location Silkspear => new WeaverBurialSpireLocation()
    {
        Name = LocationNames.Silkspear,
        SceneName = SceneNames.Mosstown_02,
    };
    
    public static Location Thread_Storm => new WeaverBurialSpireLocation()
    {
        Name = LocationNames.Thread_Storm,
        SceneName = SceneNames.Greymoor_22,
    };

    public static Location Swift_Step => new WeaverBurialSpireLocation()
    {
        Name = LocationNames.Swift_Step,
        SceneName = SceneNames.Bone_East_05,
    };

    public static Location Sharpdart => new WeaverBurialSpireLocation()
    {
        Name = LocationNames.Sharpdart,
        SceneName = SceneNames.Crawl_05,
    };
    
    public static Location Cling_Grip => new WeaverBurialSpireLocation()
    {
        Name = LocationNames.Cling_Grip,
        SceneName = SceneNames.Shellwood_10,
    };
    
    public static Location Clawline => new WeaverBurialSpireLocation()
    {
        Name = LocationNames.Clawline,
        SceneName = SceneNames.Under_18,
    };
    
    public static Location Silk_Soar => new WeaverBurialSpireLocation()
    {
        Name = LocationNames.Silk_Soar,
        SceneName = SceneNames.Abyss_08,
    };
}