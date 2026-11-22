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

namespace P3PPC.Expansion.TheAnswer.BtlData
{
    internal unsafe class EncountBgm
    {
        private Memory _memory;
        private IAsmHook _isTheAnswerSetHook;
        private BgmToggleDelegate _bgmToggleHook;
        private FemcDelegate _femcHook;
        private IReverseWrapper<IsTheAnswerSetDelegate> _isTheAnswerSetReverseWrapper;
        private IReverseWrapper<BgmToggleDelegate> _bgmToggleWrapper;
        private IReverseWrapper<FemcDelegate> _femcWrapper;

        [Function(CallingConventions.Microsoft)]
        private delegate nint BgmToggleDelegate();

        [Function(CallingConventions.Microsoft)]
        private delegate bool FemcDelegate();

        private ulong* someValue;

        internal void Hook(IReloadedHooks hooks)
        {
            _memory = Memory.Instance;

            someValue = (ulong*)_memory.Allocate((sizeof(ulong))).Address;

            *someValue = 0x0B1B74CD66210C9ED;

            Utils.SigScan("40 57 48 83 EC 20 0F B7 F9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 66 83 FF 7F 72 15 8B 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 83 C2 07 E8 80 AD A8 E8", "BgmToggle", address =>
            {
                _bgmToggleHook = hooks.CreateWrapper<BgmToggleDelegate>(address, out _);
            });

            Utils.SigScan("48 83 EC 28 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 0F B6 05 45 AD 10 03 C1 E8 07 48 83 C4 28 C3", "IsFemc", address =>
            {
                _femcHook = hooks.CreateWrapper<FemcDelegate>(address, out _);
            });

            Utils.SigScan("83 E9 01 74 57 83 F9 01 74 2F E8 ?? ?? ?? ?? 48 31 0D ?? ?? ?? ?? BA ?? ?? ?? ?? 85 C0 74 0C 8D 4A 79 48 83 C4 28 E9 32 4F E0 F9", "EncountBgm", address =>
            {
                nuint someValuePtr = (nuint)someValue;
                string[] asm =
                {
                    "use64",
                    "sub ecx, 1",
                    "je hymmOfTheSoul",
                    "sub ecx, 1",
                    "je masterOfTartarusDangerZone",
                    "sub ecx, 1",
                    "je heartfulCry",
                    "cmp ecx, 1",
                    "je Darkness",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je fesVariantBGMs",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_femcHook, out _femcWrapper)}",
                    $"xor qword [{someValuePtr}], rcx",
                    "mov edx, 1",
                    "test eax, eax",
                    "je massDestruction",
                    "lea ecx, [rdx + 0x79]",
                    "add rsp, 0x28",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(_bgmToggleHook, out _bgmToggleWrapper)}",
                    "label massDestruction",
                    "mov ecx, 2",
                    "add rsp, 0x28",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 52, true)}",
                    "label fesVariantBGMs",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_femcHook, out _femcWrapper)}",
                    $"xor qword [{someValuePtr}], rcx",
                    "mov edx, 1",
                    "test eax, eax",
                    "je massDestructionFES",
                    "mov ecx, 0x8A",
                    "add rsp, 0x28",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(_bgmToggleHook, out _bgmToggleWrapper)}",
                    "label massDestructionFES",
                    "mov ecx, 0x6A",
                    "add rsp, 0x28",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 52, true)}",
                    "label Darkness",
                    "mov ecx, 0x85",
                    "lea edx, [rcx - 0x84]",
                    "add rsp, 0x28",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(_bgmToggleHook, out _bgmToggleWrapper)}",
                    "label heartfulCry",
                    "mov ecx, 0x84",
                    "lea edx, [rcx - 0x83]",
                    "add rsp, 0x28",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(_bgmToggleHook, out _bgmToggleWrapper)}",
                    "label masterOfTartarusDangerZone",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_femcHook, out _femcWrapper)}",
                    "mov edx, 0x7B",
                    "test eax, eax",
                    "mov ecx, 0x1F",
                    "cmovne cx, dx",
                    "mov edx, 1",
                    "add rsp, 0x28",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(_bgmToggleHook, out _bgmToggleWrapper)}",
                    "label hymmOfTheSoul",
                    "mov ecx, 0x1E",
                    "lea edx, [rcx - 0x1D]",
                    "add rsp, 0x28",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(_bgmToggleHook, out _bgmToggleWrapper)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });
        }
        [Function(new[] { Register.rax, Register.r13 }, Register.rax, true)]
        private delegate Scenario IsTheAnswerSetDelegate();
    }
}