# Changelog

All notable changes to this project will be documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

**Things to notice:**

* Released releases always has a field written just below the title and before the list: The LSPDFR and RPH version built against.

* All dates are UTC+08:00.

## 0.2.3 [2022/1/6]

Built against LSPDFR 0.4.9

### Added

* New API for breathalyzer

* A wrapper class will be in distribution from this version

### Changed

* Added argument checks for all APIs

* **LICENSE is now GNU LGPL 3.0 or later**

## 0.2.2 [2022/1/5]

Built against LSPDFR 0.4.9

### Added

* A breathalyzer feature has been added, accessible via menus.

### Changed

* Fixed an issue resulted in Check Passengers failed to enable again once disabled.

* Fixed an issue resulted in menus never open again after disabled due to player moving / inside vehicle.

* Fixed a wrong coloring of NONE text of vehicle status check.

* Standing still check has been removed for now.

## 0.2.1 [2022/1/4]

Built against LSPDFR 0.4.9

### Changed

* Fixed an issue resulted in crash when traffic stop driver got out from the vehicle.

## 0.2.0 [2022/1/2]

Built against LSPDFR 0.4.9

### Changed

* Availability and seats available for checking via Request Passenger Status Check are now determined when opening the menu

* Fixed an issue resulted in Arrest Interaction menu can still be opened for suspects with transport unit assigned.

* Adds dispatch conversation message when checking for vehicle status during traffic stops.

## 0.1.3 [2022/1/1]

Built against 0.4.9

### Changed

* Updated LemonUI to 1.6.0

* The project now builds on x64 instead of Any CPU

* Adds an option in the Traffic Stop menu to check passenger IDs

* Fixed the issue resulting Transport request does not work

## 0.1.2 [2021/12/31]

Built against LSPD First Response 0.4.9

* Modified ratios for vehicle details.

## 0.1.1 [2021/12/30]

* Initial public build.
