using Microsoft.Extensions.Configuration;
using P3PPC.Expansion.TheAnswer.Configuration;
using P3PPC.Expansion.TheAnswer.Models;
using P3PPC.Expansion.TheAnswer.UI;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Memory.Sources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static P3PPC.Expansion.TheAnswer.IsTheAnswer;
using static P3PPC.Expansion.TheAnswer.Models.FileUtils;
using static P3PPC.Expansion.TheAnswer.Models.Inputs;
using static P3PPC.Expansion.TheAnswer.UI.Colours;
using static P3PPC.Expansion.TheAnswer.UI.Sprites;
using static P3PPC.Expansion.TheAnswer.UI.UI.Menu;
using static P3PPC.Expansion.TheAnswer.UI.UIUtils;

namespace P3PPC.Expansion.TheAnswer.Title
{
    internal unsafe class JourneyAnswerSelect
    {

        private IAsmHook _setNewTitleScreenHook;
        private GetSaveDataForMemoryDelegate _getSaveDataForMemoryHook;
        private GetSaveFileDelegate _getSaveFileHook;
        private GetArgsDelegate _getArgsHook;
        private TitleScreenDelegate _setTitleScreenHook;
        private GetMenuDelegate _getMenuHook;
        private FadeOutDelegate _fadeOutHook;
        private SoundCreatorDelegate _soundCreatorHook;
        private ButtonPressDelegate _buttonPressHook;
        private GetTitleMenuDelegate _getTitleMenuHook;
        private PointerDelegate _getPointerHook;
        private MemoryRelatedDelegate _memoryRelatedHook;
        private TitleScreenAnimDelegate _titleScreenAnimHook;
        private SdkotDelegate _sdkotHook;
        private SdkListState1Delegate _sdkliststate1Hook;
        private SdkListState2Delegate _sdkliststate2Hook;
        private SdkListStateButtonPressDelegate _sdkliststateButtonPressHook;
        private MenuSelectionDelegate _menuSelectionHook;
        private SdkListState4Delegate _sdkliststate4Hook;
        private ButtonPointerDelegate _getButtonPointerHook;
        private BgmFadeOutForMenuDelegate _bgmFadeOutForMenuHook;
        private IReverseWrapper<NewTitleScreenDelegate> _newTitleScreenReverseWrapper;
        private IReverseWrapper<GetTitleMenuDelegate> _getTitleMenuReverseWrapper;
        private IReverseWrapper<SdkListState1Delegate> _sdkliststate1ReverseWrapper;
        private IReverseWrapper<SdkListState2Delegate> _sdkliststate2ReverseWrapper;
        private IReverseWrapper<SdkListStateButtonPressDelegate> _sdkliststateButtonPressReverseWrapper;
        private IReverseWrapper<MenuSelectionDelegate> _menuSelectionReverseWrapper;
        private IReverseWrapper<SdkListState4Delegate> _sdkliststate4ReverseWrapper;
        private IReverseWrapper<SoundCreatorDelegate> _soundCreatorReverseWrapper;
        private IReverseWrapper<GetMenuDelegate> _getMenuReverseWrapper;
        private IReverseWrapper<BgmFadeOutForMenuDelegate> _bgmFadeOutForMenuReverseWrapper;
        private IAsmHook _setStartChooseStoryHook;
        private IReverseWrapper<StartChooseStoryDelegate> _startChooseStoryReverseWrapper;
        private IAsmHook _chooseStoryMenuHook;
        private IReverseWrapper<ChooseStoryMenuDelegate> _chooseStoryMenuReverseWrapper;
        private IReverseWrapper<MenuOpeningDelegate> _menuOpeningReverseWrapper;

        [Function(CallingConventions.Microsoft)]
        private delegate void NewTitleScreenDelegate(MenuInfo* info);

        [Function(CallingConventions.Microsoft)]
        private delegate long GetSaveDataForMemoryDelegate();

