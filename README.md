![NVSA](Images/NVSA.PNG?raw=true "Nvidia Surround Assistant")

# NVidia Surround Assistant

Like your desktop in extended mode but like playing most of your games in 1x3 surround mode or maybe you want to play only on one screen and disable the others? 

Then NVidia Surround Assistant (NVSA) could help you. NVSA will detect new/destroyed processes via two methods, hooks and wmi, and switch to a pre-configured NVidia surround/grid profile of your choosing. It uses IGDB api to query there database to get game cover art for the tiles but the images are customizable.
There are two configurable dead zone timers that run:
 1. After the initial detection of an application it prohibits a switch back to desktop. I found this was required with games by Ubisoft. They start and kill there processes before actually launching there UI, I assume it has something to do with there DRM or update checks.
 2. After the detection of an application closing/exiting. This timer allows you to cancel the switch back if so required, via a pop up and keyboard shortcut. I use this at times when I am tweaking settings or mods but don't want to switch surround profiles the whole time.

NVSA will also attempt to save your window positions and restore them. This feature is very basic but does what I want it for, most of the time.

## Getting Started

There are [setup files](https://github.com/Entr0py86/NVidia_Surround_Assistant/releases) in the repository that can be used to install the application.
I have unfortunately never tested the x86 version  of the build, as I no longer run Win x86 on any of my machines. Please create an issue and I will attempt to assist you as long as you are willing to help test.

### Installing

1. Run the installer
2. Run NVidia Surround Assistant.exe
3. Follow the message box instructions, for initial setup.
4. Add applications to your detection list.

### Testing:
Physical screen setup [images](https://robertsspaceindustries.com/spectrum/community/SC/forum/50264/thread/my-sim-pit-desk)

#### Hardware:
  * 1 x GTX 1080 
  * 1 x Acer 21.5 inch; 1600x900@60Hz
  * 1 x Benq XL2420Z; 1920x1080@120Hz (Using Custom resolution [BlurBuster.com](https://www.blurbusters.com/benq/strobe-utility/#largeverticaltotal))
  * 2 x Benq XL2430T; 1920x1080@120Hz (Using Custom resolution [BlurBuster.com](https://www.blurbusters.com/benq/strobe-utility/#largeverticaltotal))

#### Profiles:
  * 4x Screens all in extended mode 
  
  ![4xEntended](Images/4xExtended.PNG?raw=true "4 x Extended Desktop")
  
  * 1x3 in surround and 1 x extended 
  
  ![1x3x1Entended](Images/1x3x1Extended.PNG?raw=true "1 x 3 x Extended Desktop")
  
  * 1x Desktop and 3 off 
  
  ![1xDesktop](Images/1xDesktop.PNG?raw=true "1 x Desktop")
  
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

## Donations

If you like my work, a beer for the weekend would be greatly appreciated.

[![paypal]([https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=JWCEP7GLR62J2](https://www.paypal.com/donate/?hosted_button_id=YE8Y7DFXMATMJ))


