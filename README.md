[![Build status](https://ci.appveyor.com/api/projects/status/dn8mjauo4i5ghwlg?svg=true)](https://ci.appveyor.com/project/camalot/madb)

# madb
This is a Managed port of the Android Debug Bridge to allow communication from .NET applications to Android devices. 
This wraps the same methods that the ddms uses to directly communicate with ADB. 
This gives more flexibility to the developer than launching an adb process and executing one of its build in commands.

## Installation
To install madb, run the following command in the [Package Manager Console](http://docs.nuget.org/consume/package-manager-console):

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
