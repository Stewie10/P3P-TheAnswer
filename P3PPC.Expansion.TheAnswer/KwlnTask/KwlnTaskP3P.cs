using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace P3PPC.Expansion.TheAnswer.KwlnTask
{
    internal unsafe class KwlnTaskP3P
    {
        [StructLayout(LayoutKind.Explicit)]
        internal struct KwlnTaskk
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            internal char name;

            [FieldOffset(22)]
            internal byte nameHash;

            [FieldOffset(24)]
            internal int priority;

            [FieldOffset(28)]
            internal int timer;

            [FieldOffset(32)]
            internal int runningStart;

            [FieldOffset(36)]
            internal long runningDelay;

            [FieldOffset(52)]
            internal int workRelated;

            [FieldOffset(56)]
            internal KwlnTaskUpdate update;

            [FieldOffset(64)]
            internal KwlnTaskDestroy destroy;

            [FieldOffset(72)]
            internal KwlnTaskk* workData;

            [FieldOffset(80)]
            internal KwlnTaskk* next;

            [FieldOffset(88)]
            internal KwlnTaskk* prev;

            [FieldOffset(96)]
            internal KwlnTaskk* unk_54;

            [FieldOffset(104)]
            internal KwlnTaskk* parent;

            [FieldOffset(112)]
            internal KwlnTaskk* child;

            [FieldOffset(120)]
            internal KwlnTaskk* sibling;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct KwlnTaskUpdate
        {
            [FieldOffset(23)]
            internal byte stateAndFlags;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct KwlnTaskDestroy
        {
            [FieldOffset(44)]
            internal float destoryStart;

            [FieldOffset(48)]
            internal int destoryDelay;
        }
    }
}
