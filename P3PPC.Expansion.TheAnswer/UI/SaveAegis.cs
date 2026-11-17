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

        [Function(new Register[] { Register.rax }, Register.rax, true)]
        private delegate Scenario IsTheAnswerSetDelegate();

        internal void Hook(IReloadedHooks hooks)
        {
            _memory = Memory.Instance;
            string saveBin = "memcard/save.bin";
            var saveBinPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * saveBin.Length)).Address;
            var stringBytes = Encoding.ASCII.GetBytes(saveBin);
            var stringAddress = _memory.Allocate((nuint)stringBytes.Length).Address;
            _memory.WriteRaw(stringAddress, stringBytes);
            saveBinPtr = (char**)stringAddress;

            string saveBinAGS = "memcard/AGS_save.bin";
            var saveBinAGSPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * saveBinAGS.Length)).Address;
            var stringBytesAGS = Encoding.ASCII.GetBytes(saveBinAGS);
            var stringAddressAGS = _memory.Allocate((nuint)stringBytesAGS.Length).Address;
            _memory.WriteRaw(stringAddressAGS, stringBytesAGS);
            saveBinAGSPtr = (char**)stringAddressAGS;

            Utils.SigScan("48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? FF 87 ?? ?? ?? ?? 48 89 87 ?? ?? ?? ?? E9 ?? ?? ?? ?? BA 03 00 00 00", "SaveUIAegis", address =>
            {
                nuint saveBinPtrOff = (nuint)saveBinPtr;
                nuint saveBinAGSPtrOff = (nuint)saveBinAGSPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    "cmp eax, 0",
                    "jne AegisSave",
                    $"lea rcx, [qword {saveBinPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 7, true)}",
                    "label AegisSave",
                    $"lea rcx, [qword {saveBinAGSPtrOff}]"

                };

                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.ExecuteFirst).Activate();
            });

            Utils.SigScan("48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 89 43 10 48 8B 4B 10 E8 ?? ?? ?? ?? 85 C0", "SaveUIAegisLoadData", address =>
            {
                nuint saveBinPtrOff = (nuint)saveBinPtr;
                nuint saveBinAGSPtrOff = (nuint)saveBinAGSPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    "cmp eax, 0",
                    "jne AegisSave",
                    $"lea rcx, [qword {saveBinPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 7, true)}",
                    "label AegisSave",
                    $"lea rcx, [qword {saveBinAGSPtrOff}]"

                };

                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.ExecuteFirst).Activate();
            });
        }
    }
}