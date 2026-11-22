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
    internal unsafe class PartyAegis
    {
        private Memory _memory;
        private IAsmHook _isTheAnswerSetHook;
        private IReverseWrapper<IsTheAnswerSetDelegate> _isTheAnswerSetReverseWrapper;

        internal void Hook(IReloadedHooks hooks)
        {
            _memory = Memory.Instance;
            string partyBin = "camp/party/party.bin";
            var partyBinPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * partyBin.Length)).Address;
            var stringBytes = Encoding.ASCII.GetBytes(partyBin);
            var stringAddress = _memory.Allocate((nuint)stringBytes.Length).Address;
            _memory.WriteRaw(stringAddress, stringBytes);
            partyBinPtr = (char**)stringAddress;

            string partyBinAGS = "camp/party/AGS_party.bin";
            var partyBinAGSPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * partyBinAGS.Length)).Address;
            var stringBytesAGS = Encoding.ASCII.GetBytes(partyBinAGS);
            var stringAddressAGS = _memory.Allocate((nuint)stringBytesAGS.Length).Address;
            _memory.WriteRaw(stringAddressAGS, stringBytesAGS);
            partyBinAGSPtr = (char**)stringAddressAGS;

            Utils.SigScan("48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 89 83 ?? ?? ?? ?? 48 85 C0 75 15 8B 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 83 C2 0F E8 ?? ?? ?? ?? 66 89 B3 ?? ?? ?? ??", "PartyAegis", address =>
            {
                nuint partyBinPtrOff = (nuint)partyBinPtr;
                nuint partyBinAGSPtrOff = (nuint)partyBinAGSPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je AegisParty",
                    "xor edx, edx",
                    $"lea rcx, [qword {partyBinPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 7, true)}",
                    "label AegisParty",
                    "xor edx, edx",
                    $"lea rcx, [qword {partyBinAGSPtrOff}]"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });
        }
        [Function(new[] { Register.rax, Register.r13 }, Register.rax, true)]
        private delegate Scenario IsTheAnswerSetDelegate();
    }
}