# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [5.1.0] - 2025-08-24
### Added
- LoadSceneAsync waits for SceneLoader before fade out
- SceneLoader Component and Prefab
- ISceneLoadable interface
- AwaitableSystem package dependency

### Fixed
- LoadScene null exception when the game is exited from loading scene

## [5.0.0] - 2025-08-06
### Changed
- Update Attributes package dependency to 3.1.0
- Increase Unity minimum version to 6000.0
- Replace asynchronously Task Load functions by Awaitable

### Removed
- Awaitable Coroutines package dependency
- Function SceneManager.QuitGame

## [4.3.0] - 2025-05-14
### Changed
- Reset TimeScale before load scene

## [4.2.0] - 2025-04-12
### Added
- LoadingLabel component
- LoadingProgressBar component

## [4.1.0] - 2024-12-07
### Added
- SceneManager.QuitGame function
- SceneManager.LoadScene functions using Scene
- Scene class

## [4.0.0] - 2024-10-11
### Added
- DefaultSceneTransition asset

### Changed
- Refactor SceneManager into static class (it was a ScriptableObject)

### Removed
- ISceneManager interface
- Scriptable Settings Provider package dependency

## [3.1.0] - 2024-10-06
### Added
- Transition field from LoadScene component

### Changed
- LoadingText updates text from Text and/or TextMeshPro

## [3.0.0] - 2024-10-03
### Added
- LoadScene function

### Changed
- Remove transition field from LoadScene component
- Rename function LoadScene -> LoadSceneAsync

### Removed
- UI_MODULE and TEXT_MESH_PRO_MODULE modules

## [2.1.0] - 2024-05-03
### Added
- OnLoadingStarted/Finished events

## [2.0.2] - 2024-04-22
### Fixed
- AnimationScreenFader scale

## [2.0.1] - 2023-03-10
### Changed
- Fix CanvasScreenFader fade in code
- Waits to loading operation is done to fade in

## [2.0.0] - 2022-09-03
### Added
- Awaitable Coroutines package 1.0.0
- Scriptable Settings Provider package 1.0.0
- IScreenFader interface
- AnimationScreenFader component and prefab
- ScreenFaderPool
- SceneTransition
- TextMeshPro support
- Attributes 2.1.0 package dependency
- Lock/Unlock Loading Scene
- LoadScene component

### Changed
- Scriptable Settings Provider for Scene Manager 
- Replace ScreenFaderCanvas -> CanvasScreenFader
- SceneLoadingSettings -> SceneManager

### Removed
- Animation Parameters package
- ScreenFaderAnimator component
- Static SceneManager class
- SceneLoadingSettingsEditor

## [1.0.1] - 2021-01-14
### Changed
- Loading Scenes not added to Build Settings won't stop the build process

## [1.0.0] - 2021-01-13
### Added
- SceneLoadingBuilder builder report
- LoadingText component
- LoadingSlider component
- SceneManager class
- Internal SceneLoader component
- SceneLoadingSettings ScriptableObject
- Animation Parameters package 1.0.0 dependency
- ScreenFaderCanvas Prefab
- ScreenFaderCanvas component
- ScreenFaderAnimator component
- Scene attribute
- CHANGELOG
- README
- Initial commit

[Unreleased]: https://github.com/HyagoOliveira/SceneManagement/compare/5.1.0...main
[5.1.0]: https://github.com/HyagoOliveira/SceneManagement/tree/5.1.0/
[5.0.0]: https://github.com/HyagoOliveira/SceneManagement/tree/5.0.0/
[4.3.0]: https://github.com/HyagoOliveira/SceneManagement/tree/4.3.0/
[4.2.0]: https://github.com/HyagoOliveira/SceneManagement/tree/4.2.0/
[4.1.0]: https://github.com/HyagoOliveira/SceneManagement/tree/4.1.0/
[4.0.0]: https://github.com/HyagoOliveira/SceneManagement/tree/4.0.0/
[3.1.0]: https://github.com/HyagoOliveira/SceneManagement/tree/3.1.0/
[3.0.0]: https://github.com/HyagoOliveira/SceneManagement/tree/3.0.0/
[2.1.0]: https://github.com/HyagoOliveira/SceneManagement/tree/2.1.0/
[2.0.2]: https://github.com/HyagoOliveira/SceneManagement/tree/2.0.2/
[2.0.1]: https://github.com/HyagoOliveira/SceneManagement/tree/2.0.1/
[2.0.0]: https://github.com/HyagoOliveira/SceneManagement/tree/2.0.0/
[1.0.1]: https://github.com/HyagoOliveira/SceneManagement/tree/1.0.1/
[1.0.0]: https://github.com/HyagoOliveira/SceneManagement/tree/1.0.0/