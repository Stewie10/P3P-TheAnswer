using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Memory.Sources;
using Reloaded.Mod.Interfaces;
using System;
using System.Text;
using static P3PPC.Expansion.TheAnswer.IsTheAnswer;
using static Reloaded.Hooks.Definitions.X64.FunctionAttribute;
using static System.Runtime.InteropServices.JavaScript.JSType;
using IReloadedHooks = Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks;

namespace P3PPC.Expansion.TheAnswer.SaveData
{
    public class P3PSaveDataAegis
    {
        private IMemory _memory;
        private IAsmHook _isTheAnswerSetHook;
        private IReverseWrapper<IsTheAnswerSetDelegate> _isTheAnswerSetReverseWrapper;
        private IReverseWrapper<SaveDataJourneySetDelegate> _saveDataJourneyReverseWrapper;
        private IReverseWrapper<SaveDataAnswerSetDelegate> _saveDataAnswerReverseWrapper;
        private IReverseWrapper<SaveDataSlotJourneySetDelegate> _saveDataSlotJourneyReverseWrapper;
        private IReverseWrapper<SaveDataSlotAnswerSetDelegate> _saveDataSlotAnswerReverseWrapper;

        public void Hook(IReloadedHooks hooks, IMemory memory)
        {
            _memory = Memory.Instance;

            /*Utils.SigScan("41 89 F0 48 8D 15 ?? ?? ?? ?? 48 8D 4C 24 ?? E8 ?? ?? ?? ?? 48 FF CF 80 7F 01 00", "SaveDataAegis", address =>
            {
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    "cmp eax, 0",
                    "je AegisSave",
                    "mov r8d, esi",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(saveDataJourney, out _saveDataJourneyReverseWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 10, true)}",
                    "label AegisSave",
                    "mov r8d, esi",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(saveDataAnswer, out _saveDataAnswerReverseWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 10, true)}"

                };

                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.ExecuteFirst).Activate();
            });

            Utils.SigScan("41 89 F0 48 8D 15 ?? ?? ?? ?? 48 8D 8C 24 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 2B 0D ?? ?? ?? ?? 48 FF CB 80 7B 01 00", "SaveDataSlotAegis", address =>
            {
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    "cmp eax, 0",
                    "je AegisSave",
                    "mov r8d, esi",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(saveDataSlotJourney, out _saveDataSlotJourneyReverseWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 10, true)}",
                    "label AegisSave",
                    "mov r8d, esi",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(saveDataSlotAnswer, out _saveDataSlotAnswerReverseWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 10, true)}"

                };

                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.ExecuteAfter).Activate();
            });*/
        }
        private void saveDataJourney(nuint stringBuffer, uint saveFile)
        {
            string saveIDFormatted = saveFile.ToString("D4");

            _memory.Write(stringBuffer, Encoding.ASCII.GetBytes($"P3PSAVE{saveIDFormatted}.BIN"));
        }
        private void saveDataAnswer(nuint stringBuffer, uint saveFile)
        {
            string saveIDFormatted = saveFile.ToString("D4");

            _memory.Write(stringBuffer, Encoding.ASCII.GetBytes($"P3PSAVE{saveIDFormatted}_AEGIS.BIN"));
        }
        private void saveDataSlotJourney(nuint stringBuffer, uint saveFile)
        {
            string saveIDFormatted = saveFile.ToString("D4");

            _memory.Write(stringBuffer, Encoding.ASCII.GetBytes($"P3PSAVE{saveIDFormatted}.BINslot"));
        }
        private void saveDataSlotAnswer(nuint stringBuffer, uint saveFile)
        {
            string saveIDFormatted = saveFile.ToString("D4");

            _memory.Write(stringBuffer, Encoding.ASCII.GetBytes($"P3PSAVE{saveIDFormatted}_AEGIS.BINslot"));
        }

        [Function(new Register[] { Register.rax }, Register.rax, true)]
        private delegate long IsTheAnswerSetDelegate();

        [Function(new Register[] { Register.rcx, Register.r8 }, Register.rcx, true)]
        private delegate void SaveDataJourneySetDelegate(nuint stringBuffer, uint saveFile);

        [Function(new Register[] { Register.rcx, Register.r8 }, Register.rcx, true)]
        private delegate void SaveDataAnswerSetDelegate(nuint stringBuffer, uint saveFile);

        [Function(new Register[] { Register.rcx, Register.r8 }, Register.rcx, true)]
        private delegate void SaveDataSlotJourneySetDelegate(nuint stringBuffer, uint saveFile);

        [Function(new Register[] { Register.rcx, Register.r8 }, Register.rcx, true)]
        private delegate void SaveDataSlotAnswerSetDelegate(nuint stringBuffer, uint saveFile);
    }
}
