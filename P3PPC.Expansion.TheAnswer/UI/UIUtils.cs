using P3PPC.Expansion.TheAnswer.Configuration;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Memory.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static P3PPC.Expansion.TheAnswer.UI.Colours;
using static P3PPC.Expansion.TheAnswer.Models.FileUtils;

namespace P3PPC.Expansion.TheAnswer.UI
{
    internal unsafe class UIUtils
    {
        internal RenderSprTextureDelegate RenderSprTexture;
        internal LoadCampFileDelegate LoadCampFile;
        internal QueueCombineMessageDelegate QueueCombineMessage;
        internal GetMessageStateDelegate GetMessageState;
        internal QueuedMessageWaitingDelegate QueuedMessageWaiting;
        internal AfterQueuedMessageDelegate AfterQueuedMessage;

        private IMemory _memory;

        internal bool* IsFemc;

        internal UIUtils(IStartupScanner startupScanner, IReloadedHooks hooks)
        {
            _memory = Memory.Instance;

            startupScanner.AddMainModuleScan("48 89 5C 24 ?? 57 48 81 EC 80 00 00 00 0F 29 74 24 ?? 0F 29 7C 24 ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 44 24 ?? 48 8B D9 0F 28 F3 48 8D 0D ?? ?? ?? ?? 0F 28 FA 8B FA E8 ?? ?? ?? ?? 48 85 DB 75 ?? 8B 15 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 83 C2 02 E8 ?? ?? ?? ?? 0F B6 94 24 ?? ?? ?? ??", result =>
            {
                if (!result.Found)
                {
                    Utils.LogError($"Unable to find RenderSprTexture, stuff won't work :(");
                    return;
                }
                Utils.LogDebug($"Found RenderSprTexture at 0x{result.Offset + Utils.BaseAddress:X}");

                RenderSprTexture = hooks.CreateWrapper<RenderSprTextureDelegate>(Utils.BaseAddress + result.Offset, out _);
            });

            startupScanner.AddMainModuleScan("40 53 48 83 EC 20 48 8B D9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 4C 8B 0D ?? ?? ?? ?? 4D 85 C9 74 ?? 49 8D 41 ??", result =>
            {
                if (!result.Found)
                {
                    Utils.LogError($"Unable to find LoadCampFileDelegate, stuff won't work :(");
                    return;
                }
                Utils.LogDebug($"Found LoadCampFileDelegate at 0x{result.Offset + Utils.BaseAddress:X}");

                LoadCampFile = hooks.CreateWrapper<LoadCampFileDelegate>(Utils.BaseAddress + result.Offset, out _);
            });

            startupScanner.AddMainModuleScan("48 8D 35 ?? ?? ?? ?? 0F 28 05 ?? ?? ?? ??", result =>
            {
                if (!result.Found)
                {
                    Utils.LogError($"Unable to find IsFemc, stuff won't be coloured right :(");
                    return;
                }

                IsFemc = (bool*)Utils.GetGlobalAddress(Utils.BaseAddress + result.Offset - 4);
                Initialise(IsFemc);
                Utils.LogDebug($"Found IsFemc at 0x{(nuint)IsFemc:X}");
            });

            startupScanner.AddMainModuleScan("48 89 5C 24 ?? 57 48 83 EC 20 48 0F BE D9 89 D7 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 48 8B 04 ?? 48 8D 1C ?? 8B 08", result =>
            {
                if (!result.Found)
                {
                    Utils.LogError($"Unable to find QueueCombineMessage, choose skills message won't display :(");
                    return;
                }
                Utils.LogDebug($"Found QueueCombineMessage at 0x{result.Offset + Utils.BaseAddress:X}");

                QueueCombineMessage = hooks.CreateWrapper<QueueCombineMessageDelegate>(Utils.BaseAddress + result.Offset, out _);
            });

            startupScanner.AddMainModuleScan("40 53 48 83 EC 20 48 0F BE D9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 48 8B 0C ?? 48 85 C9 74 ?? 83 39 00", result =>
            {
                if (!result.Found)
                {
                    Utils.LogError($"Unable to find GetMessageState, choose skills message won't display :(");
                    return;
                }
                Utils.LogDebug($"Found GetMessageState at 0x{result.Offset + Utils.BaseAddress:X}");

                GetMessageState = hooks.CreateWrapper<GetMessageStateDelegate>(Utils.BaseAddress + result.Offset, out _);
            });

            startupScanner.AddMainModuleScan("48 89 5C 24 ?? 48 89 74 24 ?? 48 89 7C 24 ?? 41 56 48 83 EC 20 48 0F BE D9", result =>
            {
                if (!result.Found)
                {
                    Utils.LogError($"Unable to find QueuedMessageWaiting, choose skills message won't display :(");
                    return;
                }
                Utils.LogDebug($"Found QueuedMessageWaiting at 0x{result.Offset + Utils.BaseAddress:X}");

                QueuedMessageWaiting = hooks.CreateWrapper<QueuedMessageWaitingDelegate>(Utils.BaseAddress + result.Offset, out _);
            });

            startupScanner.AddMainModuleScan("48 89 5C 24 ?? 57 48 83 EC 20 48 0F BE D9 89 D7 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? 48 8B 04 ?? 48 8D 1C ?? 48 85 C0", result =>
            {
                if (!result.Found)
                {
                    Utils.LogError($"Unable to find AfterQueuedMessage, choose skills message won't display :(");
                    return;
                }
                Utils.LogDebug($"Found AfterQueuedMessage at 0x{result.Offset + Utils.BaseAddress:X}");

                AfterQueuedMessage = hooks.CreateWrapper<AfterQueuedMessageDelegate>(Utils.BaseAddress + result.Offset, out _);
            });

        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Position
        {
            /// <summary>
            /// Starts from the left edge, increasing moves right
            /// </summary>
            internal float X;
            /// <summary>
            /// Goes from the top edge, increasing moves down
            /// </summary>
            internal float Y;
        }

        [Function(CallingConventions.Microsoft)]
        private delegate string* GetSkillNameDelegate(short skillId);

        /// <summary>
        /// Some function that seems to always be called after a queued message is done
        /// Not sure if it's really needed
        /// </summary>
        /// <param name="contextId">The context id of the message when you queued it</param>
        /// <param name="param_2">Something, just use 1, idk</param>
        [Function(CallingConventions.Microsoft)]
        internal delegate bool AfterQueuedMessageDelegate(byte contextId, int param_2);

        /// <summary>
        /// Some function that needs to be run when waiting for a queued message
        /// </summary>
        /// <param name="contextId">The context id of the message when you queued it</param>
        /// <param name="param_2">Something, just use 1, idk</param>
        [Function(CallingConventions.Microsoft)]
        internal delegate void QueuedMessageWaitingDelegate(byte contextId, int param_2);

        /// <summary>
        /// Gets the message you are up to in a series of messages or 0 if you're done
        /// </summary>
        /// <param name="contextId">The context id of the message when you queued it</param>
        /// <returns>The 1 based index of the message you're up to or 0 if you're done with the messages</returns>
        [Function(CallingConventions.Microsoft)]
        internal delegate int GetMessageStateDelegate(byte contextId);

        [Function(CallingConventions.Microsoft)]
        internal delegate uint QueueCombineMessageDelegate(byte contextId, int messageId);

        [Function(CallingConventions.Microsoft)]
        internal delegate GameFile* LoadCampFileDelegate(string fileName);

        [Function(CallingConventions.Microsoft)]
        internal delegate void RenderSprTextureDelegate(GameFile* spr, int spriteIndex, float x, float y, int param_5, byte r, byte g, byte b, byte a, short param_10, short param_11, short param_12, short param_13, short param_14);
    }
}
