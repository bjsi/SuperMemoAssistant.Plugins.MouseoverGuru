#region License & Metadata

// The MIT License (MIT)
// 
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
// 
// 
// Created On:   7/16/2020 2:07:59 AM
// Modified By:  james

#endregion




namespace SuperMemoAssistant.Plugins.MouseoverGuru
{
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;
  using System.Runtime.Remoting;
  using Anotar.Serilog;
  using MouseoverPopup.Interop;
  using SuperMemoAssistant.Interop.SuperMemo.Core;
  using SuperMemoAssistant.Services;
  using SuperMemoAssistant.Services.IO.HotKeys;
  using SuperMemoAssistant.Services.Sentry;
  using SuperMemoAssistant.Services.UI.Configuration;
  using SuperMemoAssistant.Sys.Remoting;

  // ReSharper disable once UnusedMember.Global
  // ReSharper disable once ClassNeverInstantiated.Global
  [SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
  public class MouseoverGuruPlugin : SentrySMAPluginBase<MouseoverGuruPlugin>
  {
    #region Constructors

    /// <inheritdoc />
    public MouseoverGuruPlugin() : base("Enter your Sentry.io api key (strongly recommended)") { }

    #endregion


    #region Properties Impl - Public

    /// <inheritdoc />
    public override string Name => "MouseoverGuru";

    /// <inheritdoc />
    public override bool HasSettings => true;
    public MouseoverGuruCfg Config;
    private const string ProviderName = "SuperMemo Articles";
    private IContentProvider _contentProvider { get; set; }

    #endregion


    #region Methods Impl

    /// <inheritdoc />
    protected override void PluginInit()
    {

      LoadConfig();

      _contentProvider = new ContentService();

      if (!this.RegisterProvider(ProviderName, new List<string> { UrlUtils.GuruRegex, UrlUtils.HelpRegex, UrlUtils.MemopediaRegex }, _contentProvider))
      {
        LogTo.Error($"Failed to Register provider {ProviderName} with MouseoverPopup Service");
        return;
      }
      LogTo.Debug($"Successfully registered provider {ProviderName} with MouseoverPopup Service");


      // Svc.SM.UI.ElementWdw.OnElementChanged += new ActionProxy<SMDisplayedElementChangedEventArgs>(OnElementChanged);
    }

    /// <inheritdoc />
    public override void ShowSettings()
    {
      ConfigurationWindow.ShowAndActivate(HotKeyManager.Instance, Config);
    }

    public void LoadConfig()
    {
      Config = Svc.Configuration.Load<MouseoverGuruCfg>() ?? new MouseoverGuruCfg();
    }

    #endregion


    #region Methods

    [LogToErrorOnException]
    public void OnElementChanged(SMDisplayedElementChangedEventArgs e)
    {
      try
      {
      }
      catch (RemotingException) { }
    }

    #endregion
  }
}
