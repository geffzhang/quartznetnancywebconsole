#region

using Nancy;
using Nancy.Helpers;
using Nancy.ViewEngines.Razor;

#endregion

namespace QuartzNet.WebConsole.Views
{
    public static class HtmlHelpers
    {
        public static string HtmlAttribute(this string str)
        {
            return HttpUtility.HtmlAttributeEncode(str);
        }

        public static string HtmlEncode(this string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        public static string IsActive<TModel>(this UrlHelpers<TModel> urlHelpers, string url, string activeClass)
        {
            if (urlHelpers.Content(url) == urlHelpers.RenderContext.Context.Request.Url.Path)
                return activeClass;
            return string.Empty;
        }
    }
}