        [Function(CallingConventions.Microsoft)]
        private delegate long GetSaveFileDelegate();

        [Function(CallingConventions.Microsoft)]
        private delegate long GetArgsDelegate(long funcArg);

        [Function(CallingConventions.Microsoft)]
        private delegate long TitleScreenDelegate(long param_1);

        [Function(CallingConventions.Microsoft)]
        private delegate long GetMenuDelegate();

        [Function(CallingConventions.Microsoft)]
        private delegate long FadeOutDelegate(uint type, int fadeOutTime);

        [Function(CallingConventions.Microsoft)]
        private delegate long SoundCreatorDelegate(ushort type, short effectType, ushort majorID, ushort minorID);

        [Function(CallingConventions.Microsoft)]
        private delegate bool ButtonPressDelegate(uint type, int buttonID);

        [Function(CallingConventions.Microsoft)]
        private delegate long GetTitleMenuDelegate();

        [Function(CallingConventions.Microsoft)]
        private delegate long PointerDelegate();

        [Function(CallingConventions.Microsoft)]
        private delegate long MemoryRelatedDelegate();

        [Function(CallingConventions.Microsoft)]
        private delegate long TitleScreenAnimDelegate();

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
        private delegate long ButtonPointerDelegate();

        [Function(CallingConventions.Microsoft)]
        private delegate long BgmFadeOutForMenuDelegate(short param_1, long param_2, uint fadeOutTime);

        [Function(CallingConventions.Microsoft)]
        private delegate void StartChooseStoryDelegate();

        [Function(CallingConventions.Microsoft)]
        private delegate MenuState ChooseStoryMenuDelegate();

        [Function(CallingConventions.Microsoft)]
        private delegate void MenuOpeningDelegate();

        private IMemory _memory;
        private long param_1Title;
        private int _selectedIndex = 0;
        private int _selectedDisplayIndex = 2;
        private float* value;
        private float* lessThanValue;
        private IsTheAnswer _fileSetup;
        private UIUtils _ui;
        private FileUtils _files;
        private GameFile* _journeyAnswerSpr;
        private MenuState _state = MenuState.NotInMenu;
        private bool* _inMenu;
        private TimeSpan movementDelay = TimeSpan.FromMilliseconds(100);
        private TimeSpan movementInitialDelay = TimeSpan.FromMilliseconds(230);
        private InputStruct* _input;
        private int _currentIndex;
        private DisplayInfo* _dispInfo;

