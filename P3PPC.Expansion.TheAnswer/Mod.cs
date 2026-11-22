using P3PPC.Expansion.TheAnswer.BtlData;
using P3PPC.Expansion.TheAnswer.Calendar;
using P3PPC.Expansion.TheAnswer.Character;
using P3PPC.Expansion.TheAnswer.Configuration;
using P3PPC.Expansion.TheAnswer.Dungeon;
using P3PPC.Expansion.TheAnswer.SaveData;
using P3PPC.Expansion.TheAnswer.TblFiles;
using P3PPC.Expansion.TheAnswer.Template;
using P3PPC.Expansion.TheAnswer.Title;
using P3PPC.Expansion.TheAnswer.UI;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Memory;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
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
        private AGSSexSelect _agsSexSelect = null!;
        private BtlTable _btlTable = null!;
        private EncountBgm _encountBgm = null!;
        private PartyMemberInfo _partyMemberInfo = null!;
        private CharacterFModels _characterFModelsInfo = null!;
        //private IsTheAnswerFlow _isTheAnswerFlow = null!;
        private SchedulerAegis _schedulerAegis = null!;
        private PartyAegis _partyAegis = null!;
        private BattlePackAegis _battlePackAegis = null!;
        private FloorBgms _floorBgms = null!;
        private AbyssofTime _abyssofTime = null!;
        private CutinAegis _cutinAegis = null!;

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

            _agsSexSelect = new AGSSexSelect();
            _agsSexSelect.Hook(hooks);

            _btlTable = new BtlTable();
            _btlTable.Hook(hooks);

            _encountBgm = new EncountBgm();
            _encountBgm.Hook(hooks);

            _partyMemberInfo = new PartyMemberInfo();
            _partyMemberInfo.Hook(hooks);

            _characterFModelsInfo = new CharacterFModels();
            _characterFModelsInfo.Hook(hooks);

            //_isTheAnswerFlow = new IsTheAnswerFlow();
            //_isTheAnswerFlow.Hook(hooks);

            _schedulerAegis = new SchedulerAegis();
            _schedulerAegis.Hook(hooks);

            _partyAegis = new PartyAegis();
            _partyAegis.Hook(hooks);

            _battlePackAegis = new BattlePackAegis();
            _battlePackAegis.Hook(hooks);

            _floorBgms = new FloorBgms();
            _floorBgms.Hook(hooks);

            _abyssofTime = new AbyssofTime();
            _abyssofTime.Hook(hooks);

            _cutinAegis = new CutinAegis();
            _cutinAegis.Hook(hooks);
        }

#pragma warning disable CS8618
    public Mod() { }
#pragma warning restore CS8618
    }
}