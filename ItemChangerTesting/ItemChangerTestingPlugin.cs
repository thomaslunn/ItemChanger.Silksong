using BepInEx;
using BepInEx.Configuration;
using Silksong.ModMenu.Elements;
using Silksong.ModMenu.Plugin;
using Silksong.ModMenu.Screens;

namespace ItemChangerTesting
{
    [BepInDependency(ItemChanger.Silksong.ItemChangerPlugin.Id)]
    [BepInAutoPlugin(id: "io.github.testing.silksong.itemchanger")]
    public partial class ItemChangerTestingPlugin : BaseUnityPlugin, IModMenuCustomMenu
    {
        public required ConfigEntry<int> cfgSaveSlot;
        public required ConfigEntry<TestFolder> cfgTestFolder;
        public required ConfigEntry<int> cfgTestIndex;

        public static ItemChangerTestingPlugin Instance { get; private set; } = null!;
        public new BepInEx.Logging.ManualLogSource Logger => base.Logger;

        private void Awake()
        {
            Instance = this;
            cfgSaveSlot = Config.Bind(configDefinition: new ConfigDefinition(section: "Menu", key: "Save Slot"), defaultValue: 1, 
                configDescription: new ConfigDescription("The save slot to use for the test.", acceptableValues: new AcceptableValueRange<int>(1, 4)));
            cfgTestFolder = Config.Bind(configDefinition: new ConfigDefinition(section: "Menu", key: "Test Folder"), defaultValue: (TestFolder)default,
                configDescription: new ConfigDescription("The test folder to search."));
            cfgTestIndex = Config.Bind(configDefinition: new ConfigDefinition(section: "Menu", key: "Test Index"), defaultValue: (int)default,
                configDescription: new ConfigDescription("The index of the test to launch, within its folder."));
        }

        public AbstractMenuScreen BuildCustomMenu()
        {
            SimpleMenuScreen screen = new("ItemChangerTesting");
            MenuElementGenerators.CreateIntSliderGenerator()(cfgSaveSlot, out MenuElement? saveSlotSelector);
            ConfigEntryFactory.GenerateEnumChoiceElement(cfgTestFolder, out MenuElement? testFolderSelector);

            DynamicChoiceModel model = new();
            cfgTestFolder.SettingChanged += model.UpdateFolder;
            DynamicDescriptionChoiceElement<Test> testSelector = new("Test", model, "The test to launch.", t => t.GetMetadata().MenuDescription);

            TextButton run = new("Erase save slot and launch test.");
            run.OnSubmit += Run;
            screen.Add(saveSlotSelector!);
            screen.Add(testFolderSelector!);
            screen.Add(testSelector!);
            screen.Add(run);

            screen.OnDispose += () => cfgTestFolder.SettingChanged -= model.UpdateFolder;

            return screen;

            void Run()
            {
                UIManager.instance.HideMenuInstant(screen.MenuScreen);
                try
                {
                    TestDispatcher.StartTest(testSelector.Value);
                }
                catch (Exception e)
                {
                    Logger.LogError($"Error starting test: {e}");
                }
            }
        }

        public string ModMenuName() => "ItemChangerTesting";
    }
}
