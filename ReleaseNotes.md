## Release Notes

### version 8.5.0 - September 2025

#### **Major Licensing Change**

##### **Commercial Components Now Free**
- **Announcement**: All cloudscribe commercial components are now available for free use
- **Components Included**:
  - TalkAbout Commenting System
  - TalkAbout Forums
  - Membership Paywall
  - Newsletter Management
  - Forms and Surveys
  - Stripe Payment Integration
- **Impact**: License key requirements have been completely removed for all commercial components
- **Source Code**: Components remain proprietary (subject to potential future open-sourcing)
- **Benefit**: Full cloudscribe ecosystem now accessible without licensing barriers

#### **New Features**

- **[#1102](https://github.com/cloudscribe/cloudscribe/issues/1102)**: Admin Application Restart - added capability for administrators to restart the application directly from the cloudscribe admin interface. Eliminates need for direct server access when application restart is required. Controlled via appsettings.json configuration boolean for security.

- **[#1208](https://github.com/cloudscribe/cloudscribe/issues/1208)**: Enhanced Summernote Editor - Element Path Display - new element path breadcrumb display showing current cursor position in DOM hierarchy (similar to CKEditor). Real-time updates with clickable breadcrumbs for easy navigation. Custom Summernote plugin with comprehensive HTML5 tag support.
- **[#1209](https://github.com/cloudscribe/cloudscribe/issues/1209)**: Enhanced Summernote Editor - Improved Link Behavior - hyperlinks no longer open in new windows by default. Controlled via `linkTargetBlank: false` in summernote-config.json.

- **[cloudscribe.Syndication #7](https://github.com/cloudscribe/cloudscribe.Syndication/issues/7)**: RSS Feed Styling Support - added ability to style RSS feeds with custom CSS stylesheets. Support for XML stylesheet meta tags in RSS feeds. Automatic XSL and CSS file deployment with user override protection. RSS feeds can now match site branding and provide better user experience. New documentation available at https://www.cloudscribe.com/cloudscribesyndication.

#### **Enhancements**

- **[#1204](https://github.com/cloudscribe/cloudscribe/issues/1204)**: Enhanced Auto-Logout System - resolved session timeout issues for users actively using JavaScript API endpoints. Features include server-side middleware for intelligent session activity tracking, client-side JavaScript for cross-tab session management, and configurable timeout thresholds. Prevents unexpected logouts during active user workflows while maintaining security.

- **[#698](https://github.com/cloudscribe/cloudscribe/issues/698)**: System Information Improvements - updated System Information page to include previously missing packages. Added compiled views, static files, integration packages, and Bootstrap components. Removed duplicate "cloudscribe.Email.Templating.Web" entry. Improved visibility for troubleshooting and support scenarios.

#### **Bug Fixes**

- **[#1205](https://github.com/cloudscribe/cloudscribe/issues/1205)**: IdentityServer4 Support Resolution - resolved token creation issues caused by dependency version conflicts. Root cause was System.IdentityModel.Tokens.Jwt version 8.2.* breaking changes. Updated dependency chain management and explicit package references. Restored proper JWT signature validation and metadata endpoint functionality.

- **[cloudscribe.Messaging #13](https://github.com/GreatHouseBarn/cloudscribe.Messaging/issues/13)**: Email Queue Background Task Exception Handling - resolved cancellation exception thrown during app pool recycling. Fixed "A task was canceled" error in EmailQueueBackgroundTask.ExecuteAsync. Improved cancellation token handling in background services. Eliminates log noise during normal application lifecycle events.

---



### version 8.4.0 - August 2025

#### **Breaking Changes**
- **[#748](https://github.com/cloudscribe/cloudscribe/issues/748)**: Enhanced Cookie Consent System - implemented a more sophisticated cookie consent system allowing users to dismiss cookie banners without fully accepting cookies. **Breaking change** for sites with local view overrides - two key files were modified in the Bootstrap5 template. See the [documentation](https://www.cloudscribe.com/managing-cookie-consent).

#### **Security Improvements**
- **[#1054](https://github.com/cloudscribe/cloudscribe/issues/1054)**: EntityFramework Dependencies - updated Microsoft.EntityFrameworkCore dependencies to address security vulnerabilities in transitive dependencies.
- **[#1113](https://github.com/cloudscribe/cloudscribe/issues/1113)**: jQuery Validate Update - updated jQuery.validate library to address security vulnerability discovered during penetration testing.
- **[#1125](https://github.com/cloudscribe/cloudscribe/issues/1125)**: Cookie Security - improved cookie security configuration by addressing SameSite cookie settings for better protection.

#### **Bug Fixes**
- **[#1150](https://github.com/cloudscribe/cloudscribe/issues/1150)**: Summernote Editor - fixed issue where HTML code entered in raw HTML view was not retained when saving unless user switched back to WYSIWYG mode first.
- **[#1177](https://github.com/cloudscribe/cloudscribe/issues/1177)**: API Authentication - fixed EnforceSiteRulesMiddleware bug that incorrectly started HTTP responses for Terms & Conditions violations on API routes, causing authentication errors.
- **[#1169](https://github.com/cloudscribe/cloudscribe/issues/1169)**: IP Address Blocking - re-wrote IP address blocking/permitting logic to ensure "Permitted" rules consistently take precedence over "Blocked" rules when dealing with IP address ranges.
- **[#500](https://github.com/cloudscribe/cloudscribe.SimpleContent/issues/500)**: Page Manager UI - fixed scrolling issue in SimpleContent Page Manager where selecting a page would auto-scroll to top with misplaced context menu.

#### **UI/UX Improvements**
- **[#1157](https://github.com/cloudscribe/cloudscribe/issues/1157)**: Summernote Theme Consistency - added CSS override to ensure Summernote editor maintains consistent black-on-white appearance across different themes.
- **[#639](https://github.com/cloudscribe/cloudscribe.SimpleContent/issues/639)**: SimpleContent Layout - moved page metadata below child page menu for improved layout and user experience.
- **[#501](https://github.com/cloudscribe/cloudscribe.SimpleContent/issues/501)**: Access Control - improved unauthorized access handling in SimpleContent with proper redirects to login/access denied pages instead of generic 404 errors.
- **[#1134](https://github.com/cloudscribe/cloudscribe/issues/1134)**: Registration Settings - modified user interface to clarify the purpose of a confusing checkbox on "/siteadmin/registerpageinfo" page which has no backing database field.

#### **Developer Tools & Features**
- **[#482](https://github.com/cloudscribe/cloudscribe.SimpleContent/issues/482)**: CSP-Compliant Scripts - added mechanism in Developer Tools to allow adding JavaScript directly to pages with Content Security Policy compliance through tag helpers.
- **[#1182](https://github.com/cloudscribe/cloudscribe/issues/1182)**: Localization Improvements - rationalized ResX string references throughout core views, standardizing localization with consistent snake_case resource keys.
- **[#1194](https://github.com/cloudscribe/cloudscribe/issues/1194)** & **[#45](https://github.com/cloudscribe/cloudscribe.UserProperties.Kvp/issues/45)**: User Data Cleanup - added event handler for post-user deletion to clean up remaining Key-Value Pair (KVP) data from deleted users.
- **[#55](https://github.com/cloudscribe/cloudscribe.UserProperties.Kvp/issues/55)**: User Export Compatibility - added conditional UserExportPartial to KVP views to ensure compatibility with different versions of cloudscribe.Core.CompiledViews.Bootstrap5 by using view engine checks.

#### **Code Cleanup**
- **[#1138](https://github.com/cloudscribe/cloudscribe/issues/1138)** & **[#1160](https://github.com/cloudscribe/cloudscribe/issues/1160)**: Legacy Removal - removed outdated Bootstrap3 and deprecated .pgsql libraries across cloudscribe solutions to improve maintainability.
- **[#1163](https://github.com/cloudscribe/cloudscribe/issues/1163)**: Test Infrastructure - fixed and reorganized unit test infrastructure with new working tests.

### version 8.3.0 - July 2025

#### **@cloudscribe/cloudscribe**
- **[#1099](https://github.com/cloudscribe/cloudscribe/issues/1099)**: Summernote Editor Integration - added support for the Summernote editor as a replacement for CKEditor, while retaining the option to use CKEditor if desired.
- **[#1063](https://github.com/cloudscribe/cloudscribe/issues/1063)**: Fixed several issues in the "browse server" modal when invoked from the Summernote toolbar:
  
  - Restored the missing 'Select' button for image selection.
  - Reinstated the 'Crop' tab in the UI.
  - Corrected the modal title.
  - Addressed regressions caused by previous file manager and Summernote integration changes.
- **[#1111](https://github.com/cloudscribe/cloudscribe/issues/1111)**: Fixed newsletter sign-up widget compatibility with invisible reCAPTCHA:
  
  - Resolved an issue where the newsletter sign-up widget would not submit when invisible reCAPTCHA was enabled.
  - Improved JavaScript handling in `EmailListSignUpPartial` to support async validation and proper script loading.
  - Ensured compatibility for both authenticated and unauthenticated users.
- **[#918](https://github.com/cloudscribe/cloudscribe/issues/918)**: Added ability to block specific IP addresses via the admin UI.
- **[#1011](https://github.com/cloudscribe/cloudscribe/issues/1011)**: Added support to restrict site access to only permitted IP addresses, with support for both single IPs and IP ranges.
- **[#1097](https://github.com/cloudscribe/cloudscribe/issues/1097)**: Fixed an issue where saving API client secret expiry dates in PostgreSQL could fail due to UTC handling, which previously caused client and related data to be deleted. Saving and updating API clients is now reliable on PGSQL.



### version 8.1 - 2 May 2025


#### **@cloudscribe/cloudscribe.Web.Navigation**
- **[#135](https://github.com/cloudscribe/cloudscribe.Web.Navigation/issues/135)**: Fixed an issue causing incorrect child page sub-menus when the URL ends with a trailing slash. This ensures consistent navigation tree behavior across routes with or without trailing slashes.

---

#### **@cloudscribe/cloudscribe**
- **[#1073](https://github.com/cloudscribe/cloudscribe/issues/1073)**: Introduced a reserved word list for tenant site names to prevent conflicts. For example, "development" is now restricted to avoid issues with accessing developer sections.
- **[#1063](https://github.com/cloudscribe/cloudscribe/issues/1063)**: Added a feature to export user data from the User Manager, including basic attributes such as First Name, Surname, Display Name, Email Address, and Created Date.
- **[#798](https://github.com/cloudscribe/cloudscribe/issues/798)**: Enhanced the file manager with the ability to move files between folders, eliminating the need to delete and re-upload files.
- **[#762](https://github.com/cloudscribe/cloudscribe/issues/762)**: Updated the main heading on `/privacy` and other cloudscribe core pages to use `<h1>` tags or allow configurability, replacing the previous `<h2>` implementation.
- **[#942](https://github.com/cloudscribe/cloudscribe/issues/942)**: Extended reCAPTCHA protection to the "Forgot Your Password?" page to mitigate risks associated with automated email spam and enhance security.
- **[#935](https://github.com/cloudscribe/cloudscribe/issues/935)**: Resolved an issue where the `returnurl` parameter was ignored on the login page if the user was already logged in. Now, the intended return behavior is consistently applied.
- **[#929](https://github.com/cloudscribe/cloudscribe/issues/929)**: Improved the user experience for sites with social sign-in by hiding password management options (`/manage/setpassword` and `/manage/changepassword`) when database authentication is disabled.
- **[#924](https://github.com/cloudscribe/cloudscribe/issues/924)**: Fixed a UI issue in the Page Manager tree where togglers were inheriting an underline style and made them larger and easier to click.
- **[#959](https://github.com/cloudscribe/cloudscribe/issues/959)**: Fixed a crash when saving user details with a date of birth in PostgreSQL. Ensured compatibility with `timestamp with time zone` by enforcing UTC for `DateTime` values.
- **[#1056](https://github.com/cloudscribe/cloudscribe/issues/1056)**: Corrected the behavior of the "Clear Image" button to ensure it functions as expected after dragging and dropping an image.
- **[#1034](https://github.com/cloudscribe/cloudscribe/issues/1034)**: Added a "View Activity" link on the Manage User page to improve navigation to the User Activity page.

---

#### **@cloudscribe/cloudscribe.SimpleContent**
- **[#487](https://github.com/cloudscribe/cloudscribe.SimpleContent/issues/487)**: Added options in CMS settings to display created and last edited information (author and timestamps) below content. These settings include configurable defaults.
- **[#549](https://github.com/cloudscribe/cloudscribe.SimpleContent/issues/549)**: Updated the `LinksRenderPartial` to align with Bootstrap 5 standards. Replaced `<h2>` and float-based styling with semantic markup and modern flex layouts.

---