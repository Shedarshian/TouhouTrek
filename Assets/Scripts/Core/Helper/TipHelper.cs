using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    /// <summary>
    /// 统一处理游戏提示字符串
    /// </summary>
    public static class TipHelper
    {
        //暂用，以后走表
        static Dictionary<string, string> dic = new Dictionary<string, string>()
        {
            { "LimitUseTip","请使用一张{0}" }
        };

        public static string GetText(string key,params string[] param)
        {
            return string.Format(dic[key], param);
        }
    }
}
