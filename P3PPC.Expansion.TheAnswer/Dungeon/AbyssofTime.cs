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

namespace P3PPC.Expansion.TheAnswer.Dungeon
{
    internal unsafe class AbyssofTime
    {
        private Memory _memory;
        private IAsmHook _isTheAnswerSetHook;
        private FloorInitDelegate _floorInitHook;
        private IReverseWrapper<IsTheAnswerSetDelegate> _isTheAnswerSetReverseWrapper;
        private IReverseWrapper<FloorInitDelegate> _floorInitWrapper;

        [Function(CallingConventions.Microsoft)]
        private delegate long FloorInitDelegate(int fieldMajorID);

        internal void Hook(IReloadedHooks hooks)
        {
            _memory = Memory.Instance;

            Utils.SigScan("48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 48 89 7C 24 ?? 41 54 41 56 41 57 48 83 EC 30 44 8B F9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 45 33 E4 E8 ?? ?? ?? ?? 0F B6 58 71", "FloorInit", address =>
            {
                _floorInitHook = hooks.CreateWrapper<FloorInitDelegate>(address, out _);
            });

            Utils.SigScan("0F B7 43 04 66 83 F8 16 74 3E 66 83 F8 17 74 38 66 83 F8 18 74 32 66 83 F8 19 74 2C 66 83 F8 1A 74 26 66 83 F8 15 75 2B B8 ?? ?? ?? ?? 66 39 43 06", "CheckFields", address =>
            {
                string[] asm =
                {
                    "use64",
                    "movzx eax, word [rbx + 4]",
                    "cmp ax, 0x16",
                    "je getFloor",
                    "cmp ax, 0x17",
                    "je getFloor",
                    "cmp ax, 0x18",
                    "je getFloor",
                    "cmp ax, 0x19",
                    "je getFloor",
                    "cmp ax, 0x1A",
                    "je getFloor",
                    "cmp ax, 0x2A",
                    "je getFloor",
                    "cmp ax, 0x2B",
                    "je getFloor",
                    "cmp ax, 0x2C",
                    "je getFloor",
                    "cmp ax, 0x2D",
                    "je getFloor",
                    "cmp ax, 0x2E",
                    "je getFloor",
                    "cmp ax, 0x15",
                    "jne getBlock1",
                    "cmp ax, 0x29",
                    "jne getBlock1",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 40, true)}",
                    "label getFloor",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 72, true)}",
                    "label getBlock1",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 83, true)}",
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("41 8B 0A 41 8B D4 83 E9 14 E8 ?? ?? ?? ?? E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? E8 ?? ?? ?? ?? 85 C0", "LoadFields", address =>
            {
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je abyssOfTimeFloors",
                    "mov ecx, [r10]",
                    "mov edx, r12d",
                    "sub ecx, 0x14",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_floorInitHook, out _floorInitWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 14, true)}",
                    "label abyssOfTimeFloors",
                    "mov ecx, [r10]",
                    "mov edx, r12d",
                    "sub ecx, 0x28",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_floorInitHook, out _floorInitWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 14, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });
        }
        [Function(new[] { Register.rax, Register.r13 }, Register.rax, true)]
        private delegate Scenario IsTheAnswerSetDelegate();
    }
}