using Microsoft.Extensions.Configuration;
using P3PPC.Expansion.TheAnswer.Configuration;
using P3PPC.Expansion.TheAnswer.Models;
using P3PPC.Expansion.TheAnswer.UI;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Memory;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Universal.Localisation.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static P3PPC.Expansion.TheAnswer.IsTheAnswer;
using static P3PPC.Expansion.TheAnswer.Models.FileUtils;
using static P3PPC.Expansion.TheAnswer.Models.Inputs;
using static P3PPC.Expansion.TheAnswer.UI.UI.TitleMenu;
using static P3PPC.Expansion.TheAnswer.UI.UIUtils;
using static System.Net.Mime.MediaTypeNames;

namespace P3PPC.Expansion.TheAnswer.Title
{
    internal unsafe class JourneyAnswerSelect
    {

        private IAsmHook _case30PressButtonHook;
        private IAsmHook _case31LoadToMenuHook;
        private IAsmHook _case32InMenuHook;
        private IAsmHook _case33TheJourneyHook;
        private IAsmHook _case34TheAnswerHook;
        private IAsmHook _case29ToCase30Hook;
        private GetSaveDataForMemoryDelegate _getSaveDataForMemoryHook;
        private GetSaveFileDelegate _getSaveFileHook;
        private GetArgsDelegate _getArgsHook;
        private GetMenuDelegate _getMenuHook;
        private FadeOutDelegate _fadeOutHook;
        private SoundCreatorDelegate _soundCreatorHook;
        private ButtonPressDelegate _buttonPressHook;
        private ButtonPress2Delegate _buttonPress2Hook;
        private GetTitleMenuDelegate _getTitleMenuHook;
        private MemoryRelatedDelegate _memoryRelatedHook;
        private SdkotDelegate _sdkotHook;
        private SdkListState1Delegate _sdkliststate1Hook;
        private SdkListState2Delegate _sdkliststate2Hook;
        private SdkListStateButtonPressDelegate _sdkliststateButtonPressHook;
        private MenuSelectionDelegate _menuSelectionHook;
        private SdkListState4Delegate _sdkliststate4Hook;
        private BgmFadeOutForMenuDelegate _bgmFadeOutForMenuHook;
        private IHook<LoadSprListDelegate> _loadSprListHook;
        private IHook<SetSprListDelegate> _setSprListHook;
        private TouchListDelegate _touchListHook;
        private UnkMemoryDelegate _unkMemoryHook;
        private TitleDrawAnimDelegate _titleDrawAnimHook;
        private UnkAnimRelated1403D0E10Delegate _unkAnimRelated1403D0E10Hook;
        private UnkAnimRelated1403CF720Delegate _unkAnimRelated1403CF720Hook;
        private UnkAnimRelated1403160E0Delegate _unkAnimRelated1403160E0Hook;
        private UnkAnimRelated140316150Delegate _unkAnimRelated140316150Hook;
        private RenderSprDelegate _renderSprHook;
        private UnkAnimRelated1403D1380Delegate _unkAnimRelated1403D1380Hook;
        private UnkAnimRelated1403D0F70Delegate _unkAnimRelated1403D0F70Hook;
        private UnkAnimRelated1403D0F30Delegate _unkAnimRelated1403D0F30Hook;
        private UnkAnimRelated140317840Delegate _unkAnimRelated140317840Hook;
        private __security_check_cookieDelegate __security_check_cookieHook;
        private LoadSprFileDelegate _loadSprFileHook;
        private IReverseWrapper<GetTitleMenuDelegate> _getTitleMenuReverseWrapper;
        private IReverseWrapper<SdkListState1Delegate> _sdkliststate1ReverseWrapper;
        private IReverseWrapper<SdkListState2Delegate> _sdkliststate2ReverseWrapper;
        private IReverseWrapper<SdkListStateButtonPressDelegate> _sdkliststateButtonPressReverseWrapper;
        private IReverseWrapper<MenuSelectionDelegate> _menuSelectionReverseWrapper;
        private IReverseWrapper<SdkListState4Delegate> _sdkliststate4ReverseWrapper;
        private IReverseWrapper<SoundCreatorDelegate> _soundCreatorReverseWrapper;
        private IReverseWrapper<GetMenuDelegate> _getMenuReverseWrapper;
        private IReverseWrapper<FadeOutDelegate> _fadeOutReverseWrapper;
        private IReverseWrapper<BgmFadeOutForMenuDelegate> _bgmFadeOutForMenuReverseWrapper;
        private IReverseWrapper<ButtonPressDelegate> _buttonPressReverseWrapper;
        private IReverseWrapper<ButtonPress2Delegate> _buttonPress2ReverseWrapper;
        private IReverseWrapper<TouchListDelegate> _touchListReverseWrapper;
        private IReverseWrapper<UnkMemoryDelegate> _unkMemoryReverseWrapper;
        private IReverseWrapper<LoadSprFileDelegate> _loadSprFileReverseWrapper;
        private IAsmHook _journeyAnswerTitleAnimHook;
        private IAsmHook _jumpTitleScreenAnimHook;
        private IAsmHook _journeyAnswerTitleAnim2Hook;
        private IAsmHook _journeyAnswerTitleAnim3Hook;
        private IAsmHook _newSprListHook;
        private IAsmHook _jumpTitleScreenHook;
        private IReverseWrapper<TitleDrawAnimDelegate> _titleDrawAnimReverseWrapper;
        private IReverseWrapper<UnkAnimRelated1403D0E10Delegate> _unkAnimRelated1403D0E10ReverseWrapper;
        private IReverseWrapper<UnkAnimRelated1403CF720Delegate> _unkAnimRelated1403CF720ReverseWrapper;
        private IReverseWrapper<UnkAnimRelated1403160E0Delegate> _unkAnimRelated1403160E0ReverseWrapper;
        private IReverseWrapper<UnkAnimRelated140316150Delegate> _unkAnimRelated140316150ReverseWrapper;
        private IReverseWrapper<RenderSprDelegate> _renderSprReverseWrapper;
        private IReverseWrapper<UnkAnimRelated1403D1380Delegate> _unkAnimRelated1403D1380ReverseWrapper;
        private IReverseWrapper<UnkAnimRelated1403D0F70Delegate> _unkAnimRelated1403D0F70ReverseWrapper;
        private IReverseWrapper<UnkAnimRelated1403D0F30Delegate> _unkAnimRelated1403D0F30ReverseWrapper;
        private IReverseWrapper<UnkAnimRelated140317840Delegate> _unkAnimRelated140317840ReverseWrapper;
        private IReverseWrapper<__security_check_cookieDelegate> __security_check_cookieReverseWrapper;

        [Function(CallingConventions.Microsoft)]
        private delegate long GetSaveDataForMemoryDelegate();

        [Function(CallingConventions.Microsoft)]
        private delegate long GetSaveFileDelegate();

        [Function(CallingConventions.Microsoft)]
        private delegate long GetArgsDelegate(long funcArg);

        [Function(CallingConventions.Microsoft)]
        private delegate long GetMenuDelegate();

        [Function(CallingConventions.Microsoft)]
        private delegate long FadeOutDelegate(uint type, int fadeOutTime);

        [Function(CallingConventions.Microsoft)]
        private delegate long SoundCreatorDelegate(ushort type, short effectType, ushort majorID, ushort minorID);

        [Function(CallingConventions.Microsoft)]
        private delegate bool ButtonPressDelegate(uint type, int buttonID);

        [Function(CallingConventions.Microsoft)]
        private delegate long ButtonPress2Delegate();

        [Function(CallingConventions.Microsoft)]
        private delegate long GetTitleMenuDelegate();

        [Function(CallingConventions.Microsoft)]
        private delegate long MemoryRelatedDelegate();

        [Function(CallingConventions.Microsoft)]
        private delegate long SdkotDelegate(long ptrAddress, long param_2);

        [Function(CallingConventions.Microsoft)]
        private delegate long SdkListState1Delegate(long param_1, int param_2, int param_3, int param_4, uint param_5);

        [Function(CallingConventions.Microsoft)]
        private delegate long SdkListState2Delegate(uint param_1, short param_2, short param_3, short param_4, ushort param_5);

        [Function(CallingConventions.Microsoft)]
        private delegate long SdkListStateButtonPressDelegate(uint param_1);

        [Function(CallingConventions.Microsoft)]
        private delegate long MenuSelectionDelegate(int param_1, long param_2, int param_3, int param_4);

        [Function(CallingConventions.Microsoft)]
        private delegate long SdkListState4Delegate(long param_1);

        [Function(CallingConventions.Microsoft)]
        private delegate long BgmFadeOutForMenuDelegate(short param_1, long param_2, uint fadeOutTime);

        [Function(CallingConventions.Microsoft)]
        private delegate nuint LoadSprListDelegate(nuint spr);

        [Function(CallingConventions.Microsoft)]
        private delegate nuint SetSprListDelegate(int spr);

        [Function(CallingConventions.Microsoft)]
        private delegate MenuInfo* TouchListDelegate(nint spr, float xAxis, float param_3, float yAxis, int scrollCount, uint touchCount);

        [Function(CallingConventions.Microsoft)]
        private delegate long UnkMemoryDelegate(int param_1);

        [Function(CallingConventions.Microsoft)]
        private delegate long TitleDrawAnimDelegate(long param_1, byte param_2, float param_3);

        [Function(CallingConventions.Microsoft)]
        private delegate long UnkAnimRelated1403D0E10Delegate(uint param_1, uint param_2, uint param_3, uint param_4, int param_5);

        [Function(CallingConventions.Microsoft)]
        private delegate long UnkAnimRelated1403CF720Delegate(int param_1, int param_2, uint param_3, uint param_4, long param_5, long param_6, int param_7, int param_8);

        [Function(CallingConventions.Microsoft)]
        private delegate long UnkAnimRelated1403160E0Delegate();

        [Function(CallingConventions.Microsoft)]
        private delegate long UnkAnimRelated140316150Delegate();

        [Function(CallingConventions.Microsoft)]
        private delegate long RenderSprDelegate(long param_1, long param_2, long param_3, int param_4, char param_5, int fileIndex, int sprIndex, long param_8, byte r, byte g, byte b, byte a, float param_13, float param_14);

        [Function(CallingConventions.Microsoft)]
        private delegate long UnkAnimRelated1403D1380Delegate(int param_1, int param_2);

        [Function(CallingConventions.Microsoft)]
        private delegate long UnkAnimRelated1403D0F70Delegate(long param_1);

        [Function(CallingConventions.Microsoft)]
        private delegate long UnkAnimRelated1403D0F30Delegate(uint param_1);

        [Function(CallingConventions.Microsoft)]
        private delegate long UnkAnimRelated140317840Delegate();

        [Function(CallingConventions.Microsoft)]
        private delegate void __security_check_cookieDelegate(UIntPtr StackCookie);

        [Function(CallingConventions.Microsoft)]
        private delegate nuint LoadSprFileDelegate(string path);

