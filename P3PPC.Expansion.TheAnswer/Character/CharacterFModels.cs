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

namespace P3PPC.Expansion.TheAnswer.Character
{
    internal unsafe class CharacterFModels
    {
        private Memory _memory;
        private IAsmHook _isTheAnswerSetHook;
        private StringFormatDelegate _stringFormatHook;
        private CharIDDelegate _charIDHook;
        private BitCheckDelegate _bitCheckHook;
        private IReverseWrapper<IsTheAnswerSetDelegate> _isTheAnswerSetReverseWrapper;
        private IReverseWrapper<StringFormatDelegate> _stringFormatWrapper;
        private IReverseWrapper<CharIDDelegate> _charIDWrapper;
        private IReverseWrapper<BitCheckDelegate> _bitCheckWrapper;

        [Function(CallingConventions.Microsoft)]
        private delegate nint StringFormatDelegate(nint param_1, string path, int id);

        [Function(CallingConventions.Microsoft)]
        private delegate long CharIDDelegate(short id);

        [Function(CallingConventions.Microsoft)]
        private delegate long BitCheckDelegate(int bitId);

        internal void Hook(IReloadedHooks hooks)
        {
            _memory = Memory.Instance;

            string answerAigisDefaultFile = "%splayer/bc%03Xf.GMO";
            var answerAigisDefaultFilePtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * answerAigisDefaultFile.Length)).Address;
            var stringBytes = Encoding.ASCII.GetBytes(answerAigisDefaultFile);
            var stringAddress = _memory.Allocate((nuint)stringBytes.Length).Address;
            _memory.WriteRaw(stringAddress, stringBytes);
            answerAigisDefaultFilePtr = (char**)stringAddress;

