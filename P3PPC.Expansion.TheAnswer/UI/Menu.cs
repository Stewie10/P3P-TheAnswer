using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static P3PPC.Expansion.TheAnswer.UI.Colours;

namespace P3PPC.Expansion.TheAnswer.UI.UI
{
    internal unsafe class Menu
    {
        [StructLayout(LayoutKind.Explicit)]
        internal struct MenuInfo
        {
            [FieldOffset(88)]
            internal DisplayInfo Text;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct DisplayInfo
        {
            [FieldOffset(8)]
            internal int SelectedSlot;
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

        internal enum SkillTextType : short
        {
            Normal = 0,
            Selected = 1,
            New = 5,
        }
    }
}
