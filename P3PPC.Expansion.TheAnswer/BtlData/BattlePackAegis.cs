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
    internal unsafe class BattlePackAegis
    {
        private Memory _memory;
        private IAsmHook _isTheAnswerSetHook;
        private BitCheckDelegate _bitCheckHook;
        private IReverseWrapper<IsTheAnswerSetDelegate> _isTheAnswerSetReverseWrapper;
        private IReverseWrapper<BitCheckDelegate> _bitCheckWrapper;

        [Function(CallingConventions.Microsoft)]
        private delegate long BitCheckDelegate(int bitId);

        internal void Hook(IReloadedHooks hooks)
        {
            _memory = Memory.Instance;
            string battlePack = "battle/battle_pack.bin";
            var battlePackPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * battlePack.Length)).Address;
            var stringBytes = Encoding.ASCII.GetBytes(battlePack);
            var stringAddress = _memory.Allocate((nuint)stringBytes.Length).Address;
            _memory.WriteRaw(stringAddress, stringBytes);
            battlePackPtr = (char**)stringAddress;

            string battlePack2 = "battle/battle_pack2.bin";
            var battlePack2Ptr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * battlePack2.Length)).Address;
            var stringBytes2 = Encoding.ASCII.GetBytes(battlePack2);
            var stringAddress2 = _memory.Allocate((nuint)stringBytes2.Length).Address;
            _memory.WriteRaw(stringAddress2, stringBytes2);
            battlePack2Ptr = (char**)stringAddress2;

            string battlePack2AGS = "battle/AGS_battle_pack2.bin";
            var battlePack2AGSPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * battlePack2AGS.Length)).Address;
            var stringBytes2AGS = Encoding.ASCII.GetBytes(battlePack2AGS);
            var stringAddress2AGS = _memory.Allocate((nuint)stringBytes2AGS.Length).Address;
            _memory.WriteRaw(stringAddress2AGS, stringBytes2AGS);
            battlePack2AGSPtr = (char**)stringAddress2AGS;

            Utils.SigScan("40 53 48 83 EC 20 8B D9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 8B C3 99 83 E2 1F 03 C2 8B C8 83 E0 1F 2B C2", "BitCheck", address =>
            {
                _bitCheckHook = hooks.CreateWrapper<BitCheckDelegate>(address, out _);
            });

            Utils.SigScan("B9 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? BA ?? ?? ?? ?? 85 C0 75 07 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 48 89 81 ?? ?? ?? ??", "BattlePackAegis", address =>
            {
                nuint battlePackPtrOff = (nuint)battlePackPtr;
                nuint battlePack2PtrOff = (nuint)battlePack2Ptr;
                nuint battlePack2AGSPtrOff = (nuint)battlePack2AGSPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je AegisBattlePack",
                    "mov ecx, 0x141",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_bitCheckHook, out _bitCheckWrapper)}",
                    $"lea rcx, [qword {battlePackPtrOff}]",
                    "mov edx, 1",
                    "test eax, eax",
                    "jne endLabel",
                    $"lea rcx, [qword {battlePack2PtrOff}]",
                    "label endLabel",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 33, true)}",
                    "label AegisBattlePack",
                    "mov ecx, 0x141",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_bitCheckHook, out _bitCheckWrapper)}",
                    $"lea rcx, [qword {battlePackPtrOff}]",
                    "mov edx, 1",
                    "test eax, eax",
                    "jne endLabel2",
                    $"lea rcx, [qword {battlePack2AGSPtrOff}]",
                    "label endLabel2",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 33, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });
        }
        [Function(new[] { Register.rax, Register.r13 }, Register.rax, true)]
        private delegate Scenario IsTheAnswerSetDelegate();
    }
}