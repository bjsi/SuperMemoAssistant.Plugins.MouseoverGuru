using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.MouseoverGuru
{
  public static class UrlUtils
  {
    public static readonly string GuruRegex = @"^https?\:\/\/supermemo\.guru\/wiki\/([\w]+)+";
    public static readonly string MemopediaRegex = @"^https?\:\/\/supermemopedia\.org\/wiki\/([\w]+)+";
    public static readonly string HelpRegex = @"^https?\:\/\/help\.supermemo\.org\/wiki\/([\w]+)+";
  }
}
