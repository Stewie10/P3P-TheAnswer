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
using Reloaded.Memory;
using System.Net;
using static P3PPC.Expansion.TheAnswer.KwlnTask.KwlnTaskP3P;
using static P3PPC.Expansion.TheAnswer.Enums.ScenarioMode;
using static P3PPC.Expansion.TheAnswer.FieldOffsets.LoadFile;
using static P3PPC.Expansion.TheAnswer.TblFiles.HFileSetupItemTblWork;
using static P3PPC.Expansion.TheAnswer.HFileSetupWork;

namespace P3PPC.Expansion.TheAnswer
{
    public unsafe class IsTheAnswer
    {
        [ThreadStatic]

        internal static RwCallocDelegate _rwCallocHook;
        internal static kwlnTaskCreateWithAutoPriorityDelegate _kwlnTaskCreateWithAutoPriorityHook;
        internal static FreeMemoryDelegate _setFreeMemoryHook;
        internal static kwlnTaskGetStateDelegate _kwlnTaskGetStateHook;
        internal static LoadFileDelegate _loadFileHook;
        internal static IsFileLoadedDelegate _isFileLoadedHook;
        internal static DestroyDataDelegate _destroyDataHook;
        internal static ItemTblDataDelegate _itemTblDataHook;
        internal IReverseWrapper<TheJourneyDelegate> _theJourneyReverseWrapper;
        internal IReverseWrapper<TheAnswerDelegate> _theAnswerReverseWrapper;

        [Function(CallingConventions.Microsoft)]
        internal delegate void* RwCallocDelegate(uint elemCount, int elemSize, uint hint);

        [Function(CallingConventions.Microsoft)]
        internal delegate KwlnTaskk* kwlnTaskCreateWithAutoPriorityDelegate(KwlnTaskk* parentTask, string taskName, int param_3, int param_4, int param_5, void* taskFunction, void* destroyTaskFunction, void* workData);

        [Function(CallingConventions.Microsoft)]
        internal delegate void FreeMemoryDelegate(void* ptr);

        [Function(CallingConventions.Microsoft)]
        internal delegate uint kwlnTaskGetStateDelegate(KwlnTaskk* param_1);

        [Function(CallingConventions.Microsoft)]
        internal delegate KwlnTaskk* TheJourneyDelegate();

        [Function(CallingConventions.Microsoft)]
        internal delegate KwlnTaskk* TheAnswerDelegate();

        [Function(CallingConventions.Microsoft)]
        internal delegate FileData* LoadFileDelegate(string path, uint fileMode);

        [Function(CallingConventions.Microsoft)]
        internal delegate uint IsFileLoadedDelegate(FileData* file);

        [Function(CallingConventions.Microsoft)]
        internal delegate byte DestroyDataDelegate(FileData* file);

        [Function(CallingConventions.Microsoft)]
        internal delegate byte ItemTblDataDelegate();
        private Memory _memory;
        internal static Scenario isTheAnswerFlag;
        private static KwlnTaskk* _honpenTask;
        private static KwlnTaskk* _gojitudanTask;
        private static KwlnTaskk* _gojitudanItemTblTask;
        internal static HFileSetupFuncHonpen _setJourneyItemTblReadPtrHook;
        internal static HFileSetupFuncHonpen _setJourneyBtlTableReloadPtrHook;
        internal static HFileSetupFuncHonpen _setJourneyReadStartupDataPtrHook;
        internal static HFileSetupFuncGojitudan _createGojitudanItemTblTask = CreateGojitudanItemTblTask;
        internal static HFileSetupFuncGojitudan _setAnswerBtlTableReloadPtrHook;
        internal static HFileSetupFuncGojitudan _setAnswerReadStartupDataPtrHook;

        [Function(CallingConventions.Microsoft)]
        internal delegate KwlnTaskk* HFileSetupFuncHonpen(KwlnTaskk* parent);
        internal readonly HFileSetupFuncHonpen[] sHonpenFuncs =
        {
            _setJourneyItemTblReadPtrHook,
            _setJourneyBtlTableReloadPtrHook,
            _setJourneyReadStartupDataPtrHook,
            null
        };

        [Function(CallingConventions.Microsoft)]
        internal delegate KwlnTaskk* HFileSetupFuncGojitudan(KwlnTaskk* parent);
        internal readonly HFileSetupFuncGojitudan[] sGojitudanFuncs =
        {
            _createGojitudanItemTblTask,
            _setAnswerBtlTableReloadPtrHook,
            _setAnswerReadStartupDataPtrHook,
            null
        };

