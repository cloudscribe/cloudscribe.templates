# Cloudscribe VSIX Template Release Instructions

## Version Update Process

These instructions describe the steps to create a new release version of the cloudscribe VSIX template.
The examples use version 8.3.0 to 8.4.0, but for future releases, replace these with the appropriate version numbers (e.g., 8.4.0 to 8.5.0).

## Release Steps

### Step 0: Verify Release Notes (MANUAL - PROMPT USER)
**This is a manual step - Claude will prompt you to ensure this is done before proceeding**
- Ensure that release notes have been updated in this solution
- Check and update ReleaseNotes.md if needed
- Confirm any other relevant documentation has been updated for this version

### Step 1: Create Release Branch
Create a new git branch called `version_8.4_claude` based off develop, and publish that branch
```bash
git checkout develop
git pull origin develop
git checkout -b version_8.4_claude
git push -u origin version_8.4_claude  # May require manual push if auth fails
```

**Note:** If `git push` fails due to authentication in automated environments, the branch will be created locally and you'll need to manually push it using your Git credentials.

### Step 2: Update NuGet Package Version
In `cloudscribe.templates.nuspec` (line 5) increment the version from 8.3.0 to 8.4.0

### Step 3: Update VSIX Manifest Version
In `cloudscribeTemplate/source.extension.vsixmanifest` (line 4) increment the version from 8.3.0 to 8.4.0

### Step 4: Update CloudScribe Package References
In `Content/WebApp/WebApp.csproj` find every package reference that contains 'cloudscribe' within its name, and increment those ones ONLY from Version="8.3.*" to Version="8.4.*"

### Step 5: Build VSIX in Release Mode
Rebuild the cloudscribeTemplate project in Release mode. Check no build errors. Check that a new VSIX file is created (file will have today's creation date) at `\cloudscribeTemplate\bin\Release\cloudscribeTemplate.vsix`

**WSL/Linux Environment:**
```bash
# Use MSBuild with Windows path format (not WSL paths)
"/mnt/c/Program Files/Microsoft Visual Studio/2022/Professional/MSBuild/Current/Bin/MSBuild.exe" "D:\Development2\cloudscribe.templates\cloudscribeTemplate\cloudscribeTemplate.csproj" /p:Configuration=Release
```

**Windows Command Prompt:**
```cmd
# Standard MSBuild command
msbuild cloudscribeTemplate\cloudscribeTemplate.csproj /p:Configuration=Release
```

### Step 6: Archive VSIX File
Copy that VSIX file across into `\Archive` folder and rename it to `cloudscribeTemplate_8.4_net80_bs5_vs2022.vsix`

### Step 7: Commit Changes
Commit your git changes (just 3 files altered above - do not commit any of your own working md files)
```bash
git add cloudscribe.templates.nuspec
git add cloudscribeTemplate/source.extension.vsixmanifest
git add Content/WebApp/WebApp.csproj
git commit -m "Update version to 8.4.0"
```

**Note:** If git identity is not configured, you may need to set it:
```bash
git config user.name "Your Name"
git config user.email "your.email@example.com"
```

### Step 8: Create NuGet Package
Run `packtemplate.cmd` and verify that a new NuGet `\nupkgs\cloudscribe.templates.8.4.0.nupkg` is created

**WSL/Linux Environment:**
```bash
# Use nuget.exe directly (packtemplate.cmd may not work in WSL)
nuget.exe pack cloudscribe.templates.nuspec -OutputDirectory "nupkgs"
```

**Windows Command Prompt:**
```cmd
# Standard batch file
packtemplate.cmd
```

### Step 9: Create Pull Request
Create a git PR from your working release branch back to develop, but do not merge that PR

## For Future Releases

When creating a new release (e.g., 8.5.0), follow the same steps but replace:
- All occurrences of `8.3` with the current version
- All occurrences of `8.4` with the new version
- Branch name `version_8.4_claude` with `version_8.5_claude`
- Archive filename with appropriate version number

## Environment-Specific Notes

### WSL (Windows Subsystem for Linux)
- Use `dotnet.exe` instead of `dotnet` when dotnet commands are needed
- MSBuild requires Windows path format (e.g., `D:\path\to\file.csproj`) not WSL paths (`/mnt/d/path/to/file.csproj`)
- Use full path to MSBuild.exe: `"/mnt/c/Program Files/Microsoft Visual Studio/2022/Professional/MSBuild/Current/Bin/MSBuild.exe"`
- Use `nuget.exe` directly instead of packtemplate.cmd if the batch file doesn't execute properly

### Windows Command Prompt
- Standard commands work as documented (msbuild, packtemplate.cmd, etc.)
- Git authentication typically works seamlessly

## Troubleshooting

### Build Issues
- Ensure Visual Studio 2022 is installed with VSIX development tools
- Check that all NuGet packages restore properly before building
- Verify .NET SDK version is compatible

### Git Issues
- Authentication failures: Push branch manually with your credentials
- Identity not configured: Set git user.name and user.email as shown above

### Path Issues in WSL
- Always use Windows drive letter format (D:\...) for MSBuild commands
- WSL mount points (/mnt/d/...) work for file operations but not for MSBuild