        internal void Hook(IReloadedHooks hooks, IStartupScanner startupScanner)
        {
            _memory = Memory.Instance;
            _ui = new UIUtils(startupScanner, hooks);
            _files = new FileUtils(hooks, startupScanner);
            _fileSetup = new IsTheAnswer(hooks);

            value = (float*)_memory.Allocate(sizeof(float));
            lessThanValue = (float*)_memory.Allocate(sizeof(float));
            *value = 0.5f;
            *lessThanValue = 450.0f;

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

            Utils.SigScan("48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 0F B6 CB B8 ?? ?? ?? ?? 48 8B 5C 24 ?? D3 E0 09 47 18 48 83 C4 20 5F C3", "NewTitleScreenCase", address =>
            {
                nuint valueAddress = (nuint)value;
                nuint lessThanValueAddress = (nuint)lessThanValue;
                string[] function =
                {
                    "use64",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(MenuOpening, out _menuOpeningReverseWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(StartChooseStory, out _startChooseStoryReverseWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(ChooseStoryMenu, out _chooseStoryMenuReverseWrapper)}",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(JourneyAnswerMenuDisplay, out _newTitleScreenReverseWrapper)}",
                    //$"{hooks.Utilities.GetAbsoluteCallMnemonics(_getMenuHook, out _getMenuReverseWrapper)}",
                    "mov eax, [rbx + 0x28]",
                    "je resetOpTimer",
                    "movzx ecx, word [rax]",
                    "sub ecx, 0",
                    "je titleReturn",
                    "sub ecx, 1",
                    "je TheJourney",
                    "cmp ecx, 1",
                    "je TheAnswer",
                    "label resetOpTimer",
                    "mov rdi, [rsp + 0xC8]",
                    "mov rsi, [rsp + 0xC0]",
                    "mov rbp, [rsp + 0xB8]",
                    "mov r13, [rsp + 0x80]",
                    $"{hooks.Utilities.GetAbsoluteJumpMnemonics(address + 35, true)}",
                    "label TheAnswer",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_fileSetup.TheAnswer, out _fileSetup._theAnswerReverseWrapper)}",
                    "mov [rbx + 0x48], rax",
                    "mov [rbx + 8], edi",
                    "jmp resetOpTimer",
                    "label TheJourney",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_fileSetup.TheJourney, out _fileSetup._theJourneyReverseWrapper)}",
                    "mov [rbx + 0x48], rax",
                    "mov [rbx + 8], edi",
                    "jmp resetOpTimer",
                    "label titleReturn",
                    "xor edx, edx",
                    "mov dword [rbx], 0xA",
                    "mov [rbx + 8], edi",
                    "lea ecx, [rdx + 2]",
                    "lea r8d, [rdx + 0x14]",
                    $"{hooks.Utilities.GetAbsoluteCallMnemonics(_bgmFadeOutForMenuHook, out _bgmFadeOutForMenuReverseWrapper)}",
                    "jmp resetOpTimer",
                    "label returnToTitle",
                    "xor dword [rbx + 0x18], 1",
                    "or dword [rbx + 4], 2",
                    "mov dword [rbx], 6",
                    "mov [rbx + 0x1C], edi",
                    "jmp resetOpTimer"

                };
                _setNewTitleScreenHook = hooks.CreateAsmHook(function, address, AsmHookBehaviour.ExecuteFirst).Activate();
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

            /*Utils.SigScan("??", "Pointer", address =>
            {
                _getPointerHook = hooks.CreateWrapper<PointerDelegate>(0x143356720, out _);
            });*/

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

