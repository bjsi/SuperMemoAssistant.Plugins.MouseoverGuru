using Anotar.Serilog;
using HtmlAgilityPack;
using MouseoverPopupInterfaces;
using PluginManager.Interop.Sys;
using SuperMemoAssistant.Interop.SuperMemo.Elements.Builders;
using SuperMemoAssistant.Sys.Remoting;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SuperMemoAssistant.Plugins.MouseoverGuru
{
  public class ContentService : PerpetualMarshalByRefObject, IMouseoverContentProvider

  {

    private readonly HttpClient _httpClient;

    public ContentService()
    {
      _httpClient = new HttpClient();
      _httpClient.DefaultRequestHeaders.Accept.Clear();
      _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void Dispose()
    {
      _httpClient?.Dispose();
    }

    public RemoteTask<PopupContent> FetchHtml(RemoteCancellationToken ct, string url)
    {
      try
      {

        if (url.IsNullOrEmpty())
          return null;

        var regexArr = new string[] { UrlUtils.GuruRegex, UrlUtils.HelpRegex, UrlUtils.MemopediaRegex };
        if (!regexArr.Any(x => new Regex(x).Match(url).Success))
          return null;

        return GetArticleExtract(ct, url);

      }
      catch (Exception ex)
      {
        LogTo.Error($"Failed to FetchHtml for url {url} with exception {ex}");
        throw;
      }
    }

    private async Task<PopupContent> GetArticleExtract(RemoteCancellationToken ct, string url)
    {
      string response = await GetAsync(ct.Token(), url).ConfigureAwait(false);
      return CreatePopupContent(response, url);
    }

    private PopupContent CreatePopupContent(string content, string url)
    {

      if (content.IsNullOrEmpty() || url.IsNullOrEmpty())
        return null;

      var doc = new HtmlDocument();
      doc.LoadHtml(content);

      var firstHeading = doc.DocumentNode.SelectSingleNode("//h1[@id='firstHeading']");
      if (firstHeading.IsNull())
        return null;

      var body = doc.DocumentNode.SelectSingleNode("//div[@id='mw-content-text']");
      if (body.IsNull())
        return null;

      string extract = firstHeading.OuterHtml;
      var cur = body.FirstChild;
      int mwHeadlineCt = 0;

      while (cur != null)
      {
        var classes = cur.GetClasses();
        if (classes.Any(x => x == "mw-headline")
            || cur.ChildNodes.Any(c => c.GetClasses().Any(x => x == "mw-headline")))
        {
          cur = cur.NextSibling;
          mwHeadlineCt++;
        }

        if (classes.Any(x => x == "printfooter"))
          break;

        if (mwHeadlineCt == 2)
          break;

        extract += cur.OuterHtml;
        cur = cur.NextSibling;
      }

      var extractDoc = new HtmlDocument();
      extractDoc.LoadHtml(extract);
      var toc = extractDoc.DocumentNode.SelectSingleNode("//div[@id='toc']");
      if (toc != null)
        toc.Remove();

      extract = extractDoc.DocumentNode.OuterHtml;

      var titleNode = doc.GetElementbyId("firstHeading");
      string title = titleNode?.InnerText;

      if (extract.IsNullOrEmpty() || title.IsNullOrEmpty())
        return null;

      string html = @"
          <html>
            <body>
              {0}
            </body>
          </html>";

      html = String.Format(html, extract);

      var refs = new References();
      refs.Author = "Piotr Wozniak";
      refs.Link = url;
      refs.Source = "SuperMemo Guru";
      refs.Title = title;

      return new PopupContent(refs, html, true, url);

    }

    private async Task<string> GetAsync(CancellationToken ct, string url)
    {
      HttpResponseMessage responseMsg = null;

      try
      {
        responseMsg = await _httpClient.GetAsync(url, ct).ConfigureAwait(false);

        if (responseMsg.IsSuccessStatusCode)
        {
          return await responseMsg.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        else
        {
          return null;
        }
      }
      catch (HttpRequestException)
      {
        if (responseMsg != null && responseMsg.StatusCode == System.Net.HttpStatusCode.NotFound)
          return null;
        else
          throw;
      }
      catch (OperationCanceledException)
      {
        return null;
      }
      finally
      {
        responseMsg?.Dispose();
      }
    }
  }
}
