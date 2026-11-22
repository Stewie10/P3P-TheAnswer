using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace P3PPC.Expansion.TheAnswer.FieldOffsets
{
    internal unsafe class LoadFile
    {
        [StructLayout(LayoutKind.Explicit)]
        internal struct FileData
        {
            [FieldOffset(0)]
            internal FileData* prev;

            [FieldOffset(4)]
            internal FileData* next;

            [FieldOffset(16)]
            internal ulong readState;

            [FieldOffset(24)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            internal char** path;

            [FieldOffset(280)]
            internal void* fileMemory;

            [FieldOffset(288)]
            internal void* unalignedFileMemory;

            [FieldOffset(296)]
            internal uint fileSize;

            [FieldOffset(300)]
            internal uint fileMode;

            [FieldOffset(304)]
            internal long adxf;

            [FieldOffset(312)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            internal char fileName;

            [FieldOffset(568)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            internal char dir;

            [FieldOffset(824)]
            internal short refCount;
        }
    }
}
