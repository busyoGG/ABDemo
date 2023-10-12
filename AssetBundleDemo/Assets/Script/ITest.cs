using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[CSharpCallLua]
public interface ITest 
{
    public string name { get; set; }
    public void Run();
}
