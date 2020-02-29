using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaberBackUpTool.Interfaces
{
    public interface ILoadingService
    {
        void Create(Func<bool> func);
    }
}
