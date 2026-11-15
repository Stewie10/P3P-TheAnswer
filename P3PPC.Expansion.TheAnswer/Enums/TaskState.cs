using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3PPC.Expansion.TheAnswer.Enums
{
    internal class TaskState
    {
        public enum KwlnTaskState : int
        {
            Null,
            Staged,
            Running,
            Destroy,
        }
    }
}