            /*Utils.SigScan("??", "ButtonPointer", address =>
            {
                _getButtonPointerHook = hooks.CreateWrapper<ButtonPointerDelegate>(0x143624540, out _);
            });*/
        }
        /*private long JourneyAnswer(long param_1) //1400A4220
        {
            long Args;

            Args = GetArgs(param_1);
            JourneyAnswerMenuDisplay();
            ChooseStoryMenu();
            StartChooseStory();
            if (*(int*)(Args + 40) == 0)
            {
                *(long*)(Args + 72) = _fileSetup.TheJourney();
            }
            else
            {
                *(long*)(Args + 72) = _fileSetup.TheAnswer();
            }
            return 0;
        }*/
        private void StartChooseStory()
        {
            _selectedIndex = 0;
            _selectedDisplayIndex = 0;
            if (_ui.RenderSprTexture != null)
            {
                _state = MenuState.ChooseSelection;
            }
            else
            {
                _state = MenuState.DoneChooseSelection;
                *_inMenu = true;
            }
        }
        private MenuState ChooseStoryMenu()
        {
            if (_dispInfo == null || (_state == MenuState.NotInMenu && _dispInfo == null))
                return MenuState.NotInMenu;

            if (IsHeld(Input.Up, movementDelay, movementInitialDelay))
            {
                if (_selectedIndex > 0)
                {
                    _soundCreatorHook(0, 0, 0, 3);
                }
                else
                {
                    // Only wrap around if you press the button, not holding it
                    if (_inputs->Pressed.HasFlag(InputFlag.Up) || _inputs->ThumbstickPressed.HasFlag(InputFlag.Up))
                    {
                        _soundCreatorHook(0, 0, 0, 3);
                    }
                }
            }
            if (IsHeld(Input.Down, movementDelay, movementInitialDelay))
            {
                if (_selectedIndex < 1)
                {
                    _soundCreatorHook(0, 0, 0, 3);
                }
                else
                {
                    // Only wrap around if you press the button, not holding it
                    if (_inputs->Pressed.HasFlag(InputFlag.Down) || _inputs->ThumbstickPressed.HasFlag(InputFlag.Down))
                    {
                        _soundCreatorHook(0, 0, 0, 3);
                        _selectedIndex = 0;
                        _selectedDisplayIndex = 0;
                    }
                }
            }
            if (_input->Pressed.HasFlag(InputFlag.Confirm))
            {
                var newIndex = 0;
                if (newIndex != 0)
                {
                    _soundCreatorHook(0, 0, 0, 0);

                    if ((1 << newIndex + 1) == 0)
                    {
                        _state = MenuState.NotInMenu;
                        *_inMenu = false;
                        return MenuState.DoneChooseSelection;
                    }
                }
            }

            if (_input->Pressed.HasFlag(InputFlag.Escape))
            {
                _soundCreatorHook(0, 0, 0, 1);
                if (_selectedIndex < 2)
                {
                    _state = MenuState.NotInMenu;
                    _input->Pressed &= ~InputFlag.Escape;
                    *_inMenu = false;
                    return MenuState.NotInMenu;
                }
                else
                {
                    return MenuState.ChooseSelection;
                }
            }

            return MenuState.ChooseSelection;
        }
        private void JourneyAnswerMenuDisplay(MenuInfo* info)
        {
            if (_state == MenuState.ChooseSelection && _dispInfo == null)
            {
                _dispInfo = &info->Text;
                Utils.LogDebug($"Current info is at 0x{(nuint)_dispInfo:X}");
            }
            if (_state != MenuState.ChooseSelection)
                return;

            if (_dispInfo == null)
                _dispInfo = &info->Text;

            if (_journeyAnswerSpr == (GameFile*)0)
            {
                _journeyAnswerSpr = _files.LoadFile("title/story_select/journeyanswer.spr");
                if (_journeyAnswerSpr == (GameFile*)0)
                {
                    Utils.LogError($"Error loading title/story_select/journeyanswer.spr");
                    return;
                }
            }

            if (_journeyAnswerSpr->LoadStatus != FileLoadStatus.Done)
            {
                Utils.LogDebug($"LoadStatus for journeyanswer.spr is {_journeyAnswerSpr->LoadStatus}");
            }
            var bgColour = Colours.Fg;
            var textColour = Colours.Bg;
            _ui.RenderSprTexture(_journeyAnswerSpr, (int)JourneyAnswerSprite.TheJourney, 259, 78.65f, 0, textColour.R, textColour.G, textColour.B, 0xFF, 0x1000, 0x1000, 0, 0, 0);
            _ui.RenderSprTexture(_journeyAnswerSpr, (int)JourneyAnswerSprite.TheAnswer, 259, 70.65f, 0, textColour.R, textColour.G, textColour.B, 0xFF, 0x1000, 0x1000, 0, 0, 0);
            if (_currentIndex > -1)
            {

                float incrementSize = (138.6f - 83) / (_currentIndex - 2);
                int startIndex = _selectedIndex - _selectedDisplayIndex;

                // Render scroll bar slider (up to 138.6 y)
                _ui.RenderSprTexture(_journeyAnswerSpr, (int)JourneyAnswerSprite.CursorL, 251.28f, 83 + (incrementSize * startIndex), 0, bgColour.R, bgColour.G, bgColour.B, 0xFF, 0x1000, 0x1000, 0, 0, 0);
            }
            return;
        }
        private void MenuOpening()
        {
            Utils.LogDebug("Opening results menu");
            _dispInfo = null;
            _journeyAnswerSpr = (GameFile*)0;
            _state = MenuState.NotInMenu;
        }
        private enum MenuState
        {
            NotInMenu,
            ChooseSelection,
            DoneChooseSelection,
            MenuHidden,
        }
    }
}
