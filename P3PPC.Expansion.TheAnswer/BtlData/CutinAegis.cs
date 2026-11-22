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
    internal unsafe class CutinAegis
    {
        private Memory _memory;
        private IAsmHook _isTheAnswerSetHook;
        private FemcDelegate _femcHook;
        private IReverseWrapper<IsTheAnswerSetDelegate> _isTheAnswerSetReverseWrapper;
        private IReverseWrapper<FemcDelegate> _femcWrapper;

        [Function(CallingConventions.Microsoft)]
        private delegate bool FemcDelegate();

        private byte* movByte;
        private byte* addByte;

        internal void Hook(IReloadedHooks hooks)
        {
            _memory = Memory.Instance;
            string BOKOCutin = "battle/cutin/syoudaku/BOKO_SD_%X.EPL";
            var BOKOCutinPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * BOKOCutin.Length)).Address;
            var stringBytes = Encoding.ASCII.GetBytes(BOKOCutin);
            var stringAddress = _memory.Allocate((nuint)stringBytes.Length).Address;
            _memory.WriteRaw(stringAddress, stringBytes);
            BOKOCutinPtr = (char**)stringAddress;

            string SYKNCutin = "battle/cutin/syoukan/SYKN_%X.EPL";
            var SYKNCutinPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * SYKNCutin.Length)).Address;
            var stringBytesSYKN = Encoding.ASCII.GetBytes(SYKNCutin);
            var stringAddressSYKN = _memory.Allocate((nuint)stringBytesSYKN.Length).Address;
            _memory.WriteRaw(stringAddressSYKN, stringBytesSYKN);
            SYKNCutinPtr = (char**)stringAddressSYKN;

            string TUGKCutin = "battle/cutin/syoudaku/TUGK_SD_%X.EPL";
            var TUGKCutinPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * TUGKCutin.Length)).Address;
            var stringBytesTUGK = Encoding.ASCII.GetBytes(TUGKCutin);
            var stringAddressTUGK = _memory.Allocate((nuint)stringBytesTUGK.Length).Address;
            _memory.WriteRaw(stringAddressTUGK, stringBytesTUGK);
            TUGKCutinPtr = (char**)stringAddressTUGK;

            string TOUCutin = "battle/cutin/syoudaku/TOU_SD_%X.EPL"; //unused but adding incase something breaks. Not including an AGS version.
            var TOUCutinPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * TOUCutin.Length)).Address;
            var stringBytesTOU = Encoding.ASCII.GetBytes(TOUCutin);
            var stringAddressTOU = _memory.Allocate((nuint)stringBytesTOU.Length).Address;
            _memory.WriteRaw(stringAddressTOU, stringBytesTOU);
            TOUCutinPtr = (char**)stringAddressTOU;

            string BOKOCutinAGS = "battle/cutin/syoudaku/AGS_BOKO_SD_%X.EPL";
            var BOKOCutinAGSPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * BOKOCutinAGS.Length)).Address;
            var stringBytesAGS = Encoding.ASCII.GetBytes(BOKOCutinAGS);
            var stringAddressAGS = _memory.Allocate((nuint)stringBytesAGS.Length).Address;
            _memory.WriteRaw(stringAddressAGS, stringBytesAGS);
            BOKOCutinAGSPtr = (char**)stringAddressAGS;

            string SYKNCutinAGS = "battle/cutin/syoukan/AGS_SYKN_%X.EPL";
            var SYKNCutinAGSPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * SYKNCutinAGS.Length)).Address;
            var stringBytesSYKNAGS = Encoding.ASCII.GetBytes(SYKNCutinAGS);
            var stringAddressSYKNAGS = _memory.Allocate((nuint)stringBytesSYKNAGS.Length).Address;
            _memory.WriteRaw(stringAddressSYKNAGS, stringBytesSYKNAGS);
            SYKNCutinAGSPtr = (char**)stringAddressSYKNAGS;

            string TUGKCutinAGS = "battle/cutin/syoudaku/AGS_TUGK_SD_%X.EPL";
            var TUGKCutinAGSPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * TUGKCutinAGS.Length)).Address;
            var stringBytesTUGKAGS = Encoding.ASCII.GetBytes(TUGKCutinAGS);
            var stringAddressTUGKAGS = _memory.Allocate((nuint)stringBytesTUGKAGS.Length).Address;
            _memory.WriteRaw(stringAddressTUGKAGS, stringBytesTUGKAGS);
            TUGKCutinAGSPtr = (char**)stringAddressTUGKAGS;

            movByte = (byte*)_memory.Allocate((sizeof(byte))).Address;
            addByte = (byte*)_memory.Allocate((sizeof(byte))).Address;

            *movByte = 0x31;
            *addByte = 0xB8;

            Utils.SigScan("48 83 EC 28 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 0F B6 05 45 AD 10 03 C1 E8 07 48 83 C4 28 C3", "IsFemc", address =>
            {
                _femcHook = hooks.CreateWrapper<FemcDelegate>(address, out _);
            });

            Utils.SigScan("39 CA 75 42 48 8D 15 ?? ?? ?? ?? EB 10 48 8D 15 ?? ?? ?? ?? EB 07 48 8D 15 ?? ?? ?? ?? 44 0F BF 43 ?? 48 8D 4C 24 ?? E8 ?? ?? ?? ?? 31 D2", "syoudakuAegis", address =>
            {
                nuint BOKOCutinPtrOff = (nuint)BOKOCutinPtr;
                nuint BOKOCutinAGSPtrOff = (nuint)BOKOCutinAGSPtr;
                nuint TUGKCutinPtrOff = (nuint)TUGKCutinPtr;
                nuint TUGKCutinAGSPtrOff = (nuint)TUGKCutinAGSPtr;
                nuint TOUCutinPtrOff = (nuint)TOUCutinPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je AegisSyoudaku",
                    "cmp edx, 2",
                    "jne skipSyoudaku",
                    $"lea rdx, [qword {TOUCutinPtrOff}]",
                    "jmp loadSyoudaku",
                    $"lea rdx, [qword {TUGKCutinPtrOff}]",
                    "jmp loadSyoudaku",
                    $"lea rdx, [qword {BOKOCutinPtrOff}]",
                    "label loadSyoudaku",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 29, true)}",
                    "label skipSyoudaku",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 70, true)}",
                    "label AegisSyoudaku",
                    "cmp edx, 2",
                    "jne skipAegisSyoudaku",
                    $"lea rdx, [qword {TOUCutinPtrOff}]",
                    "jmp loadAegisSyoudaku",
                    $"lea rdx, [qword {TUGKCutinAGSPtrOff}]",
                    "jmp loadAegisSyoudaku",
                    $"lea rdx, [qword {BOKOCutinAGSPtrOff}]",
                    "label loadAegisSyoudaku",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 29, true)}",
                    "label skipAegisSyoudaku",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 70, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("0F B7 43 02 66 83 F8 0B 7D 2A 66 83 F8 01 75 1D E8 ?? ?? ?? ?? 8A 0D ?? ?? ?? ?? 00 0D ?? ?? ?? ?? 85 C0 74 08 41 B8 ?? ?? ?? ??", "SYKNAegis", address =>
            {
                nuint SYKNCutinPtrOff = (nuint)SYKNCutinPtr;
                nuint SYKNCutinAGSPtrOff = (nuint)SYKNCutinAGSPtr;
                nuint movByteOff = (nuint)movByte;
                nuint addByteOff = (nuint)addByte;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je AegisSYKN",
                    "movzx eax, word [rbx + 2]",
                    "cmp ax, 0xB",
                    "jnl skipFemc",
                    "cmp ax, 1",
                    "jne getSYKNFile",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_femcHook, out _femcWrapper)}",
                    $"mov cl, byte [{movByteOff}]",
                    $"add byte [{addByteOff}], cl",
                    "test eax, eax",
                    "je getSYKNFile",
                    "mov r8d, 0x63",
                    "jmp SYKNFile",
                    "label getSYKNFile",
                    "movsx r8d, word [rbx + 2]",
                    "jmp SYKNFile",
                    "label skipFemc",
                    "movsx r8d, ax",
                    "label SYKNFile",
                    $"lea rdx, [qword {SYKNCutinPtrOff}]",
                    "lea rcx, [rsp + 0x20]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 68, true)}",
                    "label AegisSYKN",
                    "movzx eax, word [rbx + 2]",
                    "cmp ax, 0xB",
                    "jnl skipFemcAegis",
                    "cmp ax, 1",
                    "jne getSYKNFileAegis",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_femcHook, out _femcWrapper)}",
                    $"mov cl, byte [{movByteOff}]",
                    $"add byte [{addByteOff}], cl",
                    "test eax, eax",
                    "je getSYKNFileAegis",
                    "mov r8d, 0x63",
                    "jmp SYKNFileAegis",
                    "label getSYKNFileAegis",
                    "movsx r8d, word [rbx + 2]",
                    "jmp SYKNFileAegis",
                    "label skipFemcAegis",
                    "movsx r8d, ax",
                    "label SYKNFileAegis",
                    $"lea rdx, [qword {SYKNCutinAGSPtrOff}]",
                    "lea rcx, [rsp + 0x20]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 68, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });
        }
        [Function(new[] { Register.rax, Register.r13 }, Register.rax, true)]
        private delegate Scenario IsTheAnswerSetDelegate();
    }
}