using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace P3PPC.Expansion.TheAnswer
{
    internal unsafe class IsTheAnswerSetup
    {
        [StructLayout(LayoutKind.Explicit)]
        internal struct SetupInfo
        {
            [FieldOffset(0)]
            internal GetFiles File;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct GetFiles
        {
            [FieldOffset(72)]
            internal int SetupIndex;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct LoadSetupFiles
        {
            internal int Index;
        }
    }
}
