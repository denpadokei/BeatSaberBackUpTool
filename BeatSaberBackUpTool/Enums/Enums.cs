using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaberBackUpTool.Enums
{
    public enum CommandType
    {
        /// <summary>
        /// バックアップ元のファイルパス
        /// </summary>
        [Description("バックアップ元")]
        From,
        /// <summary>
        /// バックアップ先のファイルパス
        /// </summary>
        [Description("バックアップ先")]
        To
    }
}
