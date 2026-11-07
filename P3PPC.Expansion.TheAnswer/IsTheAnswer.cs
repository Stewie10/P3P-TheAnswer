using P3PPC.Expansion.TheAnswer.Configuration;
using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Memory.Sources;
using System.Net;
using static P3PPC.Expansion.TheAnswer.IsTheAnswerSetup;

namespace P3PPC.Expansion.TheAnswer
{
    public unsafe class IsTheAnswer
    {
        [ThreadStatic]

        internal SetTaskToMemoryDelegate _setTaskToMemoryHook;
        internal LoadTaskDelegate _loadTaskHook;
        internal FreeMemoryDelegate _setFreeMemoryHook;
        internal SetStatDelegate _setStatHook;
        public TheJourneyDelegate _theJourneyHook;
        public TheAnswerDelegate _theAnswerHook;
        public IReverseWrapper<TheJourneyDelegate> _theJourneyReverseWrapper;
        public IReverseWrapper<TheAnswerDelegate> _theAnswerReverseWrapper;

        [Function(CallingConventions.Microsoft)]
        internal delegate long SetTaskToMemoryDelegate(int param_1, int param_2, int param_3);

        [Function(CallingConventions.Microsoft)]
        internal delegate long LoadTaskDelegate(long param_1, string taskName, int param_3, int param_4, int param_5, long taskFunction, long endTaskFunction, long getTaskMemory);

        [Function(CallingConventions.Microsoft)]
        internal delegate void FreeMemoryDelegate(long param_1);

        [Function(CallingConventions.Microsoft)]
        internal delegate int SetStatDelegate(int param_1);

        [Function(CallingConventions.Microsoft)]
        public delegate long TheJourneyDelegate();

        [Function(CallingConventions.Microsoft)]
        public delegate long TheAnswerDelegate();
        private IMemory _memory;
        internal static int isTheAnswerFlag;
        private SetupInfo* param_1SetupJourney;
        private int param_1SetupAnswer;
        private long param_1FreeMemJourney;
        private long param_1FreeMemAnswer;
        private long param_1ItemTblJourney;
        private long param_1BtlTableJourney;
        private long param_1ReadDataJourney;
        private long param_1BtlTableAnswer;
        private long param_1ReadDataAnswer;
        private ulong _setJourneyItemTblReadPtrHook;
        internal ulong _setJourneyBtlTableReloadPtrHook;
        internal ulong _setJourneyReadStartupDataPtrHook;
        internal ulong _setAnswerBtlTableReloadPtrHook;
        internal ulong _setAnswerReadStartupDataPtrHook;
        private GetFiles* _fileSetupIndex;

