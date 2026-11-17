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
using static P3PPC.Expansion.TheAnswer.Models.FileUtils;
using static Reloaded.Hooks.Definitions.X64.FunctionAttribute;
using static System.Runtime.InteropServices.JavaScript.JSType;
using IReloadedHooks = Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks;

namespace P3PPC.Expansion.TheAnswer.SaveData
{
    internal unsafe class P3PSaveDataAegis
    {
        private Memory _memory;
        private IAsmHook _isTheAnswerSetHook;
        private IReverseWrapper<IsTheAnswerSetDelegate> _isTheAnswerSetReverseWrapper;

        [Function(new Register[] { Register.rax }, Register.rax, true)]
        private delegate Scenario IsTheAnswerSetDelegate();

        uint saveFile;

        internal void Hook(IReloadedHooks hooks)
        {
            _memory = Memory.Instance;

            string saveIDFormatted = saveFile.ToString("D4");

            string p3pSave = $"P3PSAVE{saveIDFormatted}.BIN";
            var p3pSavePtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * p3pSave.Length)).Address;
            var stringBytes = Encoding.ASCII.GetBytes(p3pSave);
            var stringAddress = _memory.Allocate((nuint)stringBytes.Length).Address;
            _memory.WriteRaw(stringAddress, stringBytes);
            p3pSavePtr = (char**)stringAddress;

            string p3pSaveAegis = $"P3PSAVE{saveIDFormatted}_AEGIS.BIN";
            var p3pSaveAegisPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * p3pSaveAegis.Length)).Address;
            var stringBytesAegis = Encoding.ASCII.GetBytes(p3pSaveAegis);
            var stringAddressAegis = _memory.Allocate((nuint)stringBytesAegis.Length).Address;
            _memory.WriteRaw(stringAddressAegis, stringBytesAegis);
            p3pSaveAegisPtr = (char**)stringAddressAegis;

            string p3pSaveSlot = $"P3PSAVE{saveIDFormatted}.BINslot";
            var p3pSaveSlotPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * p3pSaveSlot.Length)).Address;
            var stringBytesSlot = Encoding.ASCII.GetBytes(p3pSaveSlot);
            var stringAddressSlot = _memory.Allocate((nuint)stringBytesSlot.Length).Address;
            _memory.WriteRaw(stringAddressSlot, stringBytesSlot);
            p3pSaveSlotPtr = (char**)stringAddressSlot;

            string p3pSaveAegisSlot = $"P3PSAVE{saveIDFormatted}_AEGIS.BINslot";
            var p3pSaveAegisSlotPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * p3pSaveAegisSlot.Length)).Address;
            var stringBytesAegisSlot = Encoding.ASCII.GetBytes(p3pSaveAegisSlot);
            var stringAddressAegisSlot = _memory.Allocate((nuint)stringBytesAegisSlot.Length).Address;
            _memory.WriteRaw(stringAddressAegisSlot, stringBytesAegisSlot);
            p3pSaveAegisSlotPtr = (char**)stringAddressAegisSlot;

            Utils.SigScan("41 89 F0 48 8D 15 ?? ?? ?? ?? 48 8D 4C 24 ?? E8 ?? ?? ?? ?? 48 FF CF 80 7F 01 00", "SaveDataAegis", address =>
            {
                nuint p3pSavePtrOff = (nuint)p3pSavePtr;
                nuint p3pSaveAegisPtrOff = (nuint)p3pSaveAegisPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    "cmp eax, 0",
                    "je AegisSave",
                    "mov r8d, esi",
                    $"lea rdx, [qword {p3pSavePtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 10, true)}",
                    "label AegisSave",
                    "mov r8d, esi",
                    $"lea rdx, [qword {p3pSaveAegisPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 10, true)}"

                };

                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.ExecuteFirst).Activate();
            });

            Utils.SigScan("41 89 F0 48 8D 15 ?? ?? ?? ?? 48 8D 8C 24 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 2B 0D ?? ?? ?? ?? 48 FF CB 80 7B 01 00", "SaveDataSlotAegis", address =>
            {
                nuint p3pSaveSlotPtrOff = (nuint)p3pSaveSlotPtr;
                nuint p3pSaveAegisSlotPtrOff = (nuint)p3pSaveAegisSlotPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    "cmp eax, 0",
                    "je AegisSave",
                    "mov r8d, esi",
                    $"lea rdx, [qword {p3pSaveSlotPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 10, true)}",
                    "label AegisSave",
                    "mov r8d, esi",
                    $"lea rdx, [qword {p3pSaveAegisSlotPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 10, true)}"

                };

                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.ExecuteAfter).Activate();
            });
        }
        private void SaveDataJourney(nuint stringBuffer, uint saveFile)
        {
            string saveIDFormatted = saveFile.ToString("D4");

            _memory.Write(stringBuffer, Encoding.ASCII.GetBytes($"P3PSAVE{saveIDFormatted}.BIN").Length);
        }
        private void SaveDataAnswer(nuint stringBuffer, uint saveFile)
        {
            string saveIDFormatted = saveFile.ToString("D4");

            _memory.Write(stringBuffer, Encoding.ASCII.GetBytes($"P3PSAVE{saveIDFormatted}_AEGIS.BIN").Length);
        }
        private void SaveDataSlotJourney(nuint stringBuffer, uint saveFile)
        {
            string saveIDFormatted = saveFile.ToString("D4");

            _memory.Write(stringBuffer, Encoding.ASCII.GetBytes($"P3PSAVE{saveIDFormatted}.BINslot").Length);
        }
    }
}
