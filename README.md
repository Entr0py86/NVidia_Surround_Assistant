# NVidia Surround Assistant

Like your desktop in extended mode but like playing most of your games in 1x3 surround mode or maybe you want to play only on one screen and disable the others? 

Then NVidia Surround Assistant (NVSA) could help you. NVSA will detect new/destroyed processes via two methods, hooks and wmi, and switch to a pre-configured NVidia surround/grid profile of your choosing. It uses IGDB api to query there database

## Getting Started

There are [setup files](https://github.com/Entr0py86/NVidia_Surround_Assistant/releases) in the repository that can be used to install the application.
I have unfortunately never tested the x86 version  of the build, as I no longer run Win x86 on any of my machines. Please create an issue and I will attempt to assist you as long as you are willing to help test.

### Installing

1. Run the installer
2. Run NVidia Surround Assistant.exe
3. Follow the message box instructions.
4. Add applications to your detection list.
5. Profit!

## Built With

* [IGDB.API](https://www.nuget.org/packages/IGDB.API/1.0.8/)
* [Json .NET](https://www.newtonsoft.com/json)
* [Task Scheduler](https://github.com/dahall/taskscheduler)

## Versioning

I used [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/Entr0py86/NVidia_Surround_Assitant/tags). 

## Development

I will support this project as much as possible, life and other projects permitting.

Any requests, improvements, criticism etc are appreciated, but please be friendly and remember I do this in my spare time.

## Authors

* **Conrad Gohl** - *Initial inspiration* - [Unknown-One](https://hardforum.com/threads/quick-switch-between-extended-desktop-and-surround.1590030/) on hardforum.com

## License

This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](LICENSE) file for details

## Acknowledgments

* Thank you to the Unknown-One for making there script available to us and for giving me the inspiration to make this project.

