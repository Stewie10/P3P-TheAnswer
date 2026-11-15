using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Memory;
using Reloaded.Mod.Interfaces;
using System;
using System.Text;
using static P3PPC.Expansion.TheAnswer.Enums.ScenarioMode;
using static P3PPC.Expansion.TheAnswer.IsTheAnswer;
using static Reloaded.Hooks.Definitions.X64.FunctionAttribute;
using IReloadedHooks = Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks;

namespace P3PPC.Expansion.TheAnswer.UI
{
    internal unsafe class AGSSaveFileUI
    {
        private Memory _memory;
        private IAsmHook _isTheAnswerSetHook;
        private IReverseWrapper<IsTheAnswerSetDelegate> _isTheAnswerSetReverseWrapper;
        private IReverseWrapper<SaveUIJourneySetDelegate> _saveUIJourneyReverseWrapper;
        private IReverseWrapper<SaveUIAnswerSetDelegate> _saveUIAnswerReverseWrapper;

        [Function(CallingConventions.Microsoft)]
        private delegate Scenario IsTheAnswerSetDelegate();

        [Function(new Register[] { Register.rdx }, Register.rdx, true)]
        private delegate void SaveUIJourneySetDelegate(nuint stringBuffer, string saveFile);

        [Function(new Register[] { Register.rdx }, Register.rdx, true)]
        private delegate void SaveUIAnswerSetDelegate(nuint stringBuffer, string saveFile);
        private void SaveUIJourney(nuint stringBuffer, string saveUI)
        {
            _memory.Write(stringBuffer, Encoding.ASCII.GetByteCount($"memcard/save.bin"));
        }
        private void SaveUIAnswer(nuint stringBuffer, string saveUI)
        {
            _memory.Write(stringBuffer, Encoding.ASCII.GetByteCount($"memcard/AGS_save.bin"));
        }

        internal void Hook(IReloadedHooks hooks)
        {
            _memory = Memory.Instance;

            Utils.SigScan("48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? FF 87 ?? ?? ?? ?? 48 89 87 ?? ?? ?? ?? E9 ?? ?? ?? ?? BA 03 00 00 00", "SaveUIAegis", address =>
            {
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    "cmp eax, 0",
                    "jne AegisSave",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(SaveUIJourney, out _saveUIJourneyReverseWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 7, true)}",
                    "label AegisSave",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(SaveUIAnswer, out _saveUIAnswerReverseWrapper)}"

                };

                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.ExecuteFirst).Activate();
            });

            Utils.SigScan("48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 89 43 10 48 8B 4B 10 E8 ?? ?? ?? ?? 85 C0", "SaveUIAegisLoadData", address =>
            {
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    "cmp eax, 0",
                    "jne AegisSave",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(SaveUIJourney, out _saveUIJourneyReverseWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 7, true)}",
                    "label AegisSave",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(SaveUIAnswer, out _saveUIAnswerReverseWrapper)}"

                };

                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.ExecuteFirst).Activate();
            });
        }
    }
}