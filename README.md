# 📒 FreeIDE
<strong>It is a simple source code editor.</strong>

## ⚡ Notiz
The application uses resources that are included in the Visual Studio solution but not included in the C Sharp project (.csproj file). For example: <strong><i>'.\FreeIDE\Source Resources\Themes\...'</i></strong> - this application's themes are stored here. When building a project <strong>(FreeIDE.Main)</strong>, a build event is triggered (see project properties) and copies all resources from the <strong>'Source Resources'</strong> folder to the application's output folder.

#### If the files are not uploaded to the output folder, then try this:
1. Rebuild the solution: Solution\Rebuild.
2. Make sure you select 'Debug' solution configuration.
3. If the above does not help, then run the 'BuildResources.bat' file in the root folder of the solution.

## ⚠️ Important
I am using <strong><a href="https://github.com/PavelTorgashov/FastColoredTextBox">PavelTorgashov/FastColoredTextBox</a></strong> library in this project. Since this library is open source, I've made small changes to suit my needs (for example, recoloring many of the dialogue forms). I do not claim ownership of this project (FastColoredTextBox). Project copyright reserved.
