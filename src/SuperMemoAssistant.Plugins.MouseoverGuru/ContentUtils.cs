﻿using mshtml;
using SuperMemoAssistant.Extensions;
using SuperMemoAssistant.Interop.SuperMemo.Content.Controls;
using SuperMemoAssistant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.MouseoverGuru
{

  public static class ContentUtils
  {
    /// <summary>
    /// Get the selection object representing the currently highlighted text in SM.
    /// </summary>
    /// <returns>IHTMLTxtRange object or null</returns>
    public static IHTMLTxtRange GetTextSelectionObject()
    {

      var ctrlGroup = Svc.SM.UI.ElementWdw.ControlGroup;
      var htmlCtrl = ctrlGroup?.FocusedControl?.AsHtml();
      var htmlDoc = htmlCtrl?.GetDocument();
      var sel = htmlDoc?.selection;

      if (!(sel?.createRange() is IHTMLTxtRange textSel))
        return null;

      return textSel;

    }

    public static IControlHtml GetFirstHtmlCtrl()
    {

      var ctrlGroup = Svc.SM.UI.ElementWdw.ControlGroup;
      return ctrlGroup?.GetFirstHtmlControl()?.AsHtml();

    }

    public static Dictionary<int, IControlHtml> GetHtmlCtrls()
    {

      var ret = new Dictionary<int, IControlHtml>();

      var ctrlGroup = Svc.SM.UI.ElementWdw.ControlGroup;
      if (ctrlGroup.IsNull())
        return ret;

      for (int i = 0; i < ctrlGroup.Count; i++)
      {
        var htmlCtrl = ctrlGroup[i].AsHtml();
        if (htmlCtrl.IsNull())
          continue;
        ret.Add(i, htmlCtrl);
      }

      return ret;

    }
  }
}
