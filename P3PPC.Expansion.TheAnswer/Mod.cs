using P3PPC.Expansion.TheAnswer.Configuration;
using P3PPC.Expansion.TheAnswer.SaveData;
using P3PPC.Expansion.TheAnswer.Template;
using P3PPC.Expansion.TheAnswer.Title;
using P3PPC.Expansion.TheAnswer.UI;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Memory;
using Reloaded.Mod.Interfaces;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using static P3PPC.Expansion.TheAnswer.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;
using IReloadedHooks = Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks;

namespace P3PPC.Expansion.TheAnswer
{

    public class Mod : ModBase
    {
        private readonly IModLoader _modLoader;
        private readonly IReloadedHooks? _hooks;
        private Config _configuration;
        internal JourneyAnswerSelect _journeyAnswerSelect = null!;
        internal P3PSaveDataAegis _p3pSaveDataAegis = null!;
        private AGSSaveFileUI _agsSaveFileUI = null!;

        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _configuration = context.Configuration;

            if (!Utils.Initialise(context.Logger, _configuration, context.ModLoader))
            {
                return;
            }
            var startupScannerController = _modLoader.GetController<IStartupScanner>();
            if (startupScannerController == null || !startupScannerController.TryGetTarget(out var startupScanner))
            {
                Utils.LogError($"Unable to get controller for Reloaded SigScan Library, stuff won't work :(");
                return;
            }

            IReloadedHooks hooks = context.Hooks ?? throw new InvalidOperationException("Reloaded.Hooks is unavailable.");

            _journeyAnswerSelect = new JourneyAnswerSelect();
            _journeyAnswerSelect.Hook(hooks, startupScanner);

            _p3pSaveDataAegis = new P3PSaveDataAegis();
            _p3pSaveDataAegis.Hook(hooks);

            _agsSaveFileUI = new AGSSaveFileUI();
            _agsSaveFileUI.Hook(hooks);
        }

#pragma warning disable CS8618
    public Mod() { }
#pragma warning restore CS8618
    }
}