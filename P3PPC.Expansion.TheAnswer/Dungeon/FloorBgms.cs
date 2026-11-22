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
    internal unsafe class FloorBgms
    {
        private Memory _memory;
        private IAsmHook _isTheAnswerSetHook;
        private IReverseWrapper<IsTheAnswerSetDelegate> _isTheAnswerSetReverseWrapper;

        internal void Hook(IReloadedHooks hooks)
        {
            _memory = Memory.Instance;

            Utils.SigScan("81 FB 91 01 00 00 0F 8D ?? ?? ?? ?? 81 FB ?? ?? ?? ?? 0F 8F ?? ?? ?? ?? 81 FB ?? ?? ?? ?? 7C 12 B9 ?? ?? ?? ?? 8D 51 B6 48 83 C4 20 5B E9 ?? ?? ?? ??", "FloorBgms", address =>
            {
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je abyssOfTime",
                    "cmp ebx, 0x191",
                    "jnl Arqa",
                    "cmp ebx, 0x121",
                    "jg Thebel",
                    "cmp ebx, 0xFF",
                    "jl block6",
                    "mov ecx, 0x4B",
                    "lea edx, [rcx - 0x4A]",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 45, true)}",
                    "label block6",
                    "cmp ebx, 0xD7",
                    "jl block5",
                    "mov ecx, 0x4A",
                    "lea edx, [rcx - 0x49]",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 71, true)}",
                    "label block5",
                    "cmp ebx, 0xA5",
                    "jl block4",
                    "mov ecx, 0x49",
                    "lea edx, [rcx - 0x48]",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 97, true)}",
                    "label block4",
                    "cmp ebx, 0x73",
                    "jl block3",
                    "mov ecx, 0x48",
                    "lea edx, [rcx - 0x47]",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 120, true)}",
                    "label block3",
                    "cmp ebx, 0x41",
                    "jl block2",
                    "mov ecx, 0x51",
                    "lea edx, [rcx - 0x50]",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 143, true)}",
                    "label block2",
                    "cmp ebx, 0x11",
                    "jnl Arqa",
                    "cmp ebx, 2",
                    "jnl Thebel",
                    "cmp ebx, 1",
                    "jl nullBgm",
                    "mov ecx, 0x47",
                    "lea edx, [rcx - 0x46]",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 180, true)}",
                    "label Thebel",
                    "mov ecx, 1",
                    "mov edx, ecx",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 197, true)}",
                    "label Arqa",
                    "mov ecx, 0x46",
                    "lea edx, [rcx - 0x45]",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 215, true)}",
                    "label nullBgm",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 1149, true)}",
                    "label abyssOfTime",
                    "cmp ebx, 0xA1",
                    "jl door6",
                    "mov ecx, 0x64",
                    "lea edx, [rcx - 0x63]",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 71, true)}",
                    "label door6",
                    "cmp ebx, 0x83",
                    "jl door5",
                    "mov ecx, 0x63",
                    "lea edx, [rcx - 0x62]",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 97, true)}",
                    "label door5",
                    "cmp ebx, 0x65",
                    "jl door4",
                    "mov ecx, 0x62",
                    "lea edx, [rcx - 0x61]",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 120, true)}",
                    "label door4",
                    "cmp ebx, 0x47",
                    "jl door3",
                    "mov ecx, 0x61",
                    "lea edx, [rcx - 0x60]",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 143, true)}",
                    "label door3",
                    "cmp ebx, 0x29",
                    "jl defaultSet",
                    "mov ecx, 0x60",
                    "lea edx, [rcx - 0x5F]",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 143, true)}",
                    "label defaultSet",
                    "cmp ebx, 0x15",
                    "jnl door2",
                    "cmp ebx, 2",
                    "jnl door1",
                    "cmp ebx, 1",
                    "jl nullBgm2",
                    "mov ecx, 0x65",
                    "lea edx, [rcx - 0x64]",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 180, true)}",
                    "label door1",
                    "mov ecx, 0x5E",
                    "lea edx, [rcx - 0x5D]",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 197, true)}",
                    "label door2",
                    "mov ecx, 0x5F",
                    "lea edx, [rcx - 0x5E]",
                    "add rsp, 0x20",
                    "pop rbx",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 215, true)}",
                    "label nullBgm2",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 1149, true)}",
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });
        }
        [Function(new[] { Register.rax, Register.r13 }, Register.rax, true)]
        private delegate Scenario IsTheAnswerSetDelegate();
    }
}