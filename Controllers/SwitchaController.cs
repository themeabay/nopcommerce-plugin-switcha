using System;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Nop.Core.Domain;
using Nop.Services.Themes;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Themeabay.Nop.Plugin.Switcha.Controllers
{
    public class SwitchaController : BasePluginController
    {

        [AuthorizeAdmin]
        public IActionResult Preview(string theme, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/";
            }
            Uri.TryCreate(new Uri(Request.GetDisplayUrl()), returnUrl, out Uri uri);
            var queryParams = QueryHelpers.ParseQuery(uri.Query);
            queryParams["theme_preview"] = theme;
            var queryStrings = string.Join("&", queryParams.Select(k => k.Key + "=" + k.Value.First()));
            returnUrl = uri.AbsolutePath;
            if (!string.IsNullOrEmpty(queryStrings))
            {
                returnUrl += "?" + queryStrings;
            }
            return Redirect(returnUrl);
        }
    }
}