        internal IsTheAnswer(IReloadedHooks hooks)
        {
            _memory = Memory.Instance;

            isTheAnswerFlag = (int)_memory.Allocate(4);

            Utils.SigScan("48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 48 8B F1 41 8B F8 48 8D 0D ?? ?? ?? ?? 48 8B DA E8 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 0F AF F3 8B D7", "SetTaskToMemory", address =>
            {
                _setTaskToMemoryHook = hooks.CreateWrapper<SetTaskToMemoryDelegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 48 89 6C 24 ?? 56 41 56 41 57 48 83 EC 20 48 89 CD 45 89 CE 48 8D 0D ?? ?? ?? ?? 45 89 C7 48 89 D6 E8 ?? ?? ?? ?? 80 3E 00 75 15 8B 15", "LoadTask", address =>
            {
                _loadTaskHook = hooks.CreateWrapper<LoadTaskDelegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 48 8B F9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B 5F F8 8B F0 48 85 DB 75 15 8B 15 ?? ?? ?? ?? 48 8D 0D", "FreeMemory", address =>
            {
                _setFreeMemoryHook = hooks.CreateWrapper<FreeMemoryDelegate>(address, out _);
            });

            Utils.SigScan("48 83 EC 48 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 8B 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 83 C2 04 E8 ?? ?? ?? ?? BA ?? ?? ?? ?? 41 B8 ?? ?? ?? ?? 8D 4A E1 E8 ?? ?? ?? ?? 48 85 C0 74 39 48 89 44 24 ?? 48 8D 0D ?? ?? ?? ?? 48 8D 05 ?? ?? ?? ?? 45 33 C9 48 89 44 24 ?? 45 33 C0 48 8D 05 ?? ?? ?? ?? BA ?? ?? ?? ?? 48 89 44 24 ?? E8 ?? ?? ?? ?? 48 85 C0", "ItemTblDataRead", address =>
            {
                _setJourneyItemTblReadPtrHook = Utils.GetGlobalAddress(address);
            });

            Utils.SigScan("40 53 48 83 EC 40 48 8B D9 48 8D 0D 42 8E 5F 03", "BtlTableReload", address =>
            {
                _setJourneyBtlTableReloadPtrHook = Utils.GetGlobalAddress(address);
            });

            Utils.SigScan("48 89 5C 24 ?? 57 48 83 EC 40 48 89 CF 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 8B 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 83 C2 04 E8 ?? ?? ?? ?? 8B 15 ?? ?? ?? ?? 81 F2 ?? ?? ?? ?? 41 B8 ?? ?? ?? ?? 8D 4A D9 E8 ?? ?? ?? ?? 48 89 C3 48 85 C0 75 12 48 8B 5C 24 ?? 48 83 C4 40 5F C3 44 32 05 ?? ?? ?? ?? 48 89 5C 24 ?? 48 8D 05 ?? ?? ?? ?? 48 89 44 24 ?? 48 8D 15 ?? ?? ?? ?? 45 31 C9 48 8D 05 ?? ?? ?? ?? 48 89 44 24 ?? 48 89 F9 C7 44 24 ?? ?? ?? ?? ?? 45 8D 41 0F E8 ?? ?? ?? ?? C7 43 04 00 00 00 00", "ReadStartupData", address =>
            {
                _setJourneyReadStartupDataPtrHook = Utils.GetGlobalAddress(address);
            });

            Utils.SigScan("40 53 48 83 EC 40 48 8B D9 48 8D 0D 52 8D 5F 03", "BtlTableReloadAnswer", address =>
            {
                _setAnswerBtlTableReloadPtrHook = Utils.GetGlobalAddress(address);
            });

            Utils.SigScan("48 89 5C 24 ?? 57 48 83 EC 40 48 8B F9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 8B 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 83 C2 04 E8 ?? ?? ?? ?? BA ?? ?? ?? ?? 41 B8 ?? ?? ?? ?? 8D 4A D9 E8 ?? ?? ?? ?? 48 8B D8 48 85 C0 75 0B 48 8B 5C 24 ?? 48 83 C4 40 5F C3 48 89 5C 24 ?? 48 8D 05 ?? ?? ?? ?? 48 89 44 24 ?? 48 8D 15 ?? ?? ?? ?? 45 33 C9 48 8D 05 ?? ?? ?? ?? 48 89 44 24 ?? 48 8B CF C7 44 24 ?? ?? ?? ?? ?? 45 8D 41 0F E8 ?? ?? ?? ?? C7 43 04 01 00 00 00", "ReadStartupDataAnswer", address =>
            {
                _setAnswerReadStartupDataPtrHook = Utils.GetGlobalAddress(address);
            });

            Utils.SigScan("40 53 48 83 EC 20 48 89 CB 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 31 C0 4C 8B 15 ?? ?? ?? ?? 41 89 C1 4C 8B 1D ?? ?? ?? ?? 90 45 89 C8 45 85 C9 74 16 41 83 E8 01", "SetStat", address =>
            {
                _setStatHook = hooks.CreateWrapper<SetStatDelegate>(address, out _);
            });
        }
        public static void IsTheAnswerHook(int isEnabled)
        {
            isTheAnswerFlag = isEnabled;
            return;
        }
        public static long IsTheAnswerSet()
        {
            return isTheAnswerFlag;
        }
        public long TheJourney()
        {
            long setTask;
            long param_2;
            string journeyTaskName = $"HonpenFileSetup";
            long journeySetup = TheJourneySetup(param_1SetupJourney);
            long journeyFreeMem = FreeMemoryJourney(param_1FreeMemJourney);

            setTask = _setTaskToMemoryHook(1, 12, 0x40000);
            if (setTask == 0)
            {
                param_2 = 0;
            }
            else
            {
                param_2 = _loadTaskHook(0, journeyTaskName, 15, 0, 0, journeySetup, journeyFreeMem, (int)setTask);
                if (param_2 == 0)
                {
                    param_2 = 0;
                }
                else
                {
                    IsTheAnswerHook(0);
                }
            }
            return param_2;
        }

        public long TheAnswer()
        {
            long setTask;
            long param_2;
            string answerTaskName = $"GojitudanFileSetup";
            long answerSetup = TheAnswerSetup(param_1SetupAnswer);
            long answerFreeMem = FreeMemoryAnswer(param_1FreeMemAnswer);

            setTask = _setTaskToMemoryHook(1, 12, 0x40000);
            if (setTask == 0)
            {
                param_2 = 0;
            }
            else
            {
                param_2 = _loadTaskHook(0, answerTaskName, 15, 0, 0, answerSetup, answerFreeMem, (int)setTask);
                if (param_2 == 0)
                {
                    param_2 = 0;
                }
                else
                {
                    IsTheAnswerHook(1);
                }
            }
            return param_2;
        }
        private long TheJourneySetup(SetupInfo* info)
        {
            int fileSetupIndex = IndexSetup(&info->File, 0);
            int iVar2;
            int uVar3;
            ulong itemTblJourney = _setJourneyItemTblReadPtrHook;
            ulong btlTableJourney = _setJourneyBtlTableReloadPtrHook;
            ulong readDataJourney = _setJourneyReadStartupDataPtrHook;
            ulong dummyPtr = 0;
            List<ulong> FileSetupPtrJourney = new List<ulong> { itemTblJourney, btlTableJourney, readDataJourney, dummyPtr };
            FileSetupPtrJourney[0] = itemTblJourney;
            FileSetupPtrJourney[1] = btlTableJourney;
            FileSetupPtrJourney[2] = readDataJourney;
            FileSetupPtrJourney[3] = dummyPtr;
            _fileSetupIndex = &info->File;
            if (*_fileSetupIndex = 1)
            {
                uVar3 = _setStatHook(_fileSetupIndex[2]);
                if (uVar3 == 3)
                {
                    _fileSetupIndex[1] = _fileSetupIndex[1] + 1;
                    *_fileSetupIndex = 0;
                }
            }
            else if (*_fileSetupIndex = 0)
            {
                if (FileSetupPtrJourney[_fileSetupIndex[1]] = 0)
                {
                    return -1;
                }
                iVar2 = (FileSetupPtrJourney[_fileSetupIndex[1]]);
                _fileSetupIndex[2] = iVar2;
                *_fileSetupIndex = *_fileSetupIndex + 1;
            }
            return 0;
        }
        public long TheAnswerSetup(int param_1)
        {
            int* fileSetupIndex;
            int iVar2;
            int uVar3;
            param_1SetupAnswer = param_1;
            ulong itemTblAnswer = _setJourneyItemTblReadPtrHook; //placeholder will change later
            ulong btlTableAnswer = _setAnswerBtlTableReloadPtrHook;
            ulong readDataAnswer = _setAnswerReadStartupDataPtrHook;
            ulong dummyPtr = 0;
            List<ulong> FileSetupPtrAnswer = new List<ulong> { itemTblAnswer, btlTableAnswer, readDataAnswer, dummyPtr };

            fileSetupIndex = *(int**)(param_1SetupAnswer + 72);
            FileSetupPtrAnswer[0] = itemTblAnswer;
            FileSetupPtrAnswer[1] = btlTableAnswer;
            FileSetupPtrAnswer[2] = readDataAnswer;
            FileSetupPtrAnswer[3] = dummyPtr;
            if (*fileSetupIndex == 1)
            {
                uVar3 = _setStatHook(fileSetupIndex[2]);
                if (uVar3 == 3)
                {
                    fileSetupIndex[1] = fileSetupIndex[1] + 1;
                    *fileSetupIndex = 0;
                }
            }
            else if (*fileSetupIndex == 0)
            {
                if ((int*)FileSetupPtrAnswer[fileSetupIndex[1]] == (int*)0)
                {
                    return -1;
                }
                iVar2 = (*(int*)FileSetupPtrAnswer[fileSetupIndex[1]]);
                fileSetupIndex[2] = iVar2;
                *fileSetupIndex = *fileSetupIndex + 1;
            }
            return 0;
        }
        public long FreeMemoryJourney(long param_1)
        {
            param_1FreeMemJourney = param_1;

            _setFreeMemoryHook(*(long*)param_1FreeMemJourney + 72);
            return 1;
        }
        public long FreeMemoryAnswer(long param_1)
        {
            param_1FreeMemAnswer = param_1;

            _setFreeMemoryHook(*(long*)param_1FreeMemAnswer + 72);
            return 1;
        }
        internal static int IndexSetup(SetupInfo* info, int file)
        {
            for (int i = 0; i < 3; i++)
            {
                if ((&info->File)[i].SetupIndex == file)
                    return i;
            }
            return -1;
        }
    }
}
