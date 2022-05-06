using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain;
using Nop.Services.Common;
using Nop.Services.Themes;
using Nop.Web.Framework.Themes;

namespace Themeabay.Nop.Plugin.Switcha
{
    public sealed class SwitchaThemeContext : ThemeContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IThemeProvider _themeProvider;

        public SwitchaThemeContext(IHttpContextAccessor httpContextAccessor,
           IGenericAttributeService genericAttributeService,
           IStoreContext storeContext,
           IThemeProvider themeProvider,
           IWorkContext workContext,
           StoreInformationSettings storeInformationSettings) :
            base(genericAttributeService, storeContext, themeProvider, workContext, storeInformationSettings)
        {
            _themeProvider = themeProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<string> GetWorkingThemeNameAsync()
        {
            const string themePreviewQueryName = "theme_preview";
            var themeName = _httpContextAccessor.HttpContext.Request.Query[themePreviewQueryName].FirstOrDefault();
            if (!string.IsNullOrEmpty(themeName))
            {
                var availableThemes = await _themeProvider.GetThemesAsync();
                if (availableThemes.Where(k => k.SystemName == themeName).Any())
                {
                    return themeName;
                }
            }
            return await base.GetWorkingThemeNameAsync();
        }
    }
}