        private Memory _memory;
        private float* value;
        private float* lessThanValue;
        private IsTheAnswer _fileSetup;
        private UIUtils _ui;
        private FileUtils _files;
        private static readonly uint someSet = 0x00B16564;
        private static readonly short animSet1 = 0x6564;
        private static readonly byte animSet2 = 0xB1;
        private static readonly float animSet3 = 480.0f;
        private static readonly float animSet4 = 50000.0f;
        private static readonly float animSet5 = 0.0f;
        private static readonly int someOtherValue = -1;
        private float* xAxis;
        private float* yAxis;
        private float* otherValue;
        private float* anim1;
        private float* anim2;
        private float* anim3;
        private float* anim4;
        private float* anim5;
        private float* anim6;
        private float* anim7;
        private float* anim8;
        private float* anim9;
        private float* anim10;
        private float* anim11;
        private float* anim12;
        private float* anim13;
        private float* anim14;
        private float* anim15;
        private float* anim16;
        private float* anim17;
        private float* anim18;
        private float* anim19;
        private float* anim20;
        private float* anim21;
        private float* anim22;
        private float* anim23;
        private float* anim24;
        private float* anim25;
        private float* anim26;
        private float* anim27;
        private float* anim28;
        private float* anim29;
        private float* anim30;
        private float* anim31;
        private float* anim32;
        private float* anim33;

        private float* titleValue1;
        private float* titleValue2;

        private int* sprCount;

