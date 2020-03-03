using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaberBackUpTool.Interfaces
{
    public interface ILoadingService : INotifyPropertyChanged
    {
        void Create(Func<bool> func);
    }
}
