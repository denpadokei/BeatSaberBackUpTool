using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaberBackUpTool.Enum
{
    public enum CommandType
    {
        [Description("バックアップ元")]
        From,
        [Description("バックアップ先")]
        To
    }
}
