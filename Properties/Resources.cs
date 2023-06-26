// Decompiled with JetBrains decompiler
// Type: TuringMachine.Properties.Resources
// Assembly: TuringMachine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 73876706-00A5-4A5C-957A-9BFECFE2040A
// Assembly location: C:\Users\POLIN\Desktop\4 семестр\Мат.логика\мт\МТ(эмулятор)\TuringMachine.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace TuringMachine.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (TuringMachine.Properties.Resources.resourceMan == null)
          TuringMachine.Properties.Resources.resourceMan = new ResourceManager("TuringMachine.Properties.Resources", typeof (TuringMachine.Properties.Resources).Assembly);
        return TuringMachine.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => TuringMachine.Properties.Resources.resourceCulture;
      set => TuringMachine.Properties.Resources.resourceCulture = value;
    }
  }
}
