using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static P3PPC.Expansion.TheAnswer.UI.Colours;

namespace P3PPC.Expansion.TheAnswer.UI.UI
{
    internal unsafe class TitleMenu
    {
        [StructLayout(LayoutKind.Explicit)]
        internal struct MenuInfo
        {
            [FieldOffset(0)]
            internal int next;

            [FieldOffset(4)]
            internal uint unk_4;

            [FieldOffset(8)]
            internal float unk_8;

            [FieldOffset(12)]
            internal float unk_c;

            [FieldOffset(16)]
            internal MenuInfo* animPtr;

            [FieldOffset(20)]
            internal int unk_14;

            [FieldOffset(24)]
            internal uint unk_18;

            [FieldOffset(28)]
            internal int unk_1c;

            [FieldOffset(32)]
            internal int unk_20;

            [FieldOffset(36)]
            internal int unk_24;

            [FieldOffset(40)]
            internal int scrollIndex;

            [FieldOffset(44)]
            internal int prevScrollIndex;

            [FieldOffset(48)]
            internal int unk_30;

            [FieldOffset(56)]
            internal float unk_38;

            [FieldOffset(72)]
            internal long buttonPtr;

            [FieldOffset(80)]
            internal PakFile* titleFile;

            [FieldOffset(100)]
            internal int newGame;

            [FieldOffset(104)]
            internal int sprIndex;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct PakFile
        {
            [FieldOffset(88)]
            internal nint spr;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PersonaDisplaySkill
        {
            short unk;

            internal short Id;

            // Not what this actually is but we're using it to change colours
            internal Colour BgColour;

            // Not what this actually is but we're using it to change colours
            internal Colour FgColour;
        }
    }
}
