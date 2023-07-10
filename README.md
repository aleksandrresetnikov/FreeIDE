# 📒 FreeIDE
### <strong>It is a simple source code editor.</strong>
![Image alt](https://github.com/aleksandrresetnikov/FreeIDE/blob/main/Screenshots/Screenshot_base(beta%20version).jpg)

## ⚡ Notiz
The application uses resources that are included in the Visual Studio solution but not included in the C Sharp project (.csproj file). For example: <strong><i>'.\FreeIDE\Source Resources\Themes\...'</i></strong> - this application's themes are stored here. When building a project <strong>(FreeIDE.Main)</strong>, a build event is triggered (see project properties) and copies all resources from the <strong>'Source Resources'</strong> folder to the application's output folder.

#### If the files are not uploaded to the output folder, then try this:
1. Rebuild the solution: Solution\Rebuild.
2. Make sure you select 'Debug' solution configuration.
3. If the above does not help, then run the 'BuildResources.bat' file in the root folder of the solution.

## 💿 Resources
Icons I took from Visual Studio 12. They are available <strong><a href="https://www.microsoft.com/en-us/download/details.aspx?id=35825">HERE</a></strong>.

## ✅ What I want to implement in the future
1. Implementation of syntax highlighting for several programming languages (settings will be stored in an .xml file).
2. Implementation of prompts for quick actions for different programming languages (quick actions will be stored in an .xml file).
3. Variable access level implementation (for example, in C#, variables that were declared within '{}' brackets will not be accessible outside of them).
4. Implementation of code hinting based on loaded libraries (using .NET reflection).
5. Adding multiple components (like in Visual Studio), the components will install as plugins (perhaps in Iron Python).
6. Implementing .NET Library Loading from NuGet.

## ⚠️ Important
I am using <strong><a href="https://github.com/PavelTorgashov/FastColoredTextBox">PavelTorgashov/FastColoredTextBox</a></strong> library in this project. Since this library is open source, I've made small changes to suit my needs (for example, recoloring many of the dialogue forms). I do not claim ownership of this project (FastColoredTextBox). Project copyright reserved.

## 📰 Information and news
Information and news of the development of the project, as well as detailed information, history and previous versions can be found in the telegram channel of this project: https://t.me/+3500S76nXz1jNTRi
