using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using StsLogger = MegaCrit.Sts2.Core.Logging.Logger;
using StsLogType = MegaCrit.Sts2.Core.Logging.LogType;

namespace Sts2Rebalance.Sts2RebalanceCode;

//You're recommended but not required to keep all your code in this package and all your assets in the Sts2Rebalance folder.
[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "Sts2Rebalance"; //At the moment, this is used only for the Logger and harmony names.

    public static StsLogger Logger { get; } = new(ModId, StsLogType.Generic);

    public static void Initialize()
    {
        //If you want to use scripts defined in your mod for Godot scenes, uncomment the following line.
        //Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());
     
        Harmony harmony = new(ModId);

        harmony.PatchAll();
    }
}
