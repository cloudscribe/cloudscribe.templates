## Release Notes

### version 8.2.0 - July 2025

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