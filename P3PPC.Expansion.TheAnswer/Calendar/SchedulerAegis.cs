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

namespace P3PPC.Expansion.TheAnswer.Calendar
{
    internal unsafe class SchedulerAegis
    {
        private Memory _memory;
        private IAsmHook _isTheAnswerSetHook;
        private IReverseWrapper<IsTheAnswerSetDelegate> _isTheAnswerSetReverseWrapper;

        private int* schedulerDefaultValue;

        internal void Hook(IReloadedHooks hooks)
        {
            _memory = Memory.Instance;

            string schedularFile = "scheduler/scheduler_%02d.bf";
            var schedularFilePtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * schedularFile.Length)).Address;
            var stringBytes = Encoding.ASCII.GetBytes(schedularFile);
            var stringAddress = _memory.Allocate((nuint)stringBytes.Length).Address;
            _memory.WriteRaw(stringAddress, stringBytes);
            schedularFilePtr = (char**)stringAddress;

            string schedularFileAegis = "scheduler/scheduler_%02d_aegis.bf";
            var schedularFileAegisPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * schedularFileAegis.Length)).Address;
            var stringBytesAegis = Encoding.ASCII.GetBytes(schedularFileAegis);
            var stringAddressAegis = _memory.Allocate((nuint)stringBytesAegis.Length).Address;
            _memory.WriteRaw(stringAddressAegis, stringBytesAegis);
            schedularFileAegisPtr = (char**)stringAddressAegis;

            schedulerDefaultValue = (int*)_memory.Allocate((sizeof(int))).Address;

            *schedulerDefaultValue = -1;

            Utils.SigScan("44 8B 05 ?? ?? ?? ?? 48 8D 15 ?? ?? ?? ?? 48 8D 4D B0 E8 ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 8D 4D B0 33 D2 48 63 B0 ?? ?? ?? ??", "SchedulerAegis1", address =>
            {
                nuint schedularFilePtrOff = (nuint)schedularFilePtr;
                nuint schedularFileAegisPtrOff = (nuint)schedularFileAegisPtr;
                nuint schedulerDefaultValueOff = (nuint)schedulerDefaultValue;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je AegisScheduler",
                    $"mov r8d, [qword {schedulerDefaultValueOff}]",
                    $"lea rdx, [qword {schedularFilePtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 14, true)}",
                    "label AegisScheduler",
                    $"mov r8d, [qword {schedulerDefaultValueOff}]",
                    $"lea rdx, [qword {schedularFileAegisPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 14, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("89 3D ?? ?? ?? ?? 48 8D 15 ?? ?? ?? ?? 48 8D 4D B0 E8 ?? ?? ?? ?? 33 D2 48 8D 4D B0 E8 ?? ?? ?? ?? BE ?? ?? ?? ?? 89 33", "SchedulerAegis2", address =>
            {
                nuint schedularFilePtrOff = (nuint)schedularFilePtr;
                nuint schedularFileAegisPtrOff = (nuint)schedularFileAegisPtr;
                nuint schedulerDefaultValueOff = (nuint)schedulerDefaultValue;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je AegisScheduler",
                    $"mov [qword {schedulerDefaultValueOff}], edi",
                    $"lea rdx, [qword {schedularFilePtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 13, true)}",
                    "label AegisScheduler",
                    $"mov [qword {schedulerDefaultValueOff}], edi",
                    $"lea rdx, [qword {schedularFileAegisPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 13, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });
        }
        [Function(new[] { Register.rax, Register.r13 }, Register.rax, true)]
        private delegate Scenario IsTheAnswerSetDelegate();
    }
}