        internal static int KWLNTASK_STOP = -1;
        internal static int KWLNTASK_CONTINUE = 0;
        internal static int KWLNTASK_STATE_NULL = 0;
        internal static int KWLNTASK_STATE_STAGED = 1;
        internal static int KWLNTASK_STATE_RUNNING = 2;
        internal static int KWLNTASK_STATE_DESTROY = 3;

        internal IsTheAnswer(IReloadedHooks hooks)
        {
            _memory = Memory.Instance;

            isTheAnswerFlag = (Scenario)_memory.Allocate(4).Address;

            Utils.SigScan("48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 48 8B F1 41 8B F8 48 8D 0D ?? ?? ?? ?? 48 8B DA E8 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 0F AF F3 8B D7", "SetTaskToMemory", address =>
            {
                _rwCallocHook = hooks.CreateWrapper<RwCallocDelegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 48 89 6C 24 ?? 56 41 56 41 57 48 83 EC 20 48 89 CD 45 89 CE 48 8D 0D ?? ?? ?? ?? 45 89 C7 48 89 D6 E8 ?? ?? ?? ?? 80 3E 00 75 15 8B 15", "LoadTask", address =>
            {
                _kwlnTaskCreateWithAutoPriorityHook = hooks.CreateWrapper<kwlnTaskCreateWithAutoPriorityDelegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC 20 48 8B F9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B 5F F8 8B F0 48 85 DB 75 15 8B 15 ?? ?? ?? ?? 48 8D 0D", "FreeMemory", address =>
            {
                _setFreeMemoryHook = hooks.CreateWrapper<FreeMemoryDelegate>(address, out _);
            });

            Utils.SigScan("48 83 EC 48 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 8B 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 83 C2 04 E8 ?? ?? ?? ?? BA ?? ?? ?? ?? 41 B8 ?? ?? ?? ?? 8D 4A E1 E8 ?? ?? ?? ?? 48 85 C0 74 39 48 89 44 24 ?? 48 8D 0D ?? ?? ?? ?? 48 8D 05 ?? ?? ?? ?? 45 33 C9 48 89 44 24 ?? 45 33 C0 48 8D 05 ?? ?? ?? ?? BA ?? ?? ?? ?? 48 89 44 24 ?? E8 ?? ?? ?? ?? 48 85 C0", "ItemTblDataRead", address =>
            {
                sHonpenFuncs[0] = hooks.CreateWrapper<HFileSetupFuncHonpen>(address, out _);
            });

            Utils.SigScan("40 53 48 83 EC 40 48 8B D9 48 8D 0D 42 8E 5F 03", "BtlTableReload", address =>
            {
                sHonpenFuncs[1] = hooks.CreateWrapper<HFileSetupFuncHonpen>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 57 48 83 EC 40 48 89 CF 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 8B 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 83 C2 04 E8 ?? ?? ?? ?? 8B 15 ?? ?? ?? ?? 81 F2 ?? ?? ?? ?? 41 B8 ?? ?? ?? ?? 8D 4A D9 E8 ?? ?? ?? ?? 48 89 C3 48 85 C0 75 12 48 8B 5C 24 ?? 48 83 C4 40 5F C3 44 32 05 ?? ?? ?? ?? 48 89 5C 24 ?? 48 8D 05 ?? ?? ?? ?? 48 89 44 24 ?? 48 8D 15 ?? ?? ?? ?? 45 31 C9 48 8D 05 ?? ?? ?? ?? 48 89 44 24 ?? 48 89 F9 C7 44 24 ?? ?? ?? ?? ?? 45 8D 41 0F E8 ?? ?? ?? ?? C7 43 04 00 00 00 00", "ReadStartupData", address =>
            {
                sHonpenFuncs[2] = hooks.CreateWrapper<HFileSetupFuncHonpen>(address, out _);
            });

            Utils.SigScan("40 53 48 83 EC 40 48 8B D9 48 8D 0D 52 8D 5F 03", "BtlTableReloadAnswer", address =>
            {
                sGojitudanFuncs[1] = hooks.CreateWrapper<HFileSetupFuncGojitudan>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 57 48 83 EC 40 48 8B F9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 8B 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 83 C2 04 E8 ?? ?? ?? ?? BA ?? ?? ?? ?? 41 B8 ?? ?? ?? ?? 8D 4A D9 E8 ?? ?? ?? ?? 48 8B D8 48 85 C0 75 0B 48 8B 5C 24 ?? 48 83 C4 40 5F C3 48 89 5C 24 ?? 48 8D 05 ?? ?? ?? ?? 48 89 44 24 ?? 48 8D 15 ?? ?? ?? ?? 45 33 C9 48 8D 05 ?? ?? ?? ?? 48 89 44 24 ?? 48 8B CF C7 44 24 ?? ?? ?? ?? ?? 45 8D 41 0F E8 ?? ?? ?? ?? C7 43 04 01 00 00 00", "ReadStartupDataAnswer", address =>
            {
                sGojitudanFuncs[2] = hooks.CreateWrapper<HFileSetupFuncGojitudan>(address, out _);
            });

            Utils.SigScan("40 53 48 83 EC 20 48 89 CB 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B 0D ?? ?? ?? ?? 31 C0 4C 8B 15 ?? ?? ?? ?? 41 89 C1 4C 8B 1D ?? ?? ?? ?? 90 45 89 C8 45 85 C9 74 16 41 83 E8 01", "SetStat", address =>
            {
                _kwlnTaskGetStateHook = hooks.CreateWrapper<kwlnTaskGetStateDelegate>(address, out _);
            });

            Utils.SigScan("48 89 5C 24 ?? 55 56 57 41 56 41 57 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 31 E0 48 89 84 24 ?? ?? ?? ?? 48 89 CF 48 89 D3 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 85 DB 4C 8D 74 24 ?? 4C 0F 45 F3 48 83 FF 01", "LoadFile", address =>
            {
                _loadFileHook = hooks.CreateWrapper<LoadFileDelegate>(address, out _);
            });

            Utils.SigScan("40 53 48 83 EC 20 48 8B D9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 33 C0 83 7B 04 0C 0F 94 C0", "IsFileLoaded", address =>
            {
                _isFileLoadedHook = hooks.CreateWrapper<IsFileLoadedDelegate>(address, out _);
            });

            Utils.SigScan("40 53 48 83 EC 20 48 89 CB 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 85 DB 0F 84 ?? ?? ?? ?? 83 7B 04 00 74 0D 66 FF 83 ?? ?? ?? ?? 48 83 C4 20 5B C3", "DestroyData", address =>
            {
                _destroyDataHook = hooks.CreateWrapper<DestroyDataDelegate>(address, out _);
            });

            Utils.SigScan("48 83 EC 28 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 44 6B 05 ?? ?? ?? ?? ?? 48 8D 05 ?? ?? ?? ?? 48 89 05 ?? ?? ?? ?? 44 89 C2 48 01 C2 8B 02 48 83 C2 10 48 89 15 ?? ?? ?? ?? 48 8D 04 40 48 8D 40 04 48 8D 04 82 48 89 05 ?? ?? ?? ?? 48 83 C0 20", "ItemTblData", address =>
            {
                _itemTblDataHook = hooks.CreateWrapper<ItemTblDataDelegate>(address, out _);
            });
        }
        internal static void datSetScenarioMode(Scenario isEnabled)
        {
            isTheAnswerFlag = isEnabled;
            return;
        }
        internal static Scenario IsTheAnswerSet()
        {
            return isTheAnswerFlag;
        }
        internal KwlnTaskk* TheJourney()
        {
            SetupWork* work;
            KwlnTaskk* task;
            void* journeySetup = TheJourneySetup(_honpenTask);
            void* journeyFreeMem = DestroyHonpenTask(_honpenTask);

            work = (SetupWork*)_rwCallocHook(1, sizeof(SetupWork), 0x40000);
            if (work == null)
            {
                return null;
            }
            task = _kwlnTaskCreateWithAutoPriorityHook(null, "HonpenFileSetup", 15, 0, 0, journeySetup, journeyFreeMem, work);
            if (task == null)
            {
                return null;
            }
            datSetScenarioMode(Scenario.Journey);
            return task;
        }
        internal KwlnTaskk* TheAnswer()
        {
            SetupWork* work;
            KwlnTaskk* task;
            void* answerSetup = TheAnswerSetup(_gojitudanTask);
            void* answerFreeMem = DestroyGojitudanTask(_gojitudanTask);

            work = (SetupWork*)_rwCallocHook(1, sizeof(SetupWork), 0x40000);
            if (work == null)
            {
                return null;
            }
            task = _kwlnTaskCreateWithAutoPriorityHook(null, "GojitudanFileSetup", 15, 0, 0, answerSetup, answerFreeMem, work);
            if (task == null)
            {
                return null;
            }
            datSetScenarioMode(Scenario.Answer);
            return task;
        }
        private void* TheJourneySetup(KwlnTaskk* honpenTask)
        {
            SetupWork* work;
            HFileSetupFuncHonpen[] funcs = new HFileSetupFuncHonpen[4];

            _honpenTask = honpenTask;
            work = (SetupWork*)_honpenTask->workData;

            funcs[0] = sHonpenFuncs[0];
            funcs[1] = sHonpenFuncs[1];
            funcs[2] = sHonpenFuncs[2];
            funcs[3] = sHonpenFuncs[3];
            switch (work->state)
            {
                case 0:
                    if (funcs[work->funcIdx] == null)
                    {
                        return (void*)KWLNTASK_STOP;
                    }
                    work->currTask = funcs[work->funcIdx](_honpenTask);
                    work->state++;
                    break;

                case 1:
                    {
                        if (_kwlnTaskGetStateHook(work->currTask) == KWLNTASK_STATE_DESTROY)
                        {
                            work->funcIdx++;
                            work->state = 0;
                        }
                        break;
                    }
            }
            return (void*)KWLNTASK_CONTINUE;
        }
        private void* TheAnswerSetup(KwlnTaskk* gojitudanTask)
        {
            SetupWork* work;
            HFileSetupFuncGojitudan[] funcs = new HFileSetupFuncGojitudan[4];

            work = (SetupWork*)gojitudanTask->workData;

            _gojitudanTask = gojitudanTask;
            funcs[0] = sGojitudanFuncs[0];
            funcs[1] = sGojitudanFuncs[1];
            funcs[2] = sGojitudanFuncs[2];
            funcs[3] = sGojitudanFuncs[3];
            switch (work->state)
            {
                case 0:
                    if (funcs[work->funcIdx] == null)
                    {
                        return (void*)KWLNTASK_STOP;
                    }
                    work->currTask = funcs[work->funcIdx](_gojitudanTask);
                    work->state++;
                    break;

                case 1:
                    {
                        if (_kwlnTaskGetStateHook(work->currTask) == KWLNTASK_STATE_DESTROY)
                        {
                            work->funcIdx++;
                            work->state = 0;
                        }
                        break;
                    }
            }
            return (void*)KWLNTASK_CONTINUE;
        }
        internal void* DestroyHonpenTask(KwlnTaskk* honpenTask)
        {
            _setFreeMemoryHook(honpenTask->workData);
            return honpenTask;
        }
        internal void* DestroyGojitudanTask(KwlnTaskk* gojitudanTask)
        {
            _setFreeMemoryHook(gojitudanTask->workData);
            return gojitudanTask;
        }
        internal static KwlnTaskk* CreateGojitudanItemTblTask(KwlnTaskk* parent)
        {
            ItemTblWork* work;
            KwlnTaskk* task;
            void* updateGojitudanItemTbl = UpdateGojitudanItemTblTask(_gojitudanItemTblTask);
            void* destroyGojitudanItemTbl = DestroyGojitudanItemTblTask(_gojitudanItemTblTask);

            work = (ItemTblWork*)_rwCallocHook(1, sizeof(ItemTblWork), 0x40000);
            if (work == null)
            {
                return null;
            }
            task = _kwlnTaskCreateWithAutoPriorityHook(null, "GojitudanItemTblDataRead", 15, 0, 0, updateGojitudanItemTbl, destroyGojitudanItemTbl, work);
            if (task == null)
            {
                return null;
            }
            return task;
        }
        internal static void* UpdateGojitudanItemTblTask(KwlnTaskk* itemTblReadTask)
        {
            ItemTblWork* work;

            work = (ItemTblWork*)itemTblReadTask->workData;

            switch(work->state)
            {
                case 0:
                    work->tblFile = _loadFileHook("init/AEGIS_itemtbl.bin", 0);
                    //work->seqFile = _loadFileHook("init/AEGIS_seq.bin", 0);
                    work->state++;
                    break;
                case 1:
                    if (*(bool*)_isFileLoadedHook(work->tblFile) == true) //&& *(bool*)_isFileLoadedHook(work->seqFile) == true)
                    {
                        _destroyDataHook(work->tblFile);
                        work->tblFile = null;

                        //_destroyDataHook(work->seqFile);
                        //work->seqFile = null;

                        _itemTblDataHook();
                        return (void*)KWLNTASK_STOP;
                    }
                    break;
            }
            return (void*)KWLNTASK_CONTINUE;
        }
        internal static void* DestroyGojitudanItemTblTask(KwlnTaskk* itemTblReadTask)
        {
            ItemTblWork* work;

            work = (ItemTblWork*)itemTblReadTask->workData;

            if (work->tblFile != null)
            {
                _destroyDataHook(work->tblFile);
                work->tblFile = null;
            }

            if (work->seqFile != null)
            {
                _destroyDataHook(work->seqFile);
                work->seqFile = null;
            }
            _setFreeMemoryHook(work);
            return (void*)KWLNTASK_CONTINUE;
        }
    }
}
