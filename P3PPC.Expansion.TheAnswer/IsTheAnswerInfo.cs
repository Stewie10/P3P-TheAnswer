using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static P3PPC.Expansion.TheAnswer.KwlnTask.KwlnTaskP3P;

namespace P3PPC.Expansion.TheAnswer
{
    internal unsafe class HFileSetupWork
    {
        [StructLayout(LayoutKind.Explicit)]
        internal struct SetupWork
        {
            [FieldOffset(0)]
            internal int state;

            [FieldOffset(4)]
            internal uint funcIdx;

            [FieldOffset(8)]
            internal KwlnTaskk* currTask;
        }
    }
}
