# EnhancedStops

EnhancedStops is a plugin for the LSPD First Response modification by G17 Media and it's contributors. This plugin adds some menus (which players can access during several conditions) to it, and adds several useful actions that can be accessed via these menus.

The plugin intends to implement features that are somewhat essential to simulate semi-realistic expierence during stops, arrested and idle, without replacing nor removing any of LSPDFR's own features. 

## Installing

You can find existing releases on Releases page or (maybe if I uploaded it to, not yet at the time of writing) the LCPDFR.com download page.

Or, you can build it from source:

1. **Clone this repository.**
   
   Normally, you can use the following code:
   
   `git clone https://gitlab.com/WithLithum/enhancedstops.git`
   
   This command uses HTTPS and does not require an account on GitLab.

2. **Open Visual Studio 2022 or any later version.**
   
   This project uses format 17 so VS2019 is not supported.
   
   You can also try use command line or build tools, which you can navigate to the folder where you cloned the repo and just type `msbuild`.
   
   Since this is a framework project do not use `dotnet build`.

3. **Restore NuGet packages and copy LSPDFR dll to References folder.**
   
   LSPDFR's dll file will be located at `plugins` folder under your game directory, it's called `LSPD First Response.dll`.
   
   I also recommend that you put XML documentation file into the references folder.

4. **Select BUILD -> Build Solution.**
   
   This will build the EnhancedStops project.

5. **Copy files from the bin folder to plugins\LSPDFR.**
   
   Located inside the second EnhancedStops folder which contains project files. You can also use `Open current bin folder` if you have sufficient extension installed.

6. **Done. Enjoy!**

## Contributing

Same as installing from source above, clone and build repo. Make your desired changes. Submit Merge Request.

There are things to notice:

* Please follow GitLab related guidelines to make things easy. If you mess something up I am not gonna approve your MR.

* Do not make changes to solution file and project file besides adding new files to the project and some neccessary work to make the project run.

* Do not add new references unless approved.

* Some code styles must be followed:
  
  * Add an underscore (_) before private fields.
  
  * Use Allman bracing, with four spaces indenting.

## License

This project is licensed under GNU GPL 3.0. You may choose any later version of GNU GPL desired to apply.

There's a copy of the full text of the GNU GPL version 3 in the [LICENSE file](LICENSE).
