# Medicore Medical Services Test App

## Overview
Medicore Medical Services Test App is a cross-platform application built with .NET MAUI, designed to provide medical services functionality in a portable and user-friendly interface. The application targets Android, iOS, macOS Catalyst and Windows platforms.

## Features
- **Medical Questionnaires**: Interactive forms for patients to fill out medical information
- **History Tracking**: View and manage patient medical history
- **Cross-Platform Compatibility**: Works seamlessly on Android, iOS, macOS and Windows

## Technology Stack
- **.NET MAUI**: Cross-platform UI framework
- **MVVM Architecture**: Using CommunityToolkit.MVVM for clean separation of concerns
- **C# 13.0**: Latest C# language features
- **.NET 9.0**: Modern .NET framework targeting multiple platforms

## Project Structure
The application follows a clean architecture approach with:
- **Models**: Data structures representing medical information
- **ViewModels**: Business logic and UI state management
- **Views**: XAML-based user interface components
- **Services**: Backend integrations and utility functions

## Requirements
- **Android**: API 21+ (Android 5.0 Lollipop and higher)
- **iOS**: iOS 15.0 and higher
- **macOS**: macOS 11.0 and higher (via Mac Catalyst)
- **Windows**: Windows 10 and higher

## Getting Started
1. Clone the repository
2. Open the solution in Visual Studio or JetBrains Rider
3. Restore NuGet packages
4. Build and run the application on your preferred platform

## Dependencies
- CommunityToolkit.Mvvm (8.4.0)
- Controls.UserDialogs.Maui (1.10.1)
- Microsoft.Maui.Controls (9.0.110)
- Microsoft.Extensions.Logging.Debug (9.0.0)
- MPowerKit.Navigation (1.4.5)
- MPowerKit.VirtualizeListView (2.5.1)
- sqlite-net-pcl (1.10.196-beta)

_This project was created for demonstration purposes._
