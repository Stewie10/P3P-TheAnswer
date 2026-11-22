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

namespace P3PPC.Expansion.TheAnswer.TblFiles
{
    internal unsafe class BtlTable
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

            Utils.SigScan("48 89 6C 24 ?? 56 57 41 56 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 31 E0 48 89 84 24 ?? ?? ?? ?? 48 89 CD 41 89 D6 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 54 24 ?? 48 89 E9 89 C6 E8 ?? ?? ?? ?? 48 8D 94 24 ?? ?? ?? ??", "GetInitBin", address =>
            {
                _getInitBinHook = hooks.CreateWrapper<GetInitBinDelegate>(address, out _);
            });

            Utils.SigScan("49 8B 0E 33 D2 E8 ?? ?? ?? ?? 48 8B D8 48 89 05 ?? ?? ?? ?? EB 07 48 8B 1D ?? ?? ?? ??", "InitTblLoad", address =>
            {
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je TblLoadF",
                    "mov rcx, [r14]",
                    "jmp loadFile",
                    "label TblLoadF",
                    "mov rcx, [r14 + 8]",
                    "label loadFile",
                    "xor edx, edx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_getInitBinHook, out _getInitBinWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 10, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("49 8B 0E 48 8D 54 24 ?? E8 ?? ?? ?? ?? 48 8B F0 48 85 C0 75 16", "InitTblLoad2", address =>
            {
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je TblLoadF",
                    "mov rcx, [r14]",
                    "jmp loadFile",
                    "label TblLoadF",
                    "mov rcx, [r14 + 8]",
                    "label loadFile",
                    "lea rdx, [rsp + 0x20]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 8, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });
        }
        [Function(new[] { Register.rax, Register.r13 }, Register.rax, true)]
        private delegate Scenario IsTheAnswerSetDelegate();
    }
}