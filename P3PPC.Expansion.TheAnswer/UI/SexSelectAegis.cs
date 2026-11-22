using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Memory;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using System;
using System.Text;
using static P3PPC.Expansion.TheAnswer.Enums.ScenarioMode;
using static P3PPC.Expansion.TheAnswer.IsTheAnswer;
using static P3PPC.Expansion.TheAnswer.KwlnTask.KwlnTaskP3P;
using static Reloaded.Hooks.Definitions.X64.FunctionAttribute;
using IReloadedHooks = Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks;

namespace P3PPC.Expansion.TheAnswer.UI
{
    internal unsafe class AGSSexSelect
    {
        private Memory _memory;
        private IAsmHook _isTheAnswerSetHook;
        private GetInitBinDelegate _getInitBinHook;
        private IReverseWrapper<IsTheAnswerSetDelegate> _isTheAnswerSetReverseWrapper;
        private IReverseWrapper<GetInitBinDelegate> _getInitBinWrapper;

        [Function(CallingConventions.Microsoft)]
        private delegate int GetInitBinDelegate(nint param_1, int param_2);

        internal void Hook(IReloadedHooks hooks)
        {
            _memory = Memory.Instance;
            string sexSelect = "title/sex_select.bin";
            var sexSelectPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * sexSelect.Length)).Address;
            var stringBytes = Encoding.ASCII.GetBytes(sexSelect);
            var stringAddress = _memory.Allocate((nuint)stringBytes.Length).Address;
            _memory.WriteRaw(stringAddress, stringBytes);
            sexSelectPtr = (char**)stringAddress;

            string sexSelectAGS = "title/AGS_sex_select.bin";
            var sexSelectAGSPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * sexSelectAGS.Length)).Address;
            var stringBytesAGS = Encoding.ASCII.GetBytes(sexSelectAGS);
            var stringAddressAGS = _memory.Allocate((nuint)stringBytesAGS.Length).Address;
            _memory.WriteRaw(stringAddressAGS, stringBytesAGS);
            sexSelectAGSPtr = (char**)stringAddressAGS;

            Utils.SigScan("48 89 6C 24 ?? 56 57 41 56 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 31 E0 48 89 84 24 ?? ?? ?? ?? 48 89 CD 41 89 D6 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 54 24 ?? 48 89 E9 89 C6 E8 ?? ?? ?? ?? 48 8D 94 24 ?? ?? ?? ??", "GetInitBin", address =>
            {
                _getInitBinHook = hooks.CreateWrapper<GetInitBinDelegate>(address, out _);
            });

            Utils.SigScan("8B D7 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 89 83 ?? ?? ?? ?? 48 85 C0 75 15 8B 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 83 C2 06", "SexSelectAegis", address =>
            {
                nuint sexSelectPtrOff = (nuint)sexSelectPtr;
                nuint sexSelectAGSPtrOff = (nuint)sexSelectAGSPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je AegisSexSel",
                    "mov edx, edi",
                    $"lea rcx, [qword {sexSelectPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_getInitBinHook, out _getInitBinWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 14, true)}",
                    "label AegisSexSel",
                    "mov edx, edi",
                    $"lea rcx, [qword {sexSelectAGSPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_getInitBinHook, out _getInitBinWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 14, true)}"
                };

                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });
        }
        [Function(new[] { Register.rax, Register.r13 }, Register.rax, true)]
        private delegate Scenario IsTheAnswerSetDelegate();
    }
}