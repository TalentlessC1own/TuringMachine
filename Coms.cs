
using System.Collections.Generic;

namespace TuringMachine
{
  public class Coms
  {
    public int CountEmpty { get; set; }

    public string CurrentContentCell { get; set; }

    public string Command { get; set; }

    public List<string> SplittedCommand { get; set; }

    public string NextColumn { get; set; }

    public string Direction { get; set; }

    public string ReplaceOnIt { get; set; }
  }
}
