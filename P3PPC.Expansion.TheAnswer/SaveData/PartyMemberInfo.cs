using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Memory;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using static P3PPC.Expansion.TheAnswer.Enums.ScenarioMode;
using static P3PPC.Expansion.TheAnswer.IsTheAnswer;
using static P3PPC.Expansion.TheAnswer.KwlnTask.KwlnTaskP3P;
using static Reloaded.Hooks.Definitions.X64.FunctionAttribute;
using IReloadedHooks = Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks;

namespace P3PPC.Expansion.TheAnswer.SaveData
{
    internal unsafe class PartyMemberInfo
    {
        private Memory _memory;
        private IAsmHook _isTheAnswerSetHook;
        private PartyMemberData1Delegate _partyMemberData1Hook;
        private PartyMemberData2Delegate _partyMemberData2Hook;
        private SetDateSettingDelegate _setDateSettingHook;
        private IReverseWrapper<IsTheAnswerSetDelegate> _isTheAnswerSetReverseWrapper;
        private IReverseWrapper<PartyMemberData1Delegate> _partyMemberData1Wrapper;
        private IReverseWrapper<PartyMemberData2Delegate> _partyMemberData2Wrapper;
        private IReverseWrapper<SetDateSettingDelegate> _setDateSettingWrapper;

        [Function(CallingConventions.Microsoft)]
        private delegate nint PartyMemberData1Delegate(short param_1, char param_2);

        [Function(CallingConventions.Microsoft)]
        private delegate nint PartyMemberData2Delegate(short param_1, int param_2);

        [Function(CallingConventions.Microsoft)]
        private delegate nint SetDateSettingDelegate(int param_1);

        private int YukariMakotoPersonaJourney = 0xC00001;
        private int MitsuruAigisPersonaJourney = 0xC400C2;
        private int FuukaJunpeiPersonaJourney = 0xC800C6;
        private int KenAkihikoPersonaJourney = 0xCC00CA;
        private int KoromaruShinjiPersonaJourney = 0xCF00CE;

        private int YukariAigisPersonaAnswer = 0xC100C3;
        private int MitsuruJAigisPersonaAnswer = 0xC500C2;
        private int FuukaJunpeiPersonaAnswer = 0xC900C7;
        private int KenAkihikoPersonaAnswer = 0xCD00CB;
        private int KoromaruMetisPersonaAnswer = 0xCF00D3;

        internal void Hook(IReloadedHooks hooks)
        {

            Utils.SigScan("48 89 5C 24 ?? 57 48 83 EC 20 0F B7 D9 0F B6 FA 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 66 83 FB 01 75 12 40 88 3D ?? ?? ?? ?? 48 8B 5C 24 ?? 48 83 C4 20 5F C3", "PartyMemberData1", address =>
            {
                _partyMemberData1Hook = hooks.CreateWrapper<PartyMemberData1Delegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 57 48 83 EC 20 0F B7 D9 89 D7 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 66 83 FB 01 75 11 89 3D ?? ?? ?? ?? 48 8B 5C 24 ?? 48 83 C4 20 5F C3", "PartyMemberData2", address =>
            {
                _partyMemberData2Hook = hooks.CreateWrapper<PartyMemberData2Delegate>(address, out _);
            });

            Utils.SigScan("40 53 48 83 EC 20 89 CB 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 89 1D ?? ?? ?? ?? 48 83 C4 20 5B C3", "SetDateSetting", address =>
            {
                _setDateSettingHook = hooks.CreateWrapper<SetDateSettingDelegate>(address, out _);
            });

            Utils.SigScan("39 FB 75 16 40 0F B6 D7 0F B7 CB E8 ?? ?? ?? ?? 31 D2 0F B7 CB E8 ?? ?? ?? ?? BA ?? ?? ?? ??", "PartyMemberInfo1", address =>
            {
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je aegisInfo1",
                    "cmp ebx, 1",
                    "jne skipToNextInfo",
                    "mov edx, 1",
                    "movzx ecx, bx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_partyMemberData1Hook, out _partyMemberData1Wrapper)}",
                    "jmp skipToNextInfo",
                    "label aegisInfo1",
                    "cmp ebx, 1",
                    "jne skipToNextInfo",
                    "mov edx, 0x14",
                    "movzx ecx, bx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_partyMemberData1Hook, out _partyMemberData1Wrapper)}",
                    "label skipToNextInfo",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je aegisInfo2",
                    "cmp ebx, 1",
                    "jne skipInfo",
                    "xor edx, edx",
                    "movzx ecx, bx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_partyMemberData2Hook, out _partyMemberData2Wrapper)}",
                    "jmp skipInfo",
                    "label aegisInfo2",
                    "cmp ebx, 1",
                    "jne skipInfo",
                    "mov edx, 0x2BCA",
                    "movzx ecx, bx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_partyMemberData2Hook, out _partyMemberData2Wrapper)}",
                    "label skipInfo",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 26, true)}"
                };

                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("B8 ?? ?? ?? ?? C7 44 24 ?? ?? ?? ?? ?? 0F B7 CB 66 89 44 24 ?? C7 44 24 ?? ?? ?? ?? ?? C7 44 24 ?? ?? ?? ?? ?? C7 44 24 ?? ?? ?? ?? ?? C7 44 24 ?? ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 63 C3", "PartyMemberPersonas", address =>
            {
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je answerPersonaSet",
                    "mov eax, 0xAE",
                    "mov dword [rsp + 0x20], 0xC00001",
                    "movzx ecx, bx",
                    "mov [rsp + 0x34], ax",
                    "mov dword [rsp + 0x24], 0xC400C2",
                    "mov dword [rsp + 0x28], 0xC800C6",
                    "mov dword [rsp + 0x2C], 0xCC00CA",
                    "mov dword [rsp + 0x30], 0xCF00CE",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 53, true)}",
                    "label answerPersonaSet",
                    "mov eax, 0xC3",
                    "mov dword [rsp + 0x20], 0xC100C3",
                    "movzx ecx, bx",
                    "mov [rsp + 0x34], ax",
                    "mov dword [rsp + 0x24], 0xC500C2",
                    "mov dword [rsp + 0x28], 0xC900C7",
                    "mov dword [rsp + 0x2C], 0xCD00CB",
                    "mov dword [rsp + 0x30], 0xCF00D3",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 53, true)}",
                };

                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("31 C9 E8 ?? ?? ?? ?? B9 ?? ?? ?? ?? E8 ?? ?? ?? ?? B1 05 E8 ?? ?? ?? ?? 31 C9 E8 ?? ?? ?? ?? BF ?? ?? ?? ?? 89 FB 0F 1F 40 00", "NewSetDateSetting", address =>
            {
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je answerDate",
                    "xor ecx, ecx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_setDateSettingHook, out _setDateSettingWrapper)}",
                    "jmp endLabel",
                    "label answerDate",
                    "mov ecx, 1",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_setDateSettingHook, out _setDateSettingWrapper)}",
                    "label endLabel",
                    "mov ecx, 5",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 12, true)}",
                };

                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });
        }
        [Function(new[] { Register.rax, Register.r13 }, Register.rax, true)]
        private delegate Scenario IsTheAnswerSetDelegate();
    }
}