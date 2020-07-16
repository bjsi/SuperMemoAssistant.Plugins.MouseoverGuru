using Forge.Forms.Annotations;
using Newtonsoft.Json;
using SuperMemoAssistant.Services.UI.Configuration;
using SuperMemoAssistant.Sys.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.MouseoverGuru
{

  [Form(Mode = DefaultFields.None)]
  [Title("Dictionary Settings",
     IsVisible = "{Env DialogHostContext}")]
  [DialogAction("cancel",
    "Cancel",
    IsCancel = true)]
  [DialogAction("save",
    "Save",
    IsDefault = true,
    Validates = true)]
  public class MouseoverGuruCfg : CfgBase<MouseoverGuruCfg>, INotifyPropertyChangedEx
  {
    [Title("Mouseover Guru Plugin")]

    [Heading("By Jamesb | Experimental Learning")]

    [Heading("Features:")]
    [Text(@"- Open previews of Piotr Wozniak's articles in popup windows")]

    [Heading("General Settings")]

    [Field(Name = "Scan SM-related articles for glossary terms?")]
    public bool ScanElements { get; set; } = true;

    [Field(Name = "Element Reference Regexes")]
    [MultiLine]
    public string ReferenceRegexes { get; set; } = @".*supermemo.*
.*super-memory.*
.*super-memo.*
.*supermemopedia.*
.*supermemory.*
.*incremental reading.*
.*incremental learning.*";

    [Field(Name = "Concept Regexes")]
    [MultiLine]
    public string ConceptRegexes { get; set; } = @"";

    [JsonIgnore]
    public bool IsChanged { get; set; }

    public override string ToString()
    {
      return "Mouseover Glossary Settings";
    }

    public event PropertyChangedEventHandler PropertyChanged;

  }
}