        internal void Hook(IReloadedHooks hooks, IStartupScanner startupScanner)
        {
            _memory = Memory.Instance;
            _ui = new UIUtils(startupScanner, hooks);
            _files = new FileUtils(hooks, startupScanner);
            _fileSetup = new IsTheAnswer(hooks);

            value = (float*)_memory.Allocate((sizeof(float))).Address;
            lessThanValue = (float*)_memory.Allocate((sizeof(float))).Address;
            xAxis = (float*)_memory.Allocate((sizeof(float))).Address;
            yAxis = (float*)_memory.Allocate((sizeof(float))).Address;
            otherValue = (float*)_memory.Allocate((sizeof(float))).Address;

            anim1 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim2 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim3 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim4 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim5 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim6 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim7 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim8 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim9 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim10 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim11 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim12 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim13 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim14 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim15 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim16 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim17 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim18 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim19 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim20 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim21 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim22 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim23 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim24 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim25 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim26 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim27 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim28 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim29 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim30 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim31 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim32 = (float*)_memory.Allocate((sizeof(float))).Address;
            anim33 = (float*)_memory.Allocate((sizeof(float))).Address;

            titleValue1 = (float*)_memory.Allocate((sizeof(float))).Address;
            titleValue2 = (float*)_memory.Allocate((sizeof(float))).Address;

            _inputs = (InputStruct*)_memory.Allocate((sizeof(InputFlag))).Address;

            *value = 0.5f;
            *lessThanValue = 450.0f;
            *xAxis = 664.0f;
            *yAxis = 26.0f;
            *otherValue = 0.0f;

            *anim1 = 1.0f;
            *anim2 = 0.2f;
            *anim3 = 408.0f;
            *anim4 = -240.0f;
            *anim5 = 0.02f;
            *anim6 = 340.0f;
            *anim7 = 68.0f;
            *anim8 = 273.0f;
            *anim9 = 148.0f;
            *anim10 = 128.0f;
            *anim11 = 127.0f;
            *anim12 = 356.0f;
            *anim13 = 50.0f;
            *anim14 = 231.0f;
            *anim15 = 255.0f;
            *anim16 = 10.0f;
            *anim17 = 329.0f;
            *anim18 = 65535.0f;
            *anim19 = 13.0f;
            *anim20 = 182.0f;
            *anim21 = 441.0f;
            *anim22 = 217.0f;
            *anim23 = 312.0f;
            *anim24 = 256.0f;
            *anim25 = 305.0f;
            *anim26 = 1.10f;
            *anim27 = -3.0f;
            *anim28 = 1.57f;
            *anim29 = 15.0f;
            *anim30 = 11.0f;
            *anim31 = 40.0f;
            *anim32 = 0.5f;
            *anim33 = 640.0f;

            *titleValue1 = 0.5f;
            *titleValue2 = 450.0f;

            var fSprTexAnim = new[] { 347.0f, 184.0f, 342.0f, 197.0f, 352.0f, 210.0f, 358.0f, 222.5f, 370.0f, 236.5f };

            var fSprTexAnimPtr = (float*)_memory.Allocate((nuint)(sizeof(float) * fSprTexAnim.Length)).Address;
            for (int i = 0; i < fSprTexAnim.Length; i++)
            {
                fSprTexAnimPtr[i] = fSprTexAnim[i];
            }
            var iSprTexAnimEnd = new[] { 0 };

            var iSprTexAnimEndPtr = (int*)_memory.Allocate((nuint)(sizeof(int) * iSprTexAnimEnd.Length)).Address;
            for (int i = 0; i < iSprTexAnimEnd.Length; i++)
            {
                iSprTexAnimEndPtr[i] = iSprTexAnimEnd[i];
            }
            var iSprMenuTexIndex = new[] { 0, 1 };

            var iSprMenuTexIndexPtr = (int*)_memory.Allocate((nuint)(sizeof(int) * iSprMenuTexIndex.Length)).Address;
            for (int i = 0; i < iSprMenuTexIndex.Length; i++)
            {
                iSprMenuTexIndexPtr[i] = iSprMenuTexIndex[i];
            }

            var sSprFiles = new[]
            {
                "title/face.spr", "title/menu.spr", "title/moon.spr", "title/p3p_logo.spr", "title/tartaros.spr",
                "title/title_chara01.spr", "title/title_chara99.spr", "title/journeyanswer.spr"
            };

            var sSprFilesPtr = (char**)_memory.Allocate((nuint)(sizeof(nuint) * sSprFiles.Length)).Address;
            for (int i = 0; i < sSprFiles.Length; i++)
            {
                var stringBytes = Encoding.ASCII.GetBytes(sSprFiles[i]);
                var stringAddress = _memory.Allocate((nuint)stringBytes.Length).Address;
                _memory.WriteRaw(stringAddress, stringBytes);
                sSprFilesPtr[i] = (char*)stringAddress;
            }

            nuint sSprFilesPtrOff = (nuint)sSprFilesPtr;

            var sSprFileInfo = new[]
            {
                sSprFilesPtr
            };

            var sSprFileInfoPtr = (nuint*)_memory.Allocate((nuint)sizeof(nuint)).Address;
            for (int i = 0; i < sSprFileInfo.Length; i++)
            {
                var stringOff = sSprFileInfo[i];
                var countAddress = _memory.Allocate((nuint)stringOff).Address;
                _memory.Write(countAddress, (int)stringOff);
                sSprFileInfoPtr[i] = (nuint)stringOff;
                *(int*)(sSprFileInfoPtr + 1) = 8;
            }

            Utils.SigScan("48 83 EC 28 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 83 C4 28 E9 F7 4F 03 00", "GetTitleMenu", address =>
            {
                _getTitleMenuHook = hooks.CreateWrapper<GetTitleMenuDelegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 48 8B D9 41 8B F1 48 8D 0D ?? ?? ?? ?? 41 8B E8 8B FA E8 ?? ?? ?? ?? 48 85 DB 75 14 8B 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? FF C2", "sdkliststate1", address =>
            {
                _sdkliststate1Hook = hooks.CreateWrapper<SdkListState1Delegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 48 8B D9 41 0F B7 F9 48 8D 0D ?? ?? ?? ?? 41 0F B7 F0 0F B7 EA E8 ?? ?? ?? ?? 48 85 DB 75 14 8B 15", "sdkliststate1", address =>
            {
                _sdkliststate2Hook = hooks.CreateWrapper<SdkListState2Delegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 48 8B D9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 33 FF E8 ?? ?? ?? ?? 8B F0 48 85 DB 75 15 8B 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 83 C2 07 E8 ?? ?? ?? ?? 8B 13 F6 C2 02", "sdkliststateButtonPress", address =>
            {
                _sdkliststateButtonPressHook = hooks.CreateWrapper<SdkListStateButtonPressDelegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 48 89 74 24 ?? 48 89 7C 24 ?? 4C 89 74 24 ?? 41 57 48 83 EC 20 48 8B F9 4D 8B F9 48 8D 0D ?? ?? ?? ?? 4D 8B F0 48 8B F2 E8 ?? ?? ?? ?? 48 85 FF 0F 84 ?? ?? ?? ?? 66 83 7F ?? ?? 75 07 66 83 7F ?? ??", "MenuSelection", address =>
            {
                _menuSelectionHook = hooks.CreateWrapper<MenuSelectionDelegate>(address, out _);
            });

            Utils.SigScan("40 53 48 83 EC 20 48 8B D9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 85 DB 75 14 8B 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? FF C2 E8 ?? ?? ?? ?? 8B 43 24 48 83 C4 20 5B C3", "sdkliststate4", address =>
            {
                _sdkliststate4Hook = hooks.CreateWrapper<SdkListState4Delegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 0F B7 F9 0F BF DA 48 8D 0D ?? ?? ?? ?? 41 0F B7 F1 41 0F B7 E8 E8 ?? ?? ?? ?? 89 DA", "SoundCreator", address =>
            {
                _soundCreatorHook = hooks.CreateWrapper<SoundCreatorDelegate>(address, out _);
            });

            Utils.SigScan("48 83 EC 28 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 83 C4 28 E9 77 CC 03 00", "GetMenu", address =>
            {
                _getMenuHook = hooks.CreateWrapper<GetMenuDelegate>(address, out _);
            });

            Utils.SigScan("48 83 EC 28 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 83 C4 28 E9 77 CC 03 00", "BgmFadeOutForMenu", address =>
            {
                _bgmFadeOutForMenuHook = hooks.CreateWrapper<BgmFadeOutForMenuDelegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 48 83 EC 50 0F 29 74 24 ?? 48 8B D9 0F 29 7C 24 ?? 48 8D 0D ?? ?? ?? ?? 0F 28 FA 0F 28 F3 48 8B FA E8 ?? ?? ?? ?? 48 85 DB", "TouchList", address =>
            {
                _touchListHook = hooks.CreateWrapper<TouchListDelegate>(address, out _);
            });

            Utils.SigScan("40 53 48 83 EC 20 8B D9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 89 1D ?? ?? ?? ?? 48 83 C4 20 5B C3", "UnkMemory", address =>
            {
                _unkMemoryHook = hooks.CreateWrapper<UnkMemoryDelegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 48 89 74 24 ?? 57 48 81 EC ?? ?? ?? ?? 48 8B D9 44 0F 29 54 24 ?? 48 8D 0D ?? ?? ?? ?? 44 0F 29 5C 24 ?? 49 8B F8 48 8B F2", "TitleDrawAnim", address =>
            {
                _titleDrawAnimHook = hooks.CreateWrapper<TitleDrawAnimDelegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 48 83 EC 30 8B F9 41 8B D9 48 8D 0D ?? ?? ?? ?? 41 8B E8 8B F2 E8 ?? ?? ?? ?? 8B 44 24 60", "UnkAnimRelated1403D0E10", address =>
            {
                _unkAnimRelated1403D0E10Hook = hooks.CreateWrapper<UnkAnimRelated1403D0E10Delegate>(address, out _);
            });

            Utils.SigScan("48 89 6C 24 ?? 48 89 74 24 ?? 57 41 54 41 56 48 83 EC 30 44 8B E1 48 89 5C 24 ??", "UnkAnimRelated1403CF720", address =>
            {
                _unkAnimRelated1403CF720Hook = hooks.CreateWrapper<UnkAnimRelated1403CF720Delegate>(address, out _);
            });

            Utils.SigScan("48 83 EC 38 0F 29 74 24 ?? 48 8D 0D ?? ?? ?? ?? 0F 28 F0 E8 ?? ?? ?? ?? 0F 28 C6 0F 28 74 24 ?? 48 83 C4 38 E9 1B DF 10 00", "UnkAnimRelated1403160E0", address =>
            {
                _unkAnimRelated1403160E0Hook = hooks.CreateWrapper<UnkAnimRelated1403160E0Delegate>(address, out _);
            });

            Utils.SigScan("48 83 EC 38 0F 29 74 24 ?? 48 8D 0D ?? ?? ?? ?? 0F 28 F0 E8 ?? ?? ?? ?? 0F 28 C6 0F 28 74 24 ?? 48 83 C4 38 E9 B1 DE 10 00", "UnkAnimRelated140316150", address =>
            {
                _unkAnimRelated140316150Hook = hooks.CreateWrapper<UnkAnimRelated140316150Delegate>(address, out _);
            });

            Utils.SigScan("48 8B C4 53 48 81 EC ?? ?? ?? ?? 0F 29 70 E8 48 8D 0D ?? ?? ?? ?? 0F 29 78 D8 41 8B D9 44 0F 29 40 ?? 0F 28 F1 44 0F 28 C2 0F 28 F8 E8 ?? ?? ?? ?? 4C 8B 94 24 ?? ?? ?? ??", "RenderSpr", address =>
            {
                _renderSprHook = hooks.CreateWrapper<RenderSprDelegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 57 48 83 EC 20 8B F9 8B DA 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 8B D3 8B CF E8 ?? ?? ?? ?? 85 DB 74 11 09 3D ?? ?? ?? ?? 48 8B 5C 24 ?? 48 83 C4 20 5F C3", "UnkAnimRelated1403D1380", address =>
            {
                _unkAnimRelated1403D1380Hook = hooks.CreateWrapper<UnkAnimRelated1403D1380Delegate>(address, out _);
            });

            Utils.SigScan("40 53 48 83 EC 20 8B D9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 33 C9 85 DB 0F 94 C1 48 83 C4 20 5B E9 9B 7A F4 FF", "UnkAnimRelated1403D0F70", address =>
            {
                _unkAnimRelated1403D0F70Hook = hooks.CreateWrapper<UnkAnimRelated1403D0F70Delegate>(address, out _);
            });

            Utils.SigScan("40 53 48 83 EC 20 8B D9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 8B CB E8 ?? ?? ?? ?? BA ?? ?? ?? ?? 8B CA E8 ?? ?? ?? ?? 83 0D ?? ?? ?? ?? ?? 48 83 C4 20 5B C3", "UnkAnimRelated1403D0F30", address =>
            {
                _unkAnimRelated1403D0F30Hook = hooks.CreateWrapper<UnkAnimRelated1403D0F30Delegate>(address, out _);
            });

            Utils.SigScan("48 83 EC 28 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 83 C4 28 E9 77 CC 03 00", "UnkAnimRelated140317840", address =>
            {
                _unkAnimRelated140317840Hook = hooks.CreateWrapper<UnkAnimRelated140317840Delegate>(address, out _);
            });

            Utils.SigScan("48 3B 0D ?? ?? ?? ?? F2 75 12 48 C1 C1 10 66 F7 C1 ?? ?? F2 75 02 F2 C3 48 C1 C9 10 E9 ?? ?? ?? ??", "__security_check_cookie", address =>
            {
                __security_check_cookieHook = hooks.CreateWrapper<__security_check_cookieDelegate>(address, out _);
            });

            Utils.SigScan("48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 31 E0 48 89 84 24 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 54 24 ?? 31 C9 E8 ?? ?? ?? ?? 48 89 C1 E8 ?? ?? ?? ?? 8B 44 24 20 48 8D 54 24 ?? 45 31 C0 05 ?? ?? ?? ?? 89 84 24 ?? ?? ?? ?? 41 8D 48 63 E8 ?? ?? ?? ?? 48 8B 8C 24 ?? ?? ?? ?? 48 31 E1 E8 ?? ?? ?? ?? 48 81 C4 ?? ?? ?? ??", "GetSaveDataForMemory", address =>
            {
                _getSaveDataForMemoryHook = hooks.CreateWrapper<GetSaveDataForMemoryDelegate>(address, out _);
            });

            Utils.SigScan("40 53 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 33 DB 0F 1F 80 ?? ?? ?? ?? FF C3 4C 8D 44 24 ?? 8B CB 48 8D 94 24 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 4C 24 ?? E8 ?? ?? ?? ?? 3C 01 74 09 83 FB 10", "GetSaveFile", address =>
            {
                _getSaveFileHook = hooks.CreateWrapper<GetSaveFileDelegate>(address, out _);
            });

            Utils.SigScan("40 53 48 83 EC 20 48 8B D9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B 43 48 48 83 C4 20 5B C3", "GetArgs", address =>
            {
                _getArgsHook = hooks.CreateWrapper<GetArgsDelegate>(address, out _);
            });

            /*Utils.SigScan("40 53 41 54 41 56 41 57 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 44 24 ?? 4C 8B F1 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 49 8B CE E8 ?? ?? ?? ?? 48 8B D8 48 63 00 83 F8 1C", "TitleScreen", address =>
            {
                _setTitleScreenHook = hooks.CreateWrapper<TitleScreenDelegate>(address, out _);
            });*/

            Utils.SigScan("48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 89 CE 89 D7 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B 1D ?? ?? ?? ?? 48 85 DB 0F 84 ?? ?? ?? ?? 83 FE 04", "FadeOut", address =>
            {
                _fadeOutHook = hooks.CreateWrapper<FadeOutDelegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 57 48 83 EC 20 8B D9 8B FA 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0C 9B B8 ?? ?? ?? ?? 48 8B 5C 24 ?? 48 8D 15 ?? ?? ?? ?? 48 C1 E1 06", "ButtonPress", address =>
            {
                _buttonPressHook = hooks.CreateWrapper<ButtonPressDelegate>(address, out _);
            });

            Utils.SigScan("40 53 48 83 EC 20 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? BA ?? ?? ?? ?? E8 ?? ?? ?? ?? 33 D2 48 8B C8 48 8B D8 44 8D 42 60 E8 ?? ?? ?? ?? 48 8B C3 48 83 C4 20 5B C3", "MemoryRelated", address =>
            {
                _memoryRelatedHook = hooks.CreateWrapper<MemoryRelatedDelegate>(address, out _);
            });

            /*Utils.SigScan("40 55 53 56 48 8D AC 24 ?? ?? ?? ?? 48 81 EC ?? ?? ?? ?? 44 0F 29 8C 24 ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 45 F0 48 8D 0D ?? ?? ?? ?? 48 8B DA E8 ?? ?? ?? ?? 48 8B CB E8 ?? ?? ?? ?? 48 8B F0 C6 44 24 ?? ??", "TitleScreenAnim", address =>
            {
                _titleScreenAnimHook = hooks.CreateWrapper<TitleScreenAnimDelegate>(address, out _);
            });*/

            Utils.SigScan("48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 48 8B F9 48 8B DA 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 85 FF 75 15 8B 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 83 C2 03", "sdkot", address =>
            {
                _sdkotHook = hooks.CreateWrapper<SdkotDelegate>(address, out _);
            });

            Utils.SigScan("48 83 EC 28 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 8B 05 ?? ?? ?? ?? 0F BA E0 0C 0F 82 ?? ?? ?? ?? 0F BA E0 0E 0F 82 ?? ?? ?? ?? 0F BA E0 0F 0F 82 ?? ?? ?? ?? 0F BA E0 0D 0F 82 ?? ?? ?? ?? BA ?? ?? ?? ??", "ButtonPress2", address =>
            {
                _buttonPress2Hook = hooks.CreateWrapper<ButtonPress2Delegate>(address, out _);
            });

            Utils.SigScan("40 53 48 83 EC 20 48 89 CB 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 8B 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 83 C2 02 E8 ?? ?? ?? ?? 8B 15 ?? ?? ?? ?? 81 F2 ?? ?? ?? ?? B9 ?? ?? ?? ?? 41 B8 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 89 C2 31 C0 66 89 02 48 89 D0 48 29 D8", "LoadSprFile", address =>
            {
                _loadSprFileHook = hooks.CreateWrapper<LoadSprFileDelegate>(address, out _);
            });

            Utils.SigScan("0F B6 84 02 ?? ?? ?? ?? 8B 8C 82 ?? ?? ?? ?? 48 03 CA FF E1 C7 45 ?? ?? ?? ?? ??", "jumpTitleScreenAnim", address =>
            {
                string[] function =
                {
                    "use64",
                    "movzx eax, byte [rdx + rax + 0x00826ED0]",
                    "mov ecx, [rdx + rax * 4 + 0x00826F20]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 15, true)}"
                };
                _jumpTitleScreenAnimHook = hooks.CreateAsmHook(function, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("41 B8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 8B D7 48 8B 34 C1 48 8D 4E 28 E8 ?? ?? ?? ?? 48 8B D8 48 85 C0 74 3F E8 ?? ?? ?? ?? 0F BA F0 1F", "JourneyAnswerTitleAnim", address =>
            {
                nuint anim1Address = (nuint)anim1;
                nuint anim2Address = (nuint)anim2;
                nuint anim3Address = (nuint)anim3;
                nuint anim4Address = (nuint)anim4;
                nuint anim5Address = (nuint)anim5;
                nuint anim6Address = (nuint)anim6;
                nuint anim7Address = (nuint)anim7;
                nuint anim8Address = (nuint)anim8;
                nuint anim9Address = (nuint)anim9;
                nuint anim10Address = (nuint)anim10;
                nuint anim11Address = (nuint)anim11;
                nuint anim12Address = (nuint)anim12;
                nuint anim13Address = (nuint)anim13;
                nuint anim14Address = (nuint)anim14;
                nuint anim15Address = (nuint)anim15;
                nuint anim16Address = (nuint)anim16;
                nuint anim17Address = (nuint)anim17;
                nuint anim18Address = (nuint)anim18;
                nuint anim19Address = (nuint)anim19;
                nuint anim20Address = (nuint)anim20;
                nuint anim21Address = (nuint)anim21;
                nuint anim22Address = (nuint)anim22;
                nuint anim23Address = (nuint)anim23;
                nuint anim24Address = (nuint)anim24;
                nuint anim25Address = (nuint)anim25;
                nuint anim26Address = (nuint)anim26;
                nuint anim27Address = (nuint)anim27;
                nuint anim28Address = (nuint)anim28;
                nuint anim29Address = (nuint)anim29;
                nuint anim30Address = (nuint)anim30;
                nuint anim31Address = (nuint)anim31;
                nuint anim32Address = (nuint)anim32;
                nuint fSprTexAnimPtrOff = (nuint)fSprTexAnimPtr;
                nuint iSprTexAnimEndPtrOff = (nuint)iSprTexAnimEndPtr;
                nuint iSprMenuTexIndexPtrOff = (nuint)iSprMenuTexIndexPtr;
                string[] function =
                {
                    "use64",
                    $"movss xmm10, [qword {anim1Address}]",
                    "lea r8, [rsp + 0x74]",
                    "xorps xmm1, xmm1",
                    "lea rdx, [rsp + 0x70]",
                    "subss xmm1, dword [rsi + 0x34]",
                    "movaps xmm0, xmm10",
                    "mov rcx, rsi",
                    "subss xmm0, dword [rsi + 0x38]",
                    $"mulss xmm1, [qword {anim2Address}]",
                    $"mulss xmm0, [qword {anim2Address}]",
                    "addss xmm1, dword [rsi + 0x34]",
                    "addss xmm0, dword [rsi + 0x38]",
                    "movss dword [rsi + 0x34], xmm1",
                    "movss dword [rsi + 0x38], xmm0",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_titleDrawAnimHook, out _titleDrawAnimReverseWrapper)}",
                    $"movss xmm0, [qword {anim3Address}]",
                    "xor r9d, r9d",
                    "movzx ebx, byte [rsp + 0x70]",
                    "xor ecx, ecx",
                    "mov eax, ebx",
                    "movss [rbp - 0x38], xmm0",
                    $"movss xmm0, [qword {anim4Address}]",
                    "shl eax, 0x18",
                    "lea edx, [r9 + 2]",
                    $"or eax, {someSet}",
                    "movss [rbp - 0x24], xmm0",
                    "lea r8d, [r9 + 0xA]",
                    "mov [rbp - 0x58], eax",
                    $"mov word [rbp - 0x70], {animSet1}",
                    $"mov byte [rbp - 0x6E], {animSet2}",
                    "mov byte [rbp - 0x6D], bl",
                    $"mov qword [rbp - 0x54], {animSet3}",
                    $"mov qword [rbp - 0x4C], {animSet4}",
                    $"mov qword [rbp - 0x3C], {animSet3}",
                    $"mov qword [rbp - 0x34], {animSet4}",
                    $"mov qword [rbp - 0x40], {someSet}",
                    $"mov qword [rbp - 0x20], {animSet5}",
                    $"mov qword [rbp - 0x1C], {animSet4}",
                    $"mov qword [rbp - 0x28], {someSet}",
                    $"mov dword [rsp + 0x20], {someOtherValue}",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0E10Hook, out _unkAnimRelated1403D0E10ReverseWrapper)}",
                    "xor edi, edi",
                    "mov qword [rsp + 0x38], 0x18",
                    "lea rax, [rbp - 0x60]",
                    "mov qword [rsp + 0x30], 2",
                    "mov [rsp + 0x28], rax",
                    "xor r8d, r8d",
                    "mov edx, 0x80019E",
                    "mov [rsp + 0x20], rdi",
                    "lea r9d, [rdi + 3]",
                    "lea ecx, [rdi + 4]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403CF720Hook, out _unkAnimRelated1403CF720ReverseWrapper)}",
                    "movss xmm7, [rsp + 0x74]",
                    $"mulss xmm7, [qword {anim5Address}]",
                    "movaps xmm0, xmm7",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403160E0Hook, out _unkAnimRelated1403160E0ReverseWrapper)}",
                    "movaps xmm8, xmm0",
                    "movaps xmm0, xmm7",
                    $"mulss xmm8, [qword {anim6Address}]",
                    $"addss xmm8, [qword {anim7Address}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated140316150Hook, out _unkAnimRelated140316150ReverseWrapper)}",
                    $"mulss xmm0, [qword {anim6Address}]",
                    "mov r9d, 0xFFFFFF",
                    "mov rax, [rsi + 0x58]",
                    "movss [rsp + 0x68], xmm10",
                    "xorps xmm2, xmm2",
                    "movss [rsp + 0x60], xmm10",
                    $"addss xmm0, [qword {anim8Address}]",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "movaps xmm1, xmm0",
                    "mov dword [rsp + 0x30], edi",
                    "movaps xmm0, xmm8",
                    "mov dword [rsp + 0x28], 0x20000",
                    "mov byte [rsp + 0x20], bl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "xor r9d, r9d",
                    "mov dword [rsp + 0x20], edi",
                    "lea edx, [rdi + 2]",
                    "xor ecx, ecx",
                    "lea r8d, [rdi + 3]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0E10Hook, out _unkAnimRelated1403D0E10ReverseWrapper)}",
                    "mov rax, [rsi + 0x58]",
                    "movaps xmm0, xmm10",
                    "subss xmm0, [rsi + 0x34]",
                    $"movss xmm1, [qword {anim9Address}]",
                    "mov r9d, 0xFFFFFF",
                    "movss [rsp + 0x68], xmm10",
                    "xorps xmm2, xmm2",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    $"mulss xmm0, [qword {anim10Address}]",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    $"addss xmm0, [qword {anim11Address}]",
                    "mov [rsp + 0x38], rax",
                    "mov dword [rsp + 0x30], 1",
                    "mov dword [rsp + 0x28], 0x40000",
                    "cvttss2si ecx, xmm0",
                    $"movss xmm0, [qword {anim12Address}]",
                    "mov byte [rsp + 0x20], cl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0xFFFFFF",
                    $"movss xmm1, [qword {anim13Address}]",
                    "xorps xmm2, xmm2",
                    $"movss xmm0, [qword {anim14Address}]",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov dword [rsp + 0x30], edi",
                    "mov dword [rsp + 0x28], 0x30000",
                    "mov byte [rsp + 0x20], -1",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "movss xmm0, [rsi + 0x34]",
                    $"lea rbx, [qword {fSprTexAnimPtrOff}]",
                    $"movss xmm8, [qword {anim15Address}]",
                    $"lea r14, [qword {iSprMenuTexIndexPtrOff}]",
                    "mulss xmm0, xmm8",
                    $"lea r15, [qword {iSprTexAnimEndPtrOff}]",
                    "mov qword [rbp - 0x80], rbx",
                    "mov r13, r14",
                    "cvttss2si r12d, xmm0",
                    "mov [rsp + 0x74], r12d",
                    "mov r12, rbx",
                    "mov ebx, [rsp + 0x74]",
                    "nop dword [rax + rax + 0]",
                    "label loopMenuAnim",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0xFFFFFF",
                    "movss xmm1, dword [r12 + 4]",
                    "movaps xmm2, xmm9",
                    "movss xmm0, dword [r12]",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov eax, [r13 + 0]",
                    "mov dword [rsp + 0x30], eax",
                    "mov dword [rsp + 0x28], 0x70000",
                    "mov byte [rsp + 0x20], bl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "add r13, 1",
                    "lea r12, [r12 + 8]",
                    "cmp r13, r15",
                    "jl loopMenuAnim",
                    "mov edx, 1",
                    "mov ecx, edx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D1380Hook, out _unkAnimRelated1403D1380ReverseWrapper)}",
                    "mov ecx, 1",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0F70Hook, out _unkAnimRelated1403D0F70ReverseWrapper)}",
                    "movd xmm7, [rsp + 0x78]",
                    "mov r9d, 0xFFFFFF",
                    "mov rax, [rsi + 0x58]",
                    $"movss xmm6, [qword {anim16Address}]",
                    $"movss xmm0, [qword {anim17Address}]",
                    "movaps xmm2, xmm6",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "cvtdq2ps xmm7, xmm7",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov dword [rsp + 0x30], 2",
                    "mov dword [rsp + 0x28], 0x70000",
                    $"divss xmm7, [qword {anim18Address}]",
                    "mov byte [rsp + 0x20], bl",
                    $"mulss xmm7, [qword {anim19Address}]",
                    $"addss xmm7, [qword {anim20Address}]",
                    "movaps xmm1, xmm7",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0xFFFFFF",
                    $"movss xmm0, [qword {anim21Address}]",
                    "movaps xmm2, xmm6",
                    "movss [rsp + 0x68], xmm10",
                    "movaps xmm1, xmm7",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov dword [rsp + 0x30], 3",
                    "mov dword [rsp + 0x28], 0x70000",
                    "mov byte [rsp + 0x20], bl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "xor ecx, ecx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0F70Hook, out _unkAnimRelated1403D0F70ReverseWrapper)}",
                    "mov ecx, 6",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0F30Hook, out _unkAnimRelated1403D0F30ReverseWrapper)}",
                    "mov rbx, qword [rbp - 0x80]",
                    "mov r12d, [rsp + 0x74]",
                    "nop word [rax + rax + 0]",
                    "label loopMenuAnim2",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0x125",
                    "movss xmm1, dword [rbx + 4]",
                    "movaps xmm2, xmm9",
                    "movss xmm0, dword [rbx]",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov eax, [r14]",
                    "mov dword [rsp + 0x30], eax",
                    "mov dword [rsp + 0x28], 0x70000",
                    "mov byte [rsp + 0x20], r12l",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "add r14, 1",
                    "add rbx, 8",
                    "cmp r14, r15",
                    "jl loopMenuAnim2",
                    "mov ecx, 1",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0F30Hook, out _unkAnimRelated1403D0F30ReverseWrapper)}",
                    "xor edx, edx",
                    "lea ecx, [rdx + 1]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D1380Hook, out _unkAnimRelated1403D1380ReverseWrapper)}",
                    "mov rax, [rsi + 0x58]",
                    "movaps xmm0, xmm10",
                    "subss xmm0, dword [rsi + 0x34]",
                    $"movss xmm1, [qword {anim22Address}]",
                    "mov r9d, 0xFFFFFF",
                    "movss [rsp + 0x68], xmm10",
                    "movaps xmm2, xmm9",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mulss xmm0, xmm8",
                    "mov dword [rsp + 0x30], 1",
                    "mov dword [rsp + 0x28], 0x10000",
                    "cvttss2si ecx, xmm0",
                    $"movss xmm0, [qword {anim23Address}]",
                    "mov byte [rsp + 0x20], cl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "movss xmm0, dword [rsi + 0x38]",
                    "mov r9d, 0xFFFFFF",
                    "mov rax, [rsi + 0x58]",
                    "movaps xmm2, xmm9",
                    $"movss xmm1, [qword {anim24Address}]",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mulss xmm0, xmm8",
                    "mov dword [rsp + 0x30], 2",
                    "mov dword [rsp + 0x28], 0x10000",
                    "cvttss2si ecx, xmm0",
                    $"movss xmm0, [qword {anim25Address}]",
                    "mov byte [rsp + 0x20], cl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "test byte [rsi + 4], 2",
                    $"movss xmm11, [qword {anim26Address}]",
                    $"movss xmm12, [qword {anim27Address}]",
                    "je makotoAnim",
                    "movss xmm0, dword [rsi + 0x1C]",
                    $"mulss xmm0, [qword {anim28Address}]",
                    $"divss xmm0, [qword {anim29Address}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated140316150Hook, out _unkAnimRelated140316150ReverseWrapper)}",
                    "mov ecx, [rsi + 0x18]",
                    "movaps xmm6, xmm10",
                    "mov rax, [rsi + 0x58]",
                    "subss xmm6, xmm0",
                    "movss [rsp + 0x68], xmm11",
                    "xor ecx, 1",
                    "movss [rsp + 0x60], xmm10",
                    "add ecx, 5",
                    "movss [rsp + 0x58], xmm9",
                    "mov r9d, 0xFFFFFF",
                    "mov [rsp + 0x50], di",
                    "movaps xmm2, xmm9",
                    "mov [rsp + 0x48], di",
                    "movaps xmm1, xmm6",
                    "mulss xmm1, xmm8",
                    "mov [rsp + 0x40], edi",
                    $"movss xmm8, [qword {anim30Address}]",
                    "mov [rsp + 0x38], rax",
                    "movaps xmm0, xmm8",
                    "cvttss2si ebx, xmm1",
                    "shl ecx, 0x10",
                    "mov dword [rsp + 0x30], 1",
                    "mov dword [rsp + 0x28], ecx",
                    "movaps xmm1, xmm12",
                    "mov byte [rsp + 0x20], bl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    $"mulss xmm6, [qword {anim31Address}]",
                    "not bl",
                    "subss xmm8, xmm6",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated140317840Hook, out _unkAnimRelated140317840ReverseWrapper)}",
                    $"mulss xmm0, [qword {anim32Address}]",
                    "addss xmm0, dword [rsi + 0x1C]",
                    $"comiss xmm0, [qword {anim29Address}]",
                    "movss dword [rsi + 0x1C], xmm0",
                    "jna continueNextCharaSpr",
                    "and dword [rsi + 4], -3",
                    "jmp continueNextCharaSpr",
                    "label makotoAnim",
                    $"movss xmm8, [qword {anim30Address}]",
                    "mov bl, -1",
                    "label continueNextCharaSpr",
                    "mov ecx, [rsi + 0x18]",
                    "mov r9d, 0xFFFFFF",
                    "mov rax, [rsi + 0x58]",
                    "add ecx, 5",
                    "movss [rsp + 0x68], xmm11",
                    "movaps xmm1, xmm12",
                    "movss [rsp + 0x60], xmm10",
                    "movaps xmm0, xmm8",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov dword [rsp + 0x30], 1",
                    "shl ecx, 0x10",
                    "mov dword [rsp + 0x28], ecx",
                    "mov byte [rsp + 0x20], bl",
                    "movaps xmm2, xmm9",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "movaps xmm12, [rsp + 0x120]",
                    "movaps xmm11, [rsp + 0x130]",
                    "movaps xmm10, [rsp + 0x140]",
                    "movaps xmm8, [rsp + 0x160]",
                    "movaps xmm7, [rsp + 0x170]",
                    "movaps xmm6, [rsp + 0x180]",
                    "mov r15, [rsp + 0x190]",
                    "mov r14, [rsp + 0x198]",
                    "mov r13, [rsp + 0x1D8]",
                    "mov r12, [rsp + 0x1D0]",
                    "mov rdi, [rsp + 0x1C0]",
                    "movaps xmm13, [rsp + 0x110]",
                    "mov rcx, [rbp - 0x10]",
                    "xor rcx, rsp",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(__security_check_cookieHook, out __security_check_cookieReverseWrapper)}",
                    "movaps xmm9, [rsp + 0x150]",
                    "add rsp, 0x1A0",
                    "pop rsi",
                    "pop rbx",
                    "pop rbp",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 188, true)}"
                };
                _journeyAnswerTitleAnimHook = hooks.CreateAsmHook(function, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("48 8B D8 48 85 C0 74 3F E8 ?? ?? ?? ?? 0F BA F0 1F 0F 57 C0 8B C8 F3 48 0F 2A C1 F3 0F 59 05 ?? ?? ?? ?? F3 48 0F 2C C8", "JourneyAnswerTitleAnim2", address =>
            {
                nuint anim1Address = (nuint)anim1;
                nuint anim2Address = (nuint)anim2;
                nuint anim3Address = (nuint)anim3;
                nuint anim4Address = (nuint)anim4;
                nuint anim5Address = (nuint)anim5;
                nuint anim6Address = (nuint)anim6;
                nuint anim7Address = (nuint)anim7;
                nuint anim8Address = (nuint)anim8;
                nuint anim9Address = (nuint)anim9;
                nuint anim10Address = (nuint)anim10;
                nuint anim11Address = (nuint)anim11;
                nuint anim12Address = (nuint)anim12;
                nuint anim13Address = (nuint)anim13;
                nuint anim14Address = (nuint)anim14;
                nuint anim15Address = (nuint)anim15;
                nuint anim16Address = (nuint)anim16;
                nuint anim17Address = (nuint)anim17;
                nuint anim18Address = (nuint)anim18;
                nuint anim19Address = (nuint)anim19;
                nuint anim20Address = (nuint)anim20;
                nuint anim21Address = (nuint)anim21;
                nuint anim22Address = (nuint)anim22;
                nuint anim23Address = (nuint)anim23;
                nuint anim24Address = (nuint)anim24;
                nuint anim25Address = (nuint)anim25;
                nuint anim26Address = (nuint)anim26;
                nuint anim27Address = (nuint)anim27;
                nuint anim28Address = (nuint)anim28;
                nuint anim29Address = (nuint)anim29;
                nuint anim30Address = (nuint)anim30;
                nuint anim31Address = (nuint)anim31;
                nuint anim32Address = (nuint)anim32;
                nuint fSprTexAnimPtrOff = (nuint)fSprTexAnimPtr;
                nuint iSprTexAnimEndPtrOff = (nuint)iSprTexAnimEndPtr;
                nuint iSprMenuTexIndexPtrOff = (nuint)iSprMenuTexIndexPtr;
                string[] function =
                {
                    "use64",
                    $"movss xmm10, [qword {anim1Address}]",
                    "lea r8, [rsp + 0x74]",
                    $"movss xmm11, [qword {anim2Address}]",
                    "lea rdx, [rsp + 0x70]",
                    "movaps xmm0, xmm10",
                    "movaps [rsp + 0x100], xmm15",
                    "subss xmm0, dword [rsi + 0x34]",
                    "mov rcx, rsi",
                    "mulss xmm0, xmm11",
                    "addss xmm0, dword [rsi + 0x34]",
                    "movss [rsi + 0x34], xmm0",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_titleDrawAnimHook, out _titleDrawAnimReverseWrapper)}",
                    $"movss xmm0, [qword {anim3Address}]",
                    "xor r9d, r9d",
                    "movzx ebx, byte [rsp + 0x70]",
                    "xor ecx, ecx",
                    "mov eax, ebx",
                    "movss [rbp - 0x38], xmm0",
                    $"movss xmm0, [qword {anim4Address}]",
                    "shl eax, 0x18",
                    "lea edx, [r9 + 2]",
                    $"or eax, {someSet}",
                    "movss [rbp - 0x24], xmm0",
                    "lea r8d, [r9 + 0xA]",
                    "mov [rbp - 0x58], eax",
                    $"mov word [rbp - 0x70], {animSet1}",
                    $"mov byte [rbp - 0x6E], {animSet2}",
                    "mov byte [rbp - 0x6D], bl",
                    $"mov qword [rbp - 0x54], {animSet3}",
                    $"mov qword [rbp - 0x4C], {animSet4}",
                    $"mov qword [rbp - 0x3C], {animSet3}",
                    $"mov qword [rbp - 0x34], {animSet4}",
                    $"mov qword [rbp - 0x40], {someSet}",
                    $"mov qword [rbp - 0x20], {animSet5}",
                    $"mov qword [rbp - 0x1C], {animSet4}",
                    $"mov qword [rbp - 0x28], {someSet}",
                    $"mov dword [rsp + 0x20], {someOtherValue}",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0E10Hook, out _unkAnimRelated1403D0E10ReverseWrapper)}",
                    "xor edi, edi",
                    "mov qword [rsp + 0x38], 0x18",
                    "lea rax, [rbp - 0x60]",
                    "mov qword [rsp + 0x30], 2",
                    "mov [rsp + 0x28], rax",
                    "xor r8d, r8d",
                    "mov edx, 0x80019E",
                    "mov [rsp + 0x20], rdi",
                    "lea r9d, [rdi + 3]",
                    "lea ecx, [rdi + 4]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403CF720Hook, out _unkAnimRelated1403CF720ReverseWrapper)}",
                    "movss xmm7, [rsp + 0x74]",
                    $"mulss xmm7, [qword {anim5Address}]",
                    "movaps xmm0, xmm7",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403160E0Hook, out _unkAnimRelated1403160E0ReverseWrapper)}",
                    "movaps xmm8, xmm0",
                    "movaps xmm0, xmm7",
                    $"mulss xmm8, [qword {anim6Address}]",
                    $"addss xmm8, [qword {anim7Address}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated140316150Hook, out _unkAnimRelated140316150ReverseWrapper)}",
                    $"mulss xmm0, [qword {anim6Address}]",
                    "mov r9d, 0xFFFFFF",
                    "mov rax, [rsi + 0x58]",
                    "movss [rsp + 0x68], xmm10",
                    "xorps xmm2, xmm2",
                    "movss [rsp + 0x60], xmm10",
                    $"addss xmm0, [qword {anim8Address}]",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "movaps xmm1, xmm0",
                    "mov dword [rsp + 0x30], edi",
                    "movaps xmm0, xmm8",
                    "mov dword [rsp + 0x28], 0x20000",
                    "mov byte [rsp + 0x20], bl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "xor r9d, r9d",
                    "mov dword [rsp + 0x20], edi",
                    "xor ecx, ecx",
                    "lea edx, [rdi + 2]",
                    "lea r8d, [rdi + 3]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0E10Hook, out _unkAnimRelated1403D0E10ReverseWrapper)}",
                    "mov rax, [rsi + 0x58]",
                    "movaps xmm0, xmm10",
                    "subss xmm0, [rsi + 0x34]",
                    $"movss xmm1, [qword {anim9Address}]",
                    "mov r9d, 0xFFFFFF",
                    "movss [rsp + 0x68], xmm10",
                    "xorps xmm2, xmm2",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    $"mulss xmm0, [qword {anim10Address}]",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    $"addss xmm0, [qword {anim11Address}]",
                    "mov [rsp + 0x38], rax",
                    "mov dword [rsp + 0x30], 1",
                    "mov dword [rsp + 0x28], 0x40000",
                    "cvttss2si ecx, xmm0",
                    $"movss xmm0, [qword {anim12Address}]",
                    "mov byte [rsp + 0x20], cl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0xFFFFFF",
                    $"movss xmm1, [qword {anim13Address}]",
                    "xorps xmm2, xmm2",
                    $"movss xmm0, [qword {anim14Address}]",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov dword [rsp + 0x30], edi",
                    "mov dword [rsp + 0x28], 0x30000",
                    "mov byte [rsp + 0x20], -1",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "mov rax, [rsi + 0x58]",
                    "movaps xmm0,xmm10",
                    "subss xmm0, [rsi + 0x34]",
                    $"movss xmm15, [qword {anim15Address}]",
                    "mov r9d, 0xFFFFFF",
                    $"movss xmm1, [qword {anim22Address}]",
                    "xorps xmm2, xmm2",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mulss xmm0, xmm15",
                    "mov dword [rsp + 0x30], 1",
                    "mov dword [rsp + 0x28], 0x10000",
                    "cvttss2si ecx, xmm0",
                    $"movss xmm0, [qword {anim23Address}]",
                    "mov byte [rsp + 0x20], cl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "movss xmm0, [rsi + 0x34]",
                    $"lea rbx, [qword {fSprTexAnimPtrOff}]",
                    "mulss xmm0, xmm15",
                    $"lea r14, [qword {iSprMenuTexIndexPtrOff}]",
                    "mov qword [rbp - 0x80], rbx",
                    $"lea r15, [qword {iSprTexAnimEndPtrOff}]",
                    "mov r13, r14",
                    "cvttss2si r12d, xmm0",
                    "mov [rsp + 0x74], r12d",
                    "mov r12, rbx",
                    "mov ebx, [rsp + 0x74]",
                    "nop dword [rax + rax + 0]",
                    "label loopMenuAnim",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0xFFFFFF",
                    "movss xmm1, dword [r12 + 4]",
                    "movaps xmm2, xmm9",
                    "movss xmm0, dword [r12]",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov eax, [r13 + 0]",
                    "mov dword [rsp + 0x30], eax",
                    "mov dword [rsp + 0x28], 0x70000",
                    "mov byte [rsp + 0x20], bl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "add r13, 1",
                    "lea r12, [r12 + 8]",
                    "cmp r13, r15",
                    "jl loopMenuAnim",
                    "mov edx, 1",
                    "mov ecx, edx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D1380Hook, out _unkAnimRelated1403D1380ReverseWrapper)}",
                    "mov ecx, 1",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0F70Hook, out _unkAnimRelated1403D0F70ReverseWrapper)}",
                    "movd xmm6, [rsp + 0x78]",
                    "mov r9d, 0xFFFFFF",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0xFFFFFF",
                    $"movss xmm8, [qword {anim16Address}]",
                    $"movss xmm0, [qword {anim17Address}]",
                    "movaps xmm2, xmm8",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "cvtdq2ps xmm6, xmm6",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov dword [rsp + 0x30], 2",
                    "mov dword [rsp + 0x28], 0x70000",
                    $"divss xmm6, [qword {anim18Address}]",
                    "mov byte [rsp + 0x20], bl",
                    $"mulss xmm6, [qword {anim19Address}]",
                    $"addss xmm6, [qword {anim20Address}]",
                    "movaps xmm1, xmm6",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0xFFFFFF",
                    $"movss xmm0, [qword {anim21Address}]",
                    "movaps xmm2, xmm8",
                    "movss [rsp + 0x68], xmm10",
                    "movaps xmm1, xmm6",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov dword [rsp + 0x30], 3",
                    "mov dword [rsp + 0x28], 0x70000",
                    "mov byte [rsp + 0x20], bl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "xor ecx, ecx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0F70Hook, out _unkAnimRelated1403D0F70ReverseWrapper)}",
                    "mov ecx, 6",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0F30Hook, out _unkAnimRelated1403D0F30ReverseWrapper)}",
                    "mov rbx, qword [rbp - 0x80]",
                    "mov r12d, [rsp + 0x74]",
                    "nop word [rax + rax + 0]",
                    "label loopMenuAnim2",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0x125",
                    "movss xmm1, dword [rbx + 4]",
                    "movaps xmm2, xmm9",
                    "movss xmm0, dword [rbx]",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov eax, [r14]",
                    "mov dword [rsp + 0x30], eax",
                    "mov dword [rsp + 0x28], 0x70000",
                    "mov byte [rsp + 0x20], r12l",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "add r14, 1",
                    "add rbx, 8",
                    "cmp r14, r15",
                    "jl loopMenuAnim2",
                    "mov ecx, 1",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0F30Hook, out _unkAnimRelated1403D0F30ReverseWrapper)}",
                    "xor edx, edx",
                    "lea ecx, [rdx + 1]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D1380Hook, out _unkAnimRelated1403D1380ReverseWrapper)}",
                    "mov eax, [rsi]",
                    "sub eax, 0xF",
                    "cmp eax, 1",
                    "jna differentAnim",
                    "movaps xmm1, xmm10",
                    "jmp continueToCopyright",
                    "label differentAnim",
                    "movaps xmm1, xmm9",
                    "label continueToCopyright",
                    "subss xmm1, dword [rsi + 0x38]",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0xFFFFFF",
                    $"movss xmm0, [qword {anim25Address}]",
                    "movaps xmm2, xmm9",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mulss xmm1, xmm11",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov dword [rsp + 0x30], 2",
                    "addss xmm1, dword [rsi + 0x38]",
                    "mov dword [rsp + 0x28], 0x10000",
                    "movss dword [rsi + 0x38], xmm1",
                    "mulss xmm1, xmm15",
                    "cvttss2si ecx, xmm1",
                    $"movss xmm1, [qword {anim24Address}]",
                    "mov byte [rsp + 0x20], cl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "test byte [rsi + 4], 2",
                    $"movss xmm12, [qword {anim26Address}]",
                    $"movss xmm13, [qword {anim27Address}]",
                    "je makotoAnim",
                    "movss xmm0, dword [rsi + 0x1C]",
                    $"mulss xmm0, [qword {anim28Address}]",
                    $"divss xmm0, [qword {anim29Address}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated140316150Hook, out _unkAnimRelated140316150ReverseWrapper)}",
                    "mov ecx, [rsi + 0x18]",
                    "movaps xmm6, xmm10",
                    "mov rax, [rsi + 0x58]",
                    "subss xmm6, xmm0",
                    $"movss xmm11, [qword {anim30Address}]",
                    "xor ecx, 1",
                    "movss [rsp + 0x68], xmm12",
                    "add ecx, 5",
                    "movss [rsp + 0x60], xmm10",
                    "mov r9d, 0xFFFFFF",
                    "movss [rsp + 0x58], xmm9",
                    "movaps xmm2, xmm9",
                    "mov [rsp + 0x50], di",
                    "movaps xmm1, xmm6",
                    "mov [rsp + 0x48], di",
                    "movaps xmm0, xmm11",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mulss xmm1, xmm15",
                    "shl ecx, 0x10",
                    "mov dword [rsp + 0x30], 1",
                    "mov dword [rsp + 0x28], ecx",
                    "cvttss2si ebx, xmm1",
                    "movaps xmm1, xmm13",
                    "mov byte [rsp + 0x20], bl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    $"mulss xmm6, [qword {anim31Address}]",
                    "not bl",
                    "subss xmm11, xmm6",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated140317840Hook, out _unkAnimRelated140317840ReverseWrapper)}",
                    $"mulss xmm0, [qword {anim32Address}]",
                    "addss xmm0, dword [rsi + 0x1C]",
                    $"comiss xmm0, [qword {anim29Address}]",
                    "movss dword [rsi + 0x1C], xmm0",
                    "jna continueNextCharaSpr",
                    "and dword [rsi + 4], -3",
                    "jmp continueNextCharaSpr",
                    "label makotoAnim",
                    $"movss xmm8, [qword {anim30Address}]",
                    "mov bl, -1",
                    "label continueNextCharaSpr",
                    "mov ecx, [rsi + 0x18]",
                    "mov r9d, 0xFFFFFF",
                    "mov rax, [rsi + 0x58]",
                    "add ecx, 5",
                    "movss [rsp + 0x68], xmm12",
                    "movaps xmm2, xmm9",
                    "movss [rsp + 0x60], xmm10",
                    "movaps xmm1, xmm13",
                    "movss [rsp + 0x58], xmm9",
                    "movaps xmm0, xmm11",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "shl ecx, 0x10",
                    "mov dword [rsp + 0x30], 1",
                    "mov dword [rsp + 0x28], ecx",
                    "mov byte [rsp + 0x20], bl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "movaps xmm12, [rsp + 0x120]",
                    "movaps xmm11, [rsp + 0x130]",
                    "movaps xmm10, [rsp + 0x140]",
                    "movaps xmm8, [rsp + 0x160]",
                    "movaps xmm7, [rsp + 0x170]",
                    "movaps xmm6, [rsp + 0x180]",
                    "mov r15, [rsp + 0x190]",
                    "mov r14, [rsp + 0x198]",
                    "mov r13, [rsp + 0x1D8]",
                    "mov r12, [rsp + 0x1D0]",
                    "mov rdi, [rsp + 0x1C0]",
                    "movaps xmm13, [rsp + 0x110]",
                    "mov rcx, [rbp - 0x10]",
                    "xor rcx, rsp",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(__security_check_cookieHook, out __security_check_cookieReverseWrapper)}",
                    "movaps xmm9, [rsp + 0x150]",
                    "add rsp, 0x1A0",
                    "pop rsi",
                    "pop rbx",
                    "pop rbp",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 160, true)}"
                };
                _journeyAnswerTitleAnim2Hook = hooks.CreateAsmHook(function, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("0F BA F0 1F 0F 57 C0 8B C8 F3 48 0F 2A C1 F3 0F 59 05 ?? ?? ?? ?? F3 48 0F 2C C8 48 63 C9 66 83 7C 4B ?? ??", "JourneyAnswerTitleAnim3", address =>
            {
                nuint anim1Address = (nuint)anim1;
                nuint anim2Address = (nuint)anim2;
                nuint anim3Address = (nuint)anim3;
                nuint anim4Address = (nuint)anim4;
                nuint anim5Address = (nuint)anim5;
                nuint anim6Address = (nuint)anim6;
                nuint anim7Address = (nuint)anim7;
                nuint anim8Address = (nuint)anim8;
                nuint anim9Address = (nuint)anim9;
                nuint anim10Address = (nuint)anim10;
                nuint anim11Address = (nuint)anim11;
                nuint anim12Address = (nuint)anim12;
                nuint anim13Address = (nuint)anim13;
                nuint anim14Address = (nuint)anim14;
                nuint anim15Address = (nuint)anim15;
                nuint anim16Address = (nuint)anim16;
                nuint anim17Address = (nuint)anim17;
                nuint anim18Address = (nuint)anim18;
                nuint anim19Address = (nuint)anim19;
                nuint anim20Address = (nuint)anim20;
                nuint anim21Address = (nuint)anim21;
                nuint anim22Address = (nuint)anim22;
                nuint anim23Address = (nuint)anim23;
                nuint anim24Address = (nuint)anim24;
                nuint anim25Address = (nuint)anim25;
                nuint anim26Address = (nuint)anim26;
                nuint anim27Address = (nuint)anim27;
                nuint anim28Address = (nuint)anim28;
                nuint anim29Address = (nuint)anim29;
                nuint anim30Address = (nuint)anim30;
                nuint anim31Address = (nuint)anim31;
                nuint anim32Address = (nuint)anim32;
                nuint anim33Address = (nuint)anim33;
                nuint fSprTexAnimPtrOff = (nuint)fSprTexAnimPtr;
                nuint iSprTexAnimEndPtrOff = (nuint)iSprTexAnimEndPtr;
                nuint iSprMenuTexIndexPtrOff = (nuint)iSprMenuTexIndexPtr;
                string[] function =
                {
                    "use64",
                    $"movss xmm10, [qword {anim1Address}]",
                    "lea r8, [rsp + 0x74]",
                    "movaps xmm0, xmm10",
                    "lea rdx, [rsp + 0x70]",
                    "subss xmm0, dword [rsi + 0x34]",
                    "mov rcx, rsi",
                    $"mulss xmm0, [qword {anim2Address}]",
                    "addss xmm0, dword [rsi + 0x34]",
                    "movss [rsi + 0x34], xmm0",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_titleDrawAnimHook, out _titleDrawAnimReverseWrapper)}",
                    $"movss xmm0, [qword {anim3Address}]",
                    "xor r9d, r9d",
                    "movzx ebx, byte [rsp + 0x70]",
                    "xor ecx, ecx",
                    "mov eax, ebx",
                    "movss [rbp - 0x38], xmm0",
                    $"movss xmm0, [qword {anim4Address}]",
                    "shl eax, 0x18",
                    "lea edx, [r9 + 2]",
                    $"or eax, {someSet}",
                    "movss [rbp - 0x24], xmm0",
                    "lea r8d, [r9 + 0xA]",
                    "mov [rbp - 0x58], eax",
                    $"mov word [rbp - 0x70], {animSet1}",
                    $"mov byte [rbp - 0x6E], {animSet2}",
                    "mov byte [rbp - 0x6D], bl",
                    $"mov qword [rbp - 0x54], {animSet3}",
                    $"mov qword [rbp - 0x4C], {animSet4}",
                    $"mov qword [rbp - 0x3C], {animSet3}",
                    $"mov qword [rbp - 0x34], {animSet4}",
                    $"mov qword [rbp - 0x40], {someSet}",
                    $"mov qword [rbp - 0x20], {animSet5}",
                    $"mov qword [rbp - 0x1C], {animSet4}",
                    $"mov qword [rbp - 0x28], {someSet}",
                    $"mov dword [rsp + 0x20], {someOtherValue}",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0E10Hook, out _unkAnimRelated1403D0E10ReverseWrapper)}",
                    "xor edi, edi",
                    "mov qword [rsp + 0x38], 0x18",
                    "lea rax, [rbp - 0x60]",
                    "mov qword [rsp + 0x30], 2",
                    "mov [rsp + 0x28], rax",
                    "xor r8d, r8d",
                    "mov edx, 0x80019E",
                    "mov [rsp + 0x20], rdi",
                    "lea r9d, [rdi + 3]",
                    "lea ecx, [rdi + 4]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403CF720Hook, out _unkAnimRelated1403CF720ReverseWrapper)}",
                    "movss xmm7, [rsp + 0x74]",
                    $"mulss xmm7, [qword {anim5Address}]",
                    "movaps xmm0, xmm7",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403160E0Hook, out _unkAnimRelated1403160E0ReverseWrapper)}",
                    "movaps xmm8, xmm0",
                    "movaps xmm0, xmm7",
                    $"mulss xmm8, [qword {anim6Address}]",
                    $"addss xmm8, [qword {anim7Address}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated140316150Hook, out _unkAnimRelated140316150ReverseWrapper)}",
                    $"mulss xmm0, [qword {anim6Address}]",
                    "mov r9d, 0xFFFFFF",
                    "mov rax, [rsi + 0x58]",
                    "movss [rsp + 0x68], xmm10",
                    "xorps xmm2, xmm2",
                    "movss [rsp + 0x60], xmm10",
                    $"addss xmm0, [qword {anim8Address}]",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "movaps xmm1, xmm0",
                    "mov dword [rsp + 0x30], edi",
                    "movaps xmm0, xmm8",
                    "mov dword [rsp + 0x28], 0x20000",
                    "mov byte [rsp + 0x20], bl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "xor r9d, r9d",
                    "mov dword [rsp + 0x20], edi",
                    "lea edx, [rdi + 2]",
                    "xor ecx, ecx",
                    "lea r8d, [rdi + 3]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0E10Hook, out _unkAnimRelated1403D0E10ReverseWrapper)}",
                    "movaps xmm0, xmm10",
                    "subss xmm0, [rsi + 0x34]",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0xFFFFFF",
                    $"movss xmm1, [qword {anim9Address}]",
                    "xorps xmm2, xmm2",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    $"mulss xmm0, [qword {anim10Address}]",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    $"addss xmm0, [qword {anim11Address}]",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov dword [rsp + 0x30], 1",
                    "cvttss2si ecx, xmm0",
                    "mov dword [rsp + 0x28], 0x40000",
                    $"movss xmm0, [qword {anim12Address}]",
                    "mov byte [rsp + 0x20], cl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0xFFFFFF",
                    $"movss xmm1, [qword {anim13Address}]",
                    "xorps xmm2, xmm2",
                    $"movss xmm0, [qword {anim14Address}]",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov dword [rsp + 0x30], edi",
                    "mov dword [rsp + 0x28], 0x30000",
                    "mov byte [rsp + 0x20], -1",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "movss xmm0, [rsi + 0x34]",
                    $"lea rbx, [qword {fSprTexAnimPtrOff}]",
                    $"mulss xmm0, [qword {anim15Address}]",
                    $"lea r14, [qword {iSprMenuTexIndexPtrOff}]",
                    "mov qword [rbp - 0x80], rbx",
                    $"lea r15, [qword {iSprTexAnimEndPtrOff}]",
                    "mov r13, r14",
                    "cvttss2si r12d, xmm0",
                    "mov [rsp + 0x74], r12d",
                    "mov r12, rbx",
                    "mov ebx, [rsp + 0x74]",
                    "nop dword [rax + rax + 0]",
                    "label loopMenuAnim",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0xFFFFFF",
                    "movss xmm1, dword [r12 + 4]",
                    "movaps xmm2, xmm9",
                    "movss xmm0, dword [r12]",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov eax, [r13 + 0]",
                    "mov dword [rsp + 0x30], eax",
                    "mov dword [rsp + 0x28], 0x70000",
                    "mov byte [rsp + 0x20], bl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "add r13, 1",
                    "lea r12, [r12 + 8]",
                    "cmp r13, r15",
                    "jl loopMenuAnim",
                    "mov edx, 1",
                    "mov ecx, edx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D1380Hook, out _unkAnimRelated1403D1380ReverseWrapper)}",
                    "mov ecx, 1",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0F70Hook, out _unkAnimRelated1403D0F70ReverseWrapper)}",
                    "movd xmm7, [rsp + 0x78]",
                    "mov r9d, 0xFFFFFF",
                    "mov rax, [rsi + 0x58]",
                    $"movss xmm6, [qword {anim16Address}]",
                    $"movss xmm0, [qword {anim17Address}]",
                    "movaps xmm2, xmm6",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "cvtdq2ps xmm7, xmm7",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov dword [rsp + 0x30], 2",
                    "mov dword [rsp + 0x28], 0x70000",
                    $"divss xmm7, [qword {anim18Address}]",
                    "mov byte [rsp + 0x20], bl",
                    $"mulss xmm7, [qword {anim19Address}]",
                    $"addss xmm7, [qword {anim20Address}]",
                    "movaps xmm1, xmm7",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0xFFFFFF",
                    $"movss xmm0, [qword {anim21Address}]",
                    "movaps xmm2, xmm6",
                    "movss [rsp + 0x68], xmm10",
                    "movaps xmm1, xmm7",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov dword [rsp + 0x30], 3",
                    "mov dword [rsp + 0x28], 0x70000",
                    "mov byte [rsp + 0x20], bl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "xor ecx, ecx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0F70Hook, out _unkAnimRelated1403D0F70ReverseWrapper)}",
                    "mov ecx, 6",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0F30Hook, out _unkAnimRelated1403D0F30ReverseWrapper)}",
                    "mov rbx, qword [rbp - 0x80]",
                    "mov r12d, [rsp + 0x74]",
                    "nop word [rax + rax + 0]",
                    "label loopMenuAnim2",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0x125",
                    "movss xmm1, dword [rbx + 4]",
                    "movaps xmm2, xmm9",
                    "movss xmm0, dword [rbx]",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mov eax, [r14]",
                    "mov dword [rsp + 0x30], eax",
                    "mov dword [rsp + 0x28], 0x70000",
                    "mov byte [rsp + 0x20], r12l",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "add r14, 1",
                    "add rbx, 8",
                    "cmp r14, r15",
                    "jl loopMenuAnim2",
                    "mov ecx, 1",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D0F30Hook, out _unkAnimRelated1403D0F30ReverseWrapper)}",
                    "xor edx, edx",
                    "lea ecx, [rdx + 1]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403D1380Hook, out _unkAnimRelated1403D1380ReverseWrapper)}",
                    "mov rax, [rsi + 0x58]",
                    "mov r9d, 0xFFFFFF",
                    $"movss xmm1, [qword {anim22Address}]",
                    "movaps xmm2, xmm9",
                    $"movss xmm0, [qword {anim23Address}]",
                    "movss [rsp + 0x68], xmm10",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "mulss xmm0, xmm8",
                    "mov dword [rsp + 0x30], 2",
                    "mov dword [rsp + 0x28], 0x10000",
                    "cvttss2si ecx, xmm0",
                    "mov byte [rsp + 0x20], cl",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "movss xmm0, [rsi + 8]",
                    $"mulss xmm0, [qword {anim28Address}]",
                    $"divss xmm0, [qword {anim29Address}]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkAnimRelated1403160E0Hook, out _unkAnimRelated1403160E0ReverseWrapper)}",
                    $"movsd xmm2, [qword {anim1Address}]",
                    "xorps xmm1, xmm1",
                    "mov ecx, [rsi + 0x18]",
                    "mov r9d, 0xFFFFFF",
                    "mov rax, [rsi + 0x58]",
                    "add ecx, 5",
                    $"movss xmm7, [qword {anim26Address}]",
                    $"movss xmm6, [qword {anim27Address}]",
                    "movss [rsp + 0x68], xmm7",
                    "movss [rsp + 0x60], xmm10",
                    "movss [rsp + 0x58], xmm9",
                    "mov [rsp + 0x50], di",
                    "mov [rsp + 0x48], di",
                    "cvtss2sd xmm1, xmm0",
                    "mov [rsp + 0x40], edi",
                    "mov [rsp + 0x38], rax",
                    "shl ecx, 0x10",
                    "mov dword [rsp + 0x30], 1",
                    "mov dword [rsp + 0x28], ecx",
                    "subsd xmm2, xmm1",
                    "mov byte [rsp + 0x20], -1",
                    "movaps xmm1, xmm6",
                    "cvtpd2ps xmm8, xmm2",
                    "movaps xmm2, xmm9",
                    $"mulss xmm8, [qword {anim33Address}]",
                    $"addss xmm8, [qword {anim30Address}]",
                    "movaps xmm0, xmm8",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "mov ecx, [rsi + 0x18]",
                    "mov eax, 0xFFFFFF94",
                    "movss [rsp + 0x68], xmm7",
                    "test ecx, ecx",
                    "movss [rsp + 0x60], xmm10",
                    "mov r8d, 0xFFFFFF9D",
                    "movss [rsp + 0x58], xmm9",
                    "cmovne eax, r8d",
                    "mov [rsp + 0x50], di",
                    "lea edx, [rcx + 1]",
                    "mov [rsp + 0x48], di",
                    "xor r9d, r9d",
                    "mov [rsp + 0x40], edi",
                    "movaps xmm1, xmm6",
                    "movd xmm0, eax",
                    "mov rax, [rsi + 0x58]",
                    "mov [rsp + 0x38], rax",
                    "mov [rsp + 0x30], edx",
                    "cvtdq2ps xmm0, xmm0",
                    "mov [rsp + 0x28], edi",
                    "mov byte [rsp + 0x20], -1",
                    "addss xmm0, xmm8",
                    "movaps xmm2, xmm9",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_renderSprHook, out _renderSprReverseWrapper)}",
                    "movaps xmm12, [rsp + 0x120]",
                    "movaps xmm11, [rsp + 0x130]",
                    "movaps xmm10, [rsp + 0x140]",
                    "movaps xmm8, [rsp + 0x160]",
                    "movaps xmm7, [rsp + 0x170]",
                    "movaps xmm6, [rsp + 0x180]",
                    "mov r15, [rsp + 0x190]",
                    "mov r14, [rsp + 0x198]",
                    "mov r13, [rsp + 0x1D8]",
                    "mov r12, [rsp + 0x1D0]",
                    "mov rdi, [rsp + 0x1C0]",
                    "movaps xmm13, [rsp + 0x110]",
                    "mov rcx, [rbp - 0x10]",
                    "xor rcx, rsp",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(__security_check_cookieHook, out __security_check_cookieReverseWrapper)}",
                    "movaps xmm9, [rsp + 0x150]",
                    "add rsp, 0x1A0",
                    "pop rsi",
                    "pop rbx",
                    "pop rbp",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 147, true)}"
                };
                _journeyAnswerTitleAnim3Hook = hooks.CreateAsmHook(function, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("41 8B 8C 85 ?? ?? ?? ?? 49 03 CD 33 FF 8D 6F 05 FF E1 89 7B 14 89 33 E8 ?? ?? ?? ?? 85 C0 75 0E C7 43 ?? ?? ?? ?? ??", "jumpTitleScreen", address =>
            {
                string[] function =
                {
                    "use64",
                    "mov ecx, [r13 + rax * 4 + 0x00826FDC]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 8, true)}"
                };
                _jumpTitleScreenHook = hooks.CreateAsmHook(function, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("48 8D 0D ?? ?? ?? ?? C7 03 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 89 43 58 E9 ?? ?? ?? ?? 48 8B 4B 58", "NewSprList", address =>
            {
                nuint sSprFileInfoPtrOff = (nuint)sSprFileInfoPtr;
                string[] function =
                {
                    "use64",
                    $"lea rcx, [qword {sSprFileInfoPtrOff}]"
                };
                _newSprListHook = hooks.CreateAsmHook(function, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("48 8B D9 0F 29 7C 24 ?? 48 8D 0D ?? ?? ?? ?? 44 0F 29 44 24 ?? 0F 28 FA 44 0F 28 C3 0F 28 F1 E8 ?? ?? ?? ?? 0F B6 84 24 ?? ?? ?? ??", "Case29ToCase30", address => //140125D0B
            {
                string[] function =
                {
                    "use64",
                    "mov dword [rbx], 0x1E",
                    "mov [rbx + 8], edi",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 36, true)}"
                };
                _case29ToCase30Hook = hooks.CreateAsmHook(function, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("0F B6 84 24 ?? ?? ?? ?? F3 0F 58 F6 0F B6 94 24 ?? ?? ?? ?? F3 0F 58 FF C1 E2 08 0B D0 F3 44 0F 11 43 ?? 0F B6 84 24 ?? ?? ?? ??", "Case30PressButton", address => //140125D2F
            {
                nuint titleValue1Address = (nuint)titleValue1;
                nuint titleValue2Address = (nuint)titleValue2;
                string[] function =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_getMenuHook, out _getMenuReverseWrapper)}",
                    $"mulss xmm0, [qword {titleValue1Address}]",
                    "addss xmm0, dword [rbx + 8]",
                    $"comiss xmm0, [qword {titleValue2Address}]",
                    "movss dword [rbx + 8], xmm0",
                    "jb skipFadeOut",
                    "mov edx, 0x14",
                    "mov dword [rbx], 0x1B",
                    "mov ecx, esi",
                    "mov [rbx + 0x24], esi",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_fadeOutHook, out _fadeOutReverseWrapper)}",
                    "jmp titleScreenAnim",
                    "label skipFadeOut",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_buttonPress2Hook, out _buttonPress2ReverseWrapper)}",
                    "test eax, eax",
                    "je titleScreenAnim",
                    "xor [rbx + 0x18], esi",
                    "or dword [rbx + 4], 2",
                    "mov dword [rbx], 0x1F",
                    "mov [rbx + 0x1C], edi",
                    "mov [rbx + 8], edi",
                    "label titleScreenAnim",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 1205686, true)}"
                };
                _case30PressButtonHook = hooks.CreateAsmHook(function, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("0F B6 84 24 ?? ?? ?? ?? 44 0F 28 44 24 ?? C1 E2 08 0B D0 F3 0F 11 73 ?? 0F B6 84 24 ?? ?? ?? ?? 0F 28 74 24 ?? C1 E2 08 0B D0", "Case31LoadToMenu", address => //140125D52
            {
                nuint xAxisAddress = (nuint)xAxis;
                nuint yAxisAddress = (nuint)yAxis;
                string[] function =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_getMenuHook, out _getMenuReverseWrapper)}",
                    $"movaps xmm0, [qword {xAxisAddress}]",
                    "lea rcx, [rbx + 0x68]",
                    $"movss xmm3, [qword {yAxisAddress}]",
                    "lea rdx, [rsp + 0x38]",
                    "mov dword [rsp + 0x28], 2",
                    "xorps xmm2, xmm2",
                    "movups [rsp + 0x38], xmm0",
                    "mov dword [rbx], 0x20",
                    "mov [rbx + 8], edi",
                    "mov word [rsp + 0x20], si",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_touchListHook, out _touchListReverseWrapper)}",
                    "xor ecx, ecx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_unkMemoryHook, out _unkMemoryReverseWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 14, true)}"
                };
                _case31LoadToMenuHook = hooks.CreateAsmHook(function, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("C1 E2 08 0B D0 F3 0F 11 73 ?? 0F B6 84 24 ?? ?? ?? ?? 0F 28 74 24 ?? C1 E2 08 0B D0 F3 0F 11 7B ?? 0F BF 84 24 ?? ?? ?? ?? 0F 28 7C 24 ?? 89 53 08 66 0F 6E C0 0F BF 84 24 ?? ?? ?? ?? 0F 5B C0 66 0F 6E C8 0F 5B C9", "Case32InMenu", address => //140125D60
            {
                nuint valueAddress = (nuint)value;
                nuint lessThanValueAddress = (nuint)lessThanValue;
                InputFlag SINGLE_BUTTON_PRESS = _inputs->Pressed;
                string[] function =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_getMenuHook, out _getMenuReverseWrapper)}",
                    $"mulss xmm0, [qword {valueAddress}]",
                    "mov edx, 0x4000",
                    "xor ecx, ecx",
                    "addss xmm0, dword [rbx + 8]",
                    "movss dword [rbx + 8], xmm0",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_buttonPressHook, out _buttonPressReverseWrapper)}",
                    "test eax, eax",
                    "jne returnToTitle",
                    "movss xmm0, dword [rbx + 8]",
                    $"comiss xmm0, [qword {lessThanValueAddress}]",
                    "jae returnToTitle",
                    "mov edx, 0x8000",
                    "xor ecx, ecx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_buttonPressHook, out _buttonPressReverseWrapper)}",
                    "test eax, eax",
                    "je titleTextDrawMain",
                    "mov dword [rbx], 0x12",
                    "mov [rbx + 8], edi",
                    "jmp titleScreenAnim",
                    "label titleTextDrawMain",
                    "mov edx, 0x1000",
                    "xor ecx, ecx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_buttonPressHook, out _buttonPressReverseWrapper)}",
                    "test eax, eax",
                    "je continueToMenu",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_getTitleMenuHook, out _getTitleMenuReverseWrapper)}",
                    "test eax, eax",
                    "je titleScreenAnim",
                    "mov dword [rbx], 0x13",
                    "mov [rbx + 8], edi",
                    "jmp titleScreenAnim",
                    "label continueToMenu",
                    "mov r9d, [rbx + 0x28]",
                    "lea rcx, [rsp + 0x48]",
                    "mov r8d, ebp",
                    "mov [rsp + 0x20], edi",
                    "mov edx, ebp",
                    "mov r12d, edi",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_sdkliststate1Hook, out _sdkliststate1ReverseWrapper)}",
                    "xor r9d, r9d",
                    "mov word [rsp + 0x20], di",
                    "lea rcx, [rsp + 0x48]",
                    "lea edx, [r9 + 0x40]",
                    "lea r8d, [r9 + 0x10]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_sdkliststate2Hook, out _sdkliststate2ReverseWrapper)}",
                    "lea rcx, [rsp + 0x48]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_sdkliststateButtonPressHook, out _sdkliststateButtonPressReverseWrapper)}",
                    "sub eax, 1",
                    "je buttonPressed",
                    "cmp eax, 1",
                    "je buttonPressed",
                    "mov r9d, [rbx + 0x28]",
                    "lea rcx, [rsp + 0x48]",
                    "mov r8d, ebp",
                    "mov [rsp + 0x20], edi",
                    "mov edx, ebp",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_sdkliststate1Hook, out _sdkliststate1ReverseWrapper)}",
                    "lea r9, [rsp + 0x30]",
                    "lea r8, [rsp + 0x34]",
                    "lea rdx, [rsp + 0x48]",
                    "lea rcx, [rbx + 0x68]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_menuSelectionHook, out _menuSelectionReverseWrapper)}",
                    "cmp eax, 1",
                    "mov r15d, edi",
                    "mov r12d, eax",
                    "cmove r15d, esi",
                    "jmp continueButtonPress",
                    "label buttonPressed",
                    "mov r15d, esi",
                    "label continueButtonPress",
                    "mov ebp, [rbx + 0x28]",
                    "lea rcx, [rsp + 0x48]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_sdkliststate4Hook, out _sdkliststate4ReverseWrapper)}",
                    "mov [rbx + 0x28], eax",
                    "cmp eax, ebp",
                    "je titleScreenAnim",
                    "mov [rbx + 0x2C], ebp",
                    "shl ebp, 0x10",
                    "mov [rbx + 0x30], ebp",
                    $"test dword [{SINGLE_BUTTON_PRESS}], 0x2000",
                    "jne playSelSound",
                    "cmp r12d, 2",
                    "je playSelSound",
                    "test r15d, r15d",
                    "je titleScreenAnim",
                    "xor r9d, r9d",
                    "xor r8d, r8d",
                    "xor edx, edx",
                    "xor ecx, ecx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_soundCreatorHook, out _soundCreatorReverseWrapper)}",
                    "mov eax, edi",
                    "jmp continueToNextMenu",
                    "label playSelSound",
                    "mov r9d, esi",
                    "xor r8d, r8d",
                    "xor edx, edx",
                    "xor ecx, ecx",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_soundCreatorHook, out _soundCreatorReverseWrapper)}",
                    "mov eax, [rbx + 0x28]",
                    "je titleScreenAnim",
                    "label continueToNextMenu",
                    "movzx ecx, word [r13 + rax]",
                    "sub ecx, 0",
                    "je titleReturn",
                    "sub ecx, 1",
                    "je TheJourney",
                    "cmp ecx, 1",
                    "je TheAnswer",
                    "label titleScreenAnim",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 1205637, true)}",
                    "label TheAnswer",
                    "mov dword [rbx], 0x22",
                    "mov [rbx + 8], edi",
                    "jmp titleScreenAnim",
                    "label TheJourney",
                    "mov dword [rbx], 0x21",
                    "mov [rbx + 8], edi",
                    "jmp titleScreenAnim",
                    "label titleReturn",
                    "xor edx, edx",
                    "mov dword [rbx], 0xA",
                    "mov [rbx + 8], edi",
                    "lea ecx, [rdx + 2]",
                    "lea r8d, [rdx + 0x14]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_bgmFadeOutForMenuHook, out _bgmFadeOutForMenuReverseWrapper)}",
                    "jmp titleScreenAnim",
                    "label returnToTitle",
                    "xor dword [rbx + 0x18], 1",
                    "or dword [rbx + 4], 2",
                    "mov dword [rbx], 6",
                    "mov [rbx + 0x1C], edi",
                    "jmp titleScreenAnim"
                };
                _case32InMenuHook = hooks.CreateAsmHook(function, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("0F B6 84 24 ?? ?? ?? ?? 0F 28 74 24 ?? C1 E2 08 0B D0 F3 0F 11 7B ?? 0F BF 84 24 ?? ?? ?? ?? 0F 28 7C 24 ?? 89 53 08 66 0F 6E C0 0F BF 84 24 ?? ?? ?? ?? 0F 5B C0 66 0F 6E C8 0F 5B C9 F3 0F 11 03 F3 0F 11 4B ?? 48 83 C4 50", "Case33TheJourney", address => //140125D6A
            {
                string[] function =
                {
                    "use64",
                    "mov dword [rbx], 8",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_fileSetup.TheJourney, out _fileSetup._theJourneyReverseWrapper)}",
                    "mov [rbx + 0x48], rax",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 1204572, true)}"
                };
                _case33TheJourneyHook = hooks.CreateAsmHook(function, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

            Utils.SigScan("0F BF 84 24 ?? ?? ?? ?? 0F 28 7C 24 ?? 89 53 08 66 0F 6E C0 0F BF 84 24 ?? ?? ?? ?? 0F 5B C0 66 0F 6E C8", "Case34TheAnswer", address => //140125D81
            {
                string[] function =
                {
                    "use64",
                    "mov dword [rbx], 8",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_fileSetup.TheAnswer, out _fileSetup._theAnswerReverseWrapper)}",
                    "mov [rbx + 0x48], rax",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 1204549, true)}"
                };
                _case34TheAnswerHook = hooks.CreateAsmHook(function, address, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });
        }
    }
}
