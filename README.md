[![MadBee](http://i.imgur.com/UxO8Lrj.png)](http://madb.bit13.com/)

[![Build status](https://ci.appveyor.com/api/projects/status/dn8mjauo4i5ghwlg?svg=true)](https://ci.appveyor.com/project/camalot/madb)
[![Code Coverage by CodeCov](https://codecov.io/github/camalot/madb/coverage.svg)](https://codecov.io/github/camalot/madb)

This is a Managed port of the Android Debug Bridge to allow communication from .NET applications to Android devices. 
This wraps the same methods that the ddms uses to directly communicate with ADB. 
This gives more flexibility to the developer than launching an adb process and executing one of its build in commands.

[![ohloh stats](http://www.ohloh.net/p/madb/widgets/project_partner_badge.gif)](http://www.ohloh.net/p/madb?ref=github)
[![Donate](http://www.paypal.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=43MEF4EUT8DLL)



![PushBullet Channels](http://i.imgur.com/GlVy4rW.png)

- [Source Code Commits](https://www.pushbullet.com/channel?tag=madbsourcecode)
- [Releases](https://www.pushbullet.com/channel?tag=madbreleases)


## Installation
To install madbee, run the following command in the [Package Manager Console](http://docs.nuget.org/consume/package-manager-console):

```

PM> Install-Package Managed.Adb 
PM> Install-Package Managed.Adb.x64

```

## FileSystem Methods / Properties
* Create 
* Move 
* Copy 
* MakeDirectory 
* Exists 
* Chmod 
* Delete 
* IsMountPointReadOnly 
* DeviceBlocks - Get a collection of the device blocks 
* Mount 
* Unmount 
* ResolveLink - Resolves a symbolic link to its full path

## Busybox Methods / Properties
* Available 
* Version 
* Commands 
* Supports ( command ) 
* Install 
* ExecuteShellCommand 
* ExecuteRootCommand

## Device Methods / Properties
* CanSU 
* State 
* MountPoints 
* Properties 
* EnvironmentVariables 
* GetProperty 
* FileSystem 
* BusyBox 
* IsOnline 
* IsOffline 
* IsEmulator 
* IsBootLoader 
* IsRecovery 
* RemountMountPoint 
* Reboot 
* Reboot ( into ) 
* SyncService 
* PackageManager 
* FileListingService 
* Screenshot 
* ExecuteShellCommand 
* ExecuteRootShellCommand 
* InstallPackage 
* SyncPackageToDevice 
* InstallRemotePackage 
* RemoveRemotePackage 
* UninstallPackage

## FileEntry Methods / Properties
* FindOrCreate *static 
* Find *static 
* Parent 
* Name 
* LinkName 
* Info 
* Permissions 
* Size 
* Date 
* Owner 
* Group 
* Type 
* IsApplicationPackage 
* IsRoot 
* IsExecutable 
* Children 
* IsLink 
* Exists 
* FindChild 
* IsDirectory 
* IsApplicationFileName 
* FullPath 
* FullResolvedPath 
* FullEscapedPath 
* PathSegments

## PackageManager Methods / Properties
* Packages 
* RefreshPackages 
* Exists 
* GetApkFileEntry 
* GetApkPath

## SyncService
* Pull 
* PullFile 
* Push 
* PushFile



### Copyright
Copyright (c) 2010-2016 Ryan Conrad

Portions are:
  
Copyright (c) 2004-2008 Jon Skeet and Marc Gravell  
Copyright (c) 2007 The Android Open Source Project  
