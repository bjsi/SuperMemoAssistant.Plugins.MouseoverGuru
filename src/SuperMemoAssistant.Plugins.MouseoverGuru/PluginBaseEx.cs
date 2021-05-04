using Anotar.Serilog;
using MouseoverPopupInterfaces;
using SuperMemoAssistant.Interop.Plugins;
using System.Collections.Generic;

namespace SuperMemoAssistant.Plugins.MouseoverGuru
{
  public static class PluginBaseEx
  {
    public static bool RegisterProvider<T>(this SMAPluginBase<T> plugin, string name, string[] urlRegexes, IMouseoverContentProvider provider) where T : SMAPluginBase<T>
    {
      var svc = plugin.GetService<IMouseoverSvc>();

      if (svc == null)
      {
        LogTo.Debug("Failed to get mouseover service - it was null");
        return false;
      }

      return svc.RegisterProvider(name, urlRegexes, provider);
    }
  }
}