            string answerAigisCos0File = "%splayer/bc%03Xf_c0.GMO";
            var answerAigisCos0FilePtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * answerAigisCos0File.Length)).Address;
            var stringBytesCos0 = Encoding.ASCII.GetBytes(answerAigisCos0File);
            var stringAddressCos0 = _memory.Allocate((nuint)stringBytesCos0.Length).Address;
            _memory.WriteRaw(stringAddressCos0, stringBytesCos0);
            answerAigisCos0FilePtr = (char**)stringAddressCos0;

            string answerAigisCos3File = "%splayer/bc%03Xf_c3.GMO";
            var answerAigisCos3FilePtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * answerAigisCos3File.Length)).Address;
            var stringBytesCos3 = Encoding.ASCII.GetBytes(answerAigisCos3File);
            var stringAddressCos3 = _memory.Allocate((nuint)stringBytesCos3.Length).Address;
            _memory.WriteRaw(stringAddressCos3, stringBytesCos3);
            answerAigisCos3FilePtr = (char**)stringAddressCos3;

            string answerAigisCos4File = "%splayer/bc%03Xf_c4.GMO";
            var answerAigisCos4FilePtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * answerAigisCos4File.Length)).Address;
            var stringBytesCos4 = Encoding.ASCII.GetBytes(answerAigisCos4File);
            var stringAddressCos4 = _memory.Allocate((nuint)stringBytesCos4.Length).Address;
            _memory.WriteRaw(stringAddressCos4, stringBytesCos4);
            answerAigisCos4FilePtr = (char**)stringAddressCos4;

            string answerAigisCos6File = "%splayer/bc%03Xf_c6.GMO";
            var answerAigisCos6FilePtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * answerAigisCos6File.Length)).Address;
            var stringBytesCos6 = Encoding.ASCII.GetBytes(answerAigisCos6File);
            var stringAddressCos6 = _memory.Allocate((nuint)stringBytesCos6.Length).Address;
            _memory.WriteRaw(stringAddressCos6, stringBytesCos6);
            answerAigisCos6FilePtr = (char**)stringAddressCos6;

            string answerAigisCos9File = "%splayer/bc%03Xf_c9.GMO";
            var answerAigisCos9FilePtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * answerAigisCos9File.Length)).Address;
            var stringBytesCos9 = Encoding.ASCII.GetBytes(answerAigisCos9File);
            var stringAddressCos9 = _memory.Allocate((nuint)stringBytesCos9.Length).Address;
            _memory.WriteRaw(stringAddressCos9, stringBytesCos9);
            answerAigisCos9FilePtr = (char**)stringAddressCos9;

            string yukariDir = "%splayer/bc002.GMO";
            var yukariDirPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * yukariDir.Length)).Address;
            var stringBytesYukari = Encoding.ASCII.GetBytes(yukariDir);
            var stringAddressYukari = _memory.Allocate((nuint)stringBytesYukari.Length).Address;
            _memory.WriteRaw(stringAddressYukari, stringBytesYukari);
            yukariDirPtr = (char**)stringAddressYukari;

            string answerYukariDir = "%splayer/bc002f.GMO";
            var answerYukariDirPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * answerYukariDir.Length)).Address;
            var stringBytesYukariF = Encoding.ASCII.GetBytes(answerYukariDir);
            var stringAddressYukariF = _memory.Allocate((nuint)stringBytesYukariF.Length).Address;
            _memory.WriteRaw(stringAddressYukariF, stringBytesYukariF);
            answerYukariDirPtr = (char**)stringAddressYukariF;

            string mitsuruDir = "%splayer/bc004.GMO";
            var mitsuruDirPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * mitsuruDir.Length)).Address;
            var stringBytesMitsuru = Encoding.ASCII.GetBytes(mitsuruDir);
            var stringAddressMitsuru = _memory.Allocate((nuint)stringBytesMitsuru.Length).Address;
            _memory.WriteRaw(stringAddressMitsuru, stringBytesMitsuru);
            mitsuruDirPtr = (char**)stringAddressMitsuru;

            string answerMitsuruDir = "%splayer/bc004f.GMO";
            var answerMitsuruDirPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * answerMitsuruDir.Length)).Address;
            var stringBytesMitsuruF = Encoding.ASCII.GetBytes(answerMitsuruDir);
            var stringAddressMitsuruF = _memory.Allocate((nuint)stringBytesMitsuruF.Length).Address;
            _memory.WriteRaw(stringAddressMitsuruF, stringBytesMitsuruF);
            answerMitsuruDirPtr = (char**)stringAddressMitsuruF;

            string junpeiDir = "%splayer/bc005.GMO";
            var junpeiDirPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * junpeiDir.Length)).Address;
            var stringBytesJunpei = Encoding.ASCII.GetBytes(junpeiDir);
            var stringAddressJunpei = _memory.Allocate((nuint)stringBytesJunpei.Length).Address;
            _memory.WriteRaw(stringAddressJunpei, stringBytesJunpei);
            junpeiDirPtr = (char**)stringAddressJunpei;

            string answerJunpeiDir = "%splayer/bc005f.GMO";
            var answerJunpeiDirPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * answerJunpeiDir.Length)).Address;
            var stringBytesJunpeiF = Encoding.ASCII.GetBytes(answerJunpeiDir);
            var stringAddressJunpeiF = _memory.Allocate((nuint)stringBytesJunpeiF.Length).Address;
            _memory.WriteRaw(stringAddressJunpeiF, stringBytesJunpeiF);
            answerJunpeiDirPtr = (char**)stringAddressJunpeiF;

            string akihikoDir = "%splayer/bc007.GMO";
            var akihikoDirPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * akihikoDir.Length)).Address;
            var stringBytesAkihiko = Encoding.ASCII.GetBytes(akihikoDir);
            var stringAddressAkihiko = _memory.Allocate((nuint)stringBytesAkihiko.Length).Address;
            _memory.WriteRaw(stringAddressAkihiko, stringBytesAkihiko);
            akihikoDirPtr = (char**)stringAddressAkihiko;

            string answerAkihikoDir = "%splayer/bc007f.GMO";
            var answerAkihikoDirPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * answerAkihikoDir.Length)).Address;
            var stringBytesAkihikoF = Encoding.ASCII.GetBytes(answerAkihikoDir);
            var stringAddressAkihikoF = _memory.Allocate((nuint)stringBytesAkihikoF.Length).Address;
            _memory.WriteRaw(stringAddressAkihikoF, stringBytesAkihikoF);
            answerAkihikoDirPtr = (char**)stringAddressAkihikoF;

            string kenDir = "%splayer/bc008.GMO";
            var kenDirPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * kenDir.Length)).Address;
            var stringBytesKen = Encoding.ASCII.GetBytes(kenDir);
            var stringAddressKen = _memory.Allocate((nuint)stringBytesKen.Length).Address;
            _memory.WriteRaw(stringAddressKen, stringBytesKen);
            kenDirPtr = (char**)stringAddressKen;

            string answerKenDir = "%splayer/bc008f.GMO";
            var answerKenDirPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * answerKenDir.Length)).Address;
            var stringBytesKenF = Encoding.ASCII.GetBytes(answerKenDir);
            var stringAddressKenF = _memory.Allocate((nuint)stringBytesKenF.Length).Address;
            _memory.WriteRaw(stringAddressKenF, stringBytesKenF);
            answerKenDirPtr = (char**)stringAddressKenF;

            string shinjiDir = "%splayer/bc009.GMO";
            var shinjiDirPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * shinjiDir.Length)).Address;
            var stringBytesShinji = Encoding.ASCII.GetBytes(shinjiDir);
            var stringAddressShinji = _memory.Allocate((nuint)stringBytesShinji.Length).Address;
            _memory.WriteRaw(stringAddressShinji, stringBytesShinji);
            shinjiDirPtr = (char**)stringAddressShinji;

            string metisDir = "%splayer/bc009f.GMO";
            var metisDirPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * metisDir.Length)).Address;
            var stringBytesMetis = Encoding.ASCII.GetBytes(metisDir);
            var stringAddressMetis = _memory.Allocate((nuint)stringBytesMetis.Length).Address;
            _memory.WriteRaw(stringAddressMetis, stringBytesMetis);
            metisDirPtr = (char**)stringAddressMetis;

            string modelDir = "model/";
            var modelDirPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * modelDir.Length)).Address;
            var stringBytesModel = Encoding.ASCII.GetBytes(modelDir);
            var stringAddressModel = _memory.Allocate((nuint)stringBytesModel.Length).Address;
            _memory.WriteRaw(stringAddressModel, stringBytesModel);
            modelDirPtr = (char**)stringAddressModel;

            Utils.SigScan("48 89 54 24 ?? 4C 89 44 24 ?? 4C 89 4C 24 ?? 53 56 57 48 83 EC 30 48 8B F9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B 5C 24 ?? 48 8D 74 24 ?? E8 ?? ?? ?? ??", "StringFormat", address =>
            {
                _stringFormatHook = hooks.CreateWrapper<StringFormatDelegate>(address, out _);
            });

            Utils.SigScan("40 53 48 83 EC 20 0F B7 D9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 66 83 FB 01 75 0D 48 8D 05 ?? ?? ?? ?? 48 83 C4 20", "CharID", address =>
            {
                _charIDHook = hooks.CreateWrapper<CharIDDelegate>(address, out _);
            });

            Utils.SigScan("40 53 48 83 EC 20 8B D9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 8B C3 99 83 E2 1F 03 C2 8B C8 83 E0 1F 2B C2", "BitCheck", address =>
            {
                _bitCheckHook = hooks.CreateWrapper<BitCheckDelegate>(address, out _);
            });

            Utils.SigScan("B9 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B C8 E8 ?? ?? ?? ?? B9 ?? ?? ?? ?? E8 ?? ?? ?? ?? 85 C0 75 1B 4C 8D 05 ?? ?? ?? ?? 48 8B CF 48 8D", "AnswerAigisMakoto", address =>
            {
                nuint answerAigisDefaultFilePtrOff = (nuint)answerAigisDefaultFilePtr;
                nuint answerAigisCos0FilePtrOff = (nuint)answerAigisCos0FilePtr;
                nuint answerAigisCos3FilePtrOff = (nuint)answerAigisCos3FilePtr;
                nuint answerAigisCos4FilePtrOff = (nuint)answerAigisCos4FilePtr;
                nuint answerAigisCos6FilePtrOff = (nuint)answerAigisCos6FilePtr;
                nuint answerAigisCos9FilePtrOff = (nuint)answerAigisCos9FilePtr;
                nuint modelDirPtrOff = (nuint)modelDirPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Journey}",
                    "je makotoModel",
                    "movsx ecx, bx",
                    "cmp ecx, 0x255",
                    "je answerAigisCos0",
                    "cmp ecx, 0x27C",
                    "je answerAigisCos4",
                    "cmp ecx, 0x27E",
                    "je answerAigisCos3",
                    "cmp ecx, 0x281",
                    "je answerAigisCos6",
                    "jmp answerAigisDefaultSet",
                    "label answerAigisCos6",
                    $"lea rdx, [qword {answerAigisCos6FilePtrOff}]",
                    "mov r9d, 1",
                    "mov rcx, rdi",
                    $"lea r8, [qword {modelDirPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_stringFormatHook, out _stringFormatWrapper)}",
                    "jmp endHook",
                    "label answerAigisCos3",
                    $"lea rdx, [qword {answerAigisCos3FilePtrOff}]",
                    "mov r9d, 1",
                    "mov rcx, rdi",
                    $"lea r8, [qword {modelDirPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_stringFormatHook, out _stringFormatWrapper)}",
                    "jmp endHook",
                    "label answerAigisCos4",
                    $"lea rdx, [qword {answerAigisCos4FilePtrOff}]",
                    "mov r9d, 1",
                    "mov rcx, rdi",
                    $"lea r8, [qword {modelDirPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_stringFormatHook, out _stringFormatWrapper)}",
                    "jmp endHook",
                    "label answerAigisCos0",
                    $"lea rdx, [qword {answerAigisCos0FilePtrOff}]",
                    "mov r9d, 1",
                    "mov rcx, rdi",
                    $"lea r8, [qword {modelDirPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_stringFormatHook, out _stringFormatWrapper)}",
                    "jmp endHook",
                    "label answerAigisDefaultSet",
                    "mov ecx, 0x2F0",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_bitCheckHook, out _bitCheckWrapper)}",
                    "test eax, eax",
                    "jne answerAigisCos9",
                    $"lea rdx, [qword {answerAigisDefaultFilePtrOff}]",
                    "mov r9d, 1",
                    "mov rcx, rdi",
                    $"lea r8, [qword {modelDirPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_stringFormatHook, out _stringFormatWrapper)}",
                    "jmp endHook",
                    "label answerAigisCos9",
                    $"lea rdx, [qword {answerAigisCos9FilePtrOff}]",
                    "mov r9d, 1",
                    "mov rcx, rdi",
                    $"lea r8, [qword {modelDirPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_stringFormatHook, out _stringFormatWrapper)}",
                    "label endHook",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 1821, true)}",
                    "label makotoModel",
                    "mov ecx, 1",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_charIDHook, out _charIDWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 10, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("B9 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B C8 E8 ?? ?? ?? ?? B9 ?? ?? ?? ?? E8 ?? ?? ?? ?? 85 C0 75 10 48 8D 15 ?? ?? ?? ?? 44 8D 48 63 E9 ?? ?? ?? ??", "AnswerAigisKotone", address =>
            {
                nuint answerAigisDefaultFilePtrOff = (nuint)answerAigisDefaultFilePtr;
                nuint answerAigisCos0FilePtrOff = (nuint)answerAigisCos0FilePtr;
                nuint answerAigisCos3FilePtrOff = (nuint)answerAigisCos3FilePtr;
                nuint answerAigisCos4FilePtrOff = (nuint)answerAigisCos4FilePtr;
                nuint answerAigisCos6FilePtrOff = (nuint)answerAigisCos6FilePtr;
                nuint answerAigisCos9FilePtrOff = (nuint)answerAigisCos9FilePtr;
                nuint modelDirPtrOff = (nuint)modelDirPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Journey}",
                    "je kotoneModel",
                    "movsx ecx, bx",
                    "cmp ecx, 0x255",
                    "je answerAigisCos0",
                    "cmp ecx, 0x27C",
                    "je answerAigisCos4",
                    "cmp ecx, 0x27E",
                    "je answerAigisCos3",
                    "cmp ecx, 0x281",
                    "je answerAigisCos6",
                    "jmp answerAigisDefaultSet",
                    "label answerAigisCos6",
                    $"lea rdx, [qword {answerAigisCos6FilePtrOff}]",
                    "mov r9d, 0x63",
                    "mov rcx, rdi",
                    $"lea r8, [qword {modelDirPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_stringFormatHook, out _stringFormatWrapper)}",
                    "jmp endHook",
                    "label answerAigisCos3",
                    $"lea rdx, [qword {answerAigisCos3FilePtrOff}]",
                    "mov r9d, 0x63",
                    "mov rcx, rdi",
                    $"lea r8, [qword {modelDirPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_stringFormatHook, out _stringFormatWrapper)}",
                    "jmp endHook",
                    "label answerAigisCos4",
                    $"lea rdx, [qword {answerAigisCos4FilePtrOff}]",
                    "mov r9d, 0x63",
                    "mov rcx, rdi",
                    $"lea r8, [qword {modelDirPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_stringFormatHook, out _stringFormatWrapper)}",
                    "jmp endHook",
                    "label answerAigisCos0",
                    $"lea rdx, [qword {answerAigisCos0FilePtrOff}]",
                    "mov r9d, 0x63",
                    "mov rcx, rdi",
                    $"lea r8, [qword {modelDirPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_stringFormatHook, out _stringFormatWrapper)}",
                    "jmp endHook",
                    "label answerAigisDefaultSet",
                    "mov ecx, 0x2F0",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_bitCheckHook, out _bitCheckWrapper)}",
                    "test eax, eax",
                    "jne answerAigisCos9",
                    $"lea rdx, [qword {answerAigisDefaultFilePtrOff}]",
                    "mov r9d, 0x63",
                    "mov rcx, rdi",
                    $"lea r8, [qword {modelDirPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_stringFormatHook, out _stringFormatWrapper)}",
                    "jmp endHook",
                    "label answerAigisCos9",
                    $"lea rdx, [qword {answerAigisCos9FilePtrOff}]",
                    "mov r9d, 0x63",
                    "mov rcx, rdi",
                    $"lea r8, [qword {modelDirPtrOff}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_stringFormatHook, out _stringFormatWrapper)}",
                    "label endHook",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 1620, true)}",
                    "label kotoneModel",
                    "mov ecx, 1",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_charIDHook, out _charIDWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 10, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("48 8D 15 ?? ?? ?? ?? 48 8B CF 4C 8D 05 ?? ?? ?? ?? E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? 0F BF CB 81 E9 55 02 00 00 74 7B 83 E9 27", "AnswerYukari", address =>
            {
                nuint yukariDirPtrOff = (nuint)yukariDirPtr;
                nuint answerYukariDirPtrOff = (nuint)answerYukariDirPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je answerYukariModel",
                    $"lea rdx, [qword {yukariDirPtrOff}]",
                    "mov rcx, rdi",
                    "jmp endLabel",
                    "label answerYukariModel",
                    $"lea rdx, [qword {answerYukariDirPtrOff}]",
                    "mov rcx, rdi",
                    "label endLabel",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 10, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("48 8D 15 ?? ?? ?? ?? 48 8B CF 4C 8D 05 ?? ?? ?? ?? E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? 0F BF CB 81 E9 ?? ?? ?? ?? 0F 84 ?? ?? ?? ?? 83 E9 32", "AnswerMitsuru", address =>
            {
                nuint mitsuruDirPtrOff = (nuint)mitsuruDirPtr;
                nuint answerMitsuruDirPtrOff = (nuint)answerMitsuruDirPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je answerMitsuruModel",
                    $"lea rdx, [qword {mitsuruDirPtrOff}]",
                    "mov rcx, rdi",
                    "jmp endLabel",
                    "label answerMitsuruModel",
                    $"lea rdx, [qword {answerMitsuruDirPtrOff}]",
                    "mov rcx, rdi",
                    "label endLabel",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 10, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("83 F9 16 74 1B 48 8D 15 ?? ?? ?? ?? 48 8B CF 4C 8D 05 ?? ?? ?? ?? E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? 48 8D 15 ?? ?? ?? ?? 48 8B CF 4C 8D 05 ?? ?? ?? ?? E8 ?? ?? ?? ?? E9 ?? ?? ?? ??", "AnswerJunpei", address =>
            {
                nuint junpeiDirPtrOff = (nuint)junpeiDirPtr;
                nuint answerJunpeiDirPtrOff = (nuint)answerJunpeiDirPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je answerJunpeiModel",
                    $"lea rdx, [qword {junpeiDirPtrOff}]",
                    "mov rcx, rdi",
                    "jmp endLabel",
                    "label answerJunpeiModel",
                    $"lea rdx, [qword {answerJunpeiDirPtrOff}]",
                    "mov rcx, rdi",
                    "label endLabel",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 15, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address + 5, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("83 F9 15 74 1B 48 8D 15 ?? ?? ?? ?? 48 8B CF 4C 8D 05 ?? ?? ?? ?? E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? 48 8D 15 ?? ?? ?? ?? 48 8B CF 4C 8D 05 ?? ?? ?? ?? E8 ?? ?? ?? ?? E9 ?? ?? ?? ??", "AnswerAkihiko", address =>
            {
                nuint akihikoDirPtrOff = (nuint)akihikoDirPtr;
                nuint answerAkihikoDirPtrOff = (nuint)answerAkihikoDirPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je answerAkihikoModel",
                    $"lea rdx, [qword {akihikoDirPtrOff}]",
                    "mov rcx, rdi",
                    "jmp endLabel",
                    "label answerAkihikoModel",
                    $"lea rdx, [qword {answerAkihikoDirPtrOff}]",
                    "mov rcx, rdi",
                    "label endLabel",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 15, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address + 5, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("83 F9 11 74 1B 48 8D 15 ?? ?? ?? ?? 48 8B CF 4C 8D 05 ?? ?? ?? ?? E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? 48 8D 15 ?? ?? ?? ?? 48 8B CF 4C 8D 05 ?? ?? ?? ?? E8 ?? ?? ?? ?? E9 ?? ?? ?? ??", "AnswerKen", address =>
            {
                nuint kenDirPtrOff = (nuint)kenDirPtr;
                nuint answerKenDirPtrOff = (nuint)answerKenDirPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je answerKenModel",
                    $"lea rdx, [qword {kenDirPtrOff}]",
                    "mov rcx, rdi",
                    "jmp endLabel",
                    "label answerKenModel",
                    $"lea rdx, [qword {answerKenDirPtrOff}]",
                    "mov rcx, rdi",
                    "label endLabel",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 15, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address + 5, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("83 F9 60 74 1B 48 8D 15 ?? ?? ?? ?? 48 8B CF 4C 8D 05 ?? ?? ?? ?? E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? 48 8D 15 ?? ?? ?? ?? 48 8B CF 4C 8D 05 ?? ?? ?? ?? E8 ?? ?? ?? ?? E9 ?? ?? ?? ??", "AnswerMetis", address =>
            {
                nuint shinjiDirPtrOff = (nuint)shinjiDirPtr;
                nuint metisDirPtrOff = (nuint)metisDirPtr;
                string[] asm =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(IsTheAnswerSet, out _isTheAnswerSetReverseWrapper)}",
                    $"cmp rax, {(int)Scenario.Answer}",
                    "je metisModel",
                    $"lea rdx, [qword {shinjiDirPtrOff}]",
                    "mov rcx, rdi",
                    "jmp endLabel",
                    "label metisModel",
                    $"lea rdx, [qword {metisDirPtrOff}]",
                    "mov rcx, rdi",
                    "label endLabel",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 15, true)}"
                };
                _isTheAnswerSetHook = hooks.CreateAsmHook(asm, address + 5, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });
        }
        [Function(new[] { Register.rax, Register.r13 }, Register.rax, true)]
        private delegate Scenario IsTheAnswerSetDelegate();
    }
}