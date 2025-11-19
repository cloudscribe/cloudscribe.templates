## Release Notes

### version 8.6.0 - November 2025

#### **Breaking Changes**

- **[cloudscribe.TalkAbout #85](https://github.com/GreatHouseBarn/cloudscribe.TalkAbout/issues/85) & [#90](https://github.com/GreatHouseBarn/cloudscribe.TalkAbout/issues/90)**: Comments System Summernote Editor - replaced Markdown editor with Summernote HTML editor in commenting system. **Breaking change** requires manual updates to local partial view overrides (CommentWrapperPartial.cshtml, CommentScriptsPartial.cshtml, CommentStylePartial.cshtml) and appsettings.json configuration. Legacy Markdown comments are preserved and automatically converted to HTML on first edit (one-way migration). CommentThread table now central to comment organization. Bootstrap4 views deprecated. Fixed PostgreSQL/MySQL/SQLite migration issues. Expect approximately half-day manual work for sites with custom comment view overrides. See upgrade documentation for detailed partial view changes required.

#### **Security Improvements**

- **[cloudscribe.TalkAbout #98](https://github.com/GreatHouseBarn/cloudscribe.TalkAbout/issues/98)**: Server-Side Posting Protection - added server-side enforcement of configuration parameters to prevent unauthorized comment and forum posting. Ensures anonymous users cannot bypass client-side restrictions to post when anonymous posting is disabled. Includes unit tests for validation logic. Reviewed EF Core usage to ensure protection against SQL injection in posted comment data.

#### **New Features**

- **[cloudscribe.Commerce #82](https://github.com/GreatHouseBarn/cloudscribe.Commerce/issues/82)**: Forms & Surveys reCAPTCHA Support - added reCAPTCHA validation to Forms & Surveys system. Each form can be configured to require reCAPTCHA for unauthenticated users, respecting site-wide cloudscribe Core reCAPTCHA settings for both visible and invisible modes. Addresses spam prevention in public-facing forms.
- **[cloudscribe.TalkAbout #95](https://github.com/GreatHouseBarn/cloudscribe.TalkAbout/issues/95)**: Email Comment Authors - added ability for moderators to email users directly from the comment administration page. New "Email user" button in `/talkadmin/administercomments` allows moderators to compose and send plain-text emails to comment authors. Includes contextual link back to the original comment page (when approved). Respects per-project moderator authorization policies. Fully localized with new ResX strings.
- **[#921](https://github.com/cloudscribe/cloudscribe/issues/921)**: Role Copying with Authorization Policies - added ability to copy roles in role management. When copying a role, any dynamic authorization policies referencing the original role are automatically updated to also reference the new role. User specifies new role name during copy operation. New role starts empty (no users). Excludes system "Administrators" role from copying. Works seamlessly with or without dynamic authorization policies installed.

#### **Enhancements**

- **[cloudscribe.TalkAbout #75](https://github.com/GreatHouseBarn/cloudscribe.TalkAbout/issues/75)**: Visible reCAPTCHA Support - added support for visible/checkbox reCAPTCHA in the TalkAbout commenting system. Previously only invisible reCAPTCHA was supported with hardcoded implementation. Now respects cloudscribe Core reCAPTCHA settings and supports both visible and invisible modes, matching the behavior of the core login system.
- **[cloudscribe.TalkAbout #79](https://github.com/GreatHouseBarn/cloudscribe.TalkAbout/issues/79)**: Forum Visible reCAPTCHA Support - added support for visible/checkbox reCAPTCHA in the TalkAbout forums system. Forums no longer hard-code invisible reCAPTCHA and now respect cloudscribe Core reCAPTCHA settings for both visible and invisible modes.
- **[#1243](https://github.com/cloudscribe/cloudscribe/issues/1243)**: IP Address Restrictions Authorization - added dedicated `IPAddressRestrictionPolicy` to protect IP address restriction management endpoints. Updated navigation configuration to use `AdminMenuPolicy` instead of `AdminPolicy` for IP restriction admin menu items. Tested compatibility with template systems without dynamic authorization policies installed.
- **[#1241](https://github.com/cloudscribe/cloudscribe/issues/1241)**: IP Address Restrictions Configuration - added ability to enable or disable IP address restriction feature via configuration. New `SiteConfigOptions.EnableIpAddressRestrictions` setting in appsettings.json allows administrators to disable the feature when not needed. Defaults to enabled (true) for backward compatibility.

#### **Bug Fixes**

- **[cloudscribe.TalkAbout #74](https://github.com/GreatHouseBarn/cloudscribe.TalkAbout/issues/74)**: reCAPTCHA Validation - fixed missing server-side reCAPTCHA verification when anonymous users submit comments. Ensures proper validation to prevent spam attempts. Also resolved race condition issues in reCAPTCHA initialization.
- **[cloudscribe.TalkAbout #80](https://github.com/GreatHouseBarn/cloudscribe.TalkAbout/issues/80)**: Forum reCAPTCHA Validation - fixed missing server-side reCAPTCHA verification for anonymous forum posts. Analogous fix to comment system to ensure proper spam prevention.
- **[cloudscribe.TalkAbout #67](https://github.com/GreatHouseBarn/cloudscribe.TalkAbout/issues/67)**: Duplicate CommentSystemSettings Records - fixed issue where duplicate rows were incorrectly created in csta_CommentSystemSettings table. Resolved confusion between Id, ProjectId, and TenantId fields where lookups now consistently use TenantId. Prevents creation of hundreds/thousands of duplicate configuration rows while preserving existing comment data.
- **[cloudscribe.Messaging #81](https://github.com/GreatHouseBarn/cloudscribe.Messaging/issues/81)**: Newsletter Sign-up Widget reCAPTCHA - fixed hard-coded invisible reCAPTCHA in newsletter sign-up widget. Widget now respects cloudscribe Core settings for both visible and invisible reCAPTCHA modes.
- **[cloudscribe.dynamic-authorization-policy #30](https://github.com/cloudscribe/dynamic-authorization-policy/issues/30)**: Role Removal from Policies - fixed UI bug where roles could not be reliably removed from authorization policies. Resolved indexing issue that caused sporadic failures when de-selecting roles and saving policy changes.
- **[#1245](https://github.com/cloudscribe/cloudscribe/issues/1245)**: IP Address Restrictions Multi-Tenancy - fixed critical bug where first tenant's IP restrictions would incorrectly apply to all tenants due to cache key missing tenant ID component. Also resolved thread locking issue caused by synchronous data access in constructor. Service changed from Transient to Scoped registration. IP restrictions now work independently per tenant.
- **[#1197](https://github.com/cloudscribe/cloudscribe/issues/1197)**: IP Restriction UI Address Display - fixed incorrect IP address display in IP restriction admin UI. Previously used historical login data from cs_user_location table which could be outdated or wrong when users switch devices, VPNs, or have dynamic IP changes. Now retrieves current IP address directly from HTTPContext for accurate real-time display.

#### **UI/UX Improvements**

- **[#1058](https://github.com/cloudscribe/cloudscribe/issues/1058)**: User Display Name Editing - added ability for users to edit their display name on the `/manage/userinfo` page. Previously users could only edit first and last name, but display name (used throughout the system including TalkAbout comments) was only editable by administrators. Includes uniqueness enforcement per tenant, character validation with international character support, and HTML sanitization.

#### **Developer Tools & Features**

- **[cloudscribe.dynamic-authorization-policy #46](https://github.com/cloudscribe/dynamic-authorization-policy/issues/46)**: Policy Definition Documentation - clarified the relationship between Roles and Claims in policy definitions. Documentation now explicitly states that Roles use OR logic (user needs ANY role), Claims use AND logic (user needs ALL claims), and when both are specified, users must satisfy both requirements (be in ANY role AND have ALL claims).
- **[#1231](https://github.com/cloudscribe/cloudscribe/issues/1231)**: IdentityServer Integration Tests - added comprehensive integration tests for IdentityServer4 authentication and authorization. Tests cover client credentials grant type flow, JWT token validation, and role-based authorization. Updated to use modern Microsoft.AspNetCore.Authentication.JwtBearer (v8.x) library instead of deprecated IdentityServer4.AccessTokenValidation (v3). Includes published test harness page for live validation testing.

---



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