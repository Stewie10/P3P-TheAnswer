using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static P3PPC.Expansion.TheAnswer.FieldOffsets.LoadFile;

namespace P3PPC.Expansion.TheAnswer.TblFiles
{
    internal unsafe class HFileSetupItemTblWork
    {
        [StructLayout(LayoutKind.Explicit)]
        internal struct ItemTblWork
        {
            [FieldOffset(0)]
            internal uint state;

            [FieldOffset(4)]
            internal FileData* tblFile;

            [FieldOffset(12)]
            internal FileData* seqFile;
        }
    }
}
