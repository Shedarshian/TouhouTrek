using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

using ZMDFQ.PlayerAction;

namespace ZMDFQ
{
    /// <summary>
    /// 脚本
    /// </summary>
    class Script
    {
        Microsoft.CodeAnalysis.Scripting.Script script { get; } = null;
        /// <summary>
        /// 新建脚本
        /// </summary>
        /// <param name="code">脚本语言代码</param>
        public Script(string code)
        {
            script = CSharpScript.Create(code, ScriptOptions.Default, typeof(Globals));
            script.Compile();
        }
        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="game">游戏参数</param>
        /// <param name="target">目标参数</param>
        public void run(Game game, ActionBase target)
        {
            script.RunAsync(new Globals(game, target));
        }
    }
    /// <summary>
    /// 带有返回类型的脚本
    /// </summary>
    /// <typeparam name="T">脚本在执行的时候的返回类型</typeparam>
    class Script<T>
    {
        Microsoft.CodeAnalysis.Scripting.Script<T> script { get; } = null;
        /// <summary>
        /// 新建脚本
        /// </summary>
        /// <param name="code">脚本语言代码</param>
        public Script(string code)
        {
            script = CSharpScript.Create<T>(code, ScriptOptions.Default, typeof(Globals));
            script.Compile();
        }
        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="game">游戏参数</param>
        /// <param name="target">目标参数</param>
        /// <returns>返回脚本执行的返回值</returns>
        public T run(Game game, ActionBase target)
        {
            return script.RunAsync(new Globals(game, target)).Result.ReturnValue;
        }
    }
}