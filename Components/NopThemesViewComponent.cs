using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain;
using Nop.Services.Security;
using Nop.Services.Themes;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Themes;
using Themeabay.Nop.Plugin.Switcha.Models;

namespace Themeabay.Nop.Plugin.Switcha.Components
{
    [ViewComponent(Name = "WidgetsNopThemesSelector")]
    public class NopThemesSelectorViewComponent : NopViewComponent
    {
        private readonly IPermissionService _permissionService;
        private readonly IThemeProvider _themeProvider;
        private readonly IThemeContext _themeContext;
        private readonly StoreInformationSettings _storeInformationSettings;

        public NopThemesSelectorViewComponent(IThemeContext themeContext,
            IThemeProvider themeProvider,
            IPermissionService permissionService,
            StoreInformationSettings storeInformationSettings)
        {
            _themeProvider = themeProvider;
            _themeContext = themeContext;
            _storeInformationSettings = storeInformationSettings;
            _permissionService = permissionService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            // only admin user can see this button.
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.AccessAdminPanel))
            {
                return Content(string.Empty);
            }

            var workingThemeName = await _themeContext.GetWorkingThemeNameAsync();
            if (string.IsNullOrEmpty(workingThemeName))
            {
                workingThemeName = _storeInformationSettings.DefaultStoreTheme;
            }
            var model = (await _themeProvider.GetThemesAsync()).Select(k => new ThemeModel()
            {
                FriendlyName = k.FriendlyName,
                SystemName = k.SystemName,
                PreviewImageUrl = k.PreviewImageUrl,
                IsSelected = k.SystemName == workingThemeName
            }).ToList();
            // no any theme is matched
            if (!model.Any(k => k.IsSelected))
            {
                var theme = model.Where(k => k.SystemName == _storeInformationSettings.DefaultStoreTheme).First();
                theme.IsSelected = true;
            }
            return View("~/Plugins/Themeabay.Nop.Plugin.Switcha/Views/NopThemesSelector.cshtml", model);
        }
